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

        protected string CurrentCategory;
        protected bool UseCurrentCategory = false;

        protected List<FieldMap> maps;

        protected Dictionary<string, dynamic> Overrides;
        protected dynamic ReturnValue;

        public Type ReturnType { get; set; }

        public Type TypeInScope { get; set; }

        public IMockDataStore DataStore { get; set; }

        protected bool SerializeToJson;
        protected Action<string> JsonAction;
        protected JsonStream<dynamic> Json;

        protected bool SerializeToXml;
        protected Action<string> XmlAction;

        protected BaseMockingStrategy(IMockDataStore mockDataStore)
        {
            DataStore = mockDataStore;
            maps = new List<FieldMap>();
            Overrides = new Dictionary<string, dynamic>();
        }

        protected T Instance<T>() where T : new()
        {
            var mockConstructor = typeof (T).GetConstructor(new Type[] {});

            if (mockConstructor == null)
            {
                var randomMethod = DataStore.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(meth => meth.ReturnType == typeof(T));
                
                if (randomMethod == null)
                {
                    throw new Exception(
                        string.Format(
                        "Could not mock random value for type \"{0}\".", 
                        typeof(T).Name));
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

        public abstract T Simple<T>(T targetType, string category);

        public abstract IList<T> SimpleList<T>(T targetType, int quantity);

        public abstract IList<T> SimpleList<T>(T targetType, int quantity, string category);

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

        public void Map(string name, string category)
        {
            maps.Add(new FieldMap(name, category));
        }

        public void Returns<T>(Func<T, dynamic> factory)
        {
            ReturnValue = factory;
        }

        public void Define<T>(string fieldName, Func<dynamic, T> factory)
        {
            var fieldDefinition = new FieldDefinition<T>(fieldName, TypeInScope, factory);
            var key = string.Format("{0}::{1}", TypeInScope.Name, fieldDefinition.Field);
            Overrides[key] = fieldDefinition;
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

        public void asXml(Action<string> action)
        {
            SerializeToXml = true;
            XmlAction = action;
        }
    }
}
