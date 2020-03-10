using SDPCRL.CORE;
using System;
using System.Web.Mvc;

namespace BWYSDPWeb.App_Start
{
    public class LibException : HandleErrorAttribute
    {
        LogHelp logHelp { get { return new LogHelp(System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", "")); } }
        public override void OnException(ExceptionContext filterContext)
        {
            #region 写日志
            logHelp.WriteLog(filterContext .HttpContext.Request .Url .ToString (),filterContext.Exception);
            #endregion 
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (filterContext.Exception.GetType().Equals(typeof(System.Runtime.Remoting.RemotingException)))
                {
                    filterContext.HttpContext.Response.RedirectToRoute(new { controller = "Server", action = "ServerPage2" });
                }
                else 
                    filterContext.HttpContext.Response.RedirectToRoute(new { controller = "Home", action = "ErrorView", msg = DM5Help.Md5Encrypt(filterContext.Exception.Message), title = DM5Help.Md5Encrypt("出现异常")});
            }
            if (filterContext.Exception.GetType().Equals(typeof(LibExceptionBase)))
            {
                filterContext.Result = new JsonResult
                {

                    Data = new { success = false, code = 520, msg = filterContext.Exception.Message },

                    JsonRequestBehavior = JsonRequestBehavior.AllowGet

                };
            }
            else
            {
                filterContext.Result = new JsonResult
                {

                    Data = new { success = false, code = 510, msg = filterContext.Exception.Message },

                    JsonRequestBehavior = JsonRequestBehavior.AllowGet

                };
            }
            //filterContext.ExceptionHandled = true;
            //filterContext.ExceptionHandled = false;
            //base.OnException(filterContext);
        }
    }
}