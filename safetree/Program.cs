using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using LangExt;
using System.IO;

namespace safetree
{
	class Program
	{
		static void Main(string[] args)
		{
			var taskname = args[0];

			var taskdir = $"./{taskname}/";

			var taskList = new List<SafetreeTask>();

			foreach(var f in Directory.GetFiles(taskdir, "*.har"))
			{
				//todo: course activity
				var t = SafetreeTask.ParseCourse(File.ReadAllText(f));
				taskList.Add(t);
			}

			var infotxt = Directory.GetFiles(taskdir, "*.txt")[0];
			foreach (var a in File.ReadAllLines(infotxt))
			{
				var username = a.Replace(" ", "");
				var client = new SafetreeClient();
				var LoginRes = client.Login(username, "123456",new FileInfo(infotxt).Name.Replace(".txt",""));

				foreach(var t in taskList)
				{
					var ret = client.DoCourse(t);
					WriteLine($"{a}:{ret}");
				}
			}

		}



	}
}
