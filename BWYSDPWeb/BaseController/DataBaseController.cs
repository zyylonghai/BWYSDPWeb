using BWYSDPWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDPCRL.CORE.FileUtils;
using System.Text;
using System.Data;
using Newtonsoft.Json;

namespace BWYSDPWeb.BaseController
{
    public class DataBaseController : BaseController
    {
        public DataBaseController()
        {

        }

        [HttpGet]
        public ActionResult LoadMenus()
        {
            List<Menu> mdata = new List<Menu>();
            Menu m = new Menu();
            //m.MenuId = "0001";
            //m.MenuName = "主页";
            //m.ProgId = "Index";
            //m.Package = "Home";
            //mdata.Add(m);

            m = new Menu();
            m.MenuId = "0101";
            m.MenuName = "供应链管理";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "010101";
            m.MenuName = "销售订单";
            m.ProgId = "SaleOrder";
            m.PmenuId = "0101";
            m.Package = "SCM";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "010102";
            m.MenuName = "采购订单";
            m.ProgId = "PurchaseOrder";
            m.Package = "SCM";
            m.PmenuId = "0101";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "010103";
            m.MenuName = "发货单";
            m.ProgId = "ShipOrder";
            m.Package = "SCM";
            m.PmenuId = "0101";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "0102";
            m.MenuName = "库存管理";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "010201";
            m.MenuName = "库存报表";
            m.ProgId = "stockbaobiao";
            m.Package = "Stock";
            m.PmenuId = "0102";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "010202";
            m.MenuName = "库存调整";
            m.ProgId = "stockpage";
            m.Package = "Stock";
            m.PmenuId = "0102";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "010203";
            m.MenuName = "库存调整2";
            m.ProgId = "stockpage2";
            m.Package = "Stock";
            m.PmenuId = "0102";
            mdata.Add(m);

            return Json(new { Message = "success", data = mdata, Flag = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单跳转功能页
        /// </summary>
        /// <param name="progId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConverToPage(string progId)
        {
            if (this.Request.Url.Segments.Length > 2)
            {
                string packagepath = this.Request.Url.Segments[1];
                if (!string.IsNullOrEmpty(packagepath))
                {
                    FileOperation fileoperation = new FileOperation();
                    fileoperation.FilePath = string.Format(@"{0}Views\{1}\{2}.cshtml", Server.MapPath("/").Replace("//", ""), packagepath, progId);

                    if (!fileoperation.ExistsFile())//不存在视图文件,需要创建
                    {
                        StringBuilder html = new StringBuilder();
                        html.Append("<div class=\"container-fluid\">");
                        //页面内容
                          html.Append("<div class='panel panel-default'>");
                            html.Append("<div class='panel-heading'>"+progId+"</div>");
                              html.Append("<div class='panel-body'>");
                             
                              html.Append("</div>");
                            html.Append("</div>");
                          html.Append("</div>");
                        html.Append("</div>");
                        fileoperation.WritText(html.ToString());
                        
                    }
                    //Server.MapPath("/")
                }
            }
            return View(progId);
        }
        /// <summary>
        /// 功能搜索
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchFunc(string q)
        {
            return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoSave()
        {
            return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }

        public string BindTableData(string progid,int page,int rows)
        {
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn();
            col.ColumnName = "Name";
            col.Caption = "姓名";
            dt.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "Mobile";
            col.Caption = "手机";
            dt.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "Email";
            col.Caption = "邮箱";
            dt.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "Gender";
            col.Caption = "性别";
            dt.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "Age";
            col.Caption = "年龄";
            dt.Columns.Add(col);

            for (int i = 0; i < 20; i++)
            {
                DataRow row = dt.NewRow();
                row["Name"] = "Name" + i.ToString();
                row["Mobile"] = "123456789";
                row["Email"] = "896501235@qq.com";
                row["Gender"] = "男";
                row["Age"] = i;
                dt.Rows.Add(row);
            }
            List<TestInfo> testlist = new List<TestInfo>();
            for (int n = 0; n < 6; n++)
            {
                TestInfo t = new TestInfo();
                t.Name = "test" + n.ToString();
                t.Mobile = "123456789";
                t.Email = "869650231@qq.com";
                t.Gender = "nv";
                t.Age = n;
                testlist.Add(t);
            }
            GetTableDataExt(dt);
            DataTable dt2 = dt.Copy();
            //DataRow[] drs = dt2.Select(string.Format("Age>={0} and Age<={1}",rows*(page -1),rows*page));
            //foreach (DataRow dr in drs)
            //{
            //    dt2.Rows.Remove(dr);
            //}
            var result = new { total=dt.Rows.Count ,rows=dt2};
            //return Json(new { total = testlist.Count , rows = testlist }, JsonRequestBehavior.AllowGet);
            return JsonConvert.SerializeObject(result);
        }

        protected virtual void GetTableDataExt(DataTable dt)
        {

        }

    }
}