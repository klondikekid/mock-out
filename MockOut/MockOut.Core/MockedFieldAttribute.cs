using System;

namespace MockOut.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class MockedFieldAttribute : Attribute
    {
        private readonly MockCategory _category;

        public MockedFieldAttribute(MockCategory category)
        {
            this._category = category;
        }

        public MockCategory Category
        {
            get { return _category; }
        }
    }
}
