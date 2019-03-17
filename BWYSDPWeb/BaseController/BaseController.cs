using BWYSDPWeb.Models;
using Newtonsoft.Json;
using SDPCRL.COM;
using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BaseController
{
    public class BaseController : Controller
    {
        #region 公开属性
        public DataTable[] Tables;

        public string ProgID
        {
            get;
            set;
        }

        public string Package
        {
            get;
            set;
        }

        public string DSID
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
            this.Tables = GetTableSchema(this.DSID);
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
            DataTable[] tbs= System.Web.HttpContext.Current.Session[this.DSID] as DataTable[]; 
            //this.Tables= System.Web.HttpContext.Current.Session[this.DSID] as DataTable[];
            if (tbs == null)
            {
                CreateTableSchemaHelp createTable = new CreateTableSchemaHelp(string.Format(@"{0}Views", Server.MapPath("/").Replace("//", "")));
                tbs = createTable.CreateTableSchema(this.DSID, this.Package);
                Session[this.DSID] = tbs;
                CopyTablesSchema(tbs,ref this.Tables);

            }

        }

        public DataTable[] GetTableSchema(string key)
        {
            DataTable[] tbs = System.Web.HttpContext.Current.Session[key] as DataTable[];
            if (tbs == null) return null;
            DataTable[] result = new DataTable[tbs.Length];
            CopyTablesSchema(tbs,ref result);
            return result;
        }
        #endregion

        #region ActionResult 扩展
        public JsonResult LibJson()
        {
            var result = new { data = this.Tables };
            
            return Json(new { data= JsonConvert.SerializeObject(this.Tables[0])},JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 私有函数
        private void CopyTablesSchema(DataTable[] srcDT,ref DataTable[] targDT)
        {
            if (srcDT == null) return;
            if (targDT == null) targDT = new DataTable[srcDT.Length];
            for (int i = 0; i < srcDT.Length; i++)
            {
                targDT[i] = srcDT[i].Copy();
            }
        }
        #endregion
    }
}