using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Simple;
using LangExt;
using System.Threading;

namespace safetree
{
	class SafetreeClient
	{
		public RestClient client;

		public SafetreeClient()
		{
			client = new RestClient();
			client.CookieContainer = new System.Net.CookieContainer();
		}

		public string Login(string username,string password)
		{
			//todo:增加各地的
			var res=client.Simple(new RestRequest($"https://sichuanlogin.xueanquan.com/LoginHandler.ashx?userName={username}&password={password}&checkcode=&type=login&loginType=1", Method.GET));

			return res.Content;
		}

		public string DoCourse(SafetreeTask SafetreeCourse)
		{
			var RetString = "";

			foreach(var req in SafetreeCourse.ReqList)
			{
				var r = client.Simple(req);
				RetString += r.Content;
				Thread.Sleep(200);
			}

			return RetString;
		}

	}

	class SafetreeTask
	{
		public List<RestRequest> ReqList = new List<RestRequest>();

		public static SafetreeTask ParseCourse(string HAR)
		{
			var har = HAR.FromJSON<HARFile>();
			var reqs = from p in har.log.entries select p.request;

			var H_CheckCourse = (from p in reqs where p.method == "POST" && p.url.Contains("_method=SkillCheckName") select p).ToArray()[0];
			var Req_CheckCourse = H_CheckCourse.ToRequest();

			var H_Exercise = (from p in reqs where p.method == "POST" && p.url.Contains("_method=TemplateIn2") select p).ToArray()[0];
			var Req_Exercise = H_Exercise.ToRequest();

			var t = new SafetreeTask();
			t.ReqList.Add(Req_CheckCourse);
			t.ReqList.Add(Req_Exercise);

			return t;
		}

	}

	struct HARFile
	{

		public H_Log log;

		public struct H_Log
		{
			public H_Entry[] entries;

			public struct H_Entry
			{
				public H_Request request;

				public struct H_Request
				{
					public string method;
					public string url;
					public H_PostData postData;

					public struct H_PostData
					{
						public string mimeType;
						public string text;
					}

					public RestRequest ToRequest()
					{
						var EMethod = (Method)Enum.Parse(typeof(Method),method);

						var req = new RestRequest(url, EMethod);
						
						if(EMethod== Method.POST)
						{
							req.AddHeader("Content-Type", postData.mimeType);
							req.AddPlainBody(postData.text);
						}

						return req;
					}

				}
			}

		}

	}

}
