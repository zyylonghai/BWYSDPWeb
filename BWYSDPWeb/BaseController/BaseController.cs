using BWYSDPWeb.Models;
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

      

        #endregion

        public BaseController()
        {
            var request = System.Web.HttpContext.Current.Request;
            this.ProgID = request.Params["sdp_pageid"] ?? string.Empty;
            this.Package = GetCookievalue(SysConstManage.PageinfoCookieNm, this.ProgID);
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

        #region Session
        public void AddorUpdatePageInfo(string sessionNm,PageInfo info)
        {
            Dictionary<string,PageInfo> sessionobj = Session[sessionNm] as Dictionary<string, PageInfo>;
            if (sessionobj == null)
            {
                sessionobj = new Dictionary<string,PageInfo>();
            }
        }
        #endregion
    }
}