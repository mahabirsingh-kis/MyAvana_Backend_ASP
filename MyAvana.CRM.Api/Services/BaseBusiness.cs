using MyAvana.CRM.Api.Contract;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
	public class BaseBusiness : IBaseBusiness
	{
		
		public JObject AddDataOnJson(string message, string result, object jsonValue, bool appendData = true)
		{
			JObject json = new JObject();
			json.Add("message", message);
			json.Add("result", result);
			if (appendData)
			{
				if (result == "0")
				{
				}
				else
				{
					json.Add("data", JToken.FromObject(jsonValue));
				}
			}
			return json;
		}
	}
}
