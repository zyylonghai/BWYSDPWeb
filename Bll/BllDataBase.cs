﻿using SDPCRL.BLL.BUS;
using SDPCRL.COM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public class BllDataBase : BllBus
    {
        #region 构造函数
        public BllDataBase(bool getcurrentserver = true)
        {
            ServerInfo info = null;
            if (getcurrentserver)
            {
                 info = new SQLite().GetCurrentServer();
                //if (info != null)
                //{
                //    this.AccoutId = info.accountid;
                //    SDPCRL.BLL.BUS.ServerInfo.ConnectType = info.connectype;
                //    SDPCRL.BLL.BUS.ServerInfo.IPAddress = info.ipAddress;
                //    SDPCRL.BLL.BUS.ServerInfo.Point = info.point;
                //}
                if (info == null)
                {
                    var all = new SQLite().SelectAllServer();
                    if (all != null && all.Count > 0)
                    {
                        //522 至少需要有一个数据层服务是当前连接。
                        throw new SDPCRL.CORE.LibExceptionBase(522);
                    }
                    else
                    {
                        SDPCRL.BLL.BUS.ServerInfo.IPAddress = string.Empty;
                        SDPCRL.BLL.BUS.ServerInfo.ConnectType = string.Empty;
                        SDPCRL.BLL.BUS.ServerInfo.Point = 0;
                    }

                }
            }
            else
            {
                //系统账套服务信息
                 info = new SQLite().GetSysServer();
            }
            if (info != null)
            {
                this.AccoutId = info.accountid;
                SDPCRL.BLL.BUS.ServerInfo.ConnectType = info.connectype;
                SDPCRL.BLL.BUS.ServerInfo.IPAddress = info.ipAddress;
                SDPCRL.BLL.BUS.ServerInfo.Point = info.point;
            }
        }

        //public BllDataBase(bool getcurrentserver = false)
        //{

        //}
        #endregion

        public Dictionary<string, string> GetAccount(Language language)
        {
            return (Dictionary<string, string>)this.ExecuteSysDalMethod((int)language,"TestFunc", "GetAccount");
        }

        public string GetFieldDesc(int languageId,string dsid,string tablenm,string fieldid)
        {
            return (string)this.ExecuteSysDalMethod(languageId,"TestFunc","InternalGetFieldDesc", languageId, dsid, tablenm, fieldid);
        }

        public DataTable GetFieldDescData( string dsid,Language language)
        {
            return (DataTable)this.ExecuteSysDalMethod((int)language,"TestFunc", "GetFieldDescByDSID", dsid);
        }
        //public object ExecuteSysDalMethod(string method, params object[] param)
        //{ }

        public object ExecuteDalSaveMethod(LibClientInfo clientInfo, string funcId, string method,LibTable[] tables)
        {
            return this.ExecuteSaveMethod(clientInfo,funcId, method, tables);
        }

        public DalResult ExecuteMethod(LibClientInfo clientInfo,string funcId, string method,LibTable[] libTables, params object[] param)
        {
            return  (DalResult)this.ExecuteDalMethod(clientInfo, funcId, method,libTables, param);
        }

        public DataTable[] GeDataLog(int language, string method, params object[] param)
        {
            return (DataTable[])this.ExecuteLogDalMethod(language, "DataLogFunc", method, param);
        }


    }
}
