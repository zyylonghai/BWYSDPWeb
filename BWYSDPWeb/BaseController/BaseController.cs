using Bll;
using BWYSDPWeb.Models;
using Newtonsoft.Json;
using SDPCRL.COM;
using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Caching;
using BWYSDPWeb.Com;

namespace BWYSDPWeb.BaseController
{
    public class BaseController : Controller,IlibException 
    {
        #region 私有属性
        private string _rootPath = string.Empty;
        private BllDataBase _bll = null;
        //private List<LibMessage> MsgList = null;
        private SessionInfo _sessionobj = null;
        #endregion
        #region 公开属性

        public LibTable[] LibTables;
        /// <summary>
        /// 功能ID即排版模型id
        /// </summary>
        public string ProgID
        {
            get;
            set;
        }
        /// <summary>
        /// 所属包
        /// </summary>
        public string Package
        {
            get;
            set;
        }
        /// <summary>
        /// 数据源模型id
        /// </summary>
        public string DSID
        {
            get;
            set;
        }
        /// <summary>
        /// 功能操作动作（新增Add，编辑Edit，删除Delete）
        /// </summary>
        public OperatAction OperatAction
        {
            get;
            set;
        }
        /// <summary>
        /// 当前功能存储在Session的信息
        /// </summary>
        public SessionInfo SessionObj {
            get {
                _sessionobj = (SessionInfo)System.Web.HttpContext.Current.Session[this.ProgID];
                if (_sessionobj == null) _sessionobj = new SessionInfo();
                return _sessionobj;
            }
            //set {
            //    Session[this.ProgID] = value;
            //}
        }
        /// <summary>
        /// 错误，警告等信息集
        /// </summary>
        public List<LibMessage> MsgList { get; set; }
        #endregion

        public BaseController()
        {
            var request = System.Web.HttpContext.Current.Request;
            this._rootPath = request.PhysicalApplicationPath;
            this.ProgID = request.Params["sdp_pageid"] ?? string.Empty;
            this.DSID = request.Params["sdp_dsid"] ?? string.Empty;
            this.Package = GetCookievalue(SysConstManage.PageinfoCookieNm, this.ProgID);
            //var action = System.Web.HttpContext.Current.Session[SysConstManage.OperateAction];
            //this.OperatAction = action == null ? OperatAction.None : (OperatAction)action;
            this.OperatAction = this.SessionObj.OperateAction;
            this.LibTables = GetTableSchema(this.DSID);
            //if (this.LibTables != null)
            //{
            //    foreach (LibTable item in this.LibTables)
            //    {
            //        foreach (DataTable dt in item.Tables)
            //        {
            //            dt.RowChanged += Dt_RowChanged;
            //        }
            //    }
            //}
            #region get temp data from db
            if (this.LibTables == null && !request.Url.ToString().Contains("BasePageLoad"))
            {
                this.LibTables = DoCreateTableSchema();
                GetTempDataFromDB();
            }
            #endregion
            //this.Package = GetCookievalue(SysConstManage.PageinfoCookieNm, SysConstManage.PackageCookieKey);
            //this.ProgID = GetCookievalue(SysConstManage.PageinfoCookieNm, SysConstManage.ProgidCookieKey);
        }

        //private void Dt_RowChanged(object sender, DataRowChangeEventArgs e)
        //{

        //    Bll.DelegateFactory df = new Bll.DelegateFactory();
        //    df.SaveRowChange(System.Web.HttpContext.Current.Session.SessionID, this.ProgID, e.Row, e.Action, (int)this.OperatAction);
        //    //TempDataDelegate compressfile = new TempDataDelegate(InsertTemp);
        //    //AsyncCallback callback = new AsyncCallback(CallBackMethod);
        //    //IAsyncResult iar = compressfile.BeginInvoke(e.Row ,e.Action , callback, compressfile);
        //}

        // GET: Base
        //public ActionResult Index()
        //{
        //    return View();
        //}
        #region cookies
        public void AddorUpdateCookies(string cookieNm, string key, string value)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieNm];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieNm);

                //cookie.Values.Add(key, value);
                //cookie.Values.Add("package", this.Package);
            }
            //else
            //{
            if (cookie.Values[key] != null)
            {
                cookie.Values.Set(key, value);
            }
            else
            {
                cookie.Values.Add(key, value);
            }
            //}
            System.Web.HttpContext.Current.Response.AppendCookie(cookie);
        }
        public string GetCookievalue(string cookieNm, string key)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieNm];
            if (cookie != null)
            {
                return cookie.Values[key];
            }
            return string.Empty;
        }
        #endregion

        #region TableSchema
        public void CreateTableSchema()
        {
            #region
            //LibTable[] tbs = System.Web.HttpContext.Current.Session[this.DSID] as LibTable[];
            ////this.Tables= System.Web.HttpContext.Current.Session[this.DSID] as DataTable[];
            //if (tbs == null)
            //{
            //    #region
            //    //CreateTableSchemaHelp createTable = new CreateTableSchemaHelp(string.Format(@"{0}Views", Server.MapPath("/").Replace("//", "")));
            //    //tbs = createTable.CreateTableSchema(this.DSID, this.Package);
            //    //Session[this.DSID] = tbs;
            //    //this.LibTables = new LibTable[tbs.Length];
            //    ////for (int i = 0; i < tbs.Length; i++)
            //    ////{
            //    ////    CopyTablesSchema(tbs[i].Tables, this.Tables[i].Tables);
            //    ////}
            //    //CopyTablesSchema(tbs, this.LibTables);


            //    //if (this.LibTables != null)
            //    //{
            //    //    foreach (LibTable item in this.LibTables)
            //    //    {
            //    //        foreach (DataTable dt in item.Tables)
            //    //        {
            //    //            dt.RowChanged += Dt_RowChanged;
            //    //        }
            //    //    }
            //    //}
            //    #endregion

            //    #region 直接存session
            //    CreateTableSchemaHelp createTable = new CreateTableSchemaHelp(string.Format(@"{0}Views", Server.MapPath("/").Replace("//", "")));
            //    tbs = createTable.CreateTableSchema(this.DSID, this.Package);
            //    Session[this.DSID] = tbs;
            //    this.LibTables = tbs;
            //    #endregion

            //}
            //else
            //{
            //    foreach (var item in tbs)
            //    {
            //        foreach (DataTable d in item.Tables)
            //        {
            //            d.Clear();
            //        }
            //    }
            //}
            #endregion
            #region 存cache
            CachHelp cachelp = new CachHelp();
            string key = string.Format("{0}_{1}", System.Web.HttpContext.Current.Session.SessionID, this.ProgID);
            LibTable[] tbs = cachelp.GetCach(key) as LibTable[];

            tbs = DoCreateTableSchema();
            cachelp.RemoveCache(key);
            cachelp.AddCachItem(key, tbs, this.ProgID);
            this.LibTables = tbs;
            //if (tbs == null)
            //{
            //    //CreateTableSchemaHelp createTable = new CreateTableSchemaHelp(string.Format(@"{0}Views", Server.MapPath("/").Replace("//", "")));
            //    //tbs = createTable.CreateTableSchema(this.DSID, this.Package);
            //    //cachelp.AddCachItem(string.Format("{0}_{1}", System.Web.HttpContext.Current.Session.SessionID, this.ProgID), tbs,this.ProgID);
            //    //this.LibTables = tbs;
            //    tbs = DoCreateTableSchema();
            //    cachelp.AddCachItem(string.Format("{0}_{1}", System.Web.HttpContext.Current.Session.SessionID, this.ProgID), tbs, this.ProgID);
            //    this.LibTables = tbs;
            //}
            //if (tbs != null)
            //{
            //    foreach (var item in tbs)
            //    {
            //        foreach (DataTable d in item.Tables)
            //        {
            //            //d.Reset();
            //            d.Clear();
            //        }
            //    }
            //}
            #endregion

        }

        public LibTable[] GetTableSchema(string key)
        {
            #region
            //LibTable[] tbs = System.Web.HttpContext.Current.Session[key] as LibTable[];
            //if (tbs == null) return null;
            //LibTable[] result = new LibTable[tbs.Length];
            ////for (int i = 0; i < tbs.Length; i++)
            ////{
            ////    CopyTablesSchema(tbs[i].Tables, result[i].Tables);
            ////}
            //CopyTablesSchema(tbs, result);
            #endregion

            #region 直接存session
            //LibTable[] tbs = System.Web.HttpContext.Current.Session[key] as LibTable[];
            //if (tbs == null) return null;
            //LibTable[] result = tbs;
            #endregion
            #region 存cache
            CachHelp cachelp = new CachHelp();
            LibTable[] tbs = cachelp.GetCach(string.Format("{0}_{1}", System.Web.HttpContext.Current.Session.SessionID, this.ProgID)) as LibTable[];
            if (tbs == null) return null;
            //LibTable[] result = tbs;
            //LibTable[] result = new LibTable[tbs.Length];
            //CopyTablesSchema(tbs, result);
            #endregion
            //return result;
            return tbs;
        }
        #endregion

        #region ActionResult 扩展
        public JsonResult LibJson()
        {
            //var result = new { data = this.Tables };
            #region 取formfield并填值
            //Bll.SQLiteHelp sQLiteHelp = new Bll.SQLiteHelp("TempData");
            TempHelp sQLiteHelp = new TempHelp("TempData");
            //string a = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            Dictionary<string, List<string>> fields = sQLiteHelp.SelectFormfield(this.ProgID);
            //string b = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            List<FormFields> fieldlst = new List<FormFields>();
            FormFields f = null;
            DataTable dt = null;
            DataColumn col = null;
            foreach (KeyValuePair<string, List<string>> item in fields)
            {
                foreach (LibTable t in this.LibTables)
                {
                    dt = t.Tables.FirstOrDefault(i => i.TableName == item.Key);
                    if (dt != null)
                        break;
                }
                if (dt.Rows.Count == 0) continue;
                if (item.Value != null)
                {
                    foreach (string s in item.Value)
                    {
                        col = dt.Columns[s];
                        f = new FormFields();
                        f.ProgId = this.ProgID;
                        f.FieldNm = string.Format("{0}{2}{1}", item.Key, s,SysConstManage .Underline);
                        if (col.DataType.Equals(typeof(DateTime)) || col.DataType.Equals(typeof(Date)))
                        {
                            f.FieldValue = dt.Rows[0][col].ToString();

                        }
                        else
                            f.FieldValue = dt.Rows[0][col];
                        fieldlst.Add(f);
                    }
                }
            }
            #endregion

            bool haserror =this.MsgList!=null && this.MsgList.FirstOrDefault(i => i.MsgType == LibMessageType.Error) != null;
            return Json(new { sdp_flag = 0, sdp_data = fieldlst,sdp_msglist=this.MsgList,sdp_haserror=haserror }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LibJson(DataRow row)
        {
            List<FormFields> fieldlst = new List<FormFields>();
            FormFields f = null;
            if (row != null)
            {
                foreach (DataColumn col in row.Table.Columns)
                {
                    f = new FormFields();
                    f.ProgId = this.ProgID;
                    f.FieldNm = string.Format("{0}{2}{1}", row.Table.TableName, col.ColumnName, SysConstManage.Underline);
                    if (col.DataType.Equals(typeof(DateTime)) || col.DataType.Equals(typeof(Date)))
                    {
                        f.FieldValue = row[col].ToString();

                    }
                    else
                        f.FieldValue = row[col];
                    fieldlst.Add(f);
                }
            }
            return Json(new { sdp_flag = 0, sdp_data = fieldlst }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 私有函数
        private void CopyTablesSchema(LibTable[] srcDT, LibTable[] targDT)
        {
            if (srcDT == null) return;
            if (targDT == null) targDT = new LibTable[srcDT.Length];

            for (int i = 0; i < srcDT.Length; i++)
            {
                targDT[i] = new LibTable(srcDT[i].Name);
                targDT[i].Tables = new DataTable[srcDT[i].Tables.Length];
                for (int n = 0; n < srcDT[i].Tables.Length; n++)
                {
                    targDT[i].Tables[n] = srcDT[i].Tables[n].Copy();
                }
                //targDT[i] = srcDT[i].Copy();
            }
        }

        private LibTable[] DoCreateTableSchema()
        {
            if (string.IsNullOrEmpty(this.DSID)) return null;
            CreateTableSchemaHelp createTable = new CreateTableSchemaHelp(string.Format(@"{0}Views", this._rootPath.Replace("//", "")));
            LibTable[] tbs = createTable.CreateTableSchema(this.DSID, this.Package);
            //cachelp.AddCachItem(string.Format("{0}_{1}", System.Web.HttpContext.Current.Session.SessionID, this.ProgID), tbs, this.ProgID);
            return tbs;
        }

        //private void ClearLibTableData(LibTable libTable)
        //{
        //    foreach (var item in tbs)
        //    {
        //        foreach (DataTable d in item.Tables)
        //        {
        //            d.Clear();
        //        }
        //    }
        //}

        private void GetTempDataFromDB()
        {
            int rowcout = 0;
            TempHelp sQLiteHelp = new TempHelp("TempData");
            DataTable result = null;

            if (this.LibTables != null)
            {
                try
                {
                    DataRow newrow = null;
                    int rowindex = -2;
                    DataTable tb = null;
                    DataColumnCollection cols = null;
                    DataColumn[] primarykeycols = null;
                    Dictionary<int, int> rowstate = null;

                    DataColumn colrowid;
                    DataColumn colfieldnm;
                    DataColumn colaction;
                    DataColumn colvalue;
                    DataColumn cololdvalue;
                    //string a = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                    foreach (LibTable item in this.LibTables)
                    {
                        for (int i = 0; i < item.Tables.Length; i++)
                        {
                            tb = item.Tables[i];
                            cols = tb.Columns;
                            primarykeycols = tb.PrimaryKey;
                            result = sQLiteHelp.GetTempData(System.Web.HttpContext.Current.Session.SessionID, this.ProgID, tb.TableName, ref rowcout);
                            if (result != null)
                            {
                                rowindex = -2;
                                colrowid = result.Columns["rowid"];
                                colfieldnm = result.Columns["fieldnm"];
                                colaction = result.Columns["actions"];
                                rowstate = new Dictionary<int, int>();
                                colvalue = result.Columns["fieldvalue"];
                                cololdvalue = result.Columns["oldfieldvalue"];
                                foreach (DataRow dr in result.Rows)
                                {
                                    if (rowindex != (int)dr[colrowid])
                                    {
                                        newrow = tb.NewRow();
                                        foreach (var c in primarykeycols)
                                        {
                                            if (c.DataType.Equals(typeof(int)) || c.DataType.Equals(typeof(decimal)) || c.DataType.Equals(typeof(long)))
                                            {
                                                newrow[c] = 0;
                                                continue;
                                            }
                                            newrow[c] = new object();
                                            
                                        }
                                        tb.Rows.Add(newrow);
                                        rowindex = (int)dr[colrowid];
                                        switch ((short)dr[colaction])
                                        {
                                            case 0: //新增状态
                                                break;
                                            case 1: //修改状态
                                                #region 赋值
                                                if (cols[dr[colfieldnm].ToString()].DataType == typeof(Date))
                                                {
                                                    newrow[dr[colfieldnm].ToString()] = new Date { value= dr[cololdvalue].ToString() };
                                                }
                                                else
                                                    newrow[dr[colfieldnm].ToString()] = dr[cololdvalue];
                                                #endregion
                                                newrow.AcceptChanges();
                                                break;
                                            case 2: //删除状态
                                                #region 赋值
                                                if (cols[dr[colfieldnm].ToString()].DataType == typeof(Date))
                                                {
                                                    newrow[dr[colfieldnm].ToString()] = new Date { value = dr[cololdvalue].ToString() };
                                                }
                                                else
                                                    newrow[dr[colfieldnm].ToString()] = dr[cololdvalue];
                                                #endregion
                                                rowstate.Add(rowindex, 2);
                                                break;
                                            case -1: //未更改状态
                                                rowstate.Add(rowindex, -1);
                                                break;
                                        }
                                    }
                                    #region 赋值
                                    if (cols[dr[colfieldnm].ToString()].DataType == typeof(Date))
                                    {
                                        newrow[dr[colfieldnm].ToString()] = new Date { value = dr[colvalue].ToString() };
                                    }
                                    else
                                        newrow[dr[colfieldnm].ToString()] = dr[colvalue];
                                    #endregion
                                }
                                foreach (KeyValuePair<int, int> keyval in rowstate)
                                {
                                    switch (keyval.Value)
                                    {
                                        case 2:
                                            tb.Rows[keyval.Key].AcceptChanges();
                                            tb.Rows[keyval.Key].Delete();
                                            break;
                                        case -1:
                                            tb.Rows[keyval.Key].AcceptChanges();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    //string b = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                }
                catch (Exception ex)
                {
                    ThrowErrorException(ex.Message);
                }
                CachHelp cachelp = new CachHelp();
                cachelp.AddCachItem(string.Format("{0}_{1}", System.Web.HttpContext.Current.Session.SessionID, this.ProgID), this.LibTables, this.ProgID);
            }
        }

        //private void InsertTemp(DataRow dr, DataRowAction action)
        //{
        //    List<string> commandlst = new List<string>();
        //    string sessionid = System.Web.HttpContext.Current.Session.SessionID;
        //    string tbNm = dr.Table.TableName;
        //    int rowindex = dr.Table.Rows.IndexOf(dr);
        //    Bll.SQLiteHelp sQLiteHelp = new Bll.SQLiteHelp("TempData");
        //    switch (action)
        //    {
        //        case DataRowAction.Add:
        //            foreach (DataColumn col in dr.Table.Columns)
        //            {
        //                commandlst.Add(string.Format("insert into [temp] values('{0}','{1}','{2}',{3},'{4}','{5}')",sessionid ,this.ProgID ,tbNm ,rowindex ,col.ColumnName,dr[col].ToString()));
        //            }
        //            break;
        //        case DataRowAction.Change:
        //            break;
        //        case DataRowAction.Delete:
        //            break;
        //    }
        //    sQLiteHelp.Insert(commandlst);
        //}

        //private void CallBackMethod(IAsyncResult ar)
        //{

        //}
        #endregion

        #region 公开函数
        public DalResult ExecuteMethod(string method, params object[] param)
        {
            if (_bll == null) this._bll = new BllDataBase();
            return _bll.ExecuteMethod(this.ProgID, method,this.LibTables, param);
        }



        public DalResult ExecuteDalMethod(string funcId, string method, params object[] param)
        {
            if (_bll == null) this._bll = new BllDataBase();
            return _bll.ExecuteMethod(funcId, method,null, param);
        }

        public DalResult ExecuteSaveMethod(string method, LibTable[] tables)
        {
            if (_bll == null) this._bll = new BllDataBase();
            DalResult dalResult=(DalResult )_bll.ExecuteDalSaveMethod(this.ProgID, method, tables);
            this.AddMessagelist(dalResult.Messagelist);
            return dalResult;
        }

        public void ThrowErrorException(string msg)
        {
            ExceptionHelp help = new ExceptionHelp();
            help.ThrowError(this, msg); 
        }

        public void AddMessage(string msg,LibMessageType msgtype=LibMessageType.Error)
        {
            if (this.MsgList == null) this.MsgList = new List<LibMessage>();
            this.MsgList.Add(new LibMessage { Message = msg, MsgType = msgtype });
        }
        public void AddMessagelist(IEnumerable<LibMessage> msglist)
        {
            if (this.MsgList == null) this.MsgList = new List<LibMessage>();
            this.MsgList.AddRange(msglist);
        }
        #endregion

        #region 受保护方法
        protected override void EndExecute(IAsyncResult asyncResult)
        {
            base.EndExecute(asyncResult);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (this._sessionobj != null)
                Session[this.ProgID] = this._sessionobj;
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
        #endregion

        #region IlibException
        public void BeforeThrow()
        {
            
        }
        #endregion
    }

    public enum OperatAction
    {
        None = 0,
        /// <summary>
        /// 功能处于新增状态
        /// </summary>
        Add = 1,
        /// <summary>
        /// 功能处于编辑状态
        /// </summary>
        Edit = 2,
        /// <summary>
        /// 
        /// </summary>
        Delet = 3,
        /// <summary>
        /// 功能处于预览状态
        /// </summary>
        Preview=4
    }
}