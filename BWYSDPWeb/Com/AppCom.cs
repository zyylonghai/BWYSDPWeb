using Bll;
using BWYSDPWeb.Models;
using SDPCRL.CORE;
using SDPCRL.CORE.FileUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Com
{
    public class AppCom
    {
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
                dt = bll.GetFieldDescData(dsid);
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

        public static ProgInfo[] GetAllProgid()
        {
            ProgInfo[] results=null;
           string rootpath= System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", "");
            FileOperation fileoperation = new FileOperation();
            fileoperation.FilePath = string.Format(@"{0}\Models\{1}", string.Format(@"{0}Views", rootpath), SysConstManage.FormSourceNm);
            List<LibFileInfo> allfiles = fileoperation.SearchAllFileInfo();
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
    }
}