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
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace MockOut.Api
{
    public class MockMessageHandler : DelegatingHandler
    {
        private MockApi _mockApi;

        public MockMessageHandler(HttpConfiguration httpConfiguration, MockApi mockApi)
        {
            if (mockApi == null)
            {
                throw new ArgumentNullException("mockApi");
            }

            //InnerHandler = new HttpControllerDispatcher(httpConfiguration);
            _mockApi = mockApi;   
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var path = request.RequestUri.AbsolutePath.Substring(1);

            var routeData = request.GetRouteData();

            if (routeData != null)
            {
                Trace.WriteLine(routeData.Route.RouteTemplate);

                path = routeData.Route.RouteTemplate;
            }
            else
            {
                Trace.WriteLine("No Route Found.");
            }

            path = request.Method.Method.ToLower() + "^" + path;

            Trace.WriteLine(path);

            if (!_mockApi.Routes.ContainsKey(path))
            {
                return base.SendAsync(request, cancellationToken);
            }

            var registeredApi = _mockApi.Routes[path];

            var result = (object)registeredApi.Invoke();

            var responseMessage = request.CreateResponse(HttpStatusCode.OK, result);

            return base.SendAsync(request, cancellationToken)
                .ContinueWith(task => responseMessage, cancellationToken);
        }
    }
}
