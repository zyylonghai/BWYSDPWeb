using BWYSDPWeb.BaseController;
using SDPCRL.COM;
using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            //userInfo.UserNm = "admintest";
            userInfo.Language = (Language)Convert.ToInt32(formparams["language"]);
            this.Language = userInfo.Language;
            DalResult result = this.ExecuteDalMethod("Account", "Login", userInfo.UserId, formparams["password"]);
            LoginInfo lginfo = (LoginInfo)result.Value;
            if (lginfo.loginResult==1)
            {
                userInfo.UserNm = lginfo.UserNm;
                //userInfo.Language = (Language)Convert.ToInt32(formparams["language"]);
                string tick = string.Empty;
                string md5 = DM5Help.Md5Encrypt(userInfo.UserId);
                foreach (char c in md5)
                {
                    tick += ((Int32)c).ToString();
                }
                tick =string .Format("{0}${1}", tick , userInfo.UserNm);
                FormsAuthentication.SetAuthCookie(tick, false);
                Session[SysConstManage.sdp_userinfo] = userInfo;
            }
            if (lginfo.loginResult == 3)//密码错误
            {
                this.ThrowErrorException("密码错误");
                //this.AddMessage("密码错误");
                //this.AddMessage()
            }
            return RedirectToAction("Index");
        }

        public ActionResult LoginOut()
        {
            Session.Clear();
            //Session[SysConstManage.sdp_userinfo] = null;
            return View("Login");
        }

        public ActionResult SysSetting()
        {
            var userinfo = Session[SysConstManage.sdp_userinfo];
            if (userinfo == null)
                return View("Login");
            else
                return View("SysSetting");
        }

    }
}