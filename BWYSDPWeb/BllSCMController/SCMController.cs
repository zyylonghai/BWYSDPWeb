﻿using BWYSDPWeb.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllSCMController
{
    public class SCMController :DataBaseController
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
    }
}