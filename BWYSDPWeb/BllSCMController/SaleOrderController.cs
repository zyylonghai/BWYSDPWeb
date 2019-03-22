using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllSCMController
{
    public class SaleOrderController : SCMController
    {
        // GET: SaleOrder

        protected override void GetGridDataExt(string gridid, DataTable dt)
        {
            base.GetGridDataExt(gridid, dt);
        }


        public void TestAjax()
        {
            //return Json(new {data="test" }, JsonRequestBehavior.AllowGet);
        }
    }
}