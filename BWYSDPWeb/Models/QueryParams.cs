using Newtonsoft.Json;
using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class QueryParams
    {
        public int flag { get; set; }
        public object data { get; set; }

        public static QueryParams ToqueryParams(string querystr)
        {
            //byte[] bts = Encoding.UTF8.GetBytes(querystr);
            //querystr = Convert.ToBase64String(bts);
            return JsonConvert.DeserializeObject<QueryParams>(DM5Help.MD5Decrypt2(querystr));
        }
        public override string ToString()
        {
            string result = DM5Help.MD5Encrypt2(Newtonsoft.Json.JsonConvert.SerializeObject(this));

            //byte[] byts = Convert.FromBase64String(result);

            return result;
        }
    }
}