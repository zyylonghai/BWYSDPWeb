using BWYSDPWeb.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllComController
{
    public class ComController : DataBaseController
    {
        // GET: Com
        public ActionResult Index()
        {
            return View();
        }
    }
}