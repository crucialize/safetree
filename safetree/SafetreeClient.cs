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

		public string Login(string username,string password,string district)
		{
			var res=client.Simple(new RestRequest($"https://{district}.xueanquan.com/LoginHandler.ashx?userName={username}&password={password}&checkcode=&type=login&loginType=1", Method.GET));

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

		public void AddRequest(RestRequest req)
		{
			ReqList.Add(req);
			Console.WriteLine($"Request detected: {req.Method.ToString()} {req.Resource}");
		}

		public static SafetreeTask ParseCourse(string HAR)
		{
			string[] Containing =
			{
				"_method=SkillCheckName",
				"_method=TemplateIn2",
				"SubmitTest",
				"Step=1",

			};

			var har = HAR.FromJSON<HARFile>();
			var reqs = from p in har.log.entries select p.request;
			
			var t = new SafetreeTask();

			foreach(var req in reqs)
			{
				foreach(var con in Containing)
				{
					if (req.url.Contains(con))
					{
						t.AddRequest(req.ToRequest());


						if(con== "Step=1")
						{
							var clone = req;
							clone.url=clone.url.Replace("Step=1", "Step=2");
							t.AddRequest(clone.ToRequest());
						}
						break;
					}
				}
			}
			/*
			try
			{
				var H_CheckCourse = (from p in reqs where p.method == "POST" && p.url.Contains("_method=SkillCheckName") select p).ToArray()[0];
				var Req_CheckCourse = H_CheckCourse.ToRequest();
				t.ReqList.Add(Req_CheckCourse);
			}
			catch { }

			try
			{
				var H_Exercise = (from p in reqs where p.method == "POST" && p.url.Contains("_method=TemplateIn2") select p).ToArray()[0];
				var Req_Exercise = H_Exercise.ToRequest();
				t.ReqList.Add(Req_Exercise);
			}
			catch { }

			try
			{
				var H_SubmitTest=(from p in reqs where p.method == "POST" && p.url.Contains("SubmitTest") select p).ToArray()[0];
				t.ReqList.Add(H_SubmitTest.ToRequest());
			}
			catch { }

			try
			{
				var H_step1 = (from p in reqs where p.method == "GET" && p.url.Contains("Step=1") select p).ToArray()[0];
				t.ReqList.Add(H_step1.ToRequest());

				H_step1.url = H_step1.url.Replace("Step=1", "Step=2");
				t.ReqList.Add(H_step1.ToRequest());
			}
			catch { }
			*/

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
