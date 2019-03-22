using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class PageInfo
    {
        public string ProgId { get; set; }
        public string Package { get; set; }
    }

    public class FormFields
    {
        public string ProgId { get; set; }
        public string FieldNm { get; set; }
        public object FieldValue { get; set; }
    }
}