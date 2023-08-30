using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace WebApiCodeGenLib
{
    public static class JsonHelper
    {

        public static dynamic GetDataFromJson(string json)
        {
            json = json.Replace("\\\"", "\"");
            return JsonConvert.DeserializeObject(json.ToString());
        }

    }
}