using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class ErrorObject
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string Stack { get; set; }
    }
}