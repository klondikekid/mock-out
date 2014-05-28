using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockOut.Proxy
{
    public class MockProxyOptions : IProxyOptions
    {
        private Lazy<Dictionary<string, Func<dynamic>>> _returns =
            new Lazy<Dictionary<string, Func<dynamic>>>(() => new Dictionary<string, Func<dynamic>>());

        public void Returns<TProxy>(System.Linq.Expressions.Expression<Func<TProxy, object>> method, Func<dynamic> factory)
        {
            var methodName = ProxyExtensions.MethodName(method.Body);

            if (!_returns.Value.Keys.Contains(methodName))
            {
                _returns.Value.Add(methodName, factory);
            }
        }

        public Dictionary<string, Func<dynamic>> ReturnValues
        {
            get { return _returns.Value;  }
        }
    }
}
