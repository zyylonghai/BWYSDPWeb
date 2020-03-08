using BWYSDPWeb.BaseController;
using BWYSDPWeb.Models;
using Newtonsoft.Json;
using SDPCRL.COM;
using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                string tick = IdentityHelp.GenerateTick (userInfo.UserId);
                IdentityCredential identity = new IdentityCredential
                {
                    CertificateID = tick,
                    UserNm = lginfo.UserNm,
                    HasAdminRole = lginfo.HasAdminRole
                };
                string identitystr = JsonConvert.SerializeObject(identity);
                Com.AppCom.AddorUpdateCookies(SysConstManage .sdp_IdentityTick, "key", identitystr);
                FormsAuthentication.SetAuthCookie(userInfo.UserNm, false);
                Session[SysConstManage.sdp_userinfo] = userInfo;
            }
            if (lginfo.loginResult == 3)//密码错误
            {
                //msg000000015     密码错误
                this.ThrowErrorException(15);
                //this.AddMessage("密码错误");
                //this.AddMessage()
            }
            if (lginfo.loginResult == -1)
            {
                //msg000000016    用户{0}不存在
                this.ThrowErrorException(16,userInfo.UserId);
            }
            return RedirectToAction("Index");
        }

        public ActionResult LoginPage()
        {
            return View("Login");
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
            {
                this.Authentication();
                return View("SysSetting");
            }
        }

        public ActionResult ErrorView(string  msg, string title)
        {
            ErrorObject error = new ErrorObject();
            error.Message = DM5Help.Md5Decrypt (msg);
            error.Title = DM5Help.Md5Decrypt(title);
            return View("Error", error);
        }

    }
}