using AuthorityViewModel;
using BWYSDPWeb.BaseController;
using BWYSDPWeb.Com;
using BWYSDPWeb.Models;
using SDPCRL.COM.ModelManager;
using SDPCRL.COM.ModelManager.FormTemplate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BWYSDPWeb.BllAuthorityControllers
{
    public class AuthorityController : DataBaseController
    {
        // GET: Authority
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>取功能权限对象</summary>
        /// <param name="progid"></param>
        /// <returns></returns>
        public ActionResult GetActionDetailView(string progid)
        {
            ActionViewModel vm = null;
            ProgInfo[] allprogid = AppCom.GetAllProgid();
            var pid = allprogid.FirstOrDefault(i => i.ProgId.ToUpper() == progid.ToUpper());
            if (pid != null)
            {
                List<ActionObj> list = new List<ActionObj>();
                ActionObj o = null;
                LibFormPage formpage = ModelManager.GetModelBypath<LibFormPage>(this.ModelRootPath, progid, pid.Package);
                LibPermissionSource libPermission = ModelManager.GetModelBypath<LibPermissionSource>(this.ModelRootPath, progid, pid.Package);
                if (libPermission != null)
                {
                    if (libPermission.IsAdd)
                    {
                        o = new ActionObj();
                        o.ObjectType = 1;
                        o.ObjectId = "bwysdp_btnadd";
                        o.ObjectNm = AppCom .GetMessageDesc("sdp_btnadd");
                        o.GroupId = formpage.FormId;
                        o.GroupNm = "功能按钮";
                        list.Add(o);
                    }
                    if (libPermission.IsDelete)
                    {
                        o = new ActionObj();
                        o.ObjectType = 1;
                        o.ObjectId = "bwysdp_btndelet";
                        o.ObjectNm = AppCom.GetMessageDesc("sdp_btnDelete");
                        o.GroupId = formpage.FormId;
                        o.GroupNm = "功能按钮";
                        list.Add(o);
                    }
                    if (libPermission.IsEdit)
                    {
                        o = new ActionObj();
                        o.ObjectType = 1;
                        o.ObjectId = "bwysdp_btnedit";
                        o.ObjectNm = AppCom.GetMessageDesc("sdp_btnedit");
                        o.GroupId = formpage.FormId;
                        o.GroupNm = "功能按钮";
                        list.Add(o);
                    }
                    if (libPermission.IsSearch)
                    {
                        o = new ActionObj();
                        o.ObjectType = 1;
                        o.ObjectId = "bwysdp_btnSearch";
                        o.ObjectNm = AppCom.GetMessageDesc("sdp_btnsearch");
                        o.GroupId = formpage.FormId;
                        o.GroupNm = "功能按钮";
                        list.Add(o);
                    }
                }
                if (formpage != null)
                {
                    if (formpage.BtnGroups != null)
                    {
                        foreach (LibButtonGroup group in formpage.BtnGroups)
                        {
                            if (group.LibButtons == null) continue;
                            foreach (LibButton btn in group.LibButtons)
                            {
                                o = new ActionObj();
                                o.ObjectType = 1;
                                o.ObjectId = btn.LibButtonID;
                                o.ObjectNm = AppCom.GetFieldDesc((int)Language, formpage.DSID, string.Empty, btn.LibButtonName);
                                o.GroupId = group.BtnGroupID;
                                o.GroupNm = AppCom.GetFieldDesc((int)Language, formpage.DSID, string.Empty, group.BtnGroupName);
                                list.Add(o);
                            }
                        }
                    }
                    if (formpage.GridGroups != null)
                    {
                        foreach (LibGridGroup item in formpage.GridGroups)
                        {
                            if (formpage.ModuleOrder.FindFirst("ID", item.GridGroupID) == null) continue;
                            if (item.GdButtons != null)
                            {
                                foreach (LibGridButton btn in item.GdButtons)
                                {
                                    o = new ActionObj();
                                    o.ObjectType = 1;
                                    o.ObjectId = btn.GridButtonID;
                                    o.ObjectNm = AppCom.GetFieldDesc((int)Language, formpage.DSID, string.Empty, btn.GridButtonName);
                                    o.GroupId = item.GridGroupID;
                                    o.GroupNm = AppCom.GetFieldDesc((int)Language, formpage.DSID, string.Empty, item.GridGroupName);
                                    list.Add(o);
                                }
                            }
                            if (item.GdGroupFields != null)
                            {
                                foreach (LibGridGroupField f in item.GdGroupFields)
                                {
                                    if (f.Hidden) continue;
                                    o = new ActionObj();
                                    o.ObjectType = 2;
                                    o.ObjectId = f.Name;
                                    o.ObjectNm = AppCom.GetFieldDesc((int)Language, formpage.DSID, f.FromTableNm, f.Name);
                                    o.GroupId = item.GridGroupID;
                                    o.GroupNm = AppCom.GetFieldDesc((int)Language, formpage.DSID, string.Empty, item.GridGroupName);
                                    list.Add(o);
                                }
                            }
                        }
                    }
                    
                }
                vm = new ActionViewModel(list);
                DataRow[] rows = this.LibTables[2].Tables[0].DataTable.Select(string.Format("ProgId='{0}'", progid));
                string groupid = string.Empty;
                string objId = string.Empty;
                int objtype = -1;
                foreach (DataRow dr in rows)
                {
                     groupid = dr["GroupId"].ToString();
                     objId = dr["ObjectId"].ToString();
                     objtype = Convert.ToInt32(dr["ObjectType"].ToString());
                    var exist = list.FirstOrDefault(i => i.GroupId == groupid && i.ObjectId == objId && i.ObjectType == objtype);
                    if (exist != null)
                        exist.IsAuthority = false;
                }

            }
            return PartialView("_ActionDetailParse",vm);
        }
    }
}