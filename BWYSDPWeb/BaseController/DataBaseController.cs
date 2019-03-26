using Bll;
using BWYSDPWeb.Com;
using BWYSDPWeb.Models;
using Newtonsoft.Json;
using SDPCRL.COM.ModelManager;
using SDPCRL.COM.ModelManager.FormTemplate;
using SDPCRL.CORE;
using SDPCRL.CORE.FileUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

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

            m = new Menu();
            m.MenuId = "0103";
            m.MenuName = "公共功能";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "010301";
            m.MenuName = "web测试";
            m.ProgId = "webceshi";
            m.Package = "com";
            m.PmenuId = "0103";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "010302";
            m.MenuName = "检验单";
            m.ProgId = "CheckBill";
            m.Package = "com";
            m.PmenuId = "0103";
            mdata.Add(m);

            return Json(new { Message = "success", data = mdata, Flag = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单跳转功能页
        /// </summary>
        /// <param name="progId">排版模型ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConverToPage(string progId)
        {
            if (this.Request.Url.Segments.Length > 2)
            {
                string packagepath = this.Request.Url.Segments[1];
                if (!string.IsNullOrEmpty(packagepath))
                {
                    this.ProgID = progId;
                    this.Package = packagepath.Replace("/", "");
                    this.AddorUpdateCookies(SysConstManage.PageinfoCookieNm, progId, this.Package);
                    //this.AddorUpdateCookies(SysConstManage.PageinfoCookieNm, SysConstManage .PackageCookieKey, packagepath.Replace("/", ""));
                    FileOperation fileoperation = new FileOperation();

                    fileoperation.FilePath = string.Format(@"{0}Views\{1}\{2}.cshtml", Server.MapPath("/").Replace("//", ""), packagepath, progId);
                    if (!fileoperation.ExistsFile())//不存在视图文件,需要创建
                    {
                        LibFormPage formpage = ModelManager.GetModelBypath<LibFormPage>(string.Format(@"{0}Views", Server.MapPath("/").Replace("//", "")), progId, this.Package);
                        if (formpage != null)
                        {
                            #region 旧代码
                            //StringBuilder html = new StringBuilder();
                            //html.Append("<div class=\"container-fluid\">");
                            ////页面内容
                            //html.Append("<div class='panel panel-default'>");
                            //html.Append("<div class='panel-heading'>" + progId + "</div>");
                            //html.Append("<div class='panel-body'>");

                            //html.Append("</div>");
                            //html.Append("</div>");
                            //html.Append("</div>");
                            //fileoperation.WritText(html.ToString());
                            #endregion

                            #region 根据排版模型对象 创建功能视图。
                            ViewFactory factory = new ViewFactory(progId);
                            factory.ControlClassNm = formpage.ControlClassNm;
                            factory.DSID = formpage.DSID;
                            factory.Package = this.Package;
                            factory.BeginPage(formpage.FormName);
                            factory.CreateBody();
                            factory.CreateForm();
                            if (formpage.FormGroups != null)
                            {
                                foreach (LibFormGroup formg in formpage.FormGroups)
                                {
                                    factory.CreatePanelGroup(formg.FormGroupDisplayNm);
                                    if (formg.FmGroupFields != null && formg.FmGroupFields.Count > 0)
                                    {
                                        factory.AddFormGroupFields(formg.FmGroupFields);
                                    }
                                }
                            }
                            if (formpage.GridGroups != null)
                            {
                                foreach (LibGridGroup grid in formpage.GridGroups)
                                {
                                    if (grid.GdGroupFields != null)
                                    {
                                        factory.CreateGridGroup(grid);
                                    }
                                }
                            }
                            factory.EndPage();

                            fileoperation.WritText(factory.PageHtml);
                            #endregion

                            #region 保存Formgroupfields
                            string a = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                            //Bll.SQLiteHelp sQLiteHelp = new Bll.SQLiteHelp("TempData");
                            TempHelp sQLiteHelp = new TempHelp("TempData");
                            string b = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                            List<string> commandtextlst = new List<string>();
                            //commandtextlst.Append("begin;");
                            foreach (KeyValuePair<string, List<string>> item in factory.Formfields)
                            {
                                foreach (string f in item.Value)
                                {
                                    commandtextlst.Add(string.Format("insert into formfields values('{0}','{1}','{2}')", progId, item.Key, f));
                                }
                            }

                            //for (int i = 0; i < 1000; i++)
                            //{
                            //    commandtextlst.Add(string.Format("insert into formfields values('{0}','{1}','{2}')", progId,"test",string.Format("field{0}",i)));
                            //}

                            //commandtextlst.Append("commit;");
                            string c = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                            sQLiteHelp.Delete(string.Format("delete from formfields where progid='{0}'", progId));
                            sQLiteHelp.Update(commandtextlst);
                            string d = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                            #endregion
                        }
                        else
                        {
                            return View("NotFindPage");
                        }

                    }
                    //Server.MapPath("/")
                }
            }
            return View(progId);
        }

        [HttpPost]
        public virtual ActionResult BasePageLoad()
        {
            this.OperatAction = OperatAction.Add;
            Session[SysConstManage.OperateAction] = this.OperatAction;
            this.CreateTableSchema();
            #region delete temp data(重新加载页面，需清除temp表中的session数据)
            //Bll.DelegateFactory df = new Bll.DelegateFactory();
            //df.ClearTempDataByProgid(System.Web.HttpContext.Current.Session.SessionID, this.ProgID);

            TempHelp sQLiteHelp = new TempHelp("TempData");
            sQLiteHelp.ClearTempData(System.Web.HttpContext.Current.Session.SessionID, this.ProgID);

            #endregion
            PageLoad();
            return LibJson();
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

        public ActionResult Save()
        {
            BeforeSave();
            this.LibTables[0].Tables[0].Rows[0].AcceptChanges();
            this.LibTables[0].Tables[0].Rows[0]["Checker"] ="66";
            string a = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            object resut2 = this.ExecuteMethod("Test", "longhaibangshan", 8888);
            object resut= this.ExecuteSaveMethod("Save", this.LibTables);
            string b = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            AfterSave();
            //return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("ConverToPage", this.Package, new { progId = this.ProgID });
        }

        public string BindTableData(string gridid, string deftb, int page, int rows, string Mobile)
        {
            #region 旧代码
            //DataTable dt = new DataTable();

            //DataColumn col = new DataColumn();
            //col.ColumnName = "Name";
            //col.Caption = "姓名";
            //dt.Columns.Add(col);

            //col = new DataColumn();
            //col.ColumnName = "Mobile";
            //col.Caption = "手机";
            //dt.Columns.Add(col);

            //col = new DataColumn();
            //col.ColumnName = "Email";
            //col.Caption = "邮箱";
            //dt.Columns.Add(col);

            //col = new DataColumn();
            //col.ColumnName = "Gender";
            //col.Caption = "性别";
            //dt.Columns.Add(col);

            //col = new DataColumn();
            //col.ColumnName = "Age";
            //col.Caption = "年龄";
            //dt.Columns.Add(col);

            //dt.PrimaryKey =new DataColumn[] { dt.Columns["Name"] };

            //DataTable dt2 = new DataTable();

            //col = new DataColumn();
            //col.ColumnName = "Name";
            //col.Caption = "姓名";
            //dt2.Columns.Add(col);

            //col = new DataColumn();
            //col.ColumnName = "extral1";
            //col.Caption = "额外字段1";
            //dt2.Columns.Add(col);

            //col = new DataColumn();
            //col.ColumnName = "extral2";
            //col.Caption = "额外字段2";
            //dt2.Columns.Add(col);

            //dt2.PrimaryKey = new DataColumn[] { dt2.Columns["Name"] };

            //DataRow r = dt2.NewRow();
            //r["Name"] = "Name0";
            //r["extral1"] = "extral1";
            //r["extral2"] = "extral2";
            //dt2.Rows.Add(r);


            //for (int i = 0; i < 20; i++)
            //{
            //    DataRow row = dt.NewRow();
            //    row["Name"] = "Name" + i.ToString();
            //    row["Mobile"] = "123456789";
            //    row["Email"] = "896501235@qq.com";
            //    row["Gender"] = "男";
            //    row["Age"] = i;
            //    dt.Rows.Add(row);
            //}

            //dt.Merge(dt2, false);

            ////List<TestInfo> testlist = new List<TestInfo>();
            ////for (int n = 0; n < 6; n++)
            ////{
            ////    TestInfo t = new TestInfo();
            ////    t.Name = "test" + n.ToString();
            ////    t.Mobile = "123456789";
            ////    t.Email = "869650231@qq.com";
            ////    t.Gender = "nv";
            ////    t.Age = n;
            ////    testlist.Add(t);
            ////}
            #endregion
            var table = this.LibTables.FirstOrDefault(i => i.Name == deftb);
            if (table == null) { var result2 = new { total = 0, rows =DBNull.Value }; return JsonConvert.SerializeObject(result2); }
                DataTable dt = table.Tables[0].Copy();
                //foreach (DataTable item in table.Tables)
                //{
                //    dt.Merge(item, false);
                //}
                for (int i = 1; i < table.Tables.Length; i++)
                {
                    dt.Merge(table.Tables[i], false);
                }

                GetGridDataExt(gridid, dt);
                //DataTable dt2 = dt.Copy();
                //DataRow[] drs = dt2.Select(string.Format("Age>={0} and Age<={1}",rows*(page -1),rows*page));
                //foreach (DataRow dr in drs)
                //{
                //    dt2.Rows.Remove(dr);
                //}
                DataTable resultdt = dt.Clone();
                for (int index = (page - 1) * rows; index < page * rows; index++)
                {
                    if (index >= dt.Rows.Count) break;
                    resultdt.ImportRow(dt.Rows[index]);
                }
            var result = new { total = dt.Rows.Count, rows = resultdt };

            //return Json(new { total = testlist.Count , rows = testlist }, JsonRequestBehavior.AllowGet);
            return JsonConvert.SerializeObject(result);
        }

        #region 受保护方法
        protected virtual void GetGridDataExt(string gridid, DataTable dt)
        {

        }

        protected virtual void PageLoad()
        {
        }

        protected virtual void BeforeSave()
        {

        }

        protected virtual void AfterSave()
        {

        }
        #endregion
    }
}