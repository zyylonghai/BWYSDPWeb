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

namespace BWYSDPWeb.BaseController
{
    public class BaseController : Controller
    {
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
        /// 操作动作（新增Add，编辑Edit，删除Delete）
        /// </summary>
        public OperatAction OperatAction
        {
            get;
            set;
        }



        #endregion

        public BaseController()
        {
            var request = System.Web.HttpContext.Current.Request;
            this.ProgID = request.Params["sdp_pageid"] ?? string.Empty;
            this.DSID = request.Params["sdp_dsid"] ?? string.Empty;
            this.Package = GetCookievalue(SysConstManage.PageinfoCookieNm, this.ProgID);
            var action =System.Web.HttpContext.Current.Session[SysConstManage.OperateAction];
            this.OperatAction = action==null ?OperatAction.None :(OperatAction)action;
            this.LibTables = GetTableSchema(this.DSID);
            if (this.LibTables != null)
            {
                foreach (LibTable item in this.LibTables)
                {
                    foreach (DataTable dt in item.Tables)
                    {
                        dt.RowChanged += Dt_RowChanged;
                    }
                }
            }
            #region get temp data from db
            TempHelp sQLiteHelp = new TempHelp("TempData");
            DataTable result= sQLiteHelp.GetTempData(System.Web.HttpContext.Current.Session.SessionID, this.ProgID);
            string str = JsonConvert.SerializeObject(result);
            DataTable table = JsonConvert.DeserializeObject<DataTable>(str);
            //if (result.Rows.Count > 0)
            //{
            //    foreach (LibTable item in this.LibTables)
            //    {
            //        foreach (DataTable tb in item.Tables)
            //        {
            //            DataRow[] rows = result.Select(string.Format("tableNm='{0}'", tb.TableName));
            //            DataRow newdr = tb.NewRow();
            //            int rowid = 0;
            //            foreach (DataRow dr in rows)
            //            {
            //                if ((int)dr["rowid"] == rowid)
            //                {
            //                    newdr[dr["fieldnm"].ToString()] = dr["fieldvalue"];
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion
            //this.Package = GetCookievalue(SysConstManage.PageinfoCookieNm, SysConstManage.PackageCookieKey);
            //this.ProgID = GetCookievalue(SysConstManage.PageinfoCookieNm, SysConstManage.ProgidCookieKey);
        }

        private void Dt_RowChanged(object sender, DataRowChangeEventArgs e)
        {

            Bll.DelegateFactory df = new Bll.DelegateFactory();
            df.SaveRowChange(System.Web.HttpContext.Current.Session.SessionID, this.ProgID, e.Row, e.Action,(int)this.OperatAction);
            //TempDataDelegate compressfile = new TempDataDelegate(InsertTemp);
            //AsyncCallback callback = new AsyncCallback(CallBackMethod);
            //IAsyncResult iar = compressfile.BeginInvoke(e.Row ,e.Action , callback, compressfile);
        }

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
            LibTable[] tbs= System.Web.HttpContext.Current.Session[this.DSID] as LibTable[];
            //this.Tables= System.Web.HttpContext.Current.Session[this.DSID] as DataTable[];
            if (tbs == null)
            {
                #region
                CreateTableSchemaHelp createTable = new CreateTableSchemaHelp(string.Format(@"{0}Views", Server.MapPath("/").Replace("//", "")));
                tbs = createTable.CreateTableSchema(this.DSID, this.Package);
                Session[this.DSID] = tbs;
                this.LibTables = new LibTable[tbs.Length];
                //for (int i = 0; i < tbs.Length; i++)
                //{
                //    CopyTablesSchema(tbs[i].Tables, this.Tables[i].Tables);
                //}
                CopyTablesSchema(tbs, this.LibTables);


                if (this.LibTables != null)
                {
                    foreach (LibTable item in this.LibTables)
                    {
                        foreach (DataTable dt in item.Tables)
                        {
                            dt.RowChanged += Dt_RowChanged;
                        }
                    }
                }
                #endregion

                #region 直接存session
                //CreateTableSchemaHelp createTable = new CreateTableSchemaHelp(string.Format(@"{0}Views", Server.MapPath("/").Replace("//", "")));
                //tbs = createTable.CreateTableSchema(this.DSID, this.Package);
                //Session[this.DSID] = tbs;
                //this.LibTables = tbs;
                #endregion

            }
            else
            {
                foreach (var item in tbs)
                {
                    foreach (DataTable d in item.Tables)
                    {
                        d.Clear();
                    }
                }
            }

        }

        public LibTable[] GetTableSchema(string key)
        {
            #region
            LibTable[] tbs = System.Web.HttpContext.Current.Session[key] as LibTable[];
            if (tbs == null) return null;
            LibTable[] result = new LibTable[tbs.Length];
            //for (int i = 0; i < tbs.Length; i++)
            //{
            //    CopyTablesSchema(tbs[i].Tables, result[i].Tables);
            //}
            CopyTablesSchema(tbs, result);
            #endregion

            #region 直接存session
            //LibTable[] tbs = System.Web.HttpContext.Current.Session[key] as LibTable[];
            //if (tbs == null) return null;
            //LibTable[] result = tbs;
            #endregion
            return result;
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
                        f.FieldNm =string.Format("{0}_{1}",item.Key,s);
                        if (col.DataType.Equals(typeof(DateTime))|| col.DataType.Equals(typeof(Date)))
                        {
                            f.FieldValue = dt.Rows[0][col].ToString();

                        }
                        //else if (col.DataType.Equals(typeof(Date)))
                        //{
                        //    f.FieldValue = dt.Rows[0][col].ToString();
                        //}
                        else
                            f.FieldValue = dt.Rows[0][col];
                        fieldlst.Add(f);
                    }
                }
            }
            #endregion

            return Json(new { sdp_flag=0, sdp_data= fieldlst },JsonRequestBehavior.AllowGet);
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
                for(int n=0;n< srcDT[i].Tables.Length;n++)
                {
                    targDT[i].Tables[n] = srcDT[i].Tables[n].Copy();
                }
                //targDT[i] = srcDT[i].Copy();
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
    }

    public enum OperatAction
    {
        None=-1,
        Add=0,
        Edit=1,
        Delet=2
    }
}