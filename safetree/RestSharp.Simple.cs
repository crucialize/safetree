using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestSharp.Simple
{

	static class RestSharpSimple
	{

		public static IRestResponse Simple(this IRestClient client,IRestRequest req)
		{
			//parse
			var url = new Uri(req.Resource);
			client.BaseUrl = new Uri(url.Scheme + "://" + url.Host);
			req.Resource = url.PathAndQuery;

			var res = client.Execute(req);
			return res;
		}

		public static IRestResponse Simple<T> (this IRestClient client, IRestRequest req) where T:new()
		{
			//parse
			var url = new Uri(req.Resource);
			client.BaseUrl = new Uri(url.Scheme + "://" + url.Host);
			req.Resource = url.PathAndQuery;

			var res = client.Execute<T>(req);
			return res;
		}

		public static void SimpleAsync(this IRestClient client, IRestRequest req,Action<IRestResponse> Callback)
		{
			//parse
			var url = new Uri(req.Resource);
			client.BaseUrl = new Uri(url.Scheme + "://" + url.Host);
			req.Resource = url.PathAndQuery;

			var res = client.ExecuteAsync(req,Callback);
		}

		public static void SimpleAsync<T>(this IRestClient client, IRestRequest req, Action<IRestResponse> Callback) where T:new()
		{
			//parse
			var url = new Uri(req.Resource);
			client.BaseUrl = new Uri(url.Scheme + "://" + url.Host);
			req.Resource = url.PathAndQuery;

			client.ExecuteAsync<T>(req, Callback);
			var res = client.ExecuteAsync(req, Callback);
		}

	}
}
