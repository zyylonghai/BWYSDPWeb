using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class TableObj
    {
        public string grid { get; set; }
        public Newtonsoft.Json.Linq.JArray addrows { get; set; }
        public Newtonsoft.Json.Linq.JArray editRows { get; set; }
        public Newtonsoft.Json.Linq.JArray removrows { get; set; }
    }
}