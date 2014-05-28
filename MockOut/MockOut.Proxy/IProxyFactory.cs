using System;
using System.Collections.Generic;

namespace MockOut.Proxy
{
    public interface IProxyFactory<T>
    {
        T Instance(params object[] args);
    }
}
