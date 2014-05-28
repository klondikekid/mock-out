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
using System.Linq;
using MockOut.Core;
using System.Collections.Generic;

namespace MockOut.Api
{
    internal class RegisteredApi<T> where T : new()
    {
        private Mock _mock = null;

        public RegisteredApi(
            string name, 
            string url, 
            object defaults, 
            Action<IMockOptions>[] actions,
            ApiMethod method,
            IMockingStrategy mockingStrategy)
        {
            Name = name;
            Url = url;
            Defaults = defaults;
            Actions = actions;
            Method = method;

            _mock = new Mock(mockingStrategy);
        }

        public string Name { get; private set; }

        public string Url { get; set; }

        public object Defaults { get; private set; }

        public Action<IMockOptions>[] Actions { get; private set; }

        public ApiMethod Method { get; private set; }

        public dynamic Invoke()
        {
            var list = new List<T>();

            _mock.Create<T>(x => list.Add(x), Actions);

            return list.Count() == 1 ? (dynamic)list.First() : (dynamic)list;
        }

        public bool MatchesRoute(string url)
        {
            return false;
        }
    }
}
