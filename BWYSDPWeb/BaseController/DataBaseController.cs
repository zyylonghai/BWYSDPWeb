using Bll;
using BWYSDPWeb.Com;
using BWYSDPWeb.Models;
using Com;
using Newtonsoft.Json;
using ProgViewModel;
using SDPCRL.COM;
using SDPCRL.COM.ModelManager;
using SDPCRL.COM.ModelManager.FormTemplate;
using SDPCRL.CORE;
using SDPCRL.CORE.FileUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
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
            m = new Menu();
            m.MenuId = "010303";
            m.MenuName = "物料主数据";
            m.ProgId = "Materials";
            m.Package = "com";
            m.PmenuId = "0103";
            mdata.Add(m);
            m = new Menu();
            m.MenuId = "010304";
            m.MenuName = "测试功能";
            m.ProgId = "WebTestFunc";
            m.Package = "com";
            m.PmenuId = "0103";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "0104";
            m.MenuName = "权限配置";
            mdata.Add(m);

            m = new Menu();
            m.MenuId = "010401";
            m.MenuName = "角色";
            m.ProgId = "Jole";
            m.Package = "Authority";
            m.PmenuId = "0104";
            mdata.Add(m);

            return Json(new { Message = "success", data = mdata, Flag = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单跳转功能页
        /// </summary>
        /// <param name="progId">排版模型ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConverToPage(string progId, string flag)
        {
            ProgBaseViewModel viewModel = new ProgBaseViewModel();
            if (this.UserInfo == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (this.Request.Url.Segments.Length > 2)
            {
                string packagepath = this.Request.Url.Segments[1];
                //this.ThrowErrorException("加载异常");
                if (!string.IsNullOrEmpty(packagepath))
                {
                    this.ProgID = progId;
                    this.Package = packagepath.Replace("/", "");
                    LibFormPage formpage = ModelManager.GetModelBypath<LibFormPage>(this.ModelRootPath, this.ProgID, this.Package);
                    #region 权限验证
                    var permresult = PermissionResult(this.ProgID, this.Package);
                    if (permresult != null)
                        return permresult;
                    DalResult result = this.ExecuteMethod("GetAuthority", this.UserInfo.UserId);
                    DataTable data = (DataTable)result.Value;
                    if (data != null)
                    {
                        DataRow[] rows = data.Select(string.Format("JoleId='{0}'", "001"));//是否有管理员角色
                        if (rows.Length == 0)//没有管理角色，则检查权限。
                        {
                            rows = data.Select(string.Format("ProgId='{0}'", this.ProgID));
                            if (rows.Length == 0)
                            {
                                //msg000000009	用户{0}没有功能{1}使用权限
                                return View("NotPermission", new NotPermission { Message = string.Format(AppCom.GetMessageDesc("msg000000009"), this.UserInfo.UserNm, this.ProgID) });
                            }
                            AuthorityObj obj = null;
                            foreach (DataRow dr in rows)
                            {
                                obj = new AuthorityObj();
                                if (LibSysUtils.IsNULLOrEmpty(dr["ObjectType"])) continue;
                                obj.ObjectType = Convert.ToInt32(dr["ObjectType"]);
                                obj.ObjectId = dr["ObjectId"].ToString();
                                obj.GroupId = dr["GroupId"].ToString();
                                viewModel.AuthorityObjs.Add(obj);
                                if (obj.ObjectType == 1) //操作对象
                                {
                                    if (formpage.FormId == obj.GroupId)
                                    {
                                        continue;
                                    }
                                    #region 检查按钮组
                                    var exist = formpage.BtnGroups.FindFirst("BtnGroupID", obj.GroupId);
                                    if (exist != null)
                                    {
                                        var btn = exist.LibButtons.FindFirst("LibButtonID", obj.ObjectId);
                                        if (btn != null)
                                        {
                                            obj.ObjectId = btn.LibButtonName;
                                            continue;
                                        }
                                    }
                                    #endregion
                                    #region 检查表格组的自定义按钮
                                    var exist2 = formpage.GridGroups.FindFirst("GridGroupID", obj.GroupId);
                                    if (exist2 != null)
                                    {
                                        obj.GroupId = exist2.GridGroupName;
                                        var btn = exist2.GdButtons.FindFirst("GridButtonID", obj.ObjectId);
                                        if (btn != null)
                                        {
                                            obj.ObjectId = btn.GridButtonName;
                                            continue;
                                        }
                                    }
                                    #endregion 
                                }
                                else if (obj.ObjectType == 2)//数据对象
                                {
                                    var grid = formpage.GridGroups.FindFirst("GridGroupID", obj.GroupId);
                                    if (grid != null)
                                    {
                                        obj.GroupId = grid.GridGroupName;
                                    }
                                    else
                                    {
                                        var formgroup = formpage.FormGroups.FindFirst("FormGroupID", obj.GroupId);
                                        if (formgroup != null)
                                        {
                                            obj.GroupId = formgroup.FormGroupName;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    #endregion
                    this.AddorUpdateCookies(SysConstManage.PageinfoCookieNm, progId, this.Package);
                    //this.AddorUpdateCookies(SysConstManage.PageinfoCookieNm, SysConstManage .PackageCookieKey, packagepath.Replace("/", ""));
                    //if (string.IsNullOrEmpty (flag ) && this.SessionObj .OperateAction==OperatAction.Preview)
                    //{
                    //    this.SessionObj.OperateAction = OperatAction.None;
                    //}
                    FileOperation fileoperation = new FileOperation();

                    fileoperation.FilePath = string.Format(@"{0}Views\{1}\{2}.cshtml", this.RootPath, packagepath, string.Format("{0}_{1}", progId, this.Language.ToString()));
                    if (!fileoperation.ExistsFile())//不存在视图文件,需要创建
                    {
                        //LibFormPage formpage = ModelManager.GetModelBypath<LibFormPage>(this.ModelRootPath, progId, this.Package);
                        if (formpage == null) { return View("NotFindPage"); }
                        LibDataSource dataSource = ModelManager.GetModelBypath<LibDataSource>(this.ModelRootPath, formpage.DSID, this.Package);
                        DataTable dt = this.GetFieldDescBydsid(dataSource.DSID);
                        CachHelp cachHelp = new CachHelp();
                        cachHelp.AddCachItem(dataSource.DSID, dt, DateTimeOffset.Now.AddMinutes(2));

                        #region 根据排版模型对象 创建功能视图。
                        ViewFactory factory = new ViewFactory(progId);
                        factory.LibDataSource = dataSource;
                        factory.LibFormPage = formpage;
                        factory.ControlClassNm = formpage.ControlClassNm;
                        factory.DSID = formpage.DSID;
                        factory.Package = this.Package;
                        factory.Language = this.Language;
                        factory.BeginPage(AppCom.GetFieldDesc((int)Language, factory.DSID, string.Empty, formpage.FormId));
                        factory.CreateBody();
                        factory.CreateForm();
                        if (formpage.ModuleOrder != null)
                        {
                            foreach (ModuleOrder item in formpage.ModuleOrder)
                            {
                                switch (item.moduleType)
                                {
                                    case ModuleType.FormGroup:
                                        if (formpage.FormGroups != null)
                                        {
                                            foreach (LibFormGroup formg in formpage.FormGroups)
                                            {
                                                if (formg.FormGroupID != item.ID) continue;
                                                factory.CreatePanelGroup(formg.FormGroupName);
                                                if (formg.FmGroupFields != null && formg.FmGroupFields.Count > 0)
                                                {
                                                    factory.AddFormGroupFields(formg.FmGroupFields, formg.FormGroupName);
                                                }
                                            }
                                        }
                                        break;
                                    case ModuleType.GridGroup:
                                        if (formpage.GridGroups != null)
                                        {
                                            foreach (LibGridGroup grid in formpage.GridGroups)
                                            {
                                                if (grid.GridGroupID != item.ID) continue;
                                                if (factory.Childrengrids.FirstOrDefault(i => i.GridGroupID == grid.GridGroupID) != null) continue;
                                                if (grid.GdGroupFields != null)
                                                {
                                                    factory.CreateGridGroup(grid);
                                                }
                                            }
                                        }
                                        break;
                                    case ModuleType.ButtonGroup:
                                        if (formpage.BtnGroups != null)
                                        {
                                            foreach (LibButtonGroup btngroup in formpage.BtnGroups)
                                            {
                                                if (btngroup.BtnGroupID != item.ID) continue;
                                                factory.CreatBtnGroup(btngroup);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        #region 旧代码
                        //if (formpage.FormGroups != null)
                        //{
                        //    foreach (LibFormGroup formg in formpage.FormGroups)
                        //    {

                        //        factory.CreatePanelGroup(AppCom.GetFieldDesc((int)Language, factory.DSID, string.Empty, formg.FormGroupName));
                        //        if (formg.FmGroupFields != null && formg.FmGroupFields.Count > 0)
                        //        {
                        //            factory.AddFormGroupFields(formg.FmGroupFields);
                        //        }
                        //    }
                        //}
                        //if (formpage.GridGroups != null)
                        //{
                        //    foreach (LibGridGroup grid in formpage.GridGroups)
                        //    {
                        //        if (grid.GdGroupFields != null)
                        //        {
                        //            factory.CreateGridGroup(grid);
                        //        }
                        //    }
                        //}
                        #endregion
                        factory.EndPage();

                        fileoperation.WritText(factory.PageHtml);
                        #endregion

                        #region 保存Formgroupfields
                        string a = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                        //Bll.SQLiteHelp sQLiteHelp = new Bll.SQLiteHelp("TempData");
                        TempHelp tempHelp = new TempHelp("TempData");
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
                        tempHelp.Delete(string.Format("delete from formfields where progid='{0}'", progId));
                        tempHelp.Update(commandtextlst);
                        string d = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                        #endregion

                    }
                    //Server.MapPath("/")
                }
            }
            //return View(progId);
            this.SessionObj.ProgBaseVM = viewModel;
            object viewObj =@JsonConvert .SerializeObject(viewModel);
            return View(string.Format("{0}_{1}", progId, this.Language.ToString()), viewObj);
        }

        [HttpPost]
        public virtual ActionResult BasePageLoad()
        {
            if (this.SessionObj.MsgforSave == null || this.SessionObj.MsgforSave.FirstOrDefault(i => i.MsgType == LibMessageType.Error) == null)
            {
                if (this.SessionObj.OperateAction != OperatAction.Preview && this.SessionObj.OperateAction!=OperatAction.Edit)
                {
                    this.SessionObj.OperateAction = OperatAction.Add;
                    //Session[SysConstManage.OperateAction] = this.OperatAction;
                    this.CreateTableSchema();
                    #region delete temp data(重新加载页面，需清除temp表中的session数据)
                    TempHelp sQLiteHelp = new TempHelp("TempData");
                    sQLiteHelp.ClearTempData(System.Web.HttpContext.Current.Session.SessionID, this.ProgID);

                    #endregion
                    //DataRow row = null;
                    foreach (var def in this.LibTables)
                    {
                        foreach (LibTableObj dtobj in def.Tables)
                        {
                            if ((dtobj.DataTable.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties).TableIndex == 0)
                            {
                                //row = dt.NewRow();

                                dtobj .DataTable .Rows.Add(dtobj .DataTable .NewRow());
                            }
                        }
                    }
                    PageLoad();
                }
            }
            #region 处理MsgforSave
            LibMessage[] msglist = null;
            if (this.SessionObj.MsgforSave != null)
            {
                msglist = new LibMessage[this.SessionObj.MsgforSave.Count];
                this.SessionObj.MsgforSave.CopyTo(msglist);
                this.SessionObj.MsgforSave.Clear();
                this.AddMessagelist(msglist);
            }
            #endregion 
            return LibJson();
        }
        /// <summary>
        /// 功能搜索
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchFunc(string q)
        {
            ProgInfo[] allprogid = AppCom.GetAllProgid();
            var pid = allprogid.FirstOrDefault(i => i.ProgId.ToUpper() == q.ToUpper());
            if (pid != null)
            {
                //LibPermissionSource permission = ModelManager.GetModelBypath<LibPermissionSource>(this.ModelRootPath, pid.ProgId, pid.Package);
                //if (permission != null&& !permission .IsMenu)
                //{
                //    //msg000000007	该功能未开放
                //    return View("NotPermission", new NotPermission { Message = AppCom .GetMessageDesc ("msg000000007") });
                //}
                //if (permission == null)
                //{
                //    //msg000000008	功能{0}未配置权限模型
                //    return View("NotPermission", new NotPermission { Message =string.Format(AppCom.GetMessageDesc("msg000000008"),pid.ProgId) });
                //}
                var permresult = PermissionResult(pid.ProgId, pid.Package);
                if (permresult != null)
                    return permresult;
                return RedirectToAction("ConverToPage", pid.Package, new { progId = pid.ProgId });
            }
            //this.AddMessage("找不到该功能", LibMessageType.Error);
            return RedirectToAction("ConverToPage", this.Package, new { progId = this.ProgID });
        }
        [HttpPost]
        public ActionResult Save()
        {
            //HttpPostedFileBase file = this.Request.Files[0];

            #region 处理前端传回的数据
            var formdata = this.Request.Form;
            string[] array;
            DataTable dt = null;
            Stream fileInStream = null;
            HttpPostedFileBase file = null;
            DataTable mdt = null;//主表，即tableindex 为0的表
            foreach (string key in formdata.AllKeys)
            {
                if (key.Contains(SysConstManage.Point))
                {
                    array = key.Split(SysConstManage.Point);
                    if (array.Length < 2)
                        continue;
                    object val = formdata[key];
                    if (dt != null && dt.TableName == array[0])
                    {
                        SetDTFirstRowColValue(dt, array[1], val);
                        #region 旧代码
                        //if (dt.Rows.Count > 0)
                        //{
                        //    //dt.Rows[0][array[1]] ="2019-08-08 21:41:30";
                        //    DataTableHelp.SetColomnValue(dt.Rows[0], array[1], val);
                        //}
                        //else
                        //{
                        //    if (!LibSysUtils.IsNULLOrEmpty(val))
                        //    {
                        //        DataRow row = dt.NewRow();
                        //        dt.Rows.Add(row);
                        //        DataTableHelp.SetColomnValue(row, array[1], val);
                        //    }
                        //}
                        #endregion 
                        continue;
                    }
                    foreach (LibTable libtb in this.LibTables)
                    {
                        string tbnm = array[0];
                        LibTableObj tbobj = libtb.Tables.FirstOrDefault(i => i.TableName == tbnm);
                        dt = tbobj ==null ?null :tbobj .DataTable;
                        if (dt != null)
                        {
                            SetDTFirstRowColValue(dt, array[1], val);
                            #region 旧代码
                            //if (dt.Rows.Count > 0)
                            //{
                            //    DataTableHelp.SetColomnValue(dt.Rows[0], array[1], val);
                            //}
                            //else
                            //{
                            //    if (!LibSysUtils.IsNULLOrEmpty(val))
                            //    {
                            //        DataRow row = dt.NewRow();
                            //        dt.Rows.Add(row);
                            //        DataTableHelp.SetColomnValue(row, array[1], val);
                            //    }
                            //}
                            #endregion
                            break;
                        }
                    }

                }
            }
            #region 主表主键值的编码规则处理
            mdt = GetTableByIndex(0);
            foreach (DataColumn col in mdt.PrimaryKey)
            {
                if (string.IsNullOrEmpty(mdt.Rows[0][col].ToString ()))
                {
                    mdt.Rows[0][col] = this.GenerateNoByprogid();
                }
            }
            #endregion
            #region 处理上传的图片
            foreach (string key in Request.Files.AllKeys)
            {
                if (key.Contains(SysConstManage.Point))
                {
                    array = key.Split(SysConstManage.Point);
                    if (array.Length < 2)
                        continue;
                    object val = null;
                    if (this.Request.Files.AllKeys.FirstOrDefault(i => i == key) != null)
                    {
                        file = this.Request.Files[key];
                        fileInStream = file.InputStream;
                        byte[] content = new byte[file.ContentLength];
                        fileInStream.Read(content, 0, file.ContentLength);
                        //string ss= Convert.ToBase64String(content);
                        if (content.Length == 0) continue;
                        val = content;
                    }
                    if (dt != null && dt.TableName == array[0])
                    {
                        SetDTFirstRowColValue(dt, array[1], val);
                        continue;
                    }
                    foreach (LibTable libtb in this.LibTables)
                    {
                        string tbnm = array[0];
                        LibTableObj tableObj = libtb.Tables.FirstOrDefault(i => i.TableName == tbnm);
                        dt = tableObj ==null?null :tableObj .DataTable;
                        if (dt != null)
                        {
                            SetDTFirstRowColValue(dt, array[1], val);
                            break;
                        }
                    }
                }
            }
            #endregion
            #region 处理关联主表的表的主键赋值。
            TableExtendedProperties tbextp = null;
            //ColExtendedProperties colextp = null;
            List<DataTable> relatedts = new List<DataTable>();
            foreach (LibTable def in this.LibTables)
            {
                foreach (LibTableObj tbojb in def.Tables)
                {
                    tbextp = tbojb.DataTable.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                    if (tbextp.TableIndex == 0) { continue; }
                    if (tbextp.RelateTableIndex == 0)
                    {
                        if (mdt != null)
                        {
                            SetPrimaryKeyWithMastTB(tbojb .DataTable, mdt);
                        }
                        relatedts.Add(tbojb .DataTable);
                    }
                    else
                    {
                        while (true)
                        {
                            DataTable t = GetTableByIndex(tbextp.RelateTableIndex);
                            if (t != null)
                            {
                                tbextp = t.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                                if (tbextp.RelateTableIndex == 0 || tbextp.RelateTableIndex == tbextp.TableIndex)
                                {
                                    if (tbextp.RelateTableIndex == 0)
                                        SetPrimaryKeyWithMastTB(tbojb.DataTable, mdt);
                                    break;
                                }

                            }
                            else
                                break;
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

            #region 系统字段的填值
            foreach (LibTable libtb in this.LibTables)
            {
                foreach (LibTableObj  tbobj in libtb.Tables)
                {
                    if (tbobj.DataTable == null) continue;
                    foreach (DataRow dr in tbobj.DataTable.Rows)
                    {
                        if (dr.RowState == DataRowState.Deleted) continue;
                        SetColumnValue(dr, SysConstManage.sysfld_creater, this.UserInfo.UserId);
                        SetColumnValue(dr, SysConstManage.sysfld_createDT, DateTime.Now);
                    }
                }
            }
            #endregion
            DalResult result = null;
            if (this.MsgList == null || this.MsgList.FirstOrDefault(i => i.MsgType == LibMessageType.Error) == null)
            {
                result = (DalResult)this.ExecuteSaveMethod("Save", this.LibTables);
            }
            //string b = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            AfterSave();

            if (result != null && result.ErrorMsglst != null && result.ErrorMsglst.Count > 0)
            {
                //string _msg = string.Empty;
                //foreach (var m in result.ErrorMsglst)
                //{
                //    _msg += m.Message + m.Stack;
                //}
                ////this.ThrowErrorException(_msg);
                //return View("Error");
                return LibReturnError(result.ErrorMsglst);
            }
            //return Content("alert(\"sdfds\")");
            //return Json(new { message = "dfceshi" }, JsonRequestBehavior.AllowGet);
            if (this.MsgList == null || this.MsgList.FirstOrDefault(i => i.MsgType == LibMessageType.Error) == null)
            {
                foreach (LibTable libtb in this.LibTables)
                {
                    foreach (LibTableObj tbobj in libtb.Tables)
                    {
                        tbobj.DataTable.AcceptChanges();
                    }
                }
                this.SessionObj.OperateAction = OperatAction.Preview;
            }
            //msg000000001 保存成功
            this.AddMessage(AppCom.GetMessageDesc("msg000000001"), LibMessageType.Prompt);
            if (this.MsgList != null && this.MsgList.Count > 0)
            {
                if (this.SessionObj.MsgforSave == null) this.SessionObj.MsgforSave = new List<LibMessage>();
                this.SessionObj.MsgforSave.AddRange(this.MsgList);
            }
            return RedirectToAction("ConverToPage", this.Package, new { progId = this.ProgID });
        }

        public ActionResult Add()
        {
            this.SessionObj.OperateAction = OperatAction.Add;
            return BasePageLoad();
        }

        public ActionResult Edit()
        {
            if (this.SessionObj.OperateAction == OperatAction.Preview)
                this.SessionObj.OperateAction = OperatAction.Edit;
            return BasePageLoad();
        }

        [HttpPost]
        public ActionResult FillAndEdit()
        {
            var parmas = this.Request.Form;
            string tablenm = parmas["tablenm"];
            List<LibTableObj> dtlist = new List<LibTableObj>();
            //StringBuilder whereformat = new StringBuilder();
            List<string> whereformat = new List<string>();
            object[] vals = null;
            foreach (LibTable def in this.LibTables)
            {
                dtlist.AddRange(def.Tables);
            }
            var tbobj = dtlist.FirstOrDefault(i => i.TableName == tablenm);
            DataTable mast = tbobj ==null ?null :tbobj .DataTable ;
            if (mast == null)
            {
                //msg000000002 找不到表
                return Json(new { data = AppCom.GetMessageDesc("msg000000002"), flag = 1 }, JsonRequestBehavior.AllowGet);
            }
            vals = new object[mast.PrimaryKey.Length];
            for (int n = 0; n < mast.PrimaryKey.Length; n++)
            {
                //if (whereformat.Length > 0)
                //{
                //    whereformat.Append(" And ");
                //}
                //whereformat.AppendFormat("{0}={1}", mast.PrimaryKey[n].ColumnName, "{" + n + "}");
                whereformat.Add(string.Format("{0}={1}", mast.PrimaryKey[n].ColumnName, "{" + n + "}"));
                vals[n] = parmas[string.Format("dr[{0}]", mast.PrimaryKey[n].ColumnName)];
            }
            DalResult result = this.ExecuteMethod("InternalFillData", whereformat, vals);
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
                            DataTableHelp dthelp = new DataTableHelp(exist, tb.DataTable);
                            dthelp.CopyStable();
                            ConvertBinaryColValue(tb.DataTable);
                        }
                    }
                }
            }
            this.SessionObj.OperateAction = OperatAction.Preview;
            return LibJson();
            //return Json(new { data = "", flag = 0 }, JsonRequestBehavior.AllowGet);
        }

        #region grid 表格操作
        public string BindTableData(string gridid, string deftb, string tableNm, string prowid, int page, int rows, string sort, string sortOrder, string Mobile)
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
            var tbobj = table.Tables.FirstOrDefault(i => i.TableName == tableNm);
            DataTable dt = tbobj ==null?null :tbobj .DataTable;
            if (dt == null) { var result2 = new { total = 0, rows = DBNull.Value }; return JsonConvert.SerializeObject(result2); }
            if (!string.IsNullOrEmpty(prowid))
            {
                TableExtendedProperties extprop = dt.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                DataTable relatedt = InternalGetParentTable(dt);
                DataRow relaterow = AppSysUtils.GetRowByRowId(relatedt, Convert.ToInt32(prowid));
                if (relaterow == null) { var result2 = new { total = 0, rows = DBNull.Value }; return JsonConvert.SerializeObject(result2); }
                //StringBuilder where = new StringBuilder();
                //ColExtendedProperties colextprop = null;
                //DataColumn col2 = null;
                //foreach (DataColumn col in dt.PrimaryKey)
                //{
                //    if (col.AutoIncrement) continue;
                //    colextprop = col.ExtendedProperties[SysConstManage.ExtProp] as ColExtendedProperties;
                //    col2 = string.IsNullOrEmpty(colextprop.MapPrimarykey) ? relatedt.Columns[col.ColumnName] : relatedt.Columns[colextprop.MapPrimarykey];
                //    if (col2 == null) continue;

                //    if (where.Length > 0)
                //    {
                //        where.Append(" and ");
                //    }
                //    where.AppendFormat("{0}='{1}'", col.ColumnName, relaterow[col2]);
                //}
                string where = AppSysUtils.GetChildRowByParentRowToExpress(relaterow, dt);
                dt = AppSysUtils.GetData(dt, where.ToString());
            }
            GetGridDataExt(gridid, dt);

            DataTable resultdt = AppSysUtils.GetDataByPage(dt, page, rows);
            //ConvertBinaryColValue(resultdt);
            if (!string.IsNullOrEmpty(sort))
            {
                resultdt.DefaultView.Sort = string.Format("{0} {1}", sort, sortOrder);
                resultdt = resultdt.DefaultView.ToTable();
            }
            var result = new { total = AppSysUtils.CalculateTotal(dt), rows = resultdt };

            return JsonConvert.SerializeObject(result);
        }

        public ActionResult GetTableRow(string gridid, string tbnm, string tableNm, string rowid, string prowid, string cmd)
        {
            DataRow dr = null;
            LibTableObj tbobj = null;
            var libtable = this.LibTables.FirstOrDefault(i => i.Name == tbnm);
            if (libtable != null)
            {
                DataTable tb=null;
                DataTable relatetb = null;
                if (libtable.Tables != null)
                {
                    tb = libtable.Tables.FirstOrDefault(i => i.TableName == tableNm).DataTable.Copy();
                    tbobj = new LibTableObj(tb);
                    switch (cmd)
                    {
                        case "Add":
                            dr = ((DataRowObj)tbobj.NewRow()).Row;
                            //dr = tb.NewRow();
                            TableExtendedProperties extprop = tb.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                            if (extprop != null)
                            {
                                #region 获取关联的表
                                relatetb = InternalGetParentTable(tb);
                                //foreach (var item in this.LibTables)
                                //{
                                //    for (int n = 0; n < item.Tables.Length; n++)
                                //    {
                                //        if (((TableExtendedProperties)item.Tables[n].ExtendedProperties[SysConstManage.ExtProp]).TableIndex == extprop.RelateTableIndex)
                                //        {
                                //            relatetb = item.Tables[n];
                                //            break;
                                //        }
                                //    }
                                //    if (relatetb != null)
                                //    {
                                //        break;
                                //    }
                                //}
                                #endregion
                                #region 填充主键列的值
                                if (relatetb != null && (relatetb.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties).TableIndex!=extprop .TableIndex)
                                {
                                    ColExtendedProperties colextprop = null;
                                    DataColumn relatecol = null;
                                    DataRow relaterow = null;
                                    if (extprop.RelateTableIndex == 0)
                                        relaterow = relatetb.Rows[0];
                                    else
                                    {
                                        if (string.IsNullOrEmpty(prowid))
                                        {
                                            this.ThrowErrorException("the parentGrid rowId is NullorEmpty");
                                        }
                                        DataRow[] drs = relatetb.Select(string.Format("{0}={1}", SysConstManage.sdp_rowid, prowid));
                                        if (drs != null && drs.Length > 0)
                                            relaterow = drs[0];
                                    }

                                    foreach (var col in tb.PrimaryKey)
                                    {
                                        if (col.AutoIncrement) continue;
                                        colextprop = col.ExtendedProperties[SysConstManage.ExtProp] as ColExtendedProperties;
                                        if (string.IsNullOrEmpty(colextprop.MapPrimarykey))
                                        {
                                            relatecol = relatetb.Columns[col.ColumnName];
                                            if (relatecol != null)
                                                dr[col] = relaterow[relatecol];
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
                                            dr[col] = relaterow[colextprop.MapPrimarykey];

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
            UpdateTableRow(gridid,tbobj .FindRow(dr) , cmd);
            return LibJson(dr);
        }
        [HttpPost]
        public ActionResult TableAction(string gridid, string tbnm, string tableNm, string cmd, string row)
        {
            DataRow dr = null;
            var libtable = this.LibTables.FirstOrDefault(i => i.Name == tbnm);
            if (libtable != null)
            {
                var formparams = this.Request.Form;
                string[] array;
                DataTable tb = null;
                DataTable relatetb = null;
                if (libtable.Tables != null)
                {
                    var tbobj = libtable.Tables.FirstOrDefault(i => i.TableName == tableNm);
                    tb =tbobj ==null ?null :tbobj .DataTable;
                    if (tb == null)
                        //msg000000003	未找到表{0}
                        this.ThrowErrorException(string.Format(AppCom.GetMessageDesc("msg000000003"), tableNm));
                    if (string.IsNullOrEmpty(row))
                        this.ThrowErrorException("error! not data");
                    List<FormFields> fieldlst = JsonConvert.DeserializeObject<List<FormFields>>(row);
                    if (fieldlst == null)
                        //msg000000004	反序列化后数据为空
                        this.ThrowErrorException(AppCom.GetMessageDesc("msg000000004"));

                    switch (cmd)
                    {
                        case "Add":
                            dr = tb.NewRow();
                            foreach (FormFields f in fieldlst)
                            {
                                dr[f.FieldNm.Replace(string.Format("{0}{1}", tableNm, SysConstManage.Underline), "")] = f.FieldValue;
                            }
                            //tb.Rows.Add(dr);
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
                            if (fieldlst != null && fieldlst.Count > 0)
                            {
                                string rowid = string.Empty;
                                Dictionary<int, List<int>> prows = new Dictionary<int, List<int>>();
                                List<int> vals = null;
                                TableExtendedProperties extprop = tb.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                                List<DataRow> deletrows = new List<DataRow>();
                                List<DataTable> childdts = InternalGetChildTable(tb);
                                #region 删当前表的行。
                                foreach (DataRow rw in tb.Rows)
                                {
                                    if (rw.RowState == DataRowState.Deleted) continue;
                                    rowid = rw[SysConstManage.sdp_rowid].ToString();
                                    if (fieldlst.FirstOrDefault(i => i.FieldNm == SysConstManage.sdp_rowid && i.FieldValue.ToString() == rowid) != null)
                                    {
                                        if (!prows.TryGetValue(extprop.TableIndex, out vals))
                                        {
                                            vals = new List<int>();
                                            prows.Add(extprop.TableIndex, vals);
                                        }
                                        //if (childdts != null && childdts.Count > 0)
                                        //{
                                        //    foreach (DataTable cdt in childdts)
                                        //    {
                                        //        vals.Add(AppSysUtils.GetChildRowByParentRowToExpress(rw, cdt));
                                        //    }
                                        //}
                                        vals.Add(Convert.ToInt32(rw[SysConstManage.sdp_rowid]));
                                        deletrows.Add(rw);
                                        //rw.Delete();
                                    }
                                }
                                //foreach (DataRow drw in deletrows)
                                //{
                                //    drw.Delete();
                                //}
                                #endregion

                                #region 删除子表的关联行。
                                List<int> hasdone = new List<int>();
                                while (childdts != null && childdts.Count > 0)
                                {
                                    foreach (DataTable child in childdts)
                                    {
                                        extprop = child.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                                        if (extprop != null)
                                        {
                                            if (prows.TryGetValue(extprop.RelateTableIndex, out vals))
                                            {
                                                if (!prows.ContainsKey(extprop.TableIndex))
                                                {
                                                    prows.Add(extprop.TableIndex, new List<int>());
                                                }
                                                hasdone.Add(extprop.RelateTableIndex);
                                                DataTable pdt = GetTableByIndex(extprop.RelateTableIndex);
                                                //deletrows.Clear();
                                                foreach (int prowid in vals) {
                                                   DataRow prow= AppSysUtils.GetRowByRowId(pdt, prowid);
                                                    DataRow[] childrows = AppSysUtils.GetChildRowByParentRow(prow, child);
                                                    if (childrows != null)
                                                    {
                                                        foreach (DataRow r in childrows)
                                                        {
                                                            prows[extprop.TableIndex].Add(Convert.ToInt32(r[SysConstManage.sdp_rowid]));
                                                            deletrows.Add(r);
                                                            //r.Delete();
                                                        }
                                                    }
                                                }
                                                //foreach (DataRow d in deletrows)
                                                //{
                                                //    d.Delete();
                                                //}
                                            }
                                        }
                                    }
                                    foreach (int i in hasdone)
                                    {
                                        prows.Remove(i);
                                    }
                                    childdts = GetChildTableByIndex(prows.Keys.ToList());
                                }
                                foreach (DataRow drow in deletrows)
                                {
                                    drow.Delete();
                                }
                                #endregion
                            }
                            break;
                    }
                }
                try
                {
                    if (cmd != "Delet")
                    {
                        DataColumn col = null;
                        foreach (string nm in formparams.AllKeys)
                        {
                            if (nm.Contains(SysConstManage.Point))
                            {
                                array = nm.Split(SysConstManage.Point);
                                if (array.Length < 2) continue;
                                if (dr != null)
                                {
                                    col = tb.Columns[array[1]];
                                    if (col != null)
                                    {
                                        if (col.DataType == typeof(bool))
                                        {
                                            dr[col] = Convert.ToBoolean(Convert.ToInt32(formparams[nm]));
                                        }
                                        else
                                            dr[col] = formparams[nm];
                                    }
                                    
                                }
                            }
                        }
                        #region 处理上传的图片
                        Stream fileInStream = null;
                        HttpPostedFileBase file = null;
                        foreach (string key in Request.Files.AllKeys)
                        {
                            if (key.Contains(SysConstManage.Point))
                            {
                                array = key.Split(SysConstManage.Point);
                                if (array.Length < 2)
                                    continue;
                                object val = null;
                                if (this.Request.Files.AllKeys.FirstOrDefault(i => i == key) != null)
                                {
                                    file = this.Request.Files[key];
                                    fileInStream = file.InputStream;
                                    byte[] content = new byte[file.ContentLength];
                                    fileInStream.Read(content, 0, file.ContentLength);
                                    //string ss= Convert.ToBase64String(content);
                                    if (content.Length == 0) continue;
                                    //val =System .Text .Encoding .ASCII.GetBytes(Convert .ToBase64String(content));
                                    val = content;
                                }
                                if (dr != null)
                                {
                                    dr[array[1]] = val;
                                }
                            }
                        }
                        #endregion
                        if (cmd == "Add") tb.Rows.Add(dr);
                    }
                }
                catch (Exception ex)
                {
                    //if (dr != null && cmd == "Add")
                    //    tb.Rows.Remove(dr);
                    this.ThrowErrorException(ex.Message);
                }
                UpdateTableAction(gridid, dr, cmd);
            }
            return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 搜索模态框 操作
        /// <summary>
        /// 获取搜索条件字段
        /// </summary>
        /// <param name="tbnm"></param>
        /// <param name="fieldnm"></param>
        /// <param name="flag">1标识单据的搜索，2标识来源主数据的搜索,3标识无来源主数据的搜索</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSearchCondFields(string tbnm, string fieldnm, string flag)
        {
            //var libtb= this.LibTables.FirstOrDefault(i => i.Name == tbnm);
            SearchConditionField cond = null;
            ColExtendedProperties colextprop = null;
            TableExtendedProperties tbextprop = null;
            int masttbindex = 0;
            DataColumn[] mastkeys = null;
            List<LibTableObj> list = new List<LibTableObj>();
            List<SearchConditionField> condcollection = new List<SearchConditionField>();
            if (string.Compare(flag, "1") == 0)
            {
                foreach (LibTable deftb in this.LibTables)
                {
                    list.AddRange(deftb.Tables);
                }
                var mtb = list.FirstOrDefault(i => i.TableName == tbnm);
                if (mtb != null&& mtb .DataTable !=null)
                {
                    tbextprop = mtb.DataTable .ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                    masttbindex = tbextprop.TableIndex;
                    foreach (LibTableObj dtobj in list)
                    {
                        tbextprop = dtobj.DataTable.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                        if (dtobj.DataTable.TableName == tbnm) { mastkeys = dtobj.DataTable.PrimaryKey; }
                        if (tbextprop.TableIndex == masttbindex || tbextprop.RelateTableIndex == masttbindex)
                        {
                            foreach (DataColumn col in dtobj.DataTable.Columns)
                            {
                                colextprop = col.ExtendedProperties[SysConstManage.ExtProp] as ColExtendedProperties;
                                if (!colextprop.IsActive) continue;
                                if (this.SessionObj.ProgBaseVM != null && 
                                    this.SessionObj.ProgBaseVM.AuthorityObjs.FirstOrDefault(i => i.ObjectType == 2 && i.ObjectId ==(string .IsNullOrEmpty(colextprop .AliasName )?col.ColumnName:colextprop .AliasName))!=null)
                                {
                                    continue;
                                }
                                cond = new SearchConditionField();
                                cond.IsCondition = true;
                                cond.DisplayNm = AppCom.GetFieldDesc((int)this.Language, this.DSID, dtobj.DataTable.TableName, col.ColumnName);
                                //cond.DefTableNm = deftb.Name;
                                cond.TableNm = dtobj.DataTable.TableName;
                                cond.FieldNm = col.ColumnName;
                                cond.TBAliasNm = LibSysUtils.ToCharByTableIndex(tbextprop.TableIndex);
                                if ((tbextprop.TableIndex != masttbindex &&
                                      mtb.DataTable.PrimaryKey.FirstOrDefault(i => i.ColumnName == col.ColumnName) != null))
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
                                cond.isBinary = col.DataType.Equals(typeof(byte[]));
                                condcollection.Add(cond);
                            }
                        }
                    }
                    //SetSearchFieldExt(condcollection);
                }
            }
            else if (string.Compare(flag, "2") == 0)
            {
                if (!string.IsNullOrEmpty(tbnm) && !string.IsNullOrEmpty(fieldnm))
                {
                    //来源字段上的搜索
                    //LibDataSource ds = ModelManager.GetModelBymodelId<LibDataSource>(this.ModelRootPath, relatedsid);
                    LibDataSource ds = ModelManager.GetModelBypath<LibDataSource>(this.ModelRootPath, this.DSID, this.Package);
                    LibField field = null;
                    foreach (LibDefineTable def in ds.DefTables)
                    {
                        LibDataTableStruct dtstruct = def.TableStruct.FindFirst("Name", tbnm);
                        if (dtstruct != null)
                        {
                            field = dtstruct.Fields.FindFirst("Name", fieldnm);
                            break;
                        }
                    }
                    if (field == null)
                    {
                        //msg000000005	未能取到字段，请确认。
                        this.ThrowErrorException(AppCom.GetMessageDesc("msg000000005"));
                    }
                    if (field.SourceField == null)
                    {
                        //msg000000006	模型未设置来源字段，请确认
                        this.ThrowErrorException(AppCom.GetMessageDesc("msg000000006"));
                    }

                    if (field.SourceField.Count == 1)
                    {
                        if (this.SessionObj.FromFieldInfo == null) this.SessionObj.FromFieldInfo = new FromFieldInfo();
                        this.SessionObj.FromFieldInfo.tableNm = tbnm;
                        this.SessionObj.FromFieldInfo.FieldNm = fieldnm;
                        this.SessionObj.FromFieldInfo.FromFieldNm = field.SourceField[0].FromFieldNm;
                        this.SessionObj.FromFieldInfo.FromFieldDesc = field.SourceField[0].FromFieldDesc;
                        if (field.SourceField[0].RelateFieldNm != null)
                        {
                            if (this.SessionObj.FromFieldInfo.RelateFields == null) this.SessionObj.FromFieldInfo.RelateFields = new List<string>();
                            else { this.SessionObj.FromFieldInfo.RelateFields.Clear(); }
                            foreach (LibRelateField libRelate in field.SourceField[0].RelateFieldNm)
                            {
                                this.SessionObj.FromFieldInfo.RelateFields.Add(string.Format("{2}_{0}_rsdp_{1}", libRelate.AliasName, libRelate.FieldNm, tbnm));
                            }
                        }

                        LibDataSource sourceds = ModelManager.GetModelBymodelId<LibDataSource>(this.ModelRootPath, field.SourceField[0].FromDataSource);
                        LibDefineTable defdt = sourceds.DefTables.FindFirst("TableName", field.SourceField[0].FromDefindTableNm);
                        LibDataTableStruct dtstruct = defdt.TableStruct.FindFirst("Name", field.SourceField[0].FromStructTableNm);
                        foreach (LibField f in dtstruct.Fields)
                        {
                            if (!f.IsActive) continue;
                            cond = new SearchConditionField();
                            cond.IsCondition = true;
                            cond.DisplayNm = AppCom.GetFieldDesc((int)this.Language, sourceds.DSID, dtstruct.Name, f.Name);
                            cond.DSID = sourceds.DSID;
                            cond.DefTableNm = defdt.TableName;
                            cond.TableNm = dtstruct.Name;
                            cond.FieldNm = f.Name;
                            cond.TBAliasNm = LibSysUtils.ToCharByTableIndex(dtstruct.TableIndex);
                            cond.AliasNm = f.AliasName;
                            cond.IsDateType = f.FieldType == LibFieldType.Date;
                            condcollection.Add(cond);
                        }
                    }
                    else
                    {

                    }
                }
            }
            else if (string.Compare(flag, "3") == 0)
            {
                if (this.SessionObj.FromFieldInfo == null) this.SessionObj.FromFieldInfo = new FromFieldInfo();
                this.SessionObj.FromFieldInfo.tableNm = tbnm;
                this.SessionObj.FromFieldInfo.FieldNm = fieldnm;
                //this.SessionObj.FromFieldInfo.FromFieldNm = field.SourceField[0].FromFieldNm;
            }
            SetSearchFieldExt(condcollection,fieldnm , Convert.ToInt32(flag));
            return Json(new { data = condcollection.Where(i => i.IsCondition).ToList(), flag = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSmodalCondition(int index)
        {
            return Json(new { data = new ElementCollection().SearchModalCondition(Convert.ToInt32(index)) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DoSearchData(int flag,string fieldnm)
        {
            var formdata = this.Request.Form;
            string index = string.Empty;
            string tbnm = formdata["tb"];
            List<string> fieldkeys = formdata.AllKeys.Where(i => i.Contains(SysConstManage.sdp_smodalfield)).ToList();
            List<LibSearchCondition> conds = new List<LibSearchCondition>();
            LibSearchCondition cond = null;
            foreach (string key in fieldkeys)
            {
                index = key.Substring(key.Length - 1);
                cond = new LibSearchCondition();
                if (flag == 1)
                {
                    cond.DSID = this.DSID;
                }
                else if (flag == 2)
                {
                    cond.DSID = formdata["dsid"];
                }
                cond.TableNm = tbnm;
                cond.Values = new object[2];
                cond.FieldNm = formdata[key];
                if (cond.FieldNm == "0") continue;
                cond.Symbol = (SmodalSymbol)Convert.ToInt32(formdata[string.Format("{0}{1}", SysConstManage.sdp_smodalsymbol, index)]);
                cond.Values[0] = formdata[string.Format("{0}{1}_1", SysConstManage.sdp_smodalval, index)];
                cond.Values[1] = formdata[string.Format("{0}{1}_2", SysConstManage.sdp_smodalval, index)];
                cond.Logic = (Smodallogic)Convert.ToInt32(formdata[string.Format("{0}{1}", SysConstManage.sdp_smodallogic, index)]);
                conds.Add(cond);
            }
            SetSearchCondition(conds,fieldnm);
            this.SessionObj.Conds = conds;
            return Json(new { data = "", flag = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>搜索模态框的绑定查询的数据 </summary>
        /// <param name="tableNm"></param>
        /// <param name="dsid"></param>
        /// <param name="flag">标识1：标识单据搜索，2标识来源主数据搜索，3标识无来源主数据的搜索。</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public string BindSmodalData(string tableNm, string dsid, int flag,string fieldnm, int page, int rows)
        {
            List<LibSearchCondition> conds = this.SessionObj.Conds;
            DataTable dt = null;
            if (flag == 3)
            {
                dt = new DataTable();
                BindSmodalDataExt(dt, flag,fieldnm);
                #region 解析搜索条件
                object[] values = { };
                StringBuilder whereformat = new StringBuilder();
                SearchConditionHelper.AnalyzeSearchCondition(conds, whereformat, ref values);
                #endregion
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = string.Format("'{0}'", values[i]);
                }
                DataRow[] rws = dt.Select(string.Format(whereformat.ToString(), values));
                DataTable dt2 = dt.Clone();
                foreach (DataRow dr in rws)
                {
                    dt2.ImportRow(dr);
                }
                DataTable resultdt = AppSysUtils.GetDataByPage(dt2, page, rows);
                if (this.SessionObj.FromFieldInfo != null)
                {
                    DataColumn primarycol = null;
                    if (resultdt.PrimaryKey != null && resultdt.PrimaryKey.Length > 0)
                        primarycol = resultdt.PrimaryKey[0];
                    DataColumn col = new DataColumn(string.Format("{0}_{1}_sdp_{2}", this.SessionObj.FromFieldInfo.tableNm, this.SessionObj.FromFieldInfo.FieldNm, primarycol == null ? string.Empty : primarycol.ColumnName));
                    resultdt.Columns.Add(col);
                    if (this.SessionObj.FromFieldInfo.RelateFields != null)
                        foreach (string s in this.SessionObj.FromFieldInfo.RelateFields)
                        {
                            if (resultdt.Columns.Contains(s)) continue;
                            col = new DataColumn(s);
                            resultdt.Columns.Add(col);
                        }
                    //this.SessionObj.FromFieldInfo = null;
                }
                return LibReturnForGrid(dt2.Rows.Count, resultdt);
            }
            if (flag == 1) dsid = this.DSID;
            //this.AddMessage("jjjjjjjjjjjj");
            //List<LibSearchCondition> conds = Session[string.Format("{0}{1}", SysConstManage.sdp_Schcond, this.ProgID)] as List<LibSearchCondition>;
            DalResult result = this.ExecuteMethod("InternalSearchByPage", dsid, tableNm, null, conds, page, rows);
            if (result.Messagelist == null || result.Messagelist.Count == 0)
            {
                dt = ((DataTable)result.Value);
                BindSmodalDataExt(dt, flag,fieldnm);
                if (this.MsgList == null || this.MsgList.FirstOrDefault(i => i.MsgType == LibMessageType.Error) == null)
                {
                    if (dt != null && flag == 2)
                    {
                        if (this.SessionObj.FromFieldInfo != null)
                        {
                            DataColumn col = new DataColumn(string.Format("{0}_{1}_sdp_{2}", this.SessionObj.FromFieldInfo.tableNm, this.SessionObj.FromFieldInfo.FieldNm, this.SessionObj.FromFieldInfo.FromFieldNm));
                            dt.Columns.Add(col);
                            col = new DataColumn(string.Format("sdp_desc{0}", this.SessionObj.FromFieldInfo.FromFieldDesc));
                            dt.Columns.Add(col);
                            foreach (string s in this.SessionObj.FromFieldInfo.RelateFields)
                            {
                                col = new DataColumn(s);
                                dt.Columns.Add(col);
                            }
                            //this.SessionObj.FromFieldInfo = null;
                        }
                    }
                    ConvertBinaryColValue(dt);
                    //List<DataColumn> binarycols = new List<DataColumn>();
                    //foreach (DataColumn c in dt.Columns)
                    //{
                    //    if (c.DataType.Equals(typeof(byte[])))
                    //    {
                    //        binarycols.Add(c);

                    //    }
                    //}
                    //if (binarycols.Count > 0)
                    //{
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        foreach (DataColumn o in binarycols)
                    //        {
                    //            dr[o] = dr[o] == DBNull.Value ? dr[o] : Convert.FromBase64String(System.Text.Encoding.ASCII.GetString((byte[])dr[o]));
                    //        }
                    //    }
                    //}
                    return LibReturnForGrid((dt.Rows.Count > 0 ? (int)dt.Rows[0][SysConstManage.sdp_total_row] : 0), dt);
                }
            }
            return LibReturnForGrid(0, null);
        }
        #endregion

        #region 来源主数据模糊搜索
        public ActionResult InternalFuzzySearch(string id, string val)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] array = id.Split(SysConstManage.Underline);
                if (array != null && array.Length > 1)
                {
                    string tablenm = array[0];
                    string fieldnm = array[1];
                    LibDataSource ds = ModelManager.GetModelBypath<LibDataSource>(this.ModelRootPath, this.DSID, this.Package);
                    LibField field = null;
                    foreach (LibDefineTable def in ds.DefTables)
                    {
                        LibDataTableStruct dtstruct = def.TableStruct.FindFirst("Name", tablenm);
                        if (dtstruct != null)
                        {
                            field = dtstruct.Fields.FindFirst("Name", fieldnm);
                            break;
                        }
                    }
                    if (field != null && field.SourceField != null)
                    {
                        List<LibSearchCondition> conds = new List<LibSearchCondition>();
                        if (field.SourceField.Count == 1)
                        {
                            LibFromSourceField fromSourceField = field.SourceField[0];
                            LibSearchCondition cond = new LibSearchCondition();
                            cond.DSID = fromSourceField.FromDataSource;
                            cond.TableNm = fromSourceField.FromStructTableNm;
                            cond.FieldNm = fromSourceField.FromFieldNm;
                            cond.Symbol = SmodalSymbol.Contains;
                            cond.Values = new object[] { val };
                            conds.Add(cond);
                            DalResult result = this.ExecuteMethod("InternalSearchByPage", cond.DSID, cond.TableNm, new string[] { fromSourceField.FromFieldNm, fromSourceField.FromFieldDesc }, conds, 1, 20);
                            if (result.Messagelist == null || result.Messagelist.Count == 0)
                            {
                                DataTable dt = ((DataTable)result.Value);
                                if (this.MsgList == null || this.MsgList.FirstOrDefault(i => i.MsgType == LibMessageType.Error) == null)
                                {
                                    if (dt != null)
                                    {

                                        dt.Columns.Remove(SysConstManage.sdp_total_row);
                                        DataColumn col = dt.Columns[fromSourceField.FromFieldNm];
                                        if (col != null) col.ColumnName = AppCom.GetFieldDesc((int)this.Language, fromSourceField.FromDataSource, fromSourceField.FromStructTableNm, fromSourceField.FromFieldNm);
                                        col = dt.Columns[fromSourceField.FromFieldDesc];
                                        if (col != null) col.ColumnName = AppCom.GetFieldDesc((int)this.Language, fromSourceField.FromDataSource, fromSourceField.FromStructTableNm, fromSourceField.FromFieldDesc);
                                        //List<string> fields = new List<string>();
                                        //foreach (DataColumn c in dt.Columns)
                                        //{
                                        //    fields.Add(c.ColumnName);
                                        //}
                                        //FuzzySearchResult obj = null;
                                        //List<FuzzySearchResult> datas = new List<FuzzySearchResult>();
                                        //foreach (DataRow dr in dt.Rows)
                                        //{
                                        //    obj = new FuzzySearchResult();
                                        //    obj.FromFieldValue = dr[fromSourceField.FromFieldNm].ToString();
                                        //    obj.Describle = dr[fromSourceField.FromFieldDesc].ToString();
                                        //    datas.Add(obj);
                                        //}
                                        return Json(new { data = JsonConvert.SerializeObject(dt.Rows) }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            return Json(new { });
        }
        #endregion

        #region Msgforsave信息取值
        //[HttpPost]
        //public ActionResult GetMsgforSave()
        //{
        //    LibMessage[] msglist = null;
        //    if (this.SessionObj.MsgforSave != null)
        //    {
        //        msglist = new LibMessage[this.SessionObj.MsgforSave.Count];
        //        this.SessionObj.MsgforSave.CopyTo(msglist);
        //        this.SessionObj.MsgforSave.Clear();
        //    }
        //    return Json(new { Messagelist = msglist }, JsonRequestBehavior.AllowGet);
        //}
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
        /// <summary>换出表格模态框时，当前行的扩展操作（供子类处理）</summary>
        /// <param name="gridid"></param>
        /// <param name="row"></param>
        /// <param name="cmd"></param>
        protected virtual void UpdateTableRow(string gridid, DataRowObj row, string cmd)
        {

        }
        /// <summary>
        /// 扩展方法, 设置搜索条件中的字段
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="fieldNm"></param>
        /// <param name="flag"></param>
        protected virtual void SetSearchFieldExt(List<SearchConditionField> fields,string fieldNm, int flag) { }
        /// <summary>
        /// 扩展方法，设置搜索条件表达式
        /// </summary>
        /// <param name="conditions"></param>
        protected virtual void SetSearchCondition(List<LibSearchCondition> conditions,string fieldnm)
        {

        }

        protected virtual void BindSmodalDataExt(DataTable currpagedata, int flag,string fieldnm)
        {

        }

        /// <summary>
        /// 添加来源主数据搜索 带出的关联字段（主要用于关联字段赋值到界面上的控件）
        /// </summary>
        /// <param name="tbnm"></param>
        /// <param name="fieldnm"></param>
        protected void AddRelateFieldsForSearchModal(string tbnm,string fieldnm)
        {
            if(this.SessionObj!=null &&this.SessionObj.FromFieldInfo==null) this.SessionObj.FromFieldInfo = new FromFieldInfo();
            if (this.SessionObj.FromFieldInfo.RelateFields == null) this.SessionObj.FromFieldInfo.RelateFields = new List<string>();
            this.SessionObj.FromFieldInfo.RelateFields.Add(string.Format("{0}__rsdp_{1}", tbnm, fieldnm));
        }
        #endregion

        #region 私有函数
        private void SetColumnValue(DataRow dr, string fieldnm, object value)
        {
            DataColumn c = dr.Table.Columns[fieldnm];
            if (c != null)
                dr[c] = value;
        }
        private void SetPrimaryKeyWithMastTB(DataTable table, DataTable mdt)
        {
            ColExtendedProperties colextp = null;
            foreach (DataRow dr in table.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;
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
        }

        private void SetDTFirstRowColValue(DataTable dt, string col, object val)
        {
            if (dt.Rows.Count > 0)
            {
                DataTableHelp.SetColomnValue(dt.Rows[0], col, val);
            }
            else
            {
                if (!LibSysUtils.IsNULLOrEmpty(val))
                {
                    DataRow row = dt.NewRow();
                    dt.Rows.Add(row);
                    DataTableHelp.SetColomnValue(row, col, val);
                }
            }
        }

        /// <summary>从数据库取出的二进制的字段图片转为Base64二进制</summary>
        /// <param name="dt"></param>
        private void ConvertBinaryColValue(DataTable dt)
        {
            List<DataColumn> binarycols = new List<DataColumn>();
            foreach (DataColumn c in dt.Columns)
            {
                if (c.DataType.Equals(typeof(byte[])))
                {
                    binarycols.Add(c);

                }
            }
            if (binarycols.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn o in binarycols)
                    {
                        dr[o] = dr[o] == DBNull.Value ? dr[o] : Convert.FromBase64String(System.Text.Encoding.ASCII.GetString((byte[])dr[o]));
                    }
                }
            }
        }

        private ActionResult PermissionResult(string progid, string package)
        {
            LibPermissionSource permission = ModelManager.GetModelBypath<LibPermissionSource>(this.ModelRootPath, progid, package);
            if (permission != null && !permission.IsMenu)
            {
                //msg000000007	该功能未开放
                return View("NotPermission", new NotPermission { Message = AppCom.GetMessageDesc("msg000000007") });
            }
            if (permission == null)
            {
                //msg000000008	功能{0}未配置权限模型
                return View("NotPermission", new NotPermission { Message = string.Format(AppCom.GetMessageDesc("msg000000008"), progid) });
            }
            return null;
        }
        #endregion

        #region 公开函数
        /// <summary>
        /// 取关联的父表
        /// </summary>
        /// <param name="tbnm"></param>
        /// <returns></returns>
        public DataTable GetParentTable(string tbnm)
        {
            DataTable dt = null;
            foreach (var libtb in this.LibTables)
            {
                dt = libtb.Tables.FirstOrDefault(i => i.TableName == tbnm).DataTable;
            }
            return InternalGetParentTable(dt);
        }

        /// <summary>
        /// 取关联的父表
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public DataTable InternalGetParentTable(DataTable tb)
        {
            if (tb != null)
            {
                TableExtendedProperties extprop = tb.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                if (extprop != null)
                {
                    foreach (var item in this.LibTables)
                    {
                        for (int n = 0; n < item.Tables.Length; n++)
                        {
                            if (((TableExtendedProperties)item.Tables[n].DataTable.ExtendedProperties[SysConstManage.ExtProp]).TableIndex == extprop.RelateTableIndex)
                            {
                                return item.Tables[n].DataTable;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 取关联的子表
        /// </summary>
        /// <param name="indexs">TableIndex集合</param>
        /// <returns></returns>
        public List<DataTable> GetChildTableByIndex(List<int> indexs)
        {
            List<DataTable> result = new List<DataTable>();
            TableExtendedProperties extprop = null;
            foreach (int index in indexs)
            {
                foreach (var libtb in this.LibTables)
                {
                    foreach (LibTableObj tbobj in libtb.Tables)
                    {
                        extprop = tbobj.DataTable.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                        if (extprop != null && extprop.TableIndex == index)
                        {
                            result.AddRange(InternalGetChildTable(tbobj.DataTable));
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 取关联的子表
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public List<DataTable> InternalGetChildTable(DataTable tb)
        {
            List<DataTable> result = new List<DataTable>();
            if (tb != null)
            {
                TableExtendedProperties extprop = tb.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                TableExtendedProperties extprop2 = null;
                if (extprop != null)
                {
                    foreach (var item in this.LibTables)
                    {
                        for (int n = 0; n < item.Tables.Length; n++)
                        {
                            extprop2 = ((TableExtendedProperties)item.Tables[n].DataTable.ExtendedProperties[SysConstManage.ExtProp]);
                            if (extprop2.RelateTableIndex == extprop.TableIndex && extprop2.TableIndex!=extprop.TableIndex)
                            {
                                result.Add(item.Tables[n].DataTable);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public DataTable GetTableByIndex(int index)
        {
            foreach (LibTable libtb in this.LibTables)
            {
                foreach (LibTableObj tableobj in libtb.Tables)
                {
                    TableExtendedProperties extprop = tableobj.DataTable.ExtendedProperties[SysConstManage.ExtProp] as TableExtendedProperties;
                    if (extprop != null && extprop.TableIndex == index)
                    {
                        return tableobj.DataTable;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}