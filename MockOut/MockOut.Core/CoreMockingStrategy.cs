#region License
// Copyright (c) 2014 John Robinson
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MockOut.Core
{
    public class CoreMockingStrategy : BaseMockingStrategy
    {
        public CoreMockingStrategy(IMockDataStore mockDataStore)
            : base(mockDataStore)
        {

        }

        public override void Mock<T>(T targetType, Action<T> act)
        {
            TypeInScope = typeof(T);

            for (var i = 0; i < NumberToGenerate; i = i + 1)
            {
                var instance = Instance<T>();

                // Return takes the highest precedence
                if (ReturnValue != null)
                {
                    var returnFnValue = ReturnValue.Invoke(instance);
                    act(returnFnValue);
                    continue;
                }

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
                        // Field attributes have the lowest precedence
                        var mockFieldAttribtes = p.GetCustomAttributes(typeof (MockedFieldAttribute), false);
                        var fieldAttributes = mockFieldAttribtes as Attribute[] ?? mockFieldAttribtes.ToArray();

                        MethodInfo methodInfo = null;

                        // Look for field maps if any
                        if (maps.Any(a => a.Field == p.Name) && !UseCurrentCategory)
                        {
                            var map = maps.First(a => a.Field == p.Name);

                            methodInfo = DataStore.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static)
                                    .FirstOrDefault(meth => meth.Name == map.Category);
                        }
                        // else try the field attributes
                        else if (fieldAttributes.Any())
                        {
                            var attr = fieldAttributes.First() as MockedFieldAttribute;

                            if (attr != null)
                            {
                                methodInfo = DataStore.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static)
                                    .FirstOrDefault(meth => meth.Name == attr.Category);
                            }
                        }

                        // If there are no maps or field attributes, look for a method named the same as the type.
                        if (methodInfo == null)
                        {
                            methodInfo =
                                DataStore.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static)
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

                        if (Overrides.ContainsKey(key) || Overrides.ContainsKey(linqPad))
                        {
                            Func<T, dynamic> fn = null;

                            if (Overrides.ContainsKey(key))
                            {
                                fn = Overrides[key].FieldFactory as Func<T, dynamic>;
                            }
                            else if (Overrides.ContainsKey(linqPad))
                            {
                                fn = Overrides[linqPad].FieldFactory as Func<T, dynamic>;
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

                // Single item serialization
                if (SerializeToJson)
                {
                    var json = instance.ToJsonString();

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
                    XmlAction(instance.ToXmlString());
                }

                // Return the value to the handler
                act(instance);
            }
        }

        public override T Simple<T>(T targetType)
        {
            var methodInfo = DataStore.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static)
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

        public override T Simple<T>(T targetType, string category)
        {
            var methodInfo = DataStore.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static)
                                    .FirstOrDefault(meth => meth.Name == category);

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

        public override IList<T> SimpleList<T>(T targetType, int quantity, string category)
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
            return Generate.Next(minValue, maxValue);
        }
    }
}
