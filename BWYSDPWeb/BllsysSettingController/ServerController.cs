using Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllsysSettingController
{
    public class ServerController : Controller
    {
        // GET: Server
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ServerPage()
        {
            return View();
        }

        public ActionResult Save()
        {
            //string conectType = this.Request.Params["conectType"] ?? string.Empty;
            //string serverNm = this.Request.Params["serverNm"] ?? string.Empty;
            //string ipAddress = this.Request.Params["ipAddress"] ?? string.Empty;
            //string point = this.Request.Params["point"] ?? string.Empty;
            //string accountname = this.Request.Params["accountname"] ?? string.Empty;

            Dictionary<string, string> dic = Session["serverinfo"] as Dictionary<string, string>;
            ServerInfo info = new ServerInfo();
            info.connectype = this.Request.Params["conectType"] ?? string.Empty;
            info.accountid = this.Request.Params["accountid"] ?? string.Empty;
            info.accountname = dic[info.accountid];
            info.ipAddress = this.Request.Params["ipAddress"] ?? string.Empty;
            info.point = Convert.ToInt32(this.Request.Params["point"] ?? "0");
            info.serverNm = this.Request.Params["serverNm"] ?? string.Empty;
            SQLite sqlite = new SQLite();
            //var dt= sqlite.SelectAllServer();
            sqlite.Insert(info);
            return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccout(string conntype,string ip,string point)
        {
            SDPCRL.BLL.BUS.ServerInfo.ConnectType = conntype;
            SDPCRL.BLL.BUS.ServerInfo.IPAddress = ip;
            SDPCRL.BLL.BUS.ServerInfo.Point = Int32.Parse(point);
            BllDataBase bll = new BllDataBase(false);
            Dictionary<string, string> dic = bll.GetAccount();
            Session["serverinfo"] = dic;
            List<ServerInfo> data = new List<ServerInfo>();
            ServerInfo info = null;
            foreach (KeyValuePair<string, string> item in dic)
            {
                info = new ServerInfo();
                info.accountid = item.Key;
                info.accountname = item.Value;
                data.Add(info);
            }
            return Json(new { data, Flag=0 }, JsonRequestBehavior.AllowGet);
        }
    }
}