using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ProgViewModel
{
    public class DataLogViewModel
    {
        public List<DataLogObj> DataLogObjs { get; set; }
        public string DSID { get; set; }

        public DataLogViewModel()
        {
            DataLogObjs = new List<DataLogObj>();
        }

    }
    public class DataLogObj
    {
        public string TableNm { get; set; }
        public ColInfo[] cols;

        public List<JObject> datas;
        //public List<JObject> JObjects;

        public string GetdataJson()
        {
            return Newtonsoft.Json .JsonConvert.SerializeObject(this.datas);
        }

    }
    public class ColInfo
    {
        public string Nm { get; set; }
        public Type Type { get; set; }
    }
}
