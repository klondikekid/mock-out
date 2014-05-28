using System;
using System.Web.Http;
using System.Web.Routing;
using MockOut.Core;
using MockOut.Api;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Linq;
using $rootnamespace$.Models;

namespace $rootnamespace$
{
    public static class MockApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
	
			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			config.Formatters.Clear();
            config.Formatters.Add(jsonFormatter);

            MockApi.Create<MockViewModel>(
                "ValuesApi", 
                "api/values",
                new {},
                act => act.Map("Value", Mocked.As(a => a.FirstName)));

            MockApi.RegisterRoutes(config);
        }
    }
}
