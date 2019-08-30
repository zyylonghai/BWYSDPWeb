﻿using Bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Com
{
    public class AppCom
    {
        public static string GetFieldDesc(int languageid,string dsid, string tablenm, string fieldnm)
        {
            #region 先从cache缓存中取数。
            CachHelp cachelp = new CachHelp();
            DataTable dt = cachelp.GetCach(dsid) as DataTable;
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
            BllDataBase bll = new BllDataBase(false);
            return bll.GetFieldDesc(languageid, dsid, tablenm, fieldnm);

        }
    }
}