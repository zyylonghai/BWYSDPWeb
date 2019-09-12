using BWYSDPWeb.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllAuthorityControllers
{
    public class AuthorityController : DataBaseController
    {
        // GET: Authority
        public ActionResult Index()
        {
            return View();
        }
    }
}