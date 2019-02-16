using BWYSDPWeb.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllStockController
{
    public class StockController : DataBaseController
    {
        // GET: Stock
        public ActionResult Index()
        {
            return View();
        }
    }
}