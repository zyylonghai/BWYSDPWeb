using BWYSDPWeb.Com;
using BWYSDPWeb.Models;
using SDPCRL.CORE;
using SDPCRL.CORE.FileUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllComController
{
    public class CodeRuleConfigController : ComController
    {
        protected override void SetSearchFieldExt(List<SearchConditionField> fields, string fieldNm, int flag)
        {
            base.SetSearchFieldExt(fields, fieldNm, flag);
            if (flag == 3 && fieldNm == "ProgId")
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
            base.BindSmodalDataExt(currpagedata, flag, fieldnm);
            if (flag == 3 && fieldnm == "ProgId")
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
    }
}