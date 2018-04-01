using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LangExt
{
	static class JsonExt
	{
		public static string ToJSON(this object p)
		{
			var jsonStr = JsonConvert.SerializeObject(p);
			return jsonStr;
		}

		public static T FromJSON<T>(this string p)
		{
			var item = JsonConvert.DeserializeObject<T>(p);
			return item;
		}

	}
}
