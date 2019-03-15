using SDPCRL.BLL.BUS;
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
        public BllDataBase()
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

        public BllDataBase(bool getcurrentserver = false)
        {

        }
        #endregion

        public Dictionary<string, string> GetAccount()
        {
            return (Dictionary<string, string>)this.ExecuteSysDalMethod("TestFunc", "GetAccount");
        }
    }
}
