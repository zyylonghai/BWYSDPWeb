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
            #region 
            //this.Tables[0].Columns["CheckDT"].DataType = typeof(Date);
            //DataRow row = this.LibTables[0].Tables[0].NewRow();
            //row["BillNo"] = "T201903160003";
            //row["Checker"] = "zyy33";
            //row["CheckDT"] = new Date { value = DateTime.Now.ToString() };
            //row["Qty"] = 10;
            ////row["billStatus"] = 2;
            ////foreach (DataRow dr in this.LibTables[0].Tables[0].Rows)
            ////{

            ////}
            //this.LibTables[0].Tables[0].Rows.Add(row);
            //row["Qty"] = 10;
            //row["Checker"] = "zyy653";
            //row["CheckDT"] = new Date { value = DateTime.Now.ToString() };

            //row = this.LibTables[0].Tables[1].NewRow();
            //row["BillNo"] = "T201903160003";
            //row["RowNo"] = 1;
            //row["check1"] = "部位1";
            //row["check2"] = "不为1";
            //this.LibTables[0].Tables[1].Rows.Add(row);

            //row = this.LibTables[0].Tables[1].NewRow();
            //row["BillNo"] = "T201903160003";
            //row["RowNo"] = 2;
            //row["check1"] = "部位2";
            //row["check2"] = "不为2";
            //this.LibTables[0].Tables[1].Rows.Add(row);

            //row = this.LibTables[1].Tables[0].NewRow();
            //row["BillNo"] = "T201903160003";
            ////row["RowNo"] = 1;
            //row["yingdu"] = "jjjj";
            //row["naiwendu"] = "dsfsd";
            //row["xingzhuang"] = "dlkfj";
            //this.LibTables[1].Tables[0].Rows.Add(row);

            //row = this.LibTables[1].Tables[0].NewRow();
            //row["BillNo"] = "T201903160003";
            ////row["RowNo"] = 1;
            //row["yingdu"] = "jjjj2";
            //row["naiwendu"] = "dsfsd2";
            //row["xingzhuang"] = "dlkfj2";
            //this.LibTables[1].Tables[0].Rows.Add(row);

            //this.LibTables[1].Tables[0].AcceptChanges();
            //this.LibTables[1].Tables[0].Rows[1].Delete();


            //for (int n = 1; n < 5; n++)
            //{
            //    row = this.LibTables[1].Tables[0].NewRow();
            //    row["BillNo"] = "T201903160003";
            //    //row["RowNo"] = n;
            //    row["yingdu"] = "jjjj2";
            //    row["naiwendu"] = "dsfsd2";
            //    row["xingzhuang"] = "dlkfj2";
            //    this.LibTables[1].Tables[0].Rows.Add(row);
            //}
            //string a = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff");
            //for (int i = 1; i < 20; i++)
            //{
            //    row = this.LibTables[1].Tables[1].NewRow();
            //    row["BillNo"] = "T201903160003";
            //    row["RowNo"] = i;
            //    row["FromRowNo"] = 1;
            //    row["testfield"] = string.Format("测试字段{0}", i);
            //    this.LibTables[1].Tables[1].Rows.Add(row);
            //}
            #endregion
            string b = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff");
            //row = this.LibTables[1].Tables[1].NewRow();
            //row["BillNo"] = "T201903160001";
            //row["RowNo"] = 2;
            //row["FromRowNo"] = 1;
            //row["testfield"] = "测试字段2";
            //this.LibTables[1].Tables[1].Rows.Add(row);
            //row["check1"] = "jjjjjjjjj";
            //string str = JsonConvert.SerializeObject(this.LibTables[0].Tables[1]);
            //DataTable dr2 = JsonConvert.DeserializeObject<DataTable>(str);
            //return LibJson();
            //var mdr= this.LibTables[0].Tables[0].NewRow();
            //mdr.BillNo = "T20200001";
            //mdr.remark = "zyytest";
            //var dr = this.LibTables[0].Tables[1].NewRow();
            //dr.check1 = "skdfj";
            //dr.check2 = "chedk";
            var tbobj = this.LibTables[0].Tables[0];
            tbobj.DataTable.Rows[0][tbobj.Columns.ID] = "T20102102";
            //this.ThrowErrorException("zyylonghai错误猜测是");
            //this.AddMessage("测试错误提示", SDPCRL.CORE.LibMessageType.Error);
            //this.AddMessage("测试错误提示2", SDPCRL.CORE.LibMessageType.Error);
        }
        protected override void GetGridDataExt(string gridid, DataTable dt)
        {
            base.GetGridDataExt(gridid, dt);
            //this.ThrowErrorException("抛出异常测试.....");
            //DataRow dr = dt.NewRow();
            ////dr["RowNo"] = "1";
            ////dr["BillNo"] = "T201903160001";
            ////dr["yingdu"] = "zyy";
            ////dr["naiwendu"] = "888";
            ////dr["xingzhuang"] = "skdjf";
            ////dr["midu"] = "kdkkdkdkk";
            //dt.Rows.Add(dr);
        }

        protected override void UpdateTableRow(string gridid, DataRow row, string cmd)
        {
            base.UpdateTableRow(gridid, row, cmd);
            //this.ThrowErrorException("抛出异常测试");
            if (string.Compare(gridid, "GridGroup1") == 0 && cmd== "Add")
            {
                var tbobj = this.LibTables[1].Tables[0];
                row[tbobj.Columns.yingdu] = "zyylonghai";
                row[tbobj.Columns.xingzhuang] = "88888888";
            }
        }

        public ActionResult Test(string staffid)
        {

            int i = 0;
            this.ThrowErrorException("异常测试");
            //string a = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            //int n = 1;
            //while (i < 10)
            //{
            //    //DataRow row = this.LibTables[0].Tables[0].NewRow();
            //    //row["BillNo"] = "T201903160001";
            //    //row["Checker"] = "test3";
            //    //row["CheckDT"] = new Date("2018-2-12");
            //    //row["Qty"] = i++;
            //    //this.LibTables[0].Tables[0].Rows.Add(row);

            //    DataRow dr = this.LibTables[1].Tables[0].NewRow();
            //    dr["RowNo"] = i++;
            //    dr["BillNo"] = "T201903160001";
            //    dr["yingdu"] = "9563";
            //    dr["naiwendu"] = "555";
            //    dr["xingzhuang"] = "777";
            //    dr["midu"] = "666";
            //    this.LibTables[1].Tables[0].Rows.Add(dr);

            //}
            //string b = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");


            //this.LibTables[0].Tables[1].Rows[0]["check1"] = "修改后部位1";
            //this.LibTables[0].Tables[1].Rows[0]["check2"] = "修改后不为1";
            //this.LibTables[0].Tables[1].AcceptChanges();

            //this.LibTables[0].Tables[1].Rows[1].Delete();
            return Json(new { message = "", Flag = 0 }, JsonRequestBehavior.AllowGet);
        }

        protected override void SetSearchFieldExt(List<SearchConditionField> fields, string fieldNm, int flag)
        {
            base.SetSearchFieldExt(fields,fieldNm , flag);
            //SearchConditionField[] rmvs = { };
            //fields.Where(i => i.TableNm != "CheckBill").ToArray().CopyTo(rmvs, 0);
            //foreach (var item in rmvs)
            //{
            //    fields.Remove(item);
            //}
            //foreach (var f in fields)
            //{
            //    if (f.TableNm != "CheckBill")
            //    {
            //        f.IsCondition = false;
            //    }
            //}
        }
        protected override void BindSmodalDataExt(DataTable currpagedata,int flag,string fieldnm)
        {
            base.BindSmodalDataExt(currpagedata,flag,fieldnm);
            //DataTable dt = currpagedata.Copy();
            //List<string> exists = new List<string>();
            //DataRow[] drs = null;
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (exists.Contains(row["BillNo"]))
            //    {
            //        drs = currpagedata.Select(string.Format("BillNo='{0}'", row["BillNo"]));
            //        if (drs != null && drs.Length > 0)
            //            currpagedata.Rows.Remove(drs[0]);
            //        //currpagedata.Rows.RemoveAt(dt.Rows.IndexOf(row));
            //        continue;
            //    }
            //    exists.Add(row["BillNo"].ToString());
            //}
            //this.AddMessage("slkdjfdskljfkldsjfkldsjfkldsjfkljdkslfjklsd",SDPCRL.CORE.LibMessageType.Warning);
        }

        protected override void AfterSave()
        {
            base.AfterSave();
            //this.AddMessage("sdfjsdkjfkdj", SDPCRL.CORE.LibMessageType.Error);
        }
        protected override void BeforeSave()
        {
            base.BeforeSave();
            //this.AddMessage("beforesave的错误信息", SDPCRL.CORE.LibMessageType.Error);
            //this.AddMessage("beforesave的警告信息", SDPCRL.CORE.LibMessageType.Warning);
        }

    }
}