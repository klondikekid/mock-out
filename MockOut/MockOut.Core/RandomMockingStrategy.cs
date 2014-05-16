using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MockOut.Core
{
    public class RandomMockingStrategy : BaseMockingStrategy
    {
        public override void Mock<T>(T targetType, Action<T> act)
        {
            TypeInScope = typeof(T);

            for (var i = 0; i < NumberToGenerate; i = i + 1)
            {
                var instance = Instance<T>();
                PropertyInfo[] propertyInfos = null;

                if (HasConstructor(instance))
                {
                    // Mock Properties
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite);

                    propertyInfos = properties as PropertyInfo[] ?? properties.ToArray();
                    
                    if (!propertyInfos.Any())
                    {
                        continue;
                    }

                    foreach (var p in propertyInfos)
                    {
                        

                        // Get any attributes
                        var mockFieldAttribtes = p.GetCustomAttributes(typeof (MockedFieldAttribute), false);

                        var fieldAttributes = mockFieldAttribtes as Attribute[] ?? mockFieldAttribtes.ToArray();

                        MethodInfo methodInfo = null;

                        // Use Category
                        if (UseCurrentCategory)
                        {
                            methodInfo = typeof (Random).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                    .FirstOrDefault(meth => meth.Name == CurrentCategory.ToString("G"));
                        }
                        else if (fieldAttributes.Any())
                        {
                            var attr = fieldAttributes.First() as MockedFieldAttribute;

                            if (attr != null)
                            {
                                methodInfo = typeof (Random).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                    .FirstOrDefault(meth => meth.Name == attr.Category.ToString("G"));
                            }
                        }

                        // Look for field Maps
                        if (maps.Any(a => a.Field == p.Name) && !UseCurrentCategory)
                        {
                            var map = maps.First(a => a.Field == p.Name);

                            methodInfo = typeof(Random).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                    .FirstOrDefault(meth => meth.Name == map.Category.ToString("G"));
                        }

                        if (methodInfo == null)
                        {

                            methodInfo =
                                typeof (Random).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                    .FirstOrDefault(meth => meth.ReturnType == p.PropertyType);

                        }

                        
                        if (methodInfo != null)
                        {
                            var obj = methodInfo.Invoke(null, null);

                            p.SetValue(instance, obj, null);
                        }

                    }

                    foreach (var p in propertyInfos)
                    {
                        var key = string.Format("{0}::{1}", typeof(T).Name, p.Name);

                        var linqPad = string.Format("UserQuery+{0}", key);

                        if (overrides.ContainsKey(key) || overrides.ContainsKey(linqPad))
                        {
                            Func<T, dynamic> fn = null;

                            if (overrides.ContainsKey(key))
                            {
                                fn = overrides[key].FieldFactory as Func<T, dynamic>;
                            }
                            else if (overrides.ContainsKey(linqPad))
                            {
                                fn = overrides[linqPad].FieldFactory as Func<T, dynamic>;
                            }

                            if (fn != null)
                            {
                                //System.Diagnostics.Trace.WriteLine("Invoking: " + key);
                                var obj = fn.Invoke(instance);

                                if (obj != null)
                                {
                                    p.SetValue(instance, obj, null);
                                }
                            }

                        }
                    }
                }


                // interceptions
                if (interceptions.ContainsKey(typeof (T)))
                {
                    var interception = interceptions[typeof (T)];

                    var action = interception as Action<T>;

                    if (action != null)
                    {
                        action.Invoke(instance);
                    }
                }

                // Serialize?
                if (SerializeToJson)
                {
                    var json = JsonConvert.SerializeObject(instance);
                    if (Json != null)
                    {
                        Json.Add(json);
                    }
                    if (JsonAction != null)
                    {
                        JsonAction(json);
                    }
                }

                if (SerializeToXml)
                {
                    var returnType = ReturnType ?? TypeInScope;
                    
                    var serializer = new XmlSerializer(returnType);
                    using (var writer = new StringWriter())
                    {
                        serializer.Serialize(writer, instance);
                        XmlAction(writer.ToString());
                    }
                }

                act(instance);
            }
        }

        public override T Simple<T>(T targetType)
        {
            var methodInfo = typeof(Random).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                    .FirstOrDefault(meth => meth.Name == typeof(T).Name);

            if (methodInfo != null)
            {
                var obj = methodInfo.Invoke(null, null);

                if (obj is T)
                {
                    return (T)obj;
                }
            }

            return default(T);
        }

        public override T Simple<T>(T targetType, MockCategory category)
        {
            var methodInfo = typeof(Random).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                    .FirstOrDefault(meth => meth.Name == category.ToString("G"));

            if (methodInfo != null)
            {
                var obj = methodInfo.Invoke(null, null);

                if (obj is T)
                {
                    return (T) obj;
                }
            }
            else // Not found
            {
                return Simple<T>(targetType);
            }

            return default(T);
        }

        public override IList<T> SimpleList<T>(T targetType, int quantity)
        {
            var list = new List<T>();

            while (quantity-- > 0)
            {
                var obj = Simple(targetType);
                if ((object) obj != null)
                {
                    list.Add(obj);
                }
            }

            return list;
        }

        public override IList<T> SimpleList<T>(T targetType, int quantity, MockCategory category)
        {
            var list = new List<T>();

            while (quantity-- > 0)
            {
                var obj = Simple(targetType, category);
                if ((object)obj != null)
                {
                    list.Add(obj);
                }
            }

            return list;
        }

        public override int Range(int minValue, int maxValue)
        {
            return Random.Generate.Next(minValue, maxValue);
        }
    }
}
