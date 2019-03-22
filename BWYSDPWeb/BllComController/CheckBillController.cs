using BWYSDPWeb.Models;
using Newtonsoft.Json;
using SDPCRL.COM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllComController
{
    public class CheckBillController : ComController
    {

        protected override void PageLoad()
        {
            //this.Tables[0].Columns["CheckDT"].DataType = typeof(Date);
            DataRow row = this.LibTables[0].Tables[0].NewRow();
            row["BillNo"] = "T201903160001";
            row["Checker"] = "zyy33";
            row["CheckDT"] = new Date("2018-2-9");
            row["Qty"] = 10;
            foreach (DataRow dr in this.LibTables[0].Tables[0].Rows)
            {

            }
            this.LibTables[0].Tables[0].Rows.Add(row);
            row["Qty"] = 110;
            row["Checker"] = "zyy653";
            row["CheckDT"] = new Date("2018-3-9");

            row = this.LibTables[0].Tables[1].NewRow();
            row["BillNo"] = "T201903160001";
            row["RowNo"] = 1;
            row["check1"] = "部位1";
            row["check2"] = "不为1";
            this.LibTables[0].Tables[1].Rows.Add(row);

            row = this.LibTables[0].Tables[1].NewRow();
            row["BillNo"] = "T201903160001";
            row["RowNo"] = 2;
            row["check1"] = "部位2";
            row["check2"] = "不为2";
            this.LibTables[0].Tables[1].Rows.Add(row);
            row["check1"] = "jjjjjjjjj";
            string str = JsonConvert.SerializeObject(this.LibTables[0].Tables[1]);
            DataTable dr2 = JsonConvert.DeserializeObject<DataTable>(str);
            //return LibJson();
        }
        protected override void GetGridDataExt(string gridid, DataTable dt)
        {
            base.GetGridDataExt(gridid, dt);
            DataRow dr = dt.NewRow();
            dr["BillNo"] = "T201903160001";
            dr["yingdu"] = "zyy";
            dr["naiwendu"] = "888";
            dr["xingzhuang"] = "skdjf";
            dr["midu"] = "kdkkdkdkk";
            dt.Rows.Add(dr);
        }

        public ActionResult Test(string staffid)
        {
            return Json(new {message="",Flag=0 }, JsonRequestBehavior.AllowGet);
        }

    }
}