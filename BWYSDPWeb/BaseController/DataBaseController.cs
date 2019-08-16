using Bll;
using BWYSDPWeb.Com;
using BWYSDPWeb.Models;
using Com;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SDPCRL.COM;
using SDPCRL.COM.ModelManager;
using SDPCRL.COM.ModelManager.FormTemplate;
using SDPCRL.CORE;
using SDPCRL.CORE.FileUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
                //this.ThrowErrorException("加载异常");
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
                        if (formpage == null) { return View("NotFindPage"); }
                        LibDataSource dataSource = ModelManager.GetModelBypath<LibDataSource>(string.Format(@"{0}Views", Server.MapPath("/").Replace("//", "")), formpage.DSID, this.Package);

                        //if (formpage != null)
                        //{
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
                        factory.LibDataSource = dataSource;
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
                        //}
                        //else
                        //{
                        //    return View("NotFindPage");
                        //}

                    }
                    //Server.MapPath("/")
                }
            }
            return View(progId);
        }

        [HttpPost]
        public virtual ActionResult BasePageLoad()
        {
            //this.OperatAction = OperatAction.Add;
            this.SessionObj.OperateAction = OperatAction.Add;
            //Session[SysConstManage.OperateAction] = this.OperatAction;
            this.CreateTableSchema();
            #region delete temp data(重新加载页面，需清除temp表中的session数据)
            //Bll.DelegateFactory df = new Bll.DelegateFactory();
            //df.ClearTempDataByProgid(System.Web.HttpContext.Current.Session.SessionID, this.ProgID);

            TempHelp sQLiteHelp = new TempHelp("TempData");
            sQLiteHelp.ClearTempData(System.Web.HttpContext.Current.Session.SessionID, this.ProgID);

            #endregion
            //DataRow row = null;
            foreach (var def in this.LibTables)
            {
                foreach (DataTable dt in def.Tables)
                {
                    if ((dt.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties).TableIndex == 0)
                    {
                        //row = dt.NewRow();

                        dt.Rows.Add(dt.NewRow());
                    }
                }
            }
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
            #region 处理前端传回的数据
            var formdata = this.Request.Form;
            string[] array;
            DataTable dt = null;
            foreach (string key in formdata.AllKeys)
            {
                if (key.Contains(SysConstManage.Point))
                {
                    array = key.Split(SysConstManage.Point);
                    if (array.Length < 2)
                        continue;
                    string val = formdata[key];
                    if (dt != null && dt.TableName == array[0])
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataTableHelp.SetColomnValue(dt.Rows[0], array[1], val);
                        }
                        else
                        {
                            if (!LibSysUtils.IsNULLOrEmpty(val))
                            {
                                DataRow row = dt.NewRow();
                                dt.Rows.Add(row);
                                DataTableHelp.SetColomnValue(row, array[1], val);
                            }
                        }
                    }
                    foreach (LibTable libtb in this.LibTables)
                    {
                        string tbnm = array[0];
                        dt = libtb.Tables.FirstOrDefault(i => i.TableName == tbnm);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DataTableHelp.SetColomnValue(dt.Rows[0], array[1], val);
                            }
                            else
                            {
                                if (!LibSysUtils.IsNULLOrEmpty(val))
                                {
                                    DataRow row = dt.NewRow();
                                    dt.Rows.Add(row);
                                    DataTableHelp.SetColomnValue(row, array[1], val);
                                }
                            }
                            break;
                        }
                    }

                }
            }
            #region 处理关联主表的表的主键赋值。
            TableExtendedProperties tbextp = null;
            ColExtendedProperties colextp = null;
            List<DataTable> relatedts = new List<DataTable>();
            DataTable mdt = null;
            foreach (LibTable def in this.LibTables)
            {
                foreach (DataTable table in def.Tables)
                {
                    tbextp = table.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                    if (tbextp.TableIndex == 0) mdt = table;
                    if (tbextp.RelateTableIndex == 0)
                    {
                        if (mdt != null)
                        {
                            foreach (DataRow dr in table.Rows)
                            {
                                foreach (DataColumn col in table.PrimaryKey)
                                {
                                    colextp = col.ExtendedProperties[SysConstManage.ExtProp] as ColExtendedProperties;
                                    DataColumn mcol = mdt.PrimaryKey.FirstOrDefault(i => i.ColumnName == (string.IsNullOrEmpty(colextp.MapPrimarykey) ? col.ColumnName : colextp.MapPrimarykey));
                                    if (mcol != null)
                                    {
                                        dr[col] = mdt.Rows[0][mcol];
                                    }
                                }
                            }
                            continue;
                        }
                        relatedts.Add(table);
                    }
                }
            }
            foreach (DataTable item in relatedts)
            {
                tbextp = item.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                if (mdt != null)
                {
                    foreach (DataRow dr in item.Rows)
                    {
                        foreach (DataColumn col in item.PrimaryKey)
                        {
                            colextp = col.ExtendedProperties[SysConstManage.ExtProp] as ColExtendedProperties;
                            DataColumn mcol = mdt.PrimaryKey.FirstOrDefault(i => i.ColumnName == (string.IsNullOrEmpty(colextp.MapPrimarykey) ? col.ColumnName : colextp.MapPrimarykey));
                            if (mcol != null)
                            {
                                dr[col] = mdt.Rows[0][mcol];
                            }
                        }
                    }
                }
            }
            #endregion
            #endregion
            BeforeSave();
            //TableExtendedProperties tbext = new TableExtendedProperties();
            //string ss= JsonConvert.SerializeObject(tbext);
            //this.LibTables[0].Tables[0].Rows[0].AcceptChanges();
            //this.LibTables[0].Tables[0].Rows[0]["Checker"] ="66";
            //string a = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            //object resut2 = this.ExecuteMethod("Test", "longhaibangshan", 8888);
            DalResult result = (DalResult)this.ExecuteSaveMethod("Save", this.LibTables);
            //string b = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            AfterSave();
            if (result.Messagelist != null && result.Messagelist.Count > 0)
            {
                if (this.SessionObj.MsgforSave == null) this.SessionObj.MsgforSave = new List<LibMessage>();
                this.SessionObj.MsgforSave.AddRange(result.Messagelist);
            }
            if (result.ErrorMsglst != null && result.ErrorMsglst.Count > 0)
            {
                string _msg = string.Empty;
                foreach (var m in result.ErrorMsglst)
                {
                    _msg += m.Message + m.Stack;
                }
                this.ThrowErrorException(_msg);
            }
            //return Content("alert(\"sdfds\")");
            //return Json(new { message = "dfceshi" }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("ConverToPage", this.Package, new { progId = this.ProgID });
        }

        [HttpPost]
        public ActionResult FillAndEdit()
        {
            var parmas = this.Request.Form;
            string tablenm = parmas["tablenm"];
            List<DataTable> dtlist = new List<DataTable>();
            StringBuilder whereformat = new StringBuilder();
            object[] vals = null;
            foreach (LibTable def in this.LibTables)
            {
                dtlist.AddRange(def.Tables);
                //foreach (DataTable dt in def.Tables)
                //{
                //    if (dt.TableName == tablenm)
                //    {
                //        foreach (var col in dt.PrimaryKey)
                //        {

                //        }
                //        break;
                //    }
                //}
            }
            DataTable mast = dtlist.FirstOrDefault(i => i.TableName == tablenm);
            if (mast == null)
            {
                return Json(new { data = "找不到表", flag = 1 }, JsonRequestBehavior.AllowGet);
            }
            vals = new object[mast.PrimaryKey.Length];
            for (int n = 0; n < mast.PrimaryKey.Length; n++)
            {
                if (whereformat.Length > 0)
                {
                    whereformat.Append(" And ");
                }
                whereformat.AppendFormat("{0}={1}", mast.PrimaryKey[n].ColumnName, "{" + n + "}");
                vals[n] = parmas[string.Format("dr[{0}]", mast.PrimaryKey[n].ColumnName)];
            }
            DalResult result = this.ExecuteMethod("InternalFillData", whereformat.ToString(), vals);
            if (result.Messagelist == null || result.Messagelist.Count == 0)
            {
                DataTable[] resultb = (DataTable[])result.Value;
                foreach (var def in this.LibTables)
                {
                    foreach (var tb in def.Tables)
                    {
                        var exist = resultb.FirstOrDefault(i => i.TableName == tb.TableName);
                        if (exist != null)
                        {
                            DataTableHelp dthelp = new DataTableHelp(exist, tb);
                            dthelp.CopyStable();
                        }
                    }
                }
            }
            this.SessionObj.OperateAction = OperatAction.Preview;
            return LibJson();
            //return Json(new { data = "", flag = 0 }, JsonRequestBehavior.AllowGet);
        }

        #region grid 表格操作
        public string BindTableData(string gridid, string deftb, string tableNm, int page, int rows, string Mobile)
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
            //string a = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff");
            var table = this.LibTables.FirstOrDefault(i => i.Name == deftb);
            if (table == null) { var result2 = new { total = 0, rows = DBNull.Value }; return JsonConvert.SerializeObject(result2); }
            #region 旧代码
            //Dictionary<int, DataTable> dic = new Dictionary<int, DataTable>();
            //DataTable mastdt = null;
            //foreach (DataTable item in table.Tables)
            //{
            //    dic.Add(((TableExtendedProperties)item.ExtendedProperties["extProp"]).TableIndex, item);
            //}
            //foreach (DataTable itm in table.Tables)
            //{
            //    if (dic.TryGetValue(((TableExtendedProperties)itm.ExtendedProperties["extProp"]).RelateTableIndex, out mastdt))
            //    {
            //        break;
            //    }
            //}
            //DataTable dt = mastdt.Copy();
            //DataTable dt2 = null;
            //DataColumn[] cols=null;
            //foreach (DataTable d in table.Tables)
            //{
            //    if (d.TableName == mastdt.TableName) continue;
            //    dt2 = d.Copy();
            //    dt2.PrimaryKey = null;
            //    dt.Merge(dt2, false);
            //    //cols = new DataColumn[d.Columns .Count];
            //    //d.Columns.CopyTo(cols, 0);

            //    //foreach (DataColumn c in cols)
            //    //{
            //    //    if (!d.PrimaryKey.Contains(c))
            //    //    {
            //    //        dt.Columns.Add(c);
            //    //    }
            //    //}
            //    //foreach (DataColumn col in d.Columns)
            //    //{
            //    //    if (!d.PrimaryKey.Contains(col))
            //    //    {
            //    //        dt.Columns.Add(col);
            //    //    }
            //    //}
            //    //dt.Merge(d, false);
            //}
            ////foreach (DataTable tb in table.Tables)
            ////{
            ////    var q = from m in dt.AsEnumerable().
            ////            join d in tb.AsEnumerable() on m.Field<Nullable>("") equals d.Field("")
            ////            select 
            ////}

            ////var query =
            ////   from rHead in dt.AsEnumerable()
            ////   join rTail in dtTail.AsEnumerable()
            ////   on rHead.Field<Int32>("GoodID") equals rTail.Field<Int32>("GoodID")
            ////   select rHead.ItemArray.Concat(rTail.ItemArray.Skip(1));
            #endregion
            DataTable dt = table.Tables.FirstOrDefault(i => i.TableName == tableNm);
            if (dt == null) { var result2 = new { total = 0, rows = DBNull.Value }; return JsonConvert.SerializeObject(result2); }
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
                if (dt.Rows[index].RowState == DataRowState.Deleted) continue;
                resultdt.ImportRow(dt.Rows[index]);
            }
            var result = new { total = AppSysUtils.CalculateTotal(dt), rows = resultdt };
            //string b = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff");

            //string c = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff");
            //var sss = JsonConvert.SerializeObject(result);
            //string d = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff");
            //return Json(new { total = testlist.Count , rows = testlist }, JsonRequestBehavior.AllowGet);
            return JsonConvert.SerializeObject(result);
        }

        public ActionResult GetTableRow(string gridid, string tbnm, string tableNm, string rowid, string cmd)
        {
            DataRow dr = null;
            var libtable = this.LibTables.FirstOrDefault(i => i.Name == tbnm);
            if (libtable != null)
            {
                DataTable tb;
                DataTable relatetb = null;
                if (libtable.Tables != null)
                {
                    tb = libtable.Tables.FirstOrDefault(i => i.TableName == tableNm).Copy();
                    switch (cmd)
                    {
                        case "Add":

                            dr = tb.NewRow();
                            TableExtendedProperties extprop = tb.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                            if (extprop != null)
                            {
                                #region 获取关联的表
                                foreach (var item in this.LibTables)
                                {
                                    for (int n = 0; n < item.Tables.Length; n++)
                                    {
                                        if (((TableExtendedProperties)item.Tables[n].ExtendedProperties[SysConstManage.ExtProp]).TableIndex == extprop.RelateTableIndex)
                                        {
                                            relatetb = item.Tables[n];
                                            break;
                                        }
                                    }
                                    if (relatetb != null)
                                    {
                                        break;
                                    }
                                }
                                #endregion
                                #region 填充主键列的值
                                if (relatetb != null && extprop.RelateTableIndex == 0)
                                {
                                    ColExtendedProperties colextprop = null;
                                    DataColumn relatecol = null;
                                    foreach (var col in tb.PrimaryKey)
                                    {
                                        if (col.AutoIncrement) continue;
                                        colextprop = col.ExtendedProperties[SysConstManage.ExtProp] as ColExtendedProperties;
                                        if (string.IsNullOrEmpty(colextprop.MapPrimarykey))
                                        {
                                            relatecol = relatetb.Columns[col.ColumnName];
                                            if (relatecol != null)
                                                dr[col] = relatetb.Rows[0][relatecol];
                                            else
                                            {
                                                if (col.DataType.Equals(typeof(int)) || col.DataType.Equals(typeof(decimal)) || col.DataType.Equals(typeof(long)))
                                                {
                                                    dr[col] = 0;
                                                    continue;
                                                }
                                                dr[col] = new object();
                                            }
                                        }
                                        else
                                            dr[col] = relatetb.Rows[0][colextprop.MapPrimarykey];

                                    }
                                }
                                #endregion
                            }
                            //tb.Rows.Add(dr);
                            break;
                        case "Edit":
                            if (!string.IsNullOrEmpty(rowid))
                            {
                                DataRow[] drs = tb.Select(string.Format("{0}={1}", SysConstManage.sdp_rowid, rowid));
                                if (drs != null && drs.Length > 0)
                                    dr = drs[0];
                            }
                            break;
                        case "Delet":

                            break;
                    }
                }
            }
            UpdateTableRow(gridid, dr, cmd);
            return LibJson(dr);
        }

        public ActionResult TableAction(string gridid, string tbnm, string tableNm, string cmd, string row)
        {
            DataRow dr = null;
            var libtable = this.LibTables.FirstOrDefault(i => i.Name == tbnm);
            if (libtable != null)
            {
                var formparams = this.Request.Form;
                string[] array;
                DataTable tb;
                DataTable relatetb = null;
                if (libtable.Tables != null)
                {
                    tb = libtable.Tables.FirstOrDefault(i => i.TableName == tableNm);
                    if (tb == null)
                        this.ThrowErrorException(string.Format("未找到表{0}", tableNm));
                    if (string.IsNullOrEmpty(row))
                        this.ThrowErrorException("error! not data");
                    List<FormFields> fieldlst = JsonConvert.DeserializeObject<List<FormFields>>(row);
                    if (fieldlst == null)
                        this.ThrowErrorException(string.Format("反序列化后数据为空"));

                    switch (cmd)
                    {
                        case "Add":
                            dr = tb.NewRow();
                            foreach (FormFields f in fieldlst)
                            {
                                dr[f.FieldNm.Replace(string.Format("{0}{1}", tableNm, SysConstManage.Underline), "")] = f.FieldValue;
                            }
                            tb.Rows.Add(dr);
                            #region 旧代码
                            //dr = tb.NewRow();
                            //TableExtendedProperties extprop = tb.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                            //if (extprop != null)
                            //{
                            //    #region 获取关联的表
                            //    foreach (var item in this.LibTables)
                            //    {
                            //        for (int n = 0; n < item.Tables.Length; n++)
                            //        {
                            //            if (((TableExtendedProperties)item.Tables[n].ExtendedProperties[SysConstManage.ExtProp]).TableIndex == extprop.RelateTableIndex)
                            //            {
                            //                relatetb = item.Tables[n];
                            //                break;
                            //            }
                            //        }
                            //        if (relatetb != null)
                            //        {
                            //            break;
                            //        } 
                            //    }
                            //    #endregion
                            //    #region 填充主键列的值
                            //    if (relatetb != null && extprop .RelateTableIndex ==0)
                            //    {
                            //        ColExtendedProperties colextprop = null;
                            //        DataColumn relatecol = null;
                            //        foreach (var col in tb.PrimaryKey)
                            //        {
                            //            colextprop = col.ExtendedProperties[SysConstManage.ExtProp] as ColExtendedProperties;
                            //            if (string.IsNullOrEmpty(colextprop.MapPrimarykey))
                            //            {
                            //                relatecol = relatetb.Columns[col.ColumnName];
                            //                if (relatecol != null)
                            //                    dr[col] = relatetb.Rows[0][relatecol];
                            //                else
                            //                {
                            //                    if (col.DataType.Equals(typeof(int)) || col.DataType.Equals(typeof(decimal)) || col.DataType.Equals(typeof(long)))
                            //                    {
                            //                        dr[col] = 0;
                            //                        continue;
                            //                    }
                            //                    dr[col] = new object();
                            //                }
                            //            }
                            //            else
                            //                dr[col] = relatetb.Rows[0][colextprop.MapPrimarykey];

                            //        }
                            //    }
                            //    #endregion
                            //}
                            //tb.Rows.Add(dr);
                            #endregion
                            break;
                        case "Edit":
                            var sdprowid = fieldlst.FirstOrDefault(i => i.FieldNm.Contains(SysConstManage.sdp_rowid));
                            if (sdprowid != null)
                            {
                                DataRow[] rows = tb.Select(string.Format("{0}={1}", SysConstManage.sdp_rowid, sdprowid.FieldValue));
                                if (rows != null && rows.Length > 0)
                                {
                                    dr = rows[0];
                                }
                            }
                            break;
                        case "Delet":
                            break;
                    }
                }
                foreach (string nm in formparams.AllKeys)
                {
                    if (nm.Contains(SysConstManage.Point))
                    {
                        array = nm.Split(SysConstManage.Point);
                        if (array.Length < 2) continue;
                        if (dr != null)
                        {
                            dr[array[1]] = formparams[nm];
                        }
                    }
                }
                UpdateTableAction(gridid, dr, cmd);
            }
            return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 搜索模态框 操作
        [HttpGet]
        public ActionResult GetSearchCondFields(string tbnm)
        {
            //var libtb= this.LibTables.FirstOrDefault(i => i.Name == tbnm);
            SearchConditionField cond = null;
            ColExtendedProperties colextprop = null;
            TableExtendedProperties tbextprop = null;
            int masttbindex = 0;
            DataColumn[] mastkeys = null;
            List<DataTable> list = new List<DataTable>();
            List<SearchConditionField> condcollection = new List<SearchConditionField>();
            foreach (LibTable deftb in this.LibTables)
            {
                list.AddRange(deftb.Tables);
            }
            var mtb = list.FirstOrDefault(i => i.TableName == tbnm);
            if (mtb != null)
            {
                tbextprop = mtb.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                masttbindex = tbextprop.TableIndex;
                foreach (DataTable dt in list)
                {
                    tbextprop = dt.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                    if (dt.TableName == tbnm) { mastkeys = dt.PrimaryKey; }
                    if (tbextprop.TableIndex == masttbindex || tbextprop.RelateTableIndex == masttbindex)
                    {
                        foreach (DataColumn col in dt.Columns)
                        {
                            colextprop = col.ExtendedProperties[SysConstManage.ExtProp] as ColExtendedProperties;
                            if (!colextprop.IsActive) continue;
                            cond = new SearchConditionField();
                            cond.DisplayNm = col.Caption;
                            //cond.DefTableNm = deftb.Name;
                            cond.TableNm = dt.TableName;
                            cond.FieldNm = col.ColumnName;
                            cond.TBAliasNm = LibSysUtils.ToCharByTableIndex(tbextprop.TableIndex);
                            if ((tbextprop.TableIndex != masttbindex &&
                                  mtb.PrimaryKey.FirstOrDefault(i => i.ColumnName == col.ColumnName) != null))
                            {
                                cond.AliasNm = colextprop.AliasName;
                            }
                            else
                            {
                                var exist = condcollection.FirstOrDefault(i => i.FieldNm == col.ColumnName);
                                cond.AliasNm = (exist != null && string.IsNullOrEmpty(colextprop.AliasName)) ? string.Format("{0}{1}{2}", cond.TBAliasNm, SysConstManage.Underline, cond.FieldNm) : colextprop.AliasName;
                            }
                            if (tbextprop.TableIndex != masttbindex) cond.Hidden = true;
                            cond.IsDateType = col.DataType.Equals(typeof(Date));
                            condcollection.Add(cond);
                        }
                    }
                }
            }
            SetSearchField(condcollection);
            return Json(new { data = condcollection, flag = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSmodalCondition(int index)
        {
            return Json(new { data = new ElementCollection().SearchModalCondition(Convert.ToInt32(index)) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DoSearchData(int flag)
        {
            var formdata = this.Request.Form;
            //DalResult result = null;
            if (flag == 1)
            {
                string index = string.Empty;
                string tbnm = formdata["tb"];
                List<string> fieldkeys = formdata.AllKeys.Where(i => i.Contains(SysConstManage.sdp_smodalfield)).ToList();
                List<LibSearchCondition> conds = new List<LibSearchCondition>();
                LibSearchCondition cond = null;
                foreach (string key in fieldkeys)
                {
                    index = key.Substring(key.Length - 1);
                    cond = new LibSearchCondition();
                    cond.Values = new object[2];
                    cond.FieldNm = formdata[key];
                    if (cond.FieldNm == "0") continue;
                    cond.Symbol = (SmodalSymbol)Convert.ToInt32(formdata[string.Format("{0}{1}", SysConstManage.sdp_smodalsymbol, index)]);
                    cond.Values[0] = formdata[string.Format("{0}{1}_1", SysConstManage.sdp_smodalval, index)];
                    cond.Values[1] = formdata[string.Format("{0}{1}_2", SysConstManage.sdp_smodalval, index)];
                    cond.Logic = (Smodallogic)Convert.ToInt32(formdata[string.Format("{0}{1}", SysConstManage.sdp_smodallogic, index)]);
                    conds.Add(cond);
                }
                this.SessionObj.Conds = conds;

                //Session[string.Format("{0}{1}",SysConstManage.sdp_Schcond,this.ProgID)] = conds;
                //result = this.ExecuteMethod("InternalSearch", tbnm, null, conds);
            }
            else if (flag == 2)
            {

            }
            //if (result != null && result.Messagelist.Count == 0)
            //{
            //    return Json(new { data =(DataTable)result.Value,flag=0  }, JsonRequestBehavior.AllowGet);
            //}
            return Json(new { data = "", flag = 0 }, JsonRequestBehavior.AllowGet);
        }

        public string BindSmodalData(string tableNm, int page, int rows)
        {
            List<LibSearchCondition> conds = this.SessionObj.Conds;
            //List<LibSearchCondition> conds = Session[string.Format("{0}{1}", SysConstManage.sdp_Schcond, this.ProgID)] as List<LibSearchCondition>;
            DalResult result = this.ExecuteMethod("InternalSearchByPage", tableNm, null, conds, page, rows);
            if (result.Messagelist == null || result.Messagelist.Count == 0)
            {
                DataTable dt = ((DataTable)result.Value);
                var resultdt = new { total = dt.Rows.Count > 0 ? dt.Rows[0][SysConstManage.sdp_total_row] : 0, rows = result.Value };
                return JsonConvert.SerializeObject(resultdt);
            }
            return string.Empty;
        }
        #endregion

        #region Msgforsave信息取值
        [HttpPost]
        public ActionResult GetMsgforSave()
        {
            LibMessage[] msglist = null;
            if (this.SessionObj.MsgforSave != null)
            {
                msglist = new LibMessage[this.SessionObj.MsgforSave.Count];
                this.SessionObj.MsgforSave.CopyTo(msglist);
                this.SessionObj.MsgforSave.Clear();
            }
            return Json(new {Messagelist=msglist }, JsonRequestBehavior.AllowGet);
        }
        #endregion 
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

        protected virtual void UpdateTableAction(string gridid, DataRow row, string cmd)
        {

        }
        protected virtual void UpdateTableRow(string gridid, DataRow row, string cmd)
        {

        }

        protected virtual void SetSearchField(List<SearchConditionField> fields) { }
        #endregion

        #region 私有函数

        #endregion
    }
}