
using System;
using System.Collections.Generic;

namespace MockOut.Core
{
    public interface IMockingStrategy : IMockOptions
    {
        void Mock<T>(T targetType, Action<T> act) where T : new();

        T Simple<T>(T targetType);

        T Simple<T>(T targetType, MockCategory category);

        IList<T> SimpleList <T>(T targetType, int quantity);

        IList<T> SimpleList<T>(T targetType, int quantity, MockCategory category);

        int Range(int minValue, int maxValue);

        Type ReturnType { get; set; }

        Type TypeInScope { get; set; }
    }
}
