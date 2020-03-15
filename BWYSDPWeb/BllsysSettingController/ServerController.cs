using Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BWYSDPWeb.BaseController;
using SDPCRL.CORE;
using BWYSDPWeb.Com;
using SDPCRL.COM;
using System.Data;
using BWYResFactory;

namespace BWYSDPWeb.BllsysSettingController
{
    public class ServerController: DataBaseController
    {
        // GET: Server
        //public ActionResult Index()
        //{
        //    return View();
        //}

        protected override void PageLoad()
        {
            base.PageLoad();
            SQLite sqlite = new SQLite();
            var allserver = sqlite.SelectAllServer();
            if (allserver != null)
            {
                LibTableObj tbobj = this.LibTables[0].Tables[0];
                tbobj.DataTable.Clear();
                foreach (var item in allserver)
                {
                    if (item.accountname == ResFactory.ResManager.SysDBNm) continue;
                    var newrow = tbobj.NewRow();
                    newrow.conectType = item.connectype;
                    newrow.serverNm = item.serverNm;
                    newrow.ipAddress = item.ipAddress;
                    newrow.point = item.point;
                    newrow.accountid = item.accountid;
                    newrow.accountname = item.accountname;
                    newrow.IsCurrentServer = item.IsCurrentServer;
                }
                tbobj.DataTable.AcceptChanges();
            }
        }

        public ActionResult ServerPage()
        {
            var userinfo = Session[SysConstManage.sdp_userinfo];
            if (userinfo == null)
                return View("Login");
            else
            {
                this.Authentication();
                return View();
            }

        }
        public ActionResult ServerPage2()
        {
            return View();
        }

        public ActionResult ServerSave()
        {
            //string conectType = this.Request.Params["conectType"] ?? string.Empty;
            //string serverNm = this.Request.Params["serverNm"] ?? string.Empty;
            //string ipAddress = this.Request.Params["ipAddress"] ?? string.Empty;
            //string point = this.Request.Params["point"] ?? string.Empty;
            //string accountname = this.Request.Params["accountname"] ?? string.Empty;

            Dictionary<string, string> dic = Session["serverinfo"] as Dictionary<string, string>;
            ServerInfo info = new ServerInfo();
            SQLite sqlite = new SQLite();
            var tbobj = this.LibTables[0].Tables[0];
            if (tbobj != null)
            {
                foreach (var row in tbobj.Rows)
                {
                    switch (((DataRowObj)row).DataRowState)
                    {
                        case DataRowState.Deleted:
                            info.serverNm = row.o_serverNm;
                            sqlite.Delete(info);
                            break;
                        case DataRowState.Modified:
                            info.serverNm = row.serverNm;
                            info.ipAddress = row.ipAddress;
                            info.IsCurrentServer = row.IsCurrentServer;
                            info.point = row.point;
                            info.serverNm = row.serverNm;
                            info.accountid = row.accountid;
                            info.accountname = row.accountname;
                            info.connectype = row.conectType;
                            sqlite.Update(info, row.o_serverNm);
                            break;
                        case DataRowState.Added:
                            info.serverNm = row.serverNm;
                            info.ipAddress = row.ipAddress;
                            info.IsCurrentServer = row.IsCurrentServer;
                            info.point = row.point;
                            info.serverNm = row.serverNm;
                            info.accountid = row.accountid;
                            info.accountname = row.accountname;
                            info.connectype = row.conectType;
                            sqlite.Insert(info);
                            break;
                    }
                }
            }
            //var allserver = sqlite.SelectAllServer();
            //var hassys = allserver.FirstOrDefault(i => i.accountname == ResFactory.ResManager.SysDBNm);
            //if (hassys == null&& allserver .Count >0)
            //{
            //    var curserver = allserver.FirstOrDefault(i => i.IsCurrentServer);
            //    if (curserver != null)
            //    {
            //        info.accountname = ResFactory.ResManager.SysDBNm;
            //        info.ipAddress = curserver.ipAddress;
            //        info.point = curserver.point;
            //        info.connectype = curserver.connectype;
            //        sqlite.Insert(info);
            //    }
            //}
            //info.connectype = this.Request.Params["conectType"] ?? string.Empty;
            //info.accountid = this.Request.Params["accountid"] ?? string.Empty;
            //info.accountname = dic[info.accountid];
            //info.ipAddress = this.Request.Params["ipAddress"] ?? string.Empty;
            //info.point = Convert.ToInt32(this.Request.Params["point"] ?? "0");
            //info.serverNm = this.Request.Params["serverNm"] ?? string.Empty;
            //SQLite sqlite = new SQLite();
            ////var dt= sqlite.SelectAllServer();
            //sqlite.Insert(info);
            //msg000000001 保存成功
            this.AddMessage(AppCom.GetMessageDesc("msg000000001"), LibMessageType.Prompt);
            return RedirectToAction("ServerPage", "Server");
        }
        public ActionResult ServerSave2()
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
            info.IsCurrentServer = true;
            SQLite sqlite = new SQLite();
            sqlite.SetAllServerNoCurrent();
            //var dt= sqlite.SelectAllServer();
            sqlite.Insert(info);

            //var allserver = sqlite.SelectAllServer();
            //var hassys = allserver.FirstOrDefault(i => i.accountname == ResFactory.ResManager.SysDBNm);
            //if (hassys == null && allserver.Count > 0)
            //{
            //    var curserver = allserver.FirstOrDefault(i => i.IsCurrentServer);
            //    if (curserver != null)
            //    {
            //        info = new ServerInfo();
            //        info.accountname = ResFactory.ResManager.SysDBNm;
            //        info.ipAddress = curserver.ipAddress;
            //        info.point = curserver.point;
            //        info.connectype = curserver.connectype;
            //        info.serverNm = "sys";
            //        info.IsCurrentServer = false;
            //        sqlite.Insert(info);
            //    }
            //}
            ////msg000000001 保存成功
            //this.AddMessage(AppCom.GetMessageDesc("msg000000001"), LibMessageType.Prompt);
            return RedirectToAction("LoginOut", "Home");
        }

        public ActionResult GetAccout(string conntype,string ip,string point)
        {
            SDPCRL.BLL.BUS.ServerInfo.ConnectType = conntype;
            SDPCRL.BLL.BUS.ServerInfo.IPAddress = ip;
            SDPCRL.BLL.BUS.ServerInfo.Point = Int32.Parse(point);
            BllDataBase bll = new BllDataBase(false);
            Dictionary<string, string> dic = bll.GetAccount(SDPCRL.COM.Language.CHS);
            Session["serverinfo"] = dic;
            List<ServerInfo> data = new List<ServerInfo>();
            ServerInfo info = null;
            foreach (KeyValuePair<string, string> item in dic)
            {
                if (item.Value == ResFactory.ResManager.SysDBNm) continue;
                info = new ServerInfo();
                info.accountid = item.Key;
                info.accountname = item.Value;
                data.Add(info);
            }
            return Json(new { data, Flag=0 }, JsonRequestBehavior.AllowGet);
        }

        protected override void UpdateTableAction(string gridid, DataRow row, string cmd)
        {
            base.UpdateTableAction(gridid, row, cmd);
            if (cmd != "Delet")
            {
                var tbobj = this.LibTables[0].Tables[0];
                var rowobj = tbobj.FindRow(row);
                Dictionary<string, string> dic = Session["serverinfo"] as Dictionary<string, string>;
                rowobj.accountname = dic[rowobj.accountid];
            }
        }
    }
}