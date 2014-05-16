using System;

namespace MockOut.Core
{
    public interface IMockOptions
    {
        void Quantity(int number);

        void MapField(string name, MockCategory category);

        void UseCategory(MockCategory category);

        void DefineField<T>(string fieldName, Func<dynamic, T> factory);

        void Intercept<T>(Action<T> fn);

        void AsJson(Action<string> action);

        void AsJson(JsonStream jsonStream);

        void asXml(Action<string> action);
    }
}
