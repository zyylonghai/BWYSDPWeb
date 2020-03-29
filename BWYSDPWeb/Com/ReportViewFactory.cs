using SDPCRL.COM;
using SDPCRL.COM.ModelManager;
using SDPCRL.COM.ModelManager.FormTemplate;
using SDPCRL.COM.ModelManager.Reports;
using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BWYSDPWeb.Com
{
    public class ReportViewFactory
    {
        private StringBuilder _page = null;
        private StringBuilder _script = null;
        private Dictionary<string, bool> _gridGroupdic = null;
        private List<string> _tableScriptlst = null;
        private string _pagetitle = string.Empty;
        private string _progid = null;

        #region 公开属性
        public string ControlClassNm { get; set; }
        //public string DSID { get; set; }
        public string Package { get; set; }
        public string ScriptFile { get; set; }

        public Language Language { get; set; }
        #endregion
        public ReportViewFactory(string progid)
        {
            _page = new StringBuilder();
            _script = new StringBuilder();
            _gridGroupdic = new Dictionary<string, bool>();
            _tableScriptlst = new List<string>();
            this._progid = progid;
        }

        public string PageHtml
        {
            get
            {
                StringBuilder jsandcss = new StringBuilder();
                //if (_hasSearchModal)
                //{
                    jsandcss.Append("@Styles.Render(\"~/Content/bootstrapTable\")");
                    jsandcss.Append("@Scripts.Render(\"~/bundles/bootstrapTable\")");
                    jsandcss.Append("@Scripts.Render(\"~/bundles/bootstrapTableExport\")");
                    //jsandcss.Append("@Scripts.Render(\"~/bundles/sdp_com\")");
                    jsandcss.Append("@Scripts.Render(\"~/bundles/searchmodal\")");
                //}
                //if (_gridGroupdic.Count > 0)
                //{
                //    //jsandcss.Append("@Scripts.Render(\"~/bundles/sdp_com\")");
                //    jsandcss.Append("@Scripts.Render(\"~/bundles/TableModal\")");
                //}
                //if (_dateElemlst.Count > 0 || _datetimeElemlst.Count > 0) //加载日期控件的js，css
                //{
                    jsandcss.Append("@Scripts.Render(\"~/Scripts/lib/laydate/laydate\")");
                //}
                if (!string.IsNullOrEmpty(ScriptFile))
                {
                    _page.AppendLine();
                    _page.Append("<script src = \"/Scripts/" + Package + "/" + ScriptFile + "\" ></script>");
                }
                //if (_hasSearchModal)
                //{
                //    if (_gridGroupdic.Count == 0)
                //        jsandcss.Append("@Scripts.Render(\"~/bundles/sdp_com\")");
                //    jsandcss.Append("@Scripts.Render(\"~/bundles/searchmodal\")");
                //}
                return jsandcss.Append(_page.ToString()).ToString();
            }
        }

        public void BeginPage(string pagetitle)
        {
            this._pagetitle = AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, pagetitle);
            _page.Append("<div class=\"container-fluid\">");
            _page.Append("<div class=\"panel panel-default\">");
            _page.Append("<div class=\"panel-heading\">@Html.GetFieldDesc(\"\",\"\",\"" + pagetitle + "\")</div>");
        }
        /// <summary>
        /// 创建body
        /// </summary>
        public void CreateBody()
        {
            _page.Append("<div class=\"panel-body\">");
            //_page.Append("<div class=\"btn-group\" role=\"group\">");//按钮组
            //_page.Append("</div>");
            //_page.Append("<br /><br />");
        }
        /// <summary>
        /// 创建表格Form
        /// </summary>
        public void CreateForm()
        {
            //_page.Append("<form class=\"form-horizontal\" action=\"Save\">");
            _page.Append("@using(Html.BeginForm(\"\", \"\",new { sdp_pageid =\"" + this._progid + "\",sdp_dsid=\"\" },FormMethod.Post,new{@class=\"form-horizontal\",@id=\"sdp_rptForm\",@enctype=\"multipart/form-data\" }))");
            _page.Append("{ <input type=\"hidden\" id=\"sdp_rptCols\"/>");
        }

        /// <summary>
        /// 创建表格组
        /// </summary>
        /// <param name="title"></param>
        public void CreateGridGroup(LibReportGrid grid)
        {
            EndprePanel();
            string id = string.Format("GridGroup_{0}", grid.GridGroupName);
            string contentid = string.Format("{0}_info", id);
            _page.Append("<div class=\"panel-group\" id=\"" + id + "\">");
            _page.Append("<div class=\"panel panel-default\">");

            //面板标题
            _page.Append("<div class=\"panel-heading\" style=\"background-color:#dff0d8; text-align:left\">");
            _page.Append("<h4 class=\"panel-title\">");
            _page.Append("<a data-toggle=\"collapse\" data-parent=\"#" + id + "\" href=\"#" + contentid + "\">@Html.GetFieldDesc(\"" + _progid + "\",\"" + string.Empty + "\",\"" + grid.GridGroupName + "\")</a>");
            _page.Append("</h4>");
            _page.Append("</div>");

            //面板内容
            _page.Append("<div id=\"" + contentid + "\" class=\"panel-collapse in \">");
            _page.Append("<div class=\"panel-body\">");

            #region 查询条件
            if (grid.ReportFields != null)
            {
                //_page.Append("<div class=\"content-head mgb10\">");
                foreach (LibReportField f in grid.ReportFields)
                {
                    if (f.IsSearchCondition)
                    {
                        string nm = string.Format("{0}_{1}", LibSysUtils.ToCharByTableIndex(f.FromTableIndex), f.Name);
                        _page.Append("<label class=\"form-inline\"><label> @Html.GetFieldDesc(\"" + (string.IsNullOrEmpty(f.FromTableNm) ? _progid : grid.DSID) + "\",\"" + (f.FromTableNm) + "\",\"" + f.Name + "\")</label>");

                        _page.AppendFormat("<select class=\"form-control\" name=\"{0}{1}\">", SysConstManage.sdp_smodalsymbol, nm);
                        foreach (var item in Enum.GetValues(typeof(SmodalSymbol)))
                        {
                            _page.AppendFormat("<option value=\"{0}\">{1}</option>", (int)item, ReSourceManage.GetResource(item));
                        }
                        _page.Append("</select>");

                        _page.AppendFormat("<input type=\"text\" class=\"form-control\" name=\"{0}{1}_1\"/>", SysConstManage.sdp_smodalval, nm);
                        _page.AppendFormat("<input type=\"text\" class=\"form-control\" name=\"{0}{1}_2\"/>", SysConstManage.sdp_smodalval, nm);

                        _page.AppendFormat("<select class=\"form-control\" name=\"{0}{1}\">", SysConstManage.sdp_smodallogic, nm);
                        foreach (var item in Enum.GetValues(typeof(Smodallogic)))
                        {
                            _page.AppendFormat("<option value=\"{0}\">{1}</option>", (int)item, ReSourceManage.GetResource(item));
                        }
                        _page.Append("</select>");

                        _page.Append("</label>");

                        _page.Append("<label class=\"form-inline\"> &nbsp;&nbsp;&nbsp;&nbsp; </label>");
                    }
                }
                //_page.Append("</div>");
            }

            _page.Append("<label class=\"form-inline\"> &nbsp;&nbsp;&nbsp;&nbsp; </label>");



            _page.Append("<button id=\"sdp_rptbtnSearch\" type=\"button\" class=\"btn btn-primary\">@Html.GetFieldDesc(\"" + string.Empty + "\",\"" + string.Empty + "\",\"sdp_btnselect\")</button>");
            #endregion 
            #region toolbar
            _page.Append("<div id=\"" + grid.GridGroupName + "_toolbar\" class=\"btn-group\">");
            //_page.Append("<form class=\"form-inline\">" +
            //    "< div class=\"form-group\">" +
            //    "<label class=\"sr-only\" for=\"product_line\">产品线</label>" +
            //    "<div class=\"input-group\">" +
            //    "<div class=\"input-group-addon\">产品线</div>" +
            //    "<select class=\"form-control\" name=\"product_line\" id=\"productLine\"><option value = \"\" > 请选择产品线...</option></select>" +
            //    "</div>" +
            //    "</div>" +
            //    "<div class=\"form-group\">" +
            //    "<label class=\"sr-only\" for=\"msg_type\">消息类型</label>" +
            //    "<div class=\"input-group\">" +
            //    "<div class=\"input-group-addon\">消息类型</div>" +
            //    "<select class=\"form-control\" name=\"msg_type\" id=\"msgType\"><option value = \"\" > 请选择消息类型...</option></select>" +
            //    "</div>" +
            //    "</div>" +
            //    "<div class=\"form-group\">" +
            //    "<label class=\"sr-only\" for=\"msg_type\">消息类型</label>" +
            //    "<div class=\"input-group\"><div class=\"input-group-addon\">消息类型</div>" +
            //    "<input type = \"text\" class=\"form-control\" name=\"searchTexts\" id=\"searchText\" placeholder=\"请输入消息名称或内容关键字...\">" +
            //    "</div>" +
            //    "<button type = \"button\" class=\"btn btn-primary queryButton\">查询</button>" +
            //    "</form>");
            if (grid.GdButtons != null)
            {
                foreach (LibGridButton btn in grid.GdButtons)
                {
                    _page.Append("<button id=\"" + btn.GridButtonName + "\" type=\"button\" class=\"btn btn-default\" onclick=\"return " + btn.GridButtonEvent + "\">");
                    //_page.Append("<button id=\"" + grid.GridGroupName + "_sdp_deletrow\" type=\"button\" class=\"btn btn-default\">");
                    _page.Append("<i class=\"glyphicon glyphicon-pause\"></i>@Html.GetFieldDesc(\"" + _progid + "\",\"" + string.Empty + "\",\"" + btn.GridButtonName + "\")");//删除
                    _page.Append("</button>");
                }
            }
            _page.Append("</div>");
            #endregion
            _page.Append("<table id=\"" + grid.GridGroupName + "\"></table>");

            _gridGroupdic.Add(id, false);
            AddGridColumns(grid);
        }

        /// <summary>
        /// 添加表格列
        /// </summary>
        /// <param name="fields"></param>
        private void AddGridColumns(LibReportGrid grid)
        {
            StringBuilder table = new StringBuilder();
            StringBuilder rptcols = new StringBuilder();
            StringBuilder hidecolumns = new StringBuilder();
            //StringBuilder tbformfield = new StringBuilder();
            StringBuilder childformfield = new StringBuilder();
            //LibFromSourceField libFromSource = null;
            //LibField libField = null;
            //StringBuilder validatorAttr = null;
            //List<string> valus = null;
            if (grid.ReportFields == null || (grid.ReportFields != null && grid.ReportFields.Count == 0)) return;
            string param = string.Format("tb{0}", _tableScriptlst.Count + 1);
            table.Append(string.Format("var {0} = new LibTable(\"{1}\");", param, grid.GridGroupName));
            table.Append(string.Format("{0}.$table.url =\"/{1}/RptBindTableData?gridid={2}&dsid={3}&tbnm={4}\";", param, string.IsNullOrEmpty(ControlClassNm) ? this.Package : ControlClassNm, grid.GridGroupName,grid.DSID ,grid.FromTable));
            table.Append(string.Format("{0}.$table.toolbar =\"#{1}_toolbar\";", param, grid.GridGroupName));
            #region 是否显示父子表
            //if (!string.IsNullOrEmpty(grid.ChildGridNm)) // 显示父子表
            //{
            //    table.Append(string.Format("{0}.$table.detailView =true;", param));
            //    //找出子表格组
            //    LibGridGroup childgrid = this.LibFormPage.GridGroups.FindFirst("GridGroupName", grid.ChildGridNm);
            //    string childgridmodalid = string.Format("GridGroup_{0}", childgrid.GridGroupName);
            //    Childrengrids.Add(childgrid);
            //    _gridGroupdic.Add(childgridmodalid, true);
            //    if (childgrid != null && childgrid.GdGroupFields != null && childgrid.GdGroupFields.Count > 0)
            //    {
            //        table.Append(string.Format("{0}.SubTable =new LibTable(\"{1}\");", param, childgrid.GridGroupName));
            //        table.Append(string.Format("{0}.SubTable.$table.detailView =false;", param));
            //        table.Append(string.Format("{0}.SubTable.$table.hasoperation =false;", param));
            //        //table.Append(string.Format("{0}.SubTable.$table.url =\"/{1}/BindTableData?gridid={2}&deftb={3}&tableNm={4}\";", param, string.IsNullOrEmpty(childgrid.ControlClassNm) ? this.Package : childgrid.ControlClassNm, childgrid.GridGroupName, childgrid.GdGroupFields[0].FromDefTableNm, childgrid.GdGroupFields[0].FromTableNm));
            //        table.Append(string.Format("{0}.SubTable.$subtableParam.gridid ='{1}';", param, childgrid.GridGroupName));
            //        table.Append(string.Format("{0}.SubTable.$subtableParam.deftbnm ='{1}';", param, childgrid.GdGroupFields[0].FromDefTableNm));
            //        table.Append(string.Format("{0}.SubTable.$subtableParam.tablenm ='{1}';", param, childgrid.GdGroupFields[0].FromTableNm));
            //        table.Append(string.Format("{0}.SubTable.$subtableParam.controlnm ='{1}';", param, string.IsNullOrEmpty(childgrid.ControlClassNm) ? this.Package : childgrid.ControlClassNm));
            //        if (childgrid.HasSummary)
            //        {
            //            table.Append(string.Format("{0}.SubTable.$table.showFooter={1};", param, childgrid.HasSummary ? "true" : "false"));
            //        }
            //        if (childgrid.SingleSelect)
            //        {
            //            table.Append(string.Format("{0}.SubTable.$table.singleSelect={1};", param, childgrid.SingleSelect ? "true" : "false"));
            //        }
            //        table.Append(string.Format("{0}.SubTable.$table.columns = [", param));
            //        table.Append("{checkbox: true,visible: true }");
            //        #region sdp_rowid 列
            //        table.Append(",{field:'sdp_rowid',title: 'sdp_rowid',align: 'center',visible: false}");
            //        //hidecolumns.Append(string.Format("$('#{0}').bootstrapTable('hideColumn', 'sdp_rowid');", grid.GridGroupName));
            //        #endregion
            //        foreach (LibGridGroupField field in childgrid.GdGroupFields)
            //        {
            //            if (field.IsFromSourceField)
            //            {
            //                libFromSource = GetSourceField(field.Name, field.FromDefTableNm, field.FromTableNm);
            //            }
            //            table.Append(",{");
            //            string fielddisplaynm = "@Html.GetFieldDesc(\"" + ((field.IsFromSourceField && libFromSource != null) ? libFromSource.FromDataSource : this.DSID) + "\",\"" + ((field.IsFromSourceField && libFromSource != null) ? libFromSource.FromStructTableNm : field.FromTableNm) + "\",\"" + field.Name + "\")";
            //            //string fielddisplaynm = AppCom.GetFieldDesc((int)Language, (field.IsFromSourceField && libFromSource != null) ? libFromSource.FromDataSource : this.DSID,
            //            //                                            (field.IsFromSourceField&& libFromSource !=null) ? libFromSource.FromStructTableNm : field.FromTableNm, field.Name);
            //            //table.Append(string.Format("field:'{0}',title: '{1}',align: 'center',sortable:{2},", field.Name, field.DisplayName, field.HasSort ? "true" : "false"));
            //            table.Append(string.Format("field:'{0}',title: '{1}',align: 'center',sortable:{2},visible:{3},", field.Name, fielddisplaynm, field.HasSort ? "true" : "false", field.Hidden ? "false" : "true"));
            //            table.Append("formatter: function (value, row, index) {");
            //            if (field.ElemType == ElementType.Date)
            //            {
            //                table.Append(string.Format("return \"<div {0}>\" + TimeConverToStr(value) + \"</div>\";", field.ReadOnly ? "readonly" : ""));
            //            }
            //            else if (field.ElemType == ElementType.Img)
            //            {
            //                table.Append(string.Format("return \"<div {0}>\" + ImgFormatter(value) + \"</div>\";", field.ReadOnly ? "readonly" : ""));
            //            }
            //            else if (field.ElemType == ElementType.Select)
            //            {
            //                LibField libField = GetField(field.FromDefTableNm, field.FromTableNm, field.Name);
            //                table.Append("var keyvalues=[");
            //                foreach (LibKeyValue item in libField.Items)
            //                {
            //                    if (libField.Items.IndexOf(item) > 0)
            //                    {
            //                        table.Append(",");
            //                    }
            //                    if (string.IsNullOrEmpty(item.FromkeyValueID))
            //                    {
            //                        table.Append("{fromkeyvalueid:\"" + item.FromkeyValueID + "\",key:\"" + item.Key + "\",value:\"@Html.GetFieldDesc(\"" + DSID + "\",\"" + field.FromDefTableNm + "\",\"" + string.Format("{0}_{1}", field.Name, item.Key) + "\")\"}");
            //                    }
            //                    else
            //                        table.Append("{fromkeyvalueid:\"" + item.FromkeyValueID + "\",key:\"" + item.Key + "\",value:\"@Html.GetFieldDesc(\"" + item.FromkeyValueID + "\",\"" + string.Empty + "\",\"" + item.Key.ToString() + "\")\"}");
            //                }
            //                table.Append("];");
            //                table.Append("var o=FindKeyValue(keyvalues,value);");
            //                table.Append(string.Format("return \"<div {0}>\" +o.value+ \"</div>\";", field.ReadOnly ? "readonly" : ""));
            //            }
            //            else
            //            {
            //                if (!string.IsNullOrEmpty(field.Formatter))
            //                {
            //                    if (field.Formatter.Contains(SysConstManage.DollarSign))
            //                    {
            //                        table.Append(string.Format("return {0};", field.Formatter.Split('.')[1]));
            //                    }
            //                    else
            //                    {
            //                        table.Append(string.Format("return \"<div {0}>\" + row." + field.Formatter + " + \"</div>\";", field.ReadOnly ? "readonly" : ""));
            //                    }
            //                }
            //                table.Append(string.Format("return \"<div {0}>\" + value + \"</div>\";", field.ReadOnly ? "readonly" : ""));
            //            }
            //            table.Append("}");
            //            if (childgrid.HasSummary)
            //            {
            //                //设置汇总行，
            //                table.Append(",footerFormatter: function() {return '汇总'}");
            //                childgrid.HasSummary = false;
            //            }
            //            table.Append("}");

            //        }
            //        childformfield.Append("</div>");// 结束 form-group
            //        table.Append("];");
            //    }
            //    _tbmodalFormfields.Add(childgridmodalid, childformfield.ToString());

            //    #region tablemodal脚本
            //    string childmdparam = string.Format("childtbmodal{0}", Childrengrids.Count + 1);
            //    table.Append(string.Format("var {0}=new libTableModal(\"{1}\");", childmdparam, string.Format("sdp_tbmdl_{0}", childgridmodalid)));
            //    table.Append(string.Format("{0}.initialModal();", childmdparam));
            //    table.Append("$('#sdp_tbmodalbtn" + childgridmodalid + "').click(function () {" + childmdparam + ".Confirm();});");
            //    #endregion
            //}
            //colcout = 0;//重置colcout值
            #endregion

            table.Append(string.Format("{0}.$table.showFooter={1};", param, "true" ));
            table.Append(string.Format("{0}.$table.singleSelect={1};", param, "true"));
            table.Append(string.Format("{0}.$table.hasoperation={1};", param, "false"));
            table.Append(string.Format("{0}.$table.columns = [", param));
            table.Append("{checkbox: true,visible: true }");
            #region sdp_rowid 列
            //table.Append(",{field:'sdp_rowid',title: 'sdp_rowid',align: 'center',visible:true,switchable:false}");
            //hidecolumns.Append(string.Format("$('#{0}').bootstrapTable('hideColumn', 'sdp_rowid');", grid.GridGroupName));
            #endregion
            bool hasfooter = true;
            foreach (LibReportField field in grid.ReportFields)
            {
                table.Append(",{");
                string fielddisplaynm = "@Html.GetFieldDesc(\"" + (string.IsNullOrEmpty(field.FromTableNm) ? _progid : grid.DSID) + "\",\"" + (field.FromTableNm) + "\",\"" + field.Name + "\")";
                table.Append(string.Format("field:'{0}',title: '{1}',align: 'center',sortable:{2},visible: true,switchable:true,", field.Name, fielddisplaynm, field.HasSort ? "true" : "false"));
                table.Append("formatter: function (value, row, index) {");
                if (field.ElemType == ElementType.Date)
                {
                    table.Append(string.Format("return \"<div >\" + TimeConverToStr(value) + \"</div>\";"));
                }
                else if (field.ElemType == ElementType.Img)
                {
                    table.Append(string.Format("return \"<div >\" + ImgFormatter(value) + \"</div>\";"));
                }

                else
                {
                    if (!string.IsNullOrEmpty(field.Formatter))
                    {
                        if (field.Formatter.Contains(SysConstManage.DollarSign))
                        {
                            table.Append(string.Format("return {0};", field.Formatter.Split('.')[1]));
                        }
                        else
                        {
                            table.Append(string.Format("return \"<div>\" + row." + field.Formatter + " + \"</div>\";"));
                        }
                    }
                    else
                        table.Append(string.Format("return \"<div>\" + NullFormatter(value) + \"</div>\";"));
                }
                table.Append("}");//结束 formatter
                if (hasfooter)
                {
                    table.Append(",footerFormatter: function(value) {return '汇总';}");
                    hasfooter = false;
                }
                table.Append("}");

                #region 存储报表列
                string colnm = string.Format("{0}.{1}", LibSysUtils.ToCharByTableIndex(field.FromTableIndex), field.Name);
                rptcols.Append("$('#sdp_rptCols').val($('#sdp_rptCols').val()+\""+colnm+"\"+',');");
                #endregion
            }

            //tbformfield.Append("</div>");// 结束 form-group
            //}
            table.Append("];");

            #region 移除权限内的字段
            //table.Append(string.Format("$.each({0},function (i, o)", string.Format("{0}.$table.columns", param)));
            //table.Append("{ let result=AuthorityCheck(authorityObjs, o.field, \"" + grid.GridGroupName + "\");");
            //table.Append("if(result){o.visible=false; o.switchable=false;" +
            //    "$('#sdp_fieldlabel_" + grid.GdGroupFields[0].FromTableNm + "_'+o.field).hide();" +
            //    "$('#sdp_fielddiv_" + grid.GdGroupFields[0].FromTableNm + "_'+o.field).hide();}");
            //table.Append("});");
            #endregion

            table.Append(string.Format("{0}.initialTable();", param));
            table.Append(hidecolumns);

            #region toobar 按钮事件 旧代码
            //table.Append("$('#"+grid .GridGroupName+"_sdp_addrow').click(function () {"+param+".AddRow();});");
            //table.Append("$('#" + grid.GridGroupName + "_sdp_editrow').click(function () {" + param + ".EditRow();});");
            //table.Append("$('#" + grid.GridGroupName + "_sdp_deletrow').click(function () {" + param + ".DeleteRow();});");
            #endregion
            _tableScriptlst.Add(table.ToString());
            _tableScriptlst.Add(rptcols.ToString());
            //_tbmodalFormfields.Add(string.Format("GridGroup_{0}", grid.GridGroupName), tbformfield.ToString());

        }

        /// <summary>
        /// 结束视图页
        /// </summary>
        public void EndPage()
        {
            EndprePanel();
            _page.Append("}");//form  结束
            _page.Append("</div>");//panel - body
            _page.Append("</div>");//panel panel - default
            _page.Append("</div>");//container - fluid
            CreateJavaScript();
            _page.AppendLine();
            _page.Append(_script.ToString());

        }

        #region 私有函数

        private void CreateJavaScript()
        {
            _script.Append("<script type=\"text/javascript\">");
            #region 表单验证
            _script.Append("$().ready(function() { $('#sdp_rptForm').validate();});");
            #endregion
            _script.Append("$(function (){");

            _script.Append("$('#bwysdp_progid').val(\"" + this._progid + "\");");
            _script.Append("$('#bwysdp_dsid').val(\"\");");

            //if (hasformandboy)
            //{
                #region 获取权限对象数据
                _script.Append("var viewModelObj = JSON.parse(\"@Model\".replace(new RegExp('&quot;', \"gm\"), '\"'));");
                _script.Append("var authorityObjs = viewModelObj.AuthorityObjs;");
                _script.Append("Authorize(authorityObjs);");
            //_script.AppendFormat("var sdp_fmfieldauthoris={0};", _fmgroupAuthorisScriplst.Append("]").ToString());
            //_script.Append("FormGroupAuthorize(authorityObjs,sdp_fmfieldauthoris);");
            #endregion

            //}
            #region pageload
            _script.Append("$.ajax({url: \" /" + (string.IsNullOrEmpty(this.ControlClassNm) ? this.Package : this.ControlClassNm) + "/BasePageLoad\",data: \"flag=0\",type: 'Post',async: false,success: function (obj) {},");
            _script.Append("error: function (XMLHttpRequest, textStatus, errorThrown) {alert(XMLHttpRequest.status.toString() + \":\" + XMLHttpRequest.readyState.toString() + \", \" + textStatus + errorThrown);}");
            _script.Append(" });");
            #endregion
            #region 表格脚本
            foreach (string script in _tableScriptlst)
            {
                _script.Append(script);
                _script.AppendLine();
            }
            #endregion
            //查询按钮事件
            _script.Append("$('#sdp_rptbtnSearch').click(function (){ SDP_RptBtnSearch(\"" + (string.IsNullOrEmpty(this.ControlClassNm) ? "DataBase" : this.ControlClassNm) + "\");});");

            _script.Append("})");
            _script.Append("</script>");
        }
        /// <summary>
        /// 给上一个面板添加结束标志
        /// </summary>
        private void EndprePanel()
        {
            KeyValuePair<string, bool> griditem = _gridGroupdic.FirstOrDefault(i => !i.Value);
            if (griditem.Key != null)
            {
                _page.Append("</div>");
                _page.Append("</div>");
                _page.Append("</div>");
                _page.Append("</div>");
                _gridGroupdic[griditem.Key] = true;
            }
        }
        #endregion 
    }
}