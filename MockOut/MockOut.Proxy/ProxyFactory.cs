using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockOut.Proxy
{
    public class ProxyFactory<T> : IProxyFactory<T>
    {
        private readonly Type _proxyType;

        public ProxyFactory(Type proxyType)
        {

            if (proxyType.Name != typeof(T).Name
                && proxyType.Name != "UserQuery+" + typeof(T).Name) // LinqPad
            {
                throw new ArgumentException("proxy type must be of type: " + typeof(T).FullName);
            }
            _proxyType = proxyType;
        }

        public T Instance(params object[] args)
        {
            var instance = Activator.CreateInstance(_proxyType, args);
            return (T)instance;
        }

        //public IEnumerable<T> Create(int number, params object[] args)
        //{
        //    var list = new List<T>();

        //    for (var i = 0; i < number; i = i + 1)
        //    {
        //        list.Add(Create(args));
        //    }

        //    return list;
        //}
    }
}
