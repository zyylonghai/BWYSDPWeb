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
            DataRow dr = dt.NewRow();
            dr["Name"] = "zhengyy";
            dr["Mobile"] = "12355685954";
            dr["Email"] = "856253265@qq.com";
            dt.Rows.Add(dr);
            base.GetGridDataExt(gridid, dt);
        }


        public void TestAjax()
        {
            //return Json(new {data="test" }, JsonRequestBehavior.AllowGet);
        }
    }
}