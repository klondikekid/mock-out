using System;
using System.Collections.Generic;

namespace MockOut.Core
{
    public class Mock
    {
        private IMockingStrategy _mockingStrategy;

        public Mock(IMockingStrategy mockingStrategy)
        {
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
        }

        public T Simple<T>()
        {
            var mock = default(T);

            _mockingStrategy.TypeInScope = typeof(T);

            return _mockingStrategy.Simple(mock);
        }

        public T Simple<T>(MockCategory category)
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
       
        public IList<T> SimpleList<T>(MockCategory category, int quantity)
        {
            var mock = default(T);

            _mockingStrategy.TypeInScope = typeof(T);

            return _mockingStrategy.SimpleList(mock, quantity, category);
        }

        public string Join(MockCategory category, int quantity, string separator = "")
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
