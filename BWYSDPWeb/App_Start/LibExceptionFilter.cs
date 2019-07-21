using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.App_Start
{
    public class LibException : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception.GetType().Equals(typeof(LibExceptionBase)))
            {
                filterContext.Result = new JsonResult
                {

                    Data = new { success = false, code = 520, msg = filterContext.Exception.Message },

                    JsonRequestBehavior = JsonRequestBehavior.AllowGet

                };
            }
            //filterContext.ExceptionHandled = true;
            //filterContext.ExceptionHandled = false;
            //base.OnException(filterContext);
        }
    }
}