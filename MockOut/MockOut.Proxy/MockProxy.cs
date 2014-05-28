using MockOut.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MockOut.Proxy
{
    public class MockProxy<T> where T : IMockDataStore, new()
    {
        private readonly Lazy<Dictionary<string, dynamic>> _returns =
            new Lazy<Dictionary<string, dynamic>>(
                () => new Dictionary<string, dynamic>());

        public K Create<K>(params Expression<Action<IProxyOptions>>[] options)
        {
            var type = typeof(K);
            
            if (!type.IsInterface)
            {
                throw new NotSupportedException("Must be an interface type.");
            }

            var moduleBuilder =
                AppDomain.CurrentDomain.BuildAssembly(
                ProxyExtensions.BuildAssemblyName("MockOut.Dynamic"),
                AssemblyBuilderAccess.Run)
                .BuildModule("Proxies");

            var proxyType = 
                ProxyExtensions.BuildInterfaceType<K, T>(
                    moduleBuilder,
                    type.Name,
                    TypeAttributes.Public,
                    options);

            //BuildInferfaceType<K, T>(type.Name + "Proxy", TypeAttributes.Public);

            var instance = Activator.CreateInstance(proxyType, null);
            return (K)instance;
        }

        public void Returns<TProxy>(Expression<Func<TProxy, object>> method, Func<dynamic> factory)
        {
            var methodName = ProxyExtensions.MethodName(method.Body);

            if (!_returns.Value.Keys.Contains(methodName))
            {
                var result = factory();

                _returns.Value.Add(methodName, result);
            }
        }
    }
}
