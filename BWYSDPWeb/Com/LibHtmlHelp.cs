using BWYSDPWeb.Com;
using SDPCRL.COM.ModelManager;
using SDPCRL.COM.ModelManager.FormTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb
{
    public static class LibHtmlHelp
    {
        public static HtmlString GetMessage<TModel>(this HtmlHelper<TModel> html, string msgid)
        {
            return new HtmlString(AppCom.GetMessageDesc(msgid));
        }

        public static HtmlString GetFieldDesc(this HtmlHelper html, string dsid, string tablenm, string fieldnm)
        {
            return new HtmlString(AppCom.GetFieldDesc(dsid, tablenm, fieldnm));
        }

        public static HtmlString ModelConvertoHtml(this HtmlHelper htmlhelp, string progid,string package)
        {
            StringBuilder builder = new StringBuilder();
            string _rootpath =System.Web.HttpContext.Current.Server.MapPath("/").Replace("//", "");
            _rootpath = string.Format(@"{0}Views", _rootpath);
            LibFormPage formpage = ModelManager.GetModelBypath<LibFormPage>(_rootpath, progid, package);
            LibDataSource dataSource = ModelManager.GetModelBypath<LibDataSource>(_rootpath, formpage.DSID, package);
            #region 根据排版模型对象 创建功能视图。
            ViewFactory factory = new ViewFactory(progid);
            factory.LibDataSource = dataSource;
            factory.LibFormPage = formpage;
            factory.ControlClassNm = formpage.ControlClassNm;
            factory.DSID = formpage.DSID;
            factory.Package = package;
            if (formpage.ModuleOrder != null)
            {
                foreach (ModuleOrder item in formpage.ModuleOrder)
                {
                    switch (item.moduleType)
                    {
                        case ModuleType.FormGroup:
                            if (formpage.FormGroups != null)
                            {
                                foreach (LibFormGroup formg in formpage.FormGroups)
                                {
                                    if (formg.FormGroupID != item.ID) continue;
                                    factory.CreatePanelGroup(formg.FormGroupName);
                                    if (formg.FmGroupFields != null && formg.FmGroupFields.Count > 0)
                                    {
                                        factory.AddFormGroupFields(formg.FmGroupFields, formg.FormGroupName);
                                    }
                                }
                            }
                            break;
                        case ModuleType.GridGroup:
                            if (formpage.GridGroups != null)
                            {
                                foreach (LibGridGroup grid in formpage.GridGroups)
                                {
                                    if (grid.GridGroupID != item.ID) continue;
                                    if (factory.Childrengrids.FirstOrDefault(i => i.GridGroupID == grid.GridGroupID) != null) continue;
                                    if (grid.GdGroupFields != null)
                                    {
                                        factory.CreateGridGroup(grid);
                                    }
                                }
                            }
                            break;
                        case ModuleType.ButtonGroup:
                            if (formpage.BtnGroups != null)
                            {
                                foreach (LibButtonGroup btngroup in formpage.BtnGroups)
                                {
                                    if (btngroup.BtnGroupID != item.ID) continue;
                                    factory.CreatBtnGroup(btngroup);
                                }
                            }
                            break;
                    }
                }
            }
            factory.EndPage(false);
            #endregion
            return new HtmlString(factory.PageHtml);
        }
    }
}