using MockOut.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MockOut.Proxy
{
    public interface IProxyOptions
    {
        void Returns<TProxy>(Expression<Func<TProxy, object>> method, Func<dynamic> factory);

        Dictionary<string, Func<dynamic>> ReturnValues { get; }
    }
}
