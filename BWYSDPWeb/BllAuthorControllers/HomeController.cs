using BWYSDPWeb.BaseController;
using SDPCRL.COM;
using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.Controllers
{
    public class HomeController : DataBaseController
    {
        private string aa = string.Empty;
        public ActionResult Index()
        {
            var userinfo = Session[SysConstManage.sdp_userinfo];
            if (userinfo == null)
                return View("Login");
            else
                return View();
        }
        [HttpPost]
        public ActionResult login()
        {
            var formparams = this.Request.Form;
            Models.UserInfo userInfo = new Models.UserInfo();
            userInfo.UserId = formparams["userId"];
            userInfo.UserNm = formparams[""];
            userInfo.Language =(Language) Convert.ToInt32(formparams["language"]);
            
            Session[SysConstManage.sdp_userinfo] =userInfo;
            return RedirectToAction("Index");
        }

    }
}