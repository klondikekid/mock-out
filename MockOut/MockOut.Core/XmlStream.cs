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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MockOut.Core
{
    public class XmlStream<T> where T : new()
    {
        private IList<string> _xml;
        private List<T> _models;
        private XmlDocument _document;

        public XmlStream()
        {
            _xml = new List<string>();
            _models = new List<T>();
        }

        public void Add(string xml)
        {
            _xml.Add(xml);
        }

        public void Add(T model)
        {
            _models.Add(model);
        }

        public XmlDocument Document
        {
            get
            {
                if (_document == null)
                {
                    _document = new XmlDocument();
                    _document.LoadXml(ToString());

                    return _document;
                }

                _document.InnerXml = this.ToString();

                return _document;
            }
        }

        public override string ToString()
        {
            if (_models.Any())
            {
                return _models.ToXmlString();
            }

            return string.Format("{0}{1}{2}",
                Header(),
                string.Join(Environment.NewLine, _xml),
                Footer());
        }

        private string Header()
        {
            return string.Format(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOf{0} xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
", typeof(T).Name);
        }

        private string Footer()
        {
            return string.Format("</ArrayOf{0}>", typeof(T).Name);
        }
    }
}
