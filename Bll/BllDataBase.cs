﻿using SDPCRL.BLL.BUS;
using SDPCRL.COM;
using System;
using System.Collections.Generic;
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
            if (getcurrentserver)
            {
                ServerInfo info = new SQLite().GetCurrentServer();
                if (info != null)
                {
                    this.AccoutId = info.accountid;
                    SDPCRL.BLL.BUS.ServerInfo.ConnectType = info.connectype;
                    SDPCRL.BLL.BUS.ServerInfo.IPAddress = info.ipAddress;
                    SDPCRL.BLL.BUS.ServerInfo.Point = info.point;
                }
            }
        }

        //public BllDataBase(bool getcurrentserver = false)
        //{

        //}
        #endregion

        public Dictionary<string, string> GetAccount()
        {
            return (Dictionary<string, string>)this.ExecuteSysDalMethod("TestFunc", "GetAccount");
        }

        public object ExecuteDalSaveMethod(string funcId, string method,LibTable[] tables)
        {
            return this.ExecuteSaveMethod(funcId, method, tables);
        }

        public DalResult ExecuteMethod(string funcId, string method,LibTable[] libTables, params object[] param)
        {
            return  (DalResult)this.ExecuteDalMethod(funcId, method,libTables, param);
        }
    }
}
