using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllComController
{
    public class CheckBillController : ComController
    {
        // GET: CheckBill
        public override ActionResult PageLoad()
        {
            base.PageLoad();
            DataRow row = this.Tables[0].NewRow();
            row["BillNo"] = "T201903160001";
            this.Tables[0].Rows.Add(row);
            return LibJson();
        }
    }
}