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
            //byte[] bts = Encoding.Default.GetBytes(querystr);
            querystr = System.Text.RegularExpressions.Regex.Unescape(querystr);
            return JsonConvert.DeserializeObject<QueryParams>(DM5Help.Md5Decrypt(querystr));
        }
        public override string ToString()
        {
            string result = DM5Help.Md5Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            
            //byte[] byts = Convert.FromBase64String(result);
            
            return System.Text.RegularExpressions.Regex.Escape(result);
        }
    }
}