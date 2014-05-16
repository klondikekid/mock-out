using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MockOut.Core
{
    public abstract class BaseMockingStrategy : IMockingStrategy
    {
        protected int NumberToGenerate = 1;
        protected int MinLengthValue = 0;
        protected int MaxLengthValue = 0;

        protected dynamic CurrentObject;

        protected MockCategory CurrentCategory;
        protected bool UseCurrentCategory = false;

        protected List<FieldMap> maps;

        protected Dictionary<Type, dynamic> interceptions;
        protected Dictionary<string, dynamic> overrides;

        public Type ReturnType { get; set; }

        public Type TypeInScope { get; set; }

        protected bool SerializeToJson;
        protected Action<string> JsonAction;
        protected JsonStream Json;

        protected bool SerializeToXml;
        protected Action<string> XmlAction;

        protected BaseMockingStrategy()
        {
            maps = new List<FieldMap>();
            interceptions = new Dictionary<Type, dynamic>();
            overrides = new Dictionary<string, dynamic>();
        }

        protected T Instance<T>() where T : new()
        {
            var mockConstructor = typeof (T).GetConstructor(new Type[] {});

            if (mockConstructor == null)
            {
                var randomMethod = typeof(Random).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(meth => meth.ReturnType == typeof(T));
                
                if (randomMethod == null)
                {
                    throw new Exception(
                        string.Format(
                        "Could not mock random value for type \"{0}\".  If this is a custom type, implement the partial class \"{1}\" with the following method: \"public static {0} {0}(){{ ... }}\"", 
                        typeof(T).Name, 
                        typeof(Random).Name));
                }

                return (T)randomMethod.Invoke(null, null);
            }
            else
            {
                var mock = (T) mockConstructor.Invoke(new Object[] {});

                return mock;
            }
        }

        public abstract void Mock<T>(T targetType, Action<T> act) where T : new();

        public abstract T Simple<T>(T targetType);

        public abstract T Simple<T>(T targetType, MockCategory category);

        public abstract IList<T> SimpleList<T>(T targetType, int quantity);

        public abstract IList<T> SimpleList<T>(T targetType, int quantity, MockCategory category);

        public abstract int Range(int minValue, int maxValue);

        public void Quantity(int number)
        {
            NumberToGenerate = number;
        }

        public void MinLength(int number)
        {
            MinLengthValue = number;
        }

        public void MaxLength(int number)
        {
            MaxLengthValue = number;
        }

        public void UseCategory(MockCategory category)
        {
            UseCurrentCategory = true;
            CurrentCategory = category;
        }

        public void MapField(string name, MockCategory category)
        {
            maps.Add(new FieldMap(name, category));
        }

        public void Intercept<T>(Action<T> fn)
        {
            interceptions[typeof (T)] = fn;
            //fn.Invoke(CurrentObject);
        }

        public void DefineField<T>(string fieldName, Func<dynamic, T> factory)
        {
            var fieldDefinition = new FieldDefinition<T>(fieldName, TypeInScope, factory);
            var key = string.Format("{0}::{1}", TypeInScope.Name, fieldDefinition.Field);
            overrides[key] = fieldDefinition;
        }

        protected bool HasConstructor<T>(T instance)
        {
            return typeof (T).GetConstructor(new Type[] {}) != null;
        }

        public void AsJson(Action<string> action)
        {
            SerializeToJson = true;
            JsonAction = action;
        }

        public void AsJson(JsonStream jsonStream)
        {
            SerializeToJson = true;
            Json = jsonStream;
        }

        public void asXml(Action<string> action)
        {
            SerializeToXml = true;
            XmlAction = action;
        }
    }
}
