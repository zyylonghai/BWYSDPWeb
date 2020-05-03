using Bll;
using BWYSDPWeb.BaseController;
using BWYSDPWeb.Com;
using BWYSDPWeb.Models;
using Com;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProgViewModel;
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
            TempHelp sQLiteHelp = new TempHelp("TempData");
            sQLiteHelp.ClearTempBysessionid(Session.SessionID);
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
                //LogHelp logHelp = new LogHelp(System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", ""));
                var loginfos = AppCom.LogHelp .GetLogInfos().OrderByDescending(i => i.DateTime).ToList();
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
            //LogHelp logHelp = new LogHelp(System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", ""));
           AppCom .LogHelp.DeleteLogFileBatch(files);
            CachHelp cach = new CachHelp();
            cach.RemoveCache("sdp_logdata");
            return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadLogfile(string filenm)
        {
            //LogHelp logHelp = new LogHelp(System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", ""));
            AppCom.LogHelp.ReadLogFile(filenm);
            return Json(new { message = AppCom.LogHelp.ReadLogFile(filenm) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 数据日志查询相关
        [HttpPost]
        public ActionResult DataLogSearch()
        {
            Dictionary<string, List<string>> tablenmAndlogids = new Dictionary<string, List<string>>();
            List<string> logids = null;
            DataLogViewModel vm = new DataLogViewModel();
            vm.DSID = this.DSID;
            ColExtendedProperties colextprop = null;
            DataLogObj logObj = null;
            if (this.LibTables != null)
            {
                foreach (LibTable libTable in this.LibTables)
                {
                    foreach (LibTableObj tableObj in libTable.Tables)
                    {
                        logids = new List<string>();
                        tablenmAndlogids.Add(tableObj.TableName, logids);
                        logObj = new DataLogObj { TableNm = tableObj.TableName };
                        if (logObj.cols == null) logObj.cols = new ColInfo []{ };

                        vm.DataLogObjs.Add(logObj);
                        foreach (DataColumn c in tableObj.DataTable .Columns)
                        {
                            colextprop = Newtonsoft.Json.JsonConvert.DeserializeObject<ColExtendedProperties>(c.ExtendedProperties[SysConstManage.ExtProp].ToString());
                            if (!colextprop.IsActive || c.ColumnName ==SysConstManage.Sdp_LogId) continue;
                            Array.Resize(ref logObj.cols, logObj.cols.Length + 1);
                            logObj.cols[logObj.cols.Length - 1] =new ColInfo {Nm=c.ColumnName , Type =c.DataType };
                        }
                        foreach (DataRow dr in tableObj.DataTable.Rows)
                        {
                            if (!string.IsNullOrEmpty(dr[SysConstManage.Sdp_LogId].ToString()))
                                logids.Add(dr[SysConstManage.Sdp_LogId].ToString());
                        }
                    }
                }
                string b = System.DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                //tablenmAndlogids.Where (i=>i.Value .Count() >0).ToList ()
                DataTable[] result = (DataTable[])this.GetDataLog("SearchData", tablenmAndlogids);
                string b2 = System.DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                if (result != null && result.Length > 0)
                {
                    string tbName = string.Empty;
                    string fldnm = string.Empty;
                    string  fldvalue = string.Empty;
                    bool hasfind = false;
                    DataTable table = null;
                    for (int i = 0; i < result.Length; i++)
                    {
                        table = result[i];
                        if (table.Rows.Count <= 0) continue;
                        tbName = table.TableName.Split(SysConstManage.Underline)[0];
                        var o = vm.DataLogObjs.FirstOrDefault(a => a.TableNm == tbName);
                        if (o != null)
                        {
                            //if (o.datas == null) o.datas = new object[] { };
                            hasfind = false;
                            List<JObject> jObjects = new List<JObject>();
                            JObject first = new JObject();
                            first.Add("DT", table.Rows[0]["DT"].ToString ());
                            first.Add("UserId", table.Rows[0]["UserId"].ToString());
                            first.Add("IP", table.Rows[0]["IP"].ToString());
                            switch (table.Rows[0]["Action"].ToString())
                            {
                                case "1":
                                    first.Add("Action", "新增");
                                    break;
                                case "2":
                                    first.Add("Action", "修改");
                                    break;
                                case "3":
                                    break;
                            }
                            jObjects.Add(first);
                            foreach (DataRow row in table.Rows)
                            {
                                fldnm = row["FieldNm"].ToString();
                                fldvalue = row["FieldValue"].ToString ();
                                //if (o.cols.FirstOrDefault(i => i.Nm == fldnm && i.Type.Equals(typeof(byte[]))) != null)
                                //{
                                //    fldvalue =(byte[])Convert.FromBase64String(fldvalue.ToString()); 
                                //}
                                foreach (var jobj in jObjects)
                                {
                                    if (jobj.Properties().FirstOrDefault(a => a.Name == fldnm) != null)
                                    {
                                        JObject jo = new JObject();
                                        jo.Add("DT", row["DT"].ToString());
                                        jo.Add("UserId", row["UserId"].ToString());
                                        jo.Add("IP", row["IP"].ToString());
                                        switch (row["Action"].ToString())
                                        {
                                            case "1":
                                                jo.Add("Action", "新增");
                                                break;
                                            case "2":
                                                jo.Add("Action", "修改");
                                                break;
                                            case "3":
                                                break;
                                        }
                                        //jo.Add("Action", row["Action"].ToString());
                                        jo.Add(fldnm, fldvalue);
                                        jObjects.Add(jo);
                                        hasfind = true;
                                        break;
                                    }
                                }
                                if (!hasfind)
                                {
                                    first.Add(fldnm, fldvalue);
                                    hasfind = false;
                                }
                            }
                            if (o.datas == null) o.datas = jObjects;
                            else
                            {
                                o.datas.Add(new JObject());
                                o.datas.AddRange(jObjects);
                            }
                            //Array.Resize(ref o.datas, o.datas.Length + 1);
                            //o.datas[o.datas.Length - 1] = jObjects;
                        }
                    }
                }
            }
              return PartialView("_DataLogDetail", vm);
        }
        #endregion 

    }
}