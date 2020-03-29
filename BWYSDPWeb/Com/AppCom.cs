using Bll;
using BWYSDPWeb.Models;
using SDPCRL.COM.ModelManager;
using SDPCRL.CORE;
using SDPCRL.CORE.FileUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace BWYSDPWeb.Com
{
    public class AppCom
    {
        public static LogHelp LogHelp { get { return new LogHelp(System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", "")); } }
        /// <summary>
        /// 获取字段或字符串的多语言描述
        /// </summary>
        /// <param name="languageid">语种</param>
        /// <param name="dsid">数据源的DSID</param>
        /// <param name="tablenm">表名</param>
        /// <param name="fieldnm">字段名</param>
        /// <returns></returns>
        public static string GetFieldDesc(int languageid,string dsid, string tablenm, string fieldnm)
        {
            #region 先从cache缓存中取数。
            CachHelp cachelp = new CachHelp();
            BllDataBase bll = new BllDataBase(false);
            DataTable dt = cachelp.GetCach(dsid) as DataTable;
            if (dt == null)
            {
                dt = bll.GetFieldDescData(dsid,(SDPCRL.COM .Language)languageid);
                cachelp.AddCachItem(dsid, dt, DateTimeOffset.Now.AddMinutes(2));
            }
            if (dt != null)
            {
                DataRow[] dr = dt.Select(string.Format("LanguageId={0} and DSID='{1}' and FieldNm='{2}' and TableNm='{3}'",
                                                     languageid, dsid, fieldnm, tablenm));
                if (dr != null && dr.Length > 0)
                {
                    return dr[0]["Vals"].ToString();
                }
            }
            #endregion 
            return bll.GetFieldDesc(languageid, dsid, tablenm, fieldnm);
        }

        public static string GetMessageDesc(string msgid)
        {
            UserInfo userInfo = System.Web.HttpContext.Current.Session[SysConstManage.sdp_userinfo] as UserInfo;
            if (userInfo == null) return string.Empty;
            return GetFieldDesc((int)userInfo.Language, string.Empty, string.Empty, msgid);
        }
        public static string GetFieldDesc(string dsid, string tablenm, string fieldnm)
        {
            UserInfo userInfo = System.Web.HttpContext.Current.Session[SysConstManage.sdp_userinfo] as UserInfo;
            return GetFieldDesc((int)userInfo.Language, dsid, tablenm, fieldnm);
        }

        public static ProgInfo[] GetAllProgid()
        {
            ProgInfo[] results=null;
           string rootpath= System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", "");
            FileOperation fileoperation = new FileOperation();
            fileoperation.FilePath = string.Format(@"{0}\Models\{1}", string.Format(@"{0}Views", rootpath), SysConstManage.FormSourceNm);
            List<LibFileInfo> allfiles = fileoperation.SearchAllFileInfo();
            fileoperation .FilePath = string.Format(@"{0}\Models\{1}", string.Format(@"{0}Views", rootpath), SysConstManage.ReportSourceNm);
            allfiles.AddRange(fileoperation.SearchAllFileInfo());
            if (allfiles != null)
            {
                ProgInfo p = null;
                results = new ProgInfo[allfiles.Count];
                for(int i=0;i<allfiles .Count;i++)
                {
                    p = new ProgInfo();
                    p.Package = allfiles[i].Folder;
                    p.ProgId = allfiles[i].FileName;
                    results[i] = p;
                }
            }
            return results;
           
        }

        public static LibDataSource GetDataSource(string dsid)
        {
            //ProgInfo[] results = null;
            string rootpath = System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", "");
            FileOperation fileoperation = new FileOperation();
            fileoperation.FilePath = string.Format(@"{0}\Models\{1}", string.Format(@"{0}Views", rootpath), SysConstManage.DataSourceNm);
            List<LibFileInfo> allfiles = fileoperation.SearchAllFileInfo();
            if (allfiles != null)
            {
                foreach (var item in allfiles)
                {
                    if (item.FileName == dsid)
                        return ModelManager.GetModelBypath<LibDataSource>(string.Format(@"{0}Views", rootpath), dsid, item.Folder);
                }
            }
            return null;
        }



        #region cookie
        public static void AddorUpdateCookies(string cookieNm, string key, string value)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieNm];
            Dictionary<string, string> values = null;
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieNm);
            }
            if (string.IsNullOrEmpty(cookie.Value))
            {
                values = new Dictionary<string, string>();
                values.Add(key, value);
            }
            else
            {
                values = DecryptCookie(cookie.Value);
                if (values.ContainsKey(key))
                {
                    values[key] = value;
                }
                else
                {
                    values.Add(key, value);
                }

            }
            cookie.Value = EncryptionCookie(values);
            System.Web.HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static string GetCookievalue(string cookieNm, string key)
        {
            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[key];
            //if(cookie!=null) return cookie.Value;
            //return string.Empty;
            #region
            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieNm];
            //if (cookie != null)
            //{
            //    return cookie.Values[key];
            //}
            //return string.Empty;
            #endregion

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieNm];
            if (cookie != null)
            {
                Dictionary<string, string> values = DecryptCookie(cookie.Value);
                if (values.ContainsKey(key)) return values[key];
                return string.Empty;
            }
            return string.Empty;
        }

        private static Dictionary<string, string> DecryptCookie(string cookievalu)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(cookievalu)) return result;
            byte[] bts = Convert.FromBase64String(cookievalu);
            string[] str = Encoding.Default.GetString(bts).Split('&');
            string[] item = null;
            if (str != null && str.Length > 0)
            {
                foreach (string s in str)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        item = s.Split('=');
                        result.Add(item[0], item[1]);
                    }
                }
            }
            return result;
        }
        private static string EncryptionCookie(Dictionary<string, string> valus)
        {
            string valu = string.Empty;
            foreach (var item in valus)
            {
                if (!string.IsNullOrEmpty(valu))
                {
                    valu += "&";
                }
                valu += string.Format("{0}={1}", item.Key, item.Value);
            }
            byte[] vals = Encoding.Default.GetBytes(valu);
            return Convert.ToBase64String(vals);
        }
        #endregion

    }
}