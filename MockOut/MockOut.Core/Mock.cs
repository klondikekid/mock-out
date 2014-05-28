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

namespace MockOut.Core
{
    public class Mock
    {
        private IMockingStrategy _mockingStrategy;

        public Mock(IMockingStrategy mockingStrategy)
        {
            if (mockingStrategy == null)
            {
                throw new ArgumentNullException("mockingStrategy");
            }

            _mockingStrategy = mockingStrategy;

        }

        public void Create(Type targetType, Action<dynamic> act, params Action<IMockOptions>[] actions)
        {
            var mock = Activator.CreateInstance(targetType, null);

            _mockingStrategy.TypeInScope = targetType;
            _mockingStrategy.ReturnType = targetType;

            var options = _mockingStrategy as IMockOptions;

            foreach (var a in actions)
            {
                a.Invoke(options);
            }
            
            _mockingStrategy.Mock(mock, act);

            // Reset quantity
            options.Quantity(1);
        }

        public void Create<T>(Action<T> act, params Action<IMockOptions>[] actions)
            where T : new()
        {
            var mock = default(T);

            _mockingStrategy.TypeInScope = typeof(T);
            _mockingStrategy.ReturnType = typeof(T);

            var options = _mockingStrategy as IMockOptions;

            foreach (var a in actions)
            {
                a.Invoke(options);
            }

            _mockingStrategy.Mock(mock, act);

            // Reset quantity
            options.Quantity(1);
        }

        public T Simple<T>()
        {
            var mock = default(T);

            _mockingStrategy.TypeInScope = typeof(T);

            return _mockingStrategy.Simple(mock);
        }

        public T Simple<T>(string category)
        {
            var mock = default(T);

            _mockingStrategy.TypeInScope = typeof(T);

            return _mockingStrategy.Simple(mock, category);
        }

        public IList<T> SimpleList<T>(int quantity)
        {
            var mock = default(T);

            _mockingStrategy.TypeInScope = typeof(T);
            
            return _mockingStrategy.SimpleList(mock, quantity);
        }
       
        public IList<T> SimpleList<T>(string category, int quantity)
        {
            var mock = default(T);

            _mockingStrategy.TypeInScope = typeof(T);

            return _mockingStrategy.SimpleList(mock, quantity, category);
        }

        public string Join(string category, int quantity, string separator = "")
        {
            _mockingStrategy.TypeInScope = typeof(string);

            var list = _mockingStrategy.SimpleList("", quantity, category);

            return string.Join(separator, list);
        }
        
        public int Range(int minValue, int maxValue)
        {
            _mockingStrategy.TypeInScope = typeof(int);

            return _mockingStrategy.Range(minValue, maxValue);
        }
    }
}
