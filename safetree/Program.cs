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
			Console.WriteLine();

			if (args.Length == 0)
			{
				Colorful.LogError("命令行参数错误");
				return;
			}

			var taskname = args[0];

			var taskdir = $"./{taskname}/";

			var taskList = new List<SafetreeTask>();

			try
			{

				foreach (var f in Directory.GetFiles(taskdir, "*.har"))
				{
					//todo: course activity
					var t = SafetreeTask.ParseCourse(File.ReadAllText(f));
					taskList.Add(t);
				}

				var infotxt = Directory.GetFiles(taskdir, "$*.txt")[0];




				foreach (var a in File.ReadAllLines(infotxt))
				{
					Console.WriteLine();
					var strs = a.Replace(" ", "").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					var username = "";
					var password = "";

					try
					{
						username = strs[0];
						if (strs.Length <= 1)
							password = "123456";
						else
							password = strs[1];
					}
					catch(Exception)
					{
						Colorful.LogWarning($"处理账号中的某一行发生错误：'{a}' ");
						continue;
					}

					var client = new SafetreeClient();
					var district = new FileInfo(infotxt).Name.TrimStart(new char[] { '$' }).Replace(".txt", "");

					var LoginRes = client.Login(username, password, district );

					if (LoginRes)
					{
						Colorful.LogSuccess($"登陆成功: {username}");
					}
					else
					{
						Colorful.LogWarning($"登陆失败: {username}");
						continue;
					}

					foreach (var t in taskList)
					{
						var ret = client.DoCourse(t);
						Colorful.WriteLine($"#Response: {ret.Substring(0, Math.Min(80, ret.Length))}"); 
					}
				}
			}
			catch (Exception ee)
			{
				Colorful.LogWarning(ee.Message);
				return;
			}

		}



	}
}
