using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    /// <summary>转单信息</summary>
    public class TransInfo
    {

        public string TargetPackage { get; set; }
        public string TargetProgId { get; set; }

        public string TransModelId { get; set; }
    }
}