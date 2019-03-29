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
            //foreach (DataRow dr in this.LibTables[0].Tables[0].Rows)
            //{

            //}
            this.LibTables[0].Tables[0].Rows.Add(row);
            row["Qty"] = 10;
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
            //row["check1"] = "jjjjjjjjj";
            //string str = JsonConvert.SerializeObject(this.LibTables[0].Tables[1]);
            //DataTable dr2 = JsonConvert.DeserializeObject<DataTable>(str);
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
            dr["midu"]="kdkkdkdkk";
            dt.Rows.Add(dr);
        }

        public ActionResult Test(string staffid)
        {

            int i = 0;
            //string a = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            //int n = 1;
            while (i < 10)
            {
                //DataRow row = this.LibTables[0].Tables[0].NewRow();
                //row["BillNo"] = "T201903160001";
                //row["Checker"] = "test3";
                //row["CheckDT"] = new Date("2018-2-12");
                //row["Qty"] = i++;
                //this.LibTables[0].Tables[0].Rows.Add(row);

                DataRow dr = this.LibTables[1].Tables[0].NewRow();
                dr["RowNo"] = i++;
                dr["BillNo"] = "T201903160001";
                dr["yingdu"] = "9563";
                dr["naiwendu"] = "555";
                dr["xingzhuang"] = "777";
                dr["midu"] = "666";
                this.LibTables[1].Tables[0].Rows.Add(dr);

            }
            //string b = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");


            //this.LibTables[0].Tables[1].Rows[0]["check1"] = "修改后部位1";
            //this.LibTables[0].Tables[1].Rows[0]["check2"] = "修改后不为1";
            //this.LibTables[0].Tables[1].AcceptChanges();

            //this.LibTables[0].Tables[1].Rows[1].Delete();
            return Json(new { message = "", Flag = 0 }, JsonRequestBehavior.AllowGet);
        }

    }
}