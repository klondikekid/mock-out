using System;

namespace MockOut.Core
{
    public class FieldDefinition<T>
    {
        public FieldDefinition(string field, Type targetType, Func<dynamic, T> factory)
        {
            TargetType = targetType;
            Field = field;
            FieldFactory = factory;
        }
        public string Field { get; private set; }

        public Type FieldType
        {
            get { return typeof (T); }
        }

        public Type TargetType { get; private set; }

        public Func<dynamic, T> FieldFactory { get; private set; }
    }
}
