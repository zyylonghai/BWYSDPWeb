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

        protected override void GetTableDataExt(DataTable dt)
        {
            base.GetTableDataExt(dt);
        }
    }
}