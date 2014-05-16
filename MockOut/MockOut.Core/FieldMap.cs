using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockOut.Core
{
    public class FieldMap
    {
        public FieldMap(string field, MockCategory category)
        {
            Field = field;
            Category = category;
        }
        public string Field { get; protected set; }
        public MockCategory Category { get; protected set; }
    }
}
