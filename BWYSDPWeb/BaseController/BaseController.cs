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
    public class BaseController : Controller, IlibException
    {
        #region 私有属性
        private string _rootPath = string.Empty;
        private BllDataBase _bll = null;
        //private List<LibMessage> MsgList = null;
        private SessionInfo _sessionobj = null;
        private string _rootpath = string.Empty;
        protected DateTimeOffset TimeOffset { get { return DateTime.Now.AddSeconds(30); } }
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
        /// 功能操作动作（新增Add，编辑Edit，删除Delete,）
        /// </summary>
        public OperatAction OperatAction
        {
            get { return this.SessionObj == null ? this.OperatAction : this.SessionObj.OperateAction; }//set;
        }
        /// <summary>
        /// 当前功能存储在Session的信息
        /// </summary>
        public SessionInfo SessionObj
        {
            get
            {
                if (string.IsNullOrEmpty(this.ProgID)) return _sessionobj;
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
        public Language Language { get; set; }

        public string RootPath {
            get {
                if (string.IsNullOrEmpty (_rootpath ))
                    _rootpath = Server.MapPath("/").Replace("//", "");
                return _rootpath;
            }
        }
        public string ModelRootPath
        {
            get {
                return string.Format(@"{0}Views", this.RootPath);
            }
        }
        /// <summary>
        /// 登陆的用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }
        #endregion

        public BaseController()
        {
            var request = System.Web.HttpContext.Current.Request;
            this._rootPath = request.PhysicalApplicationPath;
            this.ProgID = request.Params["sdp_pageid"] ?? string.Empty;
            if(string.IsNullOrEmpty(this.ProgID))
            {
                this.ProgID = request.Params["progId"] ?? string.Empty;
            }
            this.DSID = request.Params["sdp_dsid"] ?? string.Empty;
            this.Package = GetCookievalue(SysConstManage.PageinfoCookieNm, this.ProgID);
            this.UserInfo = System.Web.HttpContext.Current.Session[SysConstManage.sdp_userinfo] as UserInfo ;
            this.Language = this.UserInfo == null ? Language.CHS : this.UserInfo.Language;
            //var action = System.Web.HttpContext.Current.Session[SysConstManage.OperateAction];
            //this.OperatAction = action == null ? OperatAction.None : (OperatAction)action;
            //this.OperatAction =this.SessionObj==null ?this.OperatAction : this.SessionObj.OperateAction;
            this.LibTables = GetTableSchema(this.ProgID);
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

        // GET: Base
        //public ActionResult Index()
        //{
        //    return View();
        //}
        #region cookies
        public void AddorUpdateCookies(string cookieNm, string key, string value)
        {
            #region
            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[key];
            //if (cookie != null)
            //{
            //    cookie[key] = value;
            //}
            //else
            //{
            //    cookie = new HttpCookie(key, value);
            //}
            //System.Web.HttpContext.Current.Response.AppendCookie(cookie);
            #endregion

            #region 
            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieNm];
            //if (cookie == null)
            //{
            //    cookie = new HttpCookie(cookieNm);
            //    //cookie.Expires = DateTime.Now.AddDays(1);
            //    //cookie.Values.Add(key, value);
            //    //cookie.Values.Add("package", this.Package);
            //}
            ////else
            ////{
            //if (cookie.Values[key] != null)
            //{
            //    cookie.Values.Set(key, value);
            //}
            //else
            //{
            //    cookie.Values.Add(key, value);
            //}
            ////}
            //System.Web.HttpContext.Current.Response.AppendCookie(cookie);
            #endregion 

            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieNm];
            //Dictionary<string, string> values = null;
            //if (cookie == null)
            //{
            //    cookie = new HttpCookie(cookieNm);
            //}
            //if (string.IsNullOrEmpty(cookie.Value))
            //{
            //    values = new Dictionary<string, string>();
            //    values.Add(key, value);
            //}
            //else
            //{
            //    values = DecryptCookie(cookie.Value);
            //    if (values.ContainsKey(key))
            //    {
            //        values[key] = value;
            //    }
            //    else
            //    {
            //        values.Add(key, value);
            //    }

            //}
            //cookie.Value = EncryptionCookie(values);
            //System.Web.HttpContext.Current.Response.AppendCookie(cookie);
            AppCom.AddorUpdateCookies(cookieNm, key, value);

        }
        public string GetCookievalue(string cookieNm, string key)
        {
            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[key];
            //if(cookie!=null) return cookie.Value;
            //return string.Empty;
            #region
            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieNm];
            //if (cookie != null)
            //{
            //    return cookie.Values[key];
            //}
            //return string.Empty;
            #endregion

            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieNm];
            //if (cookie != null)
            //{
            //    Dictionary<string, string> values = DecryptCookie(cookie.Value);
            //    if (values.ContainsKey(key)) return values[key];
            //    return string.Empty;
            //}
            //return string.Empty;
            return AppCom.GetCookievalue(cookieNm, key);
        }

        private Dictionary<string,string> DecryptCookie(string cookievalu)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(cookievalu)) return result;
            byte[] bts = Convert.FromBase64String(cookievalu);
            string[] str = Encoding.Default.GetString(bts).Split ('&');
            string[] item = null;
            if (str != null && str.Length > 0)
            {
                foreach (string s in str)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        item = s.Split('=');
                        result.Add(item[0], item[1]);
                    }
                }
            }
            return result;
        }
        private string EncryptionCookie(Dictionary<string ,string > valus)
        {
            string valu = string.Empty;
            foreach (var item in valus)
            {
                if (!string.IsNullOrEmpty(valu))
                {
                    valu += "&";
                }
                valu += string.Format("{0}={1}", item.Key, item.Value);
            }
            byte[] vals = Encoding.Default.GetBytes(valu);
            return  Convert.ToBase64String(vals);
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
            var policy = new CacheItemPolicy() { AbsoluteExpiration = this.TimeOffset };
            if (tbs != null)
                cachelp.AddCachItem(key, tbs, this.TimeOffset, new LibTableChangeMonitor2(key, tbs, this.ProgID));
            //cachelp.AddCachItem(key, tbs, this.ProgID);
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

        public LibTable[] GetTableSchema(string progid)
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
            LibTable[] tbs = cachelp.GetCach(string.Format("{0}_{1}", System.Web.HttpContext.Current.Session.SessionID, progid)) as LibTable[];
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
            LibTableObj tableObj = null;
            foreach (KeyValuePair<string, List<string>> item in fields)
            {
                if (this.LibTables == null)
                {
                    this.AddMessage(13,null);
                    break;
                }
                foreach (LibTable t in this.LibTables)
                {
                    tableObj = t.Tables.FirstOrDefault(i => i.TableName == item.Key);
                    dt = tableObj ==null ?null :tableObj .DataTable;
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
                        f.Isbinary = false;
                        //f.Isbinary = col.DataType.Equals(typeof(byte[]));
                        f.FieldNm = string.Format("{0}{2}{1}", item.Key, s, SysConstManage.Underline);
                        if (col.DataType.Equals(typeof(DateTime)) || col.DataType.Equals(typeof(Date)))
                        {
                            f.FieldValue = dt.Rows[0][col].ToString();

                        }
                        else if (col.DataType.Equals(typeof(byte[])))
                        {
                            f.Isbinary = true;
                            f.FieldValue = dt.Rows[0][col] != DBNull.Value ? Convert .ToBase64String((byte[])dt.Rows[0][col]) : dt.Rows[0][col];
                            //f.FieldValue = dt.Rows[0][col] != DBNull.Value ? System.Text.Encoding.ASCII.GetString((byte[])dt.Rows[0][col]) : dt.Rows[0][col];
                        }
                        else
                            f.FieldValue = dt.Rows[0][col];
                        fieldlst.Add(f);
                    }
                }
            }
            #endregion
            //System.Web.Script.Serialization.JavaScriptSerializer
            bool haserror = this.MsgList != null && this.MsgList.FirstOrDefault(i => i.MsgType == LibMessageType.Error) != null;
            //return Json(new { sdp_flag = 0, sdp_data = fieldlst, sdp_msglist = this.MsgList, sdp_haserror = haserror }, JsonRequestBehavior.AllowGet);
            return new JsonResult
            {
                Data = new { sdp_flag = 0, sdp_data = fieldlst, sdp_msglist = this.MsgList, sdp_haserror = haserror,sdp_preview=this.SessionObj.OperateAction==OperatAction.Preview },
                MaxJsonLength = int.MaxValue ,
                JsonRequestBehavior= JsonRequestBehavior.AllowGet
            };
        }
        //public JsonResult LibJson(object result)
        //{
        //    bool haserror = this.MsgList != null && this.MsgList.FirstOrDefault(i => i.MsgType == LibMessageType.Error) != null;
        //    return Json(new { sdp_flag = 0, sdp_Msg = fieldlst, sdp_msglist = this.MsgList, sdp_haserror = haserror }, JsonRequestBehavior.AllowGet);
        //}

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
                    f.Isbinary = col.DataType.Equals(typeof(byte[]));
                    f.FieldNm = string.Format("{0}{2}{1}", row.Table.TableName, col.ColumnName, SysConstManage.Underline);
                    if (col.DataType.Equals(typeof(DateTime)) || col.DataType.Equals(typeof(Date)))
                    {
                        f.FieldValue = row[col].ToString();

                    }
                    else if (col.DataType.Equals(typeof(byte[])))
                    {
                        f.Isbinary = true;
                        f.FieldValue = row[col] != DBNull.Value ? Convert.ToBase64String((byte[])row[col]) : row[col];
                        //f.FieldValue = dt.Rows[0][col] != DBNull.Value ? System.Text.Encoding.ASCII.GetString((byte[])dt.Rows[0][col]) : dt.Rows[0][col];
                    }
                    else if (col.DataType.Equals(typeof(bool)))
                    {
                        f.FieldValue = Convert.ToInt32(row[col]);
                    }
                    else
                        f.FieldValue = row[col];
                    fieldlst.Add(f);
                }
            }
            bool haserror = this.MsgList != null && this.MsgList.FirstOrDefault(i => i.MsgType == LibMessageType.Error) != null;
            return new JsonResult
            {
                Data = new { sdp_flag = 0, sdp_data = fieldlst, sdp_msglist = this.MsgList, sdp_haserror = haserror },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            //return Json(new { sdp_flag = 0, sdp_data = fieldlst, sdp_msglist = this.MsgList, sdp_haserror = haserror }, JsonRequestBehavior.AllowGet);
        }

        public string LibReturnForGrid(int total, DataTable dt)
        {
            bool haserror = this.MsgList != null && this.MsgList.FirstOrDefault(i => i.MsgType == LibMessageType.Error) != null;
            var o = new { total = total, rows = dt, sdp_msglist = this.MsgList, sdp_haserror = haserror };
            return JsonConvert.SerializeObject(o);
        }

        public ViewResult LibReturnError(List<ErrorMessage> errors)
        {
            ErrorObject error = new ErrorObject();
            error.Title = "错误信息：";
            //string _msg = string.Empty;
            foreach (var m in errors)
            {
                error.Message += m.Message;
                error.Stack += m.Stack;
                //_msg += m.Message + m.Stack;
            }
            
            //this.ThrowErrorException(_msg);
            return View("Error",error);
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
                targDT[i].Tables = new LibTableObj[srcDT[i].Tables.Length];
                DataTable dt = null;
                for (int n = 0; n < srcDT[i].Tables.Length; n++)
                {
                     dt = srcDT[i].Tables[n].DataTable.Copy();
                    targDT[i].Tables[n] = new LibTableObj (dt);
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
                    Dictionary<string, object> newvalus = new Dictionary<string, object>();

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
                            tb = item.Tables[i].DataTable;
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
                                newvalus.Clear();
                                foreach (DataRow dr in result.Rows)
                                {
                                    if (rowindex != (int)dr[colrowid])
                                    {
                                        if (newvalus.Count > 0)
                                        {
                                            newrow.AcceptChanges();
                                            foreach (var keyvalu in newvalus)
                                            {
                                                SetColumnValue(newrow.Table .Columns, keyvalu .Key, newrow, keyvalu .Value);
                                            }
                                            newvalus.Clear();
                                        }
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
                                    }
                                    #region 赋值
                                    switch ((short)dr[colaction])
                                    {
                                        case 0: //新增状态
                                            SetColumnValue(cols, dr[colfieldnm].ToString(), newrow, dr[colvalue]);
                                            break;
                                        case 1: //修改状态
                                            #region 赋值
                                            SetColumnValue(cols, dr[colfieldnm].ToString(),newrow, dr[cololdvalue]);
                                            newvalus.Add(dr[colfieldnm].ToString(), dr[colvalue]);

                                            #endregion
                                            //newrow.AcceptChanges();
                                            break;
                                        case 2: //删除状态
                                            #region 赋值
                                            SetColumnValue(cols, dr[colfieldnm].ToString(), newrow, dr[cololdvalue]);
                                            newvalus.Add(dr[colfieldnm].ToString(), dr[colvalue]);

                                            #endregion
                                            if (!rowstate.ContainsKey(rowindex))
                                                rowstate.Add(rowindex, 2);
                                            break;
                                        case -1: //未更改状态
                                            SetColumnValue(cols, dr[colfieldnm].ToString(),newrow, dr[colvalue]);
                                            if (!rowstate.ContainsKey(rowindex))
                                                rowstate.Add(rowindex, -1);
                                            break;
                                    }
                                    //SetColumnValue(cols, colfieldnm, colvalue, newrow, dr);
                                    #endregion
                                }
                                if (newvalus.Count > 0)
                                {
                                    newrow.AcceptChanges();
                                    foreach (var keyvalu in newvalus)
                                    {
                                        SetColumnValue(newrow.Table.Columns, keyvalu.Key, newrow, keyvalu.Value);
                                    }
                                    newvalus.Clear();
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
                string key = string.Format("{0}_{1}", System.Web.HttpContext.Current.Session.SessionID, this.ProgID);
                cachelp.AddCachItem(key, this.LibTables, this.TimeOffset, new LibTableChangeMonitor2(key, this.LibTables, this.ProgID));
                //cachelp.AddCachItem(key, this.LibTables, this.ProgID);
            }
        }

        private void SetColumnValue(DataColumnCollection cols, string fieldnm, DataRow newrow,object valu)
        {
            if (cols[fieldnm].DataType == typeof(Date))
            {
                newrow[fieldnm] = new Date { value = valu.ToString() };
            }
            else if (cols[fieldnm].DataType == typeof(byte[]))
            {
                newrow[fieldnm] = Convert.FromBase64String(valu.ToString());
            }
            else if (cols[fieldnm].DataType == typeof(DateTime) && LibSysUtils.IsNULLOrEmpty(valu))
            {
                newrow[fieldnm] = DBNull.Value;
                //if (LibSysUtils.IsNULLOrEmpty(valu))
                //{
                //    
                //}

            }
            else
                newrow[fieldnm] = valu;
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
            string _msg = string.Empty;
            LibClientInfo clientInfo = new LibClientInfo { 
                Language = this.Language, 
                SessionId = System.Web.HttpContext.Current.Session.SessionID,
                UserId = this.UserInfo == null ? string.Empty : this.UserInfo.UserId };
            DalResult dalResult = _bll.ExecuteMethod(clientInfo ,this.ProgID, method, this.LibTables, param);
            if (dalResult != null && dalResult.ErrorMsglst != null && dalResult.ErrorMsglst.Count > 0)
            {
                
                foreach (var m in dalResult.ErrorMsglst)
                {
                    _msg += m.Message + m.Stack;
                }
                this.ThrowErrorException(_msg);
            }
            if (dalResult != null && dalResult.Messagelist != null)
            {
                var error = dalResult.Messagelist.Where(i => i.MsgType == LibMessageType.Error).ToList();
                if (error.Count > 0)
                {
                    foreach (var m in error)
                    {
                        _msg += m.Message;
                    }
                    this.ThrowErrorException(_msg);
                }

            }
            return dalResult;
        }



        public DalResult ExecuteDalMethod(string funcId, string method, params object[] param)
        {
            if (_bll == null) this._bll = new BllDataBase();
            LibClientInfo clientInfo = new LibClientInfo { 
                Language = this.Language, 
                SessionId = System.Web.HttpContext.Current.Session.SessionID, 
                UserId=this.UserInfo==null?string .Empty : this.UserInfo.UserId };
            DalResult dalResult = _bll.ExecuteMethod(clientInfo , funcId, method, null, param);
            string _msg = string.Empty;
            if (dalResult != null && dalResult.ErrorMsglst != null && dalResult.ErrorMsglst.Count > 0)
            {
                foreach (var m in dalResult.ErrorMsglst)
                {
                    _msg += m.Message + m.Stack;
                }
                this.ThrowErrorException(_msg);
            }
            if (dalResult != null && dalResult.Messagelist != null)
            {
                var error = dalResult.Messagelist.Where(i => i.MsgType == LibMessageType.Error).ToList();
                if (error.Count > 0)
                {
                    foreach (var m in error)
                    {
                        _msg += m.Message;  
                    }
                    this.ThrowErrorException(_msg);
                }
 
            }
            return dalResult;
        }

        public DalResult ExecuteSaveMethod(string method, LibTable[] tables)
        {
            if (_bll == null) this._bll = new BllDataBase();
            LibClientInfo clientInfo = new LibClientInfo();
            clientInfo.UserId = this.UserInfo == null ? string.Empty : this.UserInfo.UserId;
            clientInfo.SessionId = System.Web.HttpContext.Current.Session.SessionID;
            clientInfo.Language = this.Language;
            clientInfo.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            DalResult dalResult = (DalResult)_bll.ExecuteDalSaveMethod(clientInfo, this.ProgID, method, tables);
            this.AddMessagelist(dalResult.Messagelist);
            return dalResult;
        }
        /// <summary>
        ///如果是页面处于刷新或提交则页面输出的是一串错误信息的json字符，否则以消息弹出框的形式弹出。
        /// </summary>
        /// <param name="msg">信息</param>
        public void ThrowErrorException(string msg)
        {
            ExceptionHelp help = new ExceptionHelp();
            help.ThrowError(this, msg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgcode">信息代码</param>
        /// <param name="parms">formatter 中的参数值</param>
        public void ThrowErrorException(int msgcode, params object[] parms)
        {
           string message= AppCom.GetMessageDesc(string .Format("msg{0}", msgcode.ToString().PadLeft(9, '0')));
            ExceptionHelp help = new ExceptionHelp();
            help.ThrowError(this, string.Format(message, parms));
        }

        public void AddMessage(int msgcode, object[] parms, LibMessageType msgtype = LibMessageType.Error)
        {
            if (this.MsgList == null) this.MsgList = new List<LibMessage>();
            string message = AppCom.GetMessageDesc(string.Format("msg{0}", msgcode.ToString().PadLeft(9, '0')));
            if (parms != null)
            {
                message = string.Format(message, parms);
            }
            this.MsgList.Add(new LibMessage { Message = message, MsgType = msgtype });
        }

        public void AddMessage(string msg, LibMessageType msgtype = LibMessageType.Error)
        {
            if (this.MsgList == null) this.MsgList = new List<LibMessage>();
            this.MsgList.Add(new LibMessage { Message = msg, MsgType = msgtype });
        }
        public void AddMessagelist(IEnumerable<LibMessage> msglist)
        {
            if (this.MsgList == null) this.MsgList = new List<LibMessage>();
            this.MsgList.AddRange(msglist);
        }

        public string GetFieldDesc(string tablenm,string fieldnm)
        {
            return AppCom.GetFieldDesc((int)this.Language, this.DSID, tablenm, fieldnm);

        }
        public DataTable GetFieldDescBydsid(string dsid)
        {
            //CachHelp cachelp = new CachHelp();
            //DataTable dt = cachelp.GetCach(dsid) as DataTable;
            BllDataBase bll = new BllDataBase(false);
            return bll.GetFieldDescData(dsid,this.Language);
        }

        public string GenerateNoByprogid()
        {
            return (string)ExecuteMethod("GenerateNoByprogid", this.ProgID).Value;
        }

        public DataTable[] GetDataLog(string method, params object[] param)
        {
            if (_bll == null) this._bll = new BllDataBase();
            return _bll.GeDataLog((int)this.Language, method, param);
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

        /// <summary>
        /// 身份验证
        /// </summary>
        protected void Authentication()
        {
            string identityjson = AppCom.GetCookievalue(SysConstManage.sdp_IdentityTick, "key");
            IdentityCredential identityCredential = JsonConvert.DeserializeObject<IdentityCredential>(identityjson);
            if (string.IsNullOrEmpty(identityjson)||!IdentityHelp.CompareTick(this.UserInfo.UserId, identityCredential.CertificateID))
            {
                //msg000000014    身份验证失败!请重新登录
                this.ThrowErrorException(14);
            }
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
        ///// <summary>
        ///// 功能处于搜索后预览状态
        ///// </summary>
        //SearchView = 3,
        /// <summary>
        /// 功能处于保存后预览状态
        /// </summary>
        Preview = 4
    }

    /// <summary>表格上的新增，删除，编辑，复制等按钮动作</summary>
    public enum DataGridAction
    {
        None=-1,
        Add=0,
        Edit=1,
        Delet=2,
        Copy=3
    }
}