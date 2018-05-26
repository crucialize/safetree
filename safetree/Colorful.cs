using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LangExt
{
	static class Colorful
	{

		public static string WorkPath
		{
			get
			{
				return Environment.CurrentDirectory;
			}
		}

		public static string FilePath
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		private static void _Write(string Text, ConsoleColor TextColor = ConsoleColor.White)
		{
			var last = Console.ForegroundColor;
			Console.ForegroundColor = TextColor;
			Console.Write(Text);
			Console.ForegroundColor = last;
		}

		public static void WriteLine(string text)
		{
			Write(text);
			Console.WriteLine();
		}

		public static void Write(string Text)
		{
			Regex rgx = new Regex("([rgy])\\{(.*?)\\}", RegexOptions.Singleline);

			var results = rgx.Matches(Text);

			var mark = 0;
			foreach (Match m in results)
			{
				_Write(Text.Substring(mark, m.Index - mark));
				mark = m.Index + m.Length;

				string color = m.Groups[1].Value;
				string coloredText = m.Groups[2].Value;
				ConsoleColor cc;
				switch (color)
				{
					case "r":
						cc = ConsoleColor.Red;
						break;
					case "g":
						cc = ConsoleColor.Green;
						break;
					case "y":
						cc = ConsoleColor.Yellow;
						break;
					default:
						cc = ConsoleColor.White;
						break;
				}
				_Write(coloredText, cc);

			}

			_Write(Text.Substring(mark));
		}

		public static void BackSpace()
		{
			Console.Write('\u0008');
		}

		public static void BackSpace(int count)
		{
			for (int i = 1; i <= count; i++)
				BackSpace();
		}

		public static void LogInfo(string text)
		{
			WriteLine($"[Info] {text}");
		}

		public static void LogSuccess(string text)
		{
			WriteLine($"g{{[Success]}} {text}");
		}

		public static void LogWarning(string text)
		{
			WriteLine($"y{{[Warning]}} {text}");
		}

		public static void LogError(string text)
		{
			WriteLine($"r{{[Error]}} {text}");
		}

	}
}
