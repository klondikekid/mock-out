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
using System.Web.Http;
using System.Web.Http.Dispatcher;
using MockOut.Core;
using System.Net.Http;

namespace MockOut.Api
{
    public class MockApi
    {
        private IMockingStrategy _mockingStrategy;
        private Dictionary<string, dynamic> _routes;
            
        public MockApi(IMockingStrategy mockingStrategy)
        {
            if (mockingStrategy == null)
            {
                throw new ArgumentNullException("mockingStrategy");
            }

            _mockingStrategy = mockingStrategy;
            _routes = new Dictionary<string, dynamic>();
        }

        public Dictionary<string, dynamic> Routes
        {
            get
            {
                return _routes;
            }
        }

        public void Get<T>(string name, string url, object defaults, params Action<IMockOptions>[] actions)
            where T : new()
        {
            _routes["get^" + url] = new RegisteredApi<T>(name, url, defaults, actions, ApiMethod.Get, _mockingStrategy);
        }

        public void Post<T>(string name, string url, object defaults, params Action<IMockOptions>[] actions)
            where T : new()
        {
            _routes["post^" + url] = new RegisteredApi<T>(name, url, defaults, actions, ApiMethod.Post, _mockingStrategy);
        }

        public void Put<T>(string name, string url, object defaults, params Action<IMockOptions>[] actions)
            where T : new()
        {
            _routes["put^" + url] = new RegisteredApi<T>(name, url, defaults, actions, ApiMethod.Update, _mockingStrategy);
        }

        public void Delete<T>(string name, string url, object defaults, params Action<IMockOptions>[] actions)
            where T : new()
        {
            _routes["delete^" + url] = new RegisteredApi<T>(name, url, defaults, actions, ApiMethod.Delete, _mockingStrategy);
        }

        public void RegisterRoutes(
            HttpConfiguration config)
        {

            var handler = new MockMessageHandler(config, this);

            config.MessageHandlers.Add(handler);

            foreach (KeyValuePair<string, dynamic> kvp in _routes)
            {
                var name = kvp.Value.Name as string;
                var defaults = kvp.Value.Defaults as object;

                config.Routes.MapHttpRoute(
                    name,
                    kvp.Key.Split('^')[1],
                    defaults,
                    null,
                    HttpClientFactory.CreatePipeline(
                    new HttpControllerDispatcher(config),
                    new DelegatingHandler[] { new MockMessageHandler(config, this) }));
            }
        }
    }
}
