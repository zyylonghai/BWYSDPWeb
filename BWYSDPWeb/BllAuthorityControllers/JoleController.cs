using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorityViewModel;
using BWYSDPWeb.Com;
using BWYSDPWeb.Models;
using SDPCRL.COM.ModelManager;
using SDPCRL.COM.ModelManager.FormTemplate;
using SDPCRL.CORE;
using SDPCRL.CORE.FileUtils;

namespace BWYSDPWeb.BllAuthorityControllers
{
    public class JoleController : AuthorityController
    {
        protected override void PageLoad()
        {
            base.PageLoad();
        }

        protected override void SetSearchFieldExt(List<SearchConditionField> fields, string fieldNm, int flag)
        {
            base.SetSearchFieldExt(fields,fieldNm , flag);
            if (flag == 3)
            {
                SearchConditionField field = new SearchConditionField();
                field.FieldNm = "ProgId";
                field.DisplayNm = "功能ID";
                field.TBAliasNm = 'a';
                field.IsCondition = true;
                fields.Add(field);

                field = new SearchConditionField();
                field.FieldNm = "ProgNm";
                field.DisplayNm = "功能名称";
                field.TBAliasNm = 'a';
                field.IsCondition = true;
                fields.Add(field);
            }
        }

        protected override void BindSmodalDataExt(DataTable currpagedata, int flag, string fieldnm)
        {
            base.BindSmodalDataExt(currpagedata, flag,fieldnm);
            if (flag == 3)
            {
                #region 获取所有功能模型的Progid
                FileOperation fileoperation = new FileOperation();
                fileoperation.FilePath = string.Format(@"{0}\Models\{1}", this.ModelRootPath, SysConstManage.FormSourceNm);
                string[] array = fileoperation.SearchFileNm();
                #region 添加列
                DataColumn col = new DataColumn("ProgId");
                currpagedata.Columns.Add(col);
                currpagedata.PrimaryKey = new DataColumn[] { col };
                col = new DataColumn("ProgNm");
                currpagedata.Columns.Add(col);
                #endregion
                foreach (string item in array)
                {
                    DataRow row = currpagedata.NewRow();
                    row["ProgId"] = item;
                    row["ProgNm"] = AppCom.GetFieldDesc((int)this.Language, item, string.Empty, item);
                    currpagedata.Rows.Add(row);
                }

                #endregion
            }
        }

        public ActionResult SaveActionDetail(List<ActionObj> data, string progid)
        {
            if (data != null)
            {
                DataTable dt = this.LibTables[2].Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].RowState == DataRowState.Deleted) continue;
                    if (string.Compare(dt.Rows[i]["ProgId"].ToString(), progid, true) != 0) continue;
                    if (dt.Rows[i].RowState == DataRowState.Added)
                    {
                        dt.Rows[i].Delete();
                        i--;
                    }
                    else
                    {
                        dt.Rows[i].Delete();
                    }
                }
                
                foreach (ActionObj item in data)
                {
                    DataRow dr = this.LibTables[2].Tables[0].NewRow();
                    dr["ProgId"] = progid;
                    dr["ObjectType"] = item.ObjectType;
                    dr["ObjectId"] = item.ObjectId;
                    dr["GroupId"] = item.GroupId;
                    this.LibTables[2].Tables[0].Rows.Add(dr);
                }
                if (data.Where(i => (i.ObjectId == "bwysdp_btnedit" || i.ObjectId == "bwysdp_btnadd") && i.ObjectType == 1).Count()>=2)
                {
                    DataRow dr = this.LibTables[2].Tables[0].NewRow();
                    dr["ProgId"] = progid;
                    dr["ObjectType"] = 1;
                    dr["ObjectId"] = "bwysdp_btnsave";
                    dr["GroupId"] = progid;
                    this.LibTables[2].Tables[0].Rows.Add(dr);
                }
                
            }
            return Json(new { });
        }

        //public ActionResult GetActionData(string progid,int flag)
        //{
        //    ProgInfo[] allprogid = AppCom.GetAllProgid();
        //    if (flag == 1)
        //    {
        //        var pid = allprogid.FirstOrDefault(i => i.ProgId.ToUpper() == progid.ToUpper());
        //        if (pid != null)
        //        {
        //            LibFormPage formpage = ModelManager.GetModelBypath<LibFormPage>(this.ModelRootPath, progid, pid.Package);

        //        }
        //    }
        //    return Json(new { });
        //}
    }
}