#region License
// Copyright (c) 2014 John Robinson
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockOut.Core
{
    public class JsonStream<T> where T : new()
    {
        private IList<string> _json;
        private List<T> _models;
        private JsonFormat _jsonFormat;

        public JsonStream() : this(JsonFormat.None)
        {
        }

        public JsonStream(JsonFormat format = JsonFormat.None)
        {
            _jsonFormat = format;
            _json = new List<string>();
            _models = new List<T>();
        }

        public void Add(string json)
        {
            _json.Add(json);

            System.Diagnostics.Trace.WriteLine(json);
        }

        public void Add(T model)
        {
            _models.Add(model);
        }

        public override string ToString()
        {
            if (_models.Any())
            {
                return _models.ToJsonString();
            }
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
