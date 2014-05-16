using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockOut.Core
{
    public class JsonStream
    {
        private IList<string> _json;
        private JsonFormat _jsonFormat;

        public JsonStream() : this(JsonFormat.None)
        {
        }

        public JsonStream(JsonFormat format = JsonFormat.None)
        {
            _jsonFormat = format;
            _json = new List<string>();
        }

        public void Add(string json)
        {
            _json.Add(json);
        }

        public override string ToString()
        {
            return formatJson(string.Format("[{0}]",
                string.Join(",", _json)));
        }

        private string formatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson,
                _jsonFormat == JsonFormat.Indented ? Formatting.Indented
                : Formatting.None);
        }
    }
}
