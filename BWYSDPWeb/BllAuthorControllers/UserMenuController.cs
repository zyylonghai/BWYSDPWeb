using BWYSDPWeb.BaseController;
using BWYSDPWeb.Com;
using BWYSDPWeb.Controllers;
using BWYSDPWeb.Models;
using SDPCRL.COM;
using SDPCRL.CORE;
using SDPCRL.CORE.FileUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllAuthorControllers
{
    public class UserMenuController : HomeController
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            DalResult result = this.ExecuteMethod("GetUserMenus", this.UserInfo.UserId);
            LibTableObj tableObj = (LibTableObj)result.Value;
            LibTableObj mtbobj = this.LibTables[0].Tables[0];
            //LibTableObj dtbobj = this.LibTables[0].Tables[1];
            mtbobj.FillData(tableObj.DataTable);
            InitilData();
            //if (mtbobj != null && mtbobj.Rows !=null && mtbobj.Rows.Count > 0)
            //{
            //    for (int i=0;i<mtbobj.Rows.Count;)
            //    {
            //        var row = mtbobj.Rows[i];
            //        if ((row.PmenuId != 0 && row.PmenuId != row.MenuId) && !string.IsNullOrEmpty(row.ProgId))
            //        {
            //            dtbobj.AppendRow(((DataRowObj)row).Row, false);
            //            mtbobj.DataTable.Rows.Remove(((DataRowObj)row).Row);
            //        }
            //        else
            //            i++;
            //    }
            //    //mtbobj.DataTable.AcceptChanges();
            //    dtbobj.DataTable.AcceptChanges();
            //}
            //
        }

        protected override void BeforeSave()
        {
            base.BeforeSave();
            
            LibTableObj tbobj = this.LibTables[0].Tables[1];
            LibTableObj mtbobj = this.LibTables[0].Tables[0];
            if (tbobj !=null && tbobj.Rows.Count > 0)
            {
                DataRowObj rowobj = null;
                foreach (var row in tbobj .Rows)
                {
                    rowobj = (DataRowObj)row;
                    if (rowobj.DataRowState == DataRowState.Deleted)
                    {
                        //long v = row.o_MenuId;
                        var r = mtbobj.NewRow();
                        r.MenuId = row.o_MenuId;
                        ((DataRowObj)r).Row.AcceptChanges();
                        ((DataRowObj)r).Row.Delete();
                    }
                    else if (rowobj.DataRowState == DataRowState.Unchanged)
                    {
                        //long v = row.MenuId;
                        var r = mtbobj.NewRow();
                        r.MenuId = row.MenuId;
                        r.ProgId = row.ProgId;
                        r.Package = row.Package;
                        r.PmenuId = row.PmenuId;
                        ((DataRowObj)r).Row.AcceptChanges();
                    }
                    else if (rowobj.DataRowState == DataRowState.Modified)
                    {
                        var r = mtbobj.NewRow();
                        r.MenuId = row.o_MenuId;
                        r.ProgId = row.o_ProgId;
                        r.Package = row.o_Package;
                        r.PmenuId = row.o_PmenuId;
                        r.MenuName = row.o_MenuName;
                        r.UId = row.o_UId;
                        r.UserMenuCode = row.o_UserMenuCode;
                        ((DataRowObj)r).Row.AcceptChanges();
                        r.MenuId = row.MenuId;
                        r.ProgId = row.ProgId;
                        r.Package = row.Package;
                        r.PmenuId = row.PmenuId;
                        r.MenuName = row.MenuName;
                        r.UId = row.UId;
                        r.UserMenuCode = row.UserMenuCode;
                    }
                    else 
                        mtbobj.AppendRow(rowobj.Row, rowobj.DataRowState == DataRowState.Added);
                }
                //mtbobj.AppendDataTable(tbobj.DataTable);
                foreach (var dr in mtbobj.Rows)
                {
                    if (((DataRowObj)dr).DataRowState == DataRowState.Deleted) continue;
                    dr.UId = this.UserInfo.UserId;
                }
            }
        }

        protected override void SetSearchFieldExt(List<SearchConditionField> fields, string fieldNm, int flag)
        {
            base.SetSearchFieldExt(fields, fieldNm, flag);
            if (flag == 3)
            {
                SearchConditionField field = new SearchConditionField();
                field.FieldNm = "ProgId";
                field.DisplayNm = "功能ID";
                //field.TBAliasNm = 'a';
                field.IsCondition = true;
                fields.Add(field);

                field = new SearchConditionField();
                field.FieldNm = "ProgNm";
                field.DisplayNm = "功能名称";
                //field.TBAliasNm = 'a';
                field.IsCondition = true;
                fields.Add(field);

                field = new SearchConditionField();
                field.FieldNm = "Package";
                field.DisplayNm = "所属包";
                //field.TBAliasNm = 'a';
                field.IsCondition = true;
                fields.Add(field);
                this.AddRelateFieldsForSearchModal("UserMenu_D", "ProgNm");
                this.AddRelateFieldsForSearchModal("UserMenu_D", "Package");
                this.AddRelateFieldsForSearchModal("UserMenu_D", "MenuName");
                //this.SessionObj.FromFieldInfo.RelateFields.Add(string.Format("{0}__rsdp_{1}", "JoleD", "ProgNm"));
            }
        }

        protected override void BindSmodalDataExt(DataTable currpagedata, int flag, string fieldnm)
        {
            base.BindSmodalDataExt(currpagedata, flag, fieldnm);
            if (flag == 3)
            {
                #region 获取所有功能模型的Progid
                //FileOperation fileoperation = new FileOperation();
                //fileoperation.FilePath = string.Format(@"{0}\Models\{1}", this.ModelRootPath, SysConstManage.FormSourceNm);
                //List<LibFileInfo> array = fileoperation.SearchAllFileInfo();
                ProgInfo[] array = AppCom.GetAllProgid();
                DalResult joleresult = this.ExecuteMethod("GetAuthority", this.UserInfo.UserId);
                LibTableObj joledata = (LibTableObj)joleresult.Value;
                bool hasadmin = joledata.Rows.FirstOrDefault(i => i.JoleId == "001")!= null;
                #region 添加列
                DataColumn col = new DataColumn("ProgId");
                currpagedata.Columns.Add(col);
                currpagedata.PrimaryKey = new DataColumn[] { col };
                col = new DataColumn("ProgNm");
                currpagedata.Columns.Add(col);
                col = new DataColumn("Package");
                currpagedata.Columns.Add(col);
                col = new DataColumn("MenuName");
                currpagedata.Columns.Add(col);
                #endregion
                foreach (var item in array)
                {
                    if (!hasadmin&& joledata.Rows.FirstOrDefault(i => i.ProgId == item.ProgId) == null) continue;
                    DataRow row = currpagedata.NewRow();
                    row["ProgId"] = item.ProgId;
                    row["Package"] = item.Package;
                    row["ProgNm"] = AppCom.GetFieldDesc((int)this.Language, item.ProgId, string.Empty, item.ProgId);
                    row["MenuName"] = row["ProgNm"];
                    currpagedata.Rows.Add(row);
                }
                #endregion
            }
        }

        protected override void UpdateTableRow(string gridid, DataRowObj row, DataGridAction cmd)
        {
            base.UpdateTableRow(gridid, row, cmd);
            if (gridid == "GridGroup2"|| gridid == "Menusetting")
            {
                if (cmd ==DataGridAction.Add)
                {
                    dynamic rowobj = row;
                    rowobj.UId = this.UserInfo.UserId;
                }
            }
    }
        protected override void GetGridDataExt(string gridid, DataTable dt)
        {
            base.GetGridDataExt(gridid, dt);
            //if (gridid == "GridGroup1")
            //{
            //    DataTable copydt = dt.Copy();
            //    LibTableObj tableObj = new LibTableObj(copydt);
            //    for(int i=0;i<tableObj.Rows .Count;)
            //    {
            //        var row = tableObj.Rows[i];
            //        if (((DataRowObj)row).DataRowState == DataRowState.Deleted)
            //        {
            //            i++;
            //            continue;
            //        }
            //        if ((row.PmenuId != 0 && row.PmenuId != row.MenuId) && !string.IsNullOrEmpty(row.ProgId))
            //        {
            //            copydt.Rows.Remove(((DataRowObj)row).Row);
            //        }
            //        else
            //        {
            //            i++;
            //        }
            //    }
                
            //}
        }

        protected override void SaveSuccessRedirect(ref string actionNm, ref string controlnm, object routvalues)
        {
            base.SaveSuccessRedirect(ref actionNm, ref controlnm, routvalues);
            actionNm = "SysSetting";
            controlnm = "Home";
            routvalues = new { flag = 1 };
            //this.SessionObj.OperateAction = OperatAction.None;
            
        }
        protected override void AfterSave()
        {
            base.AfterSave();
            InitilData();
        }

        private void InitilData()
        {
            LibTableObj mtbobj = this.LibTables[0].Tables[0];
            LibTableObj dtbobj = this.LibTables[0].Tables[1];
            if (dtbobj != null && dtbobj.DataTable != null)
            {
                dtbobj.DataTable.Clear();
            }
            if (mtbobj != null && mtbobj.Rows != null && mtbobj.Rows.Count > 0)
            {
                for (int i = 0; i < mtbobj.Rows.Count;)
                {
                    var row = mtbobj.Rows[i];
                    if (((DataRowObj)row).DataRowState == DataRowState.Deleted) { i++; continue; };
                    if ((row.PmenuId != 0 && row.PmenuId != row.MenuId) && !string.IsNullOrEmpty(row.ProgId))
                    {
                        dtbobj.AppendRow(((DataRowObj)row).Row, false);
                        mtbobj.DataTable.Rows.Remove(((DataRowObj)row).Row);
                    }
                    else
                        i++;
                }
                dtbobj.DataTable.AcceptChanges();
            }
        }
    }
}