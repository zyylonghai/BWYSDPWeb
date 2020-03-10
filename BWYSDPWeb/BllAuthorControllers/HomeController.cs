using BWYSDPWeb.BaseController;
using BWYSDPWeb.Models;
using Com;
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
            if (lginfo.loginResult == 1)
            {
                userInfo.UserNm = lginfo.UserNm;
                //userInfo.Language = (Language)Convert.ToInt32(formparams["language"]);
                string tick = IdentityHelp.GenerateTick(userInfo.UserId);
                IdentityCredential identity = new IdentityCredential
                {
                    CertificateID = tick,
                    UserNm = lginfo.UserNm,
                    HasAdminRole = lginfo.HasAdminRole
                };
                string identitystr = JsonConvert.SerializeObject(identity);
                Com.AppCom.AddorUpdateCookies(SysConstManage.sdp_IdentityTick, "key", identitystr);
                FormsAuthentication.SetAuthCookie(userInfo.UserNm, false);
                Session[SysConstManage.sdp_userinfo] = userInfo;
            }
            if (lginfo.loginResult == 3)//密码错误
            {
                //109     密码错误
                throw new LibExceptionBase(109);
                //this.ThrowErrorException(15);
                //this.AddMessage("密码错误");
                //this.AddMessage()
            }
            if (lginfo.loginResult == -1)
            {
                //108    用户{0}不存在
                throw new LibExceptionBase(108, userInfo.UserId);
                //this.ThrowErrorException(16, userInfo.UserId);
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

        public ActionResult ErrorView(string msg, string title)
        {
            ErrorObject error = new ErrorObject();
            error.Message = DM5Help.Md5Decrypt(msg);
            error.Title = DM5Help.Md5Decrypt(title);
            return View("Error", error);
        }

        #region 日志
        public ActionResult LogSearch()
        {
            var userinfo = Session[SysConstManage.sdp_userinfo];
            if (userinfo == null)
                return View("Login");
            else
            {
                this.Authentication();
                return View("LogSearch");
            }
        }

        public string GetExceptionData(int page, int rows, string url)
        {
            CachHelp cach = new CachHelp();
            DataTable dt =(DataTable)cach.GetCach("sdp_logdata");
            if (dt == null || dt.Rows.Count == 0)
            {
                LogHelp logHelp = new LogHelp(System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", ""));
                var loginfos = logHelp.GetLogInfos().OrderByDescending(i => i.DateTime).ToList();
                dt = LibSysUtils.ToDataTable(loginfos);
                cach.AddCachItem("sdp_logdata", dt, DateTime.Now.AddSeconds(60));
            }
            if (!string.IsNullOrEmpty(url))
            {
                dt = AppSysUtils.GetData(dt, string.Format("Head like '%{0}%'",url));
            }
            DataTable resultdt = AppSysUtils.GetDataByPage(dt, page, rows);
            if (resultdt == null) { var result2 = new { total = 0, rows = DBNull.Value }; return JsonConvert.SerializeObject(result2); }
            var result = new { total = AppSysUtils.CalculateTotal(dt), rows = resultdt };

            return JsonConvert.SerializeObject(result);
        }

        public ActionResult Deletelogfile(List<string> files)
        {
            LogHelp logHelp = new LogHelp(System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", ""));
            logHelp.DeleteLogFileBatch(files);
            CachHelp cach = new CachHelp();
            cach.RemoveCache("sdp_logdata");
            return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadLogfile(string filenm)
        {
            LogHelp logHelp = new LogHelp(System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", ""));
            logHelp.ReadLogFile(filenm);
            return Json(new { message = logHelp.ReadLogFile(filenm) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}