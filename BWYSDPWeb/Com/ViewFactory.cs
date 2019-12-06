using BWYSDPWeb.Models;
using SDPCRL.COM;
using SDPCRL.COM.ModelManager;
using SDPCRL.COM.ModelManager.FormTemplate;
using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BWYSDPWeb.Com
{
    public class ViewFactory
    {
        private StringBuilder _page = null;
        private StringBuilder _script = null;
        private Dictionary<string, bool> _panelgroupdic = null;
        private Dictionary<string, bool> _gridGroupdic = null;
        private Dictionary<string, string> _tbmodalFormfields = null;
        private List<string> _dateElemlst = null;
        private List<string> _datetimeElemlst = null;
        private List<string> _tableScriptlst = null;
        private List<string> _searchModelIds = null;
        private string _progid = null;
        //private string _dsid = null;
        private bool _hasSearchModal = true;// 是否有搜索控件。
        private string _pagetitle = string.Empty;
       //private Dictionary<string, bool> _fomGroupdic=null;

        #region 公开属性
        public string ControlClassNm { get; set; }
        public string DSID { get; set; }
        public string Package { get; set; }

        public Language Language { get; set; }

        /// <summary>
        /// 用于存储 信息组的字段。
        /// </summary>
        public Dictionary<string, List<string>> Formfields { get; set; }

        public LibDataSource LibDataSource { get; set; }

        public LibFormPage LibFormPage { get; set; }

        public List<LibGridGroup> Childrengrids { get; set; }
        #endregion
        public ViewFactory()
        {
            _page = new StringBuilder();
            _script = new StringBuilder();
            _dateElemlst = new List<string>();
            _datetimeElemlst = new List<string>();
            _panelgroupdic = new Dictionary<string, bool>();
            _gridGroupdic = new Dictionary<string, bool>();
            _tableScriptlst = new List<string>();
            _searchModelIds = new List<string>();
            Formfields = new Dictionary<string, List<string>>();
            _tbmodalFormfields = new Dictionary<string, string>();
            Childrengrids = new List<LibGridGroup>();
            //_fomGroupdic = new Dictionary<string, bool>();
        }
        public ViewFactory(string progid)
            : this()
        {
            this._progid = progid;
        }
        //public ViewFactory(string progid,string dsid)
        //    : this()
        //{
        //    this._progid = progid;
        //    this._dsid = dsid;
        //}

        public string PageHtml
        {
            get
            {
                StringBuilder jsandcss = new StringBuilder();
                if (_hasSearchModal)
                {
                    jsandcss.Append("@Styles.Render(\"~/Content/bootstrapTable\")");
                    jsandcss.Append("@Scripts.Render(\"~/bundles/bootstrapTable\")");
                    jsandcss.Append("@Scripts.Render(\"~/bundles/bootstrapTableExport\")");
                    //jsandcss.Append("@Scripts.Render(\"~/bundles/sdp_com\")");
                    jsandcss.Append("@Scripts.Render(\"~/bundles/searchmodal\")");
                }
                if (_gridGroupdic.Count > 0)
                {
                    //jsandcss.Append("@Scripts.Render(\"~/bundles/sdp_com\")");
                    jsandcss.Append("@Scripts.Render(\"~/bundles/TableModal\")");
                }
                if (_dateElemlst.Count > 0 ||_datetimeElemlst .Count >0) //加载日期控件的js，css
                {
                    jsandcss.Append("@Scripts.Render(\"~/Scripts/lib/laydate/laydate\")");
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
        /// <summary>
        /// 开始创建视图页
        /// </summary>
        /// <param name="pagetitle"></param>
        public void BeginPage(string pagetitle)
        {
            this._pagetitle = pagetitle;
            _page.Append("<div class=\"container-fluid\">");
            _page.Append("<div class=\"panel panel-default\">");
            _page.Append("<div class=\"panel-heading\">" + pagetitle + "</div>");
        }
        /// <summary>
        /// 创建body
        /// </summary>
        public void CreateBody()
        {
            _page.Append("<div class=\"panel-body\">");
            _page.Append("<div class=\"btn-group\" role=\"group\">");//按钮组
            _page.Append("<button id=\"bwysdp_btnsave\" type=\"button\" class=\"btn btn-default\"><i class=\"fa fa-fw fa-save\"></i>" + AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, "sdp_btnsave") + "</button>");
            //_page.Append("<i class=\"fa fa-fw fa-save\"></i>"+AppCom .GetFieldDesc ((int)Language ,string.Empty ,string.Empty , "sdp_btnsave") +"</button>");
            _page.Append("<button type=\"button\" class=\"btn btn-default\"><i class=\"glyphicon glyphicon-trash\"></i>" + AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, "sdp_btnDelete") + "</button>");
            //_page.Append("<i class=\"glyphicon glyphicon-trash\"></i>" + AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, "sdp_btnDelete") + "</button>");
            _page.Append("<button type=\"button\" class=\"btn btn-default\"><i class=\"fa fa-fw fa-copy\"></i>" + AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, "sdp_btncopy") + "</button>");

            //_page.Append("<button type=\"button\" class=\"btn btn-default\"><i class=\"fa fa-fw fa-cut\"></i></button>");
            //_page.Append("<button type=\"button\" class=\"btn btn-default\"><i class=\"fa fa-fw fa-copy\"></i></button>");
            //_page.Append("<button type=\"button\" class=\"btn btn-default\"><i class=\"fa fa-fw fa-clipboard\"></i></button>");

            _page.Append("<button id=\"bwysdp_btnSearch\" type=\"button\" class=\"btn btn-default\" data-toggle=\"modal\" data-target=\"#searchModal\" data-modalnm=\"" + this._pagetitle + "\" data-progid=\"" + this._progid + "\" data-deftb=\"\" data-tbstruct=\"" + GetMastTable() + "\"  data-controlnm=\"" + (string.IsNullOrEmpty(this.ControlClassNm) ? this.Package : this.ControlClassNm) + "\" data-flag=\"1\">");
            _page.Append("<i class=\"glyphicon glyphicon-search\"></i>" + AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, "sdp_btnsearch") + "</button>");

            _page.Append("</div>");
            _page.Append("<br /><br />");
        }
        /// <summary>
        /// 创建表格Form
        /// </summary>
        public void CreateForm()
        {
            //_page.Append("<form class=\"form-horizontal\" action=\"Save\">");
            _page.Append("@using(Html.BeginForm(\"Save\", \"" + (string.IsNullOrEmpty(this.ControlClassNm) ? "DataBase" : this.ControlClassNm) + "\",new { sdp_pageid =\"" + this._progid + "\",sdp_dsid=\"" + this.DSID + "\" },FormMethod.Post,new{@class=\"form-horizontal\",@id=\"sdp_form\" }))");
            _page.Append("{");
        }

        /// <summary>
        ///创建面板(PanelGroup)
        /// </summary>
        public void CreatePanelGroup(string title)
        {
            EndprePanel();
            string id = string.Format("PanelGroup{0}", _panelgroupdic.Count + 1);
            string contentid = string.Format("{0}_info", id);
            _page.Append("<div class=\"panel-group\" id=\"" + id + "\">");
            _page.Append("<div class=\"panel panel-default\">");

            //面板标题
            _page.Append("<div class=\"panel-heading\" style=\"background-color:#dff0d8; text-align:left\">");
            _page.Append("<h4 class=\"panel-title\">");
            _page.Append("<a data-toggle=\"collapse\" data-parent=\"#" + id + "\" href=\"#" + contentid + "\">" + title + "</a>");
            _page.Append("</h4>");
            _page.Append("</div>");

            //面板内容
            _page.Append("<div id=\"" + contentid + "\" class=\"panel-collapse in \">");
            _page.Append("<div class=\"panel-body\">");


            _panelgroupdic.Add(id, false);
        }

        //public void CreateFormGroup()
        //{
        //    foreach (KeyValuePair<string, bool> item in _fomGroupdic)
        //    {
        //        if (!item.Value)
        //        {
        //            _page.Append("</div>");
        //            _fomGroupdic[item.Key] = true;
        //        }
        //    }
        //    _page.Append("<div class=\"form-group\">");
        //    _fomGroupdic.Add(string.Format("formgroup{0}", _fomGroupdic.Count + 1), false);
        //}

        /// <summary>
        /// 添加信息组字段
        /// </summary>
        /// <param name="fields"></param>
        public void AddFormGroupFields(LibCollection<LibFormGroupField> fields)
        {
            int colcout = 0;
            List<string> valus = null;
            StringBuilder validatorAttr = null;
            LibField libField = null;
            List<LibFormGroupField> textarealst = new List<LibFormGroupField>();
            foreach (LibFormGroupField field in fields)
            {
                if (!this.Formfields.TryGetValue(field.FromTableNm, out valus))
                {
                    valus = new List<string>();
                    this.Formfields.Add(field.FromTableNm, valus);
                }
                valus.Add(field.Name);
                if (field.ElemType == ElementType.Textarea) { textarealst.Add(field); continue; }
                if (colcout % 12 == 0)
                {
                    if (colcout != 0)
                    {
                        _page.Append("</div>");
                    }
                    _page.Append("<div class=\"form-group\">");
                }
                string id = string.Format("{0}_{1}", field.FromTableNm, field.Name);
                string name = string.Format("{0}.{1}", field.FromTableNm, field.Name);
                #region 字段属性验证设置
                validatorAttr = new StringBuilder();
                validatorAttr.Append(field.IsAllowNull ? " required=\"required\"" : "");
                validatorAttr.AppendFormat(" maxlength=\"{0}\"", field.FieldLength);
                validatorAttr.AppendFormat(" {0} ", field.Readonly ? "readonly" : "");
                #endregion
                string displaynm = AppCom.GetFieldDesc((int)Language, this.DSID, field.FromTableNm, field.Name);
                //_page.Append("<label for=\"" + field.Name + "\" class=\"col-sm-1 control-label\">" + field.DisplayName+ (field.IsAllowNull ? "<font color=\"red\">*</font>" : "") + "</label>");
                _page.Append("<label for=\"" + field.Name + "\" class=\"col-sm-1 control-label\">" + displaynm + (field.IsAllowNull ? "<font color=\"red\">*</font>" : "") + "</label>");
                _page.Append("<div class=\"col-sm-" + field.Width + "\">");
                switch (field.ElemType)
                {
                    case ElementType.Date:
                        _dateElemlst.Add(id);
                        _page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + displaynm + "\" " + validatorAttr.ToString() + ">");
                        break;
                    case ElementType.DateTime:
                        _datetimeElemlst.Add(id);
                        _page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + displaynm + "\" " + validatorAttr.ToString() + ">");
                        break;
                    case ElementType.Select:
                        _page.Append("<select class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" " + validatorAttr.ToString() + ">");
                         libField = GetField(field.FromDefTableNm, field.FromTableNm, field.Name);
                        foreach (LibKeyValue keyval in libField.Items)
                        {
                            if (string.IsNullOrEmpty(keyval.FromkeyValueID))
                            {
                                _page.Append("<option value=\"" + keyval.Key + "\">" + AppCom.GetFieldDesc((int)Language, this.DSID, field.FromDefTableNm, string.Format("{0}_{1}", field.Name, keyval.Key)) + "</option>");
                            }
                            else
                                _page.Append("<option value=\"" + keyval.Key + "\">" + AppCom.GetFieldDesc((int)Language, keyval.FromkeyValueID, string.Empty, keyval.Key.ToString ()) + "</option>");
                        }
                        _page.Append("</select>");
                        break;
                    case ElementType.Text:
                        _page.Append("<input type=\"" + (field.IsNumber ? "number" : "text") + "\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + displaynm + "\" " + validatorAttr.ToString() + ">");
                        break;
                    case ElementType.Search:
                        libField = GetField(field.FromDefTableNm, field.FromTableNm, field.Name);
                        _page.Append("<div class=\"input-group\">");
                        _page.Append("<input type=\"" + (field.IsNumber ? "number" : "text") + "\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + displaynm + "\" " + validatorAttr.ToString() + ">");
                        _page.Append("<label id=\"" +string.Format("{0}_desc", id) + "\" ></label>");
                        _page.Append("<span class=\"input-group-btn\">");
                        _page.Append("<button class=\"btn btn-default\" type=\"button\" data-toggle=\"modal\" data-target=\"#searchModal\" data-modalnm=\"" + displaynm + "\" data-fromdsid=\"\" data-deftb=\"\" data-tbstruct=\"" + field.FromTableNm + "\" data-fieldnm=\"" + field.Name + "\"  data-controlnm=\"" + (string.IsNullOrEmpty(this.ControlClassNm) ? this.Package : this.ControlClassNm) + "\"   data-flag=\"2\">");
                        _page.Append("<i class=\"glyphicon glyphicon-search\"></i>");
                        _page.Append("</button>");
                        _page.Append("</span>");
                        _page.Append("</div>");
                        this._searchModelIds.Add(id);
                        //this._hasSearchModal = true;
                        break;
                    //case ElementType.Textarea:
                    //    _page.Append("<textarea class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" rows=\"3\" " + validatorAttr.ToString() + " ></textarea>");
                    //    break;
                }
                //_page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + id + "\" placeholder=\""+field.DisplayName+"\">");
                _page.Append("</div>");//结束 col-sm
                colcout += field.Width + 1;
            }
            _page.Append("</div>");// 结束 form-group
            //textarea控件处理
            foreach (LibFormGroupField item in textarealst)
            {
                _page.Append("<div class=\"form-group\">");
                string id = string.Format("{0}_{1}", item.FromTableNm, item.Name);
                string name = string.Format("{0}.{1}", item.FromTableNm, item.Name);
                #region 字段属性验证设置
                validatorAttr = new StringBuilder();
                validatorAttr.Append(item.IsAllowNull ? " required=\"required\"" : "");
                //validatorAttr.AppendFormat("maxlength=\"{0}\"", item.FieldLength);
                #endregion
                _page.Append("<label for=\"" + item.Name + "\" class=\"col-sm-1 control-label\">" + AppCom.GetFieldDesc((int)Language, this.DSID, item.FromTableNm, item.Name) + (item.IsAllowNull ? "<font color=\"red\">*</font>" : "") + "</label>");
                _page.Append("<div class=\"col-sm-" + item.Width + "\">");
                _page.Append("<textarea class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" rows=\"3\" " + validatorAttr.ToString() + " ></textarea>");
                _page.Append("</div>");//结束 col-sm
                _page.Append("</div>");// 结束 form-group
            }
        }

        /// <summary>
        /// 创建表格组
        /// </summary>
        /// <param name="title"></param>
        public void CreateGridGroup(LibGridGroup grid)
        {
            EndprePanel();
            string id = string.Format("GridGroup_{0}", grid.GridGroupName);
            string contentid = string.Format("{0}_info", id);
            _page.Append("<div class=\"panel-group\" id=\"" + id + "\">");
            _page.Append("<div class=\"panel panel-default\">");

            //面板标题
            _page.Append("<div class=\"panel-heading\" style=\"background-color:#dff0d8; text-align:left\">");
            _page.Append("<h4 class=\"panel-title\">");
            _page.Append("<a data-toggle=\"collapse\" data-parent=\"#" + id + "\" href=\"#" + contentid + "\">" + AppCom .GetFieldDesc((int)Language ,this.DSID ,string.Empty ,grid.GridGroupName ) + "</a>");
            _page.Append("</h4>");
            _page.Append("</div>");

            //面板内容
            _page.Append("<div id=\"" + contentid + "\" class=\"panel-collapse in \">");
            _page.Append("<div class=\"panel-body\">");

            #region toolbar
            _page.Append("<div id=\"" + grid.GridGroupName + "_toolbar\" class=\"btn-group\">");
            _page.Append("<button type=\"button\" class=\"btn btn-default\"  data-toggle=\"modal\" data-target=\"#sdp_tbmdl_" + id + "\" data-gridid=\"" + grid.GridGroupName + "\" data-deftbnm=\"" + grid.GdGroupFields[0].FromDefTableNm + "\" data-tablenm=\"" + grid.GdGroupFields[0].FromTableNm + "\" data-controlnm=\"" + ControlClassNm + "\"  data-cmd=\"Add\">");
            //_page.Append("<button id=\"" + grid.GridGroupName + "_sdp_addrow\" type=\"button\" class=\"btn btn-default\">");
            _page.Append("<i class=\"glyphicon glyphicon-plus\"></i>"+AppCom .GetFieldDesc ((int)Language ,string.Empty ,string.Empty , "sdp_btngridadd") +"");//新增
            _page.Append("</button>");

            _page.Append("<button type=\"button\" class=\"btn btn-default\" onclick=\"return TableBtnEdit(this,'" + grid.GridGroupName + "')\" data-toggle=\"modal\"  data-target=\"#sdp_tbmdl_" + id + "\"  data-gridid=\"" + grid.GridGroupName + "\" data-deftbnm=\"" + grid.GdGroupFields[0].FromDefTableNm + "\" data-tablenm=\"" + grid.GdGroupFields[0].FromTableNm + "\" data-controlnm=\"" + ControlClassNm + "\"  data-cmd=\"Edit\">");
            //_page.Append("<button id=\"" + grid.GridGroupName + "_sdp_editrow\" type=\"button\" class=\"btn btn-default\">");
            _page.Append("<i class=\"glyphicon glyphicon-pencil\"></i>" + AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, "sdp_btngridedit") + "");//编辑
            _page.Append("</button>");

            _page.Append("<button type=\"button\" class=\"btn btn-default\">");
            //_page.Append("<button id=\"" + grid.GridGroupName + "_sdp_deletrow\" type=\"button\" class=\"btn btn-default\">");
            _page.Append("<i class=\"glyphicon glyphicon-trash\"></i>" + AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, "sdp_btngriddelete") + "");//删除
            _page.Append("</button>");

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
        private void AddGridColumns(LibGridGroup grid)
        {
            StringBuilder table = new StringBuilder();
            StringBuilder hidecolumns = new StringBuilder();
            StringBuilder tbformfield = new StringBuilder();
            StringBuilder childformfield = new StringBuilder();
            //LibField libField = null;
            //StringBuilder validatorAttr = null;
            int colcout = 0;
            //List<string> valus = null;
            if (grid.GdGroupFields == null || (grid.GdGroupFields != null && grid.GdGroupFields.Count == 0)) return;
            string param = string.Format("tb{0}", _tableScriptlst.Count + 1);
            table.Append(string.Format("var {0} = new LibTable(\"{1}\");", param, grid.GridGroupName));
            table.Append(string.Format("{0}.$table.url =\"/{1}/BindTableData?gridid={2}&deftb={3}&tableNm={4}\";", param, string.IsNullOrEmpty(grid.ControlClassNm) ? this.Package : grid.ControlClassNm, grid.GridGroupName, grid.GdGroupFields[0].FromDefTableNm, grid.GdGroupFields[0].FromTableNm));
            table.Append(string.Format("{0}.$table.toolbar =\"#{1}_toolbar\";", param, grid.GridGroupName));
            #region 是否显示父子表
            if (!string.IsNullOrEmpty(grid.ChildGridNm)) // 显示父子表
            {
                table.Append(string.Format("{0}.$table.detailView =true;", param));
                //找出子表格组
                LibGridGroup childgrid= this.LibFormPage.GridGroups.FindFirst("GridGroupName", grid.ChildGridNm);
                string childgridmodalid = string.Format("GridGroup_{0}", childgrid.GridGroupName);
                Childrengrids.Add(childgrid);
                _gridGroupdic.Add(childgridmodalid, true);
                if (childgrid != null && childgrid .GdGroupFields!=null && childgrid .GdGroupFields.Count >0)
                {
                    table.Append(string.Format("{0}.SubTable =new LibTable(\"{1}\");", param, childgrid .GridGroupName));
                    table.Append(string.Format("{0}.SubTable.$table.detailView =false;", param));
                    table.Append(string.Format("{0}.SubTable.$table.hasoperation =false;", param));
                    //table.Append(string.Format("{0}.SubTable.$table.url =\"/{1}/BindTableData?gridid={2}&deftb={3}&tableNm={4}\";", param, string.IsNullOrEmpty(childgrid.ControlClassNm) ? this.Package : childgrid.ControlClassNm, childgrid.GridGroupName, childgrid.GdGroupFields[0].FromDefTableNm, childgrid.GdGroupFields[0].FromTableNm));
                    table.Append(string.Format("{0}.SubTable.$subtableParam.gridid ='{1}';", param, childgrid.GridGroupName));
                    table.Append(string.Format("{0}.SubTable.$subtableParam.deftbnm ='{1}';", param, childgrid.GdGroupFields[0].FromDefTableNm));
                    table.Append(string.Format("{0}.SubTable.$subtableParam.tablenm ='{1}';", param, childgrid.GdGroupFields[0].FromTableNm));
                    table.Append(string.Format("{0}.SubTable.$subtableParam.controlnm ='{1}';", param, string.IsNullOrEmpty(childgrid.ControlClassNm) ? this.Package : childgrid.ControlClassNm));
                    if (grid.HasSummary)
                    {
                        table.Append(string.Format("{0}.SubTable.$table.showFooter={1};", param, childgrid.HasSummary ? "true" : "false"));
                    }
                    table.Append(string.Format("{0}.SubTable.$table.columns = [", param));
                    table.Append("{checkbox: true,visible: true }");
                    #region sdp_rowid 列
                    table.Append(",{field:'sdp_rowid',title: 'sdp_rowid',align: 'center',visible: false}");
                    //hidecolumns.Append(string.Format("$('#{0}').bootstrapTable('hideColumn', 'sdp_rowid');", grid.GridGroupName));
                    #endregion
                    foreach (LibGridGroupField field in childgrid.GdGroupFields)
                    {
                        table.Append(",{");
                        string fielddisplaynm = AppCom.GetFieldDesc((int)Language, this.DSID, field.FromTableNm, field.Name);
                        //table.Append(string.Format("field:'{0}',title: '{1}',align: 'center',sortable:{2},", field.Name, field.DisplayName, field.HasSort ? "true" : "false"));
                        table.Append(string.Format("field:'{0}',title: '{1}',align: 'center',sortable:{2},visible:{3},", field.Name, fielddisplaynm, field.HasSort ? "true" : "false", field.Hidden ? "false" : "true"));
                        table.Append("formatter: function (value, row, index) {");
                        if (field.ElemType == ElementType.Date)
                        {
                            table.Append(string.Format("return \"<div {0}>\" + TimeConverToStr(value) + \"</div>\";", field.ReadOnly ? "readonly" : ""));
                        }
                        else
                            table.Append(string.Format("return \"<div {0}>\" + value + \"</div>\";", field.ReadOnly ? "readonly" : ""));
                        table.Append("}");
                        if (childgrid.HasSummary)
                        {
                            //设置汇总行，
                            table.Append(",footerFormatter: function() {return '汇总'}");
                            childgrid.HasSummary = false;
                        }
                        table.Append("}");

                        #region 模态框 的控件
                        if (field.Hidden) continue;
                        if (colcout % 9 == 0)
                        {
                            if (colcout != 0)
                            {
                                childformfield.Append("</div>");
                            }
                            childformfield.Append("<div class=\"form-group\">");
                        }
                        CreatModalFields(childformfield, field, fielddisplaynm);

                        colcout += field.Width + 1;
                        #endregion
                    }
                    childformfield.Append("</div>");// 结束 form-group
                    table.Append("];");
                }
                _tbmodalFormfields.Add(childgridmodalid, childformfield.ToString());

                #region tablemodal脚本
                string childmdparam = string.Format("childtbmodal{0}", Childrengrids.Count + 1);
                table.Append(string.Format("var {0}=new libTableModal(\"{1}\");", childmdparam, string.Format("sdp_tbmdl_{0}", childgridmodalid)));
                table.Append(string.Format("{0}.initialModal();", childmdparam));
                table.Append("$('#sdp_tbmodalbtn" + childgridmodalid + "').click(function () {" + childmdparam + ".Confirm();});");
                #endregion
            }
            colcout = 0;//重置colcout值
            #endregion
            if (grid.HasSummary)
            {
                table.Append(string.Format("{0}.$table.showFooter={1};", param, grid.HasSummary ? "true" : "false"));
            }
            table.Append(string.Format("{0}.$table.columns = [", param));
            table.Append("{checkbox: true,visible: true }");
            #region sdp_rowid 列
            table.Append(",{field:'sdp_rowid',title: 'sdp_rowid',align: 'center',visible: true}");
            hidecolumns.Append(string.Format("$('#{0}').bootstrapTable('hideColumn', 'sdp_rowid');", grid.GridGroupName));
            #endregion
            //if (grid.GdGroupFields != null)
            //{
            //bool flag = false;//用于标识是否已设置了 汇总行。
            foreach (LibGridGroupField field in grid.GdGroupFields)
            {
                //if (!this.Formfields.TryGetValue(field.FromTableNm, out valus))
                //{
                //    valus = new List<string>();
                //    this.Formfields.Add(field.FromTableNm, valus);
                //}
                //valus.Add(field.Name);

                table.Append(",{");
                string fielddisplaynm = AppCom.GetFieldDesc((int)Language, this.DSID, field.FromTableNm, field.Name);
                //table.Append(string.Format("field:'{0}',title: '{1}',align: 'center',sortable:{2},", field.Name, field.DisplayName, field.HasSort ? "true" : "false"));
                table.Append(string.Format("field:'{0}',title: '{1}',align: 'center',sortable:{2},", field.Name, fielddisplaynm, field.HasSort ? "true" : "false"));
                table.Append("formatter: function (value, row, index) {");
                if (field.ElemType == ElementType.Date) {
                    table.Append(string.Format("return \"<div {0}>\" + TimeConverToStr(value) + \"</div>\";", field.ReadOnly ? "readonly" : ""));
                }
                else
                    table.Append(string.Format("return \"<div {0}>\" + value + \"</div>\";", field.ReadOnly ? "readonly" : ""));
                table.Append("}");
                if (grid.HasSummary)
                {
                    //设置汇总行，
                    table.Append(",footerFormatter: function() {return '汇总'}");
                    grid.HasSummary = false;
                }
                table.Append("}");
                if (field.Hidden)
                {
                    hidecolumns.Append(string.Format("$('#{0}').bootstrapTable('hideColumn', '{1}');", grid.GridGroupName, field.Name));
                }

                #region 模态框 的控件
                if (field.Hidden) continue;
                if (colcout % 9 == 0)
                {
                    if (colcout != 0)
                    {
                        tbformfield.Append("</div>");
                    }
                    tbformfield.Append("<div class=\"form-group\">");
                }
                CreatModalFields(tbformfield, field, fielddisplaynm);
                //string id = string.Format("{0}_{1}", field.FromTableNm, field.Name);
                //string name = string.Format("{0}.{1}", field.FromTableNm, field.Name);

                //#region 字段属性验证设置
                //validatorAttr = new StringBuilder();
                ////validatorAttr.Append(field.IsAllowNull ? " required=\"required\"" : "");
                ////validatorAttr.AppendFormat(" maxlength=\"{0}\"", field.FieldLength);
                //validatorAttr.AppendFormat(" {0} ", field.ReadOnly  ? "readonly" : "");
                //#endregion

                //tbformfield.Append("<label for=\"" + field.Name + "\" class=\"col-sm-1 control-label\">" + fielddisplaynm + "</label>");
                //tbformfield.Append("<div class=\"col-sm-" + field.Width + "\">");
                //switch (field.ElemType)
                //{
                //    case ElementType.Date:
                //        _dateElemlst.Add(id);
                //        tbformfield.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + fielddisplaynm + "\" " + validatorAttr.ToString() + ">");
                //        break;
                //    case ElementType.DateTime:
                //        tbformfield.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + fielddisplaynm + "\" " + validatorAttr.ToString() + ">");
                //        break;
                //    case ElementType.Select:
                //        break;
                //    case ElementType.Text:
                //        tbformfield.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + fielddisplaynm + "\" " + validatorAttr.ToString() + ">");
                //        break;
                //    case ElementType.Search:
                //        libField = GetField(field.FromDefTableNm, field.FromTableNm, field.Name);
                //        if (libField.SourceField == null || libField.SourceField.Count == 0)
                //        {
                //        }
                //        tbformfield.Append("<div class=\"input-group\">");
                //        tbformfield.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + fielddisplaynm + "\" " + validatorAttr.ToString() + ">");
                //        tbformfield.Append("<span class=\"input-group-btn\">");
                //        //_page.Append("<button class=\"btn btn-default\" type=\"button\" data-toggle=\"modal\" data-target=\"#searchModal\" data-modalnm=\"" + displaynm + "\" data-fromdsid=\"\" data-deftb=\"\" data-tbstruct=\"" + field.FromTableNm + "\" data-fieldnm=\"" + field.Name + "\"  data-controlnm=\"" + (string.IsNullOrEmpty(this.ControlClassNm) ? this.Package : this.ControlClassNm) + "\"   data-flag=\"2\">");
                //        tbformfield.Append("<button class=\"btn btn-default\" type=\"button\" data-toggle=\"modal\" data-target=\"#searchModal\" data-modalnm=\"" + fielddisplaynm + "\" data-fromdsid=\"\" data-deftb=\"\" data-tbstruct=\""+field.FromTableNm+"\" data-fieldnm=\""+field.Name+"\"  data-controlnm=\"" + (string.IsNullOrEmpty(this.ControlClassNm) ? this.Package : this.ControlClassNm) + "\"   data-flag=\""+((libField.SourceField == null || libField.SourceField.Count == 0)?3:2)+"\">");
                //        tbformfield.Append("<i class=\"glyphicon glyphicon-search\"></i>");
                //        tbformfield.Append("</button>");
                //        tbformfield.Append("</span>");
                //        tbformfield.Append("</div>");
                //        this._hasSearchModal = true;
                //        break;
                //}
                ////_page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + id + "\" placeholder=\""+field.DisplayName+"\">");
                //tbformfield.Append("</div>");//结束 col-sm
                colcout += field.Width + 1;
                #endregion
            }
            tbformfield.Append("</div>");// 结束 form-group
            //}
            table.Append("];");
            table.Append(string.Format("{0}.initialTable();", param));
            table.Append(hidecolumns);

            #region toobar 按钮事件 旧代码
            //table.Append("$('#"+grid .GridGroupName+"_sdp_addrow').click(function () {"+param+".AddRow();});");
            //table.Append("$('#" + grid.GridGroupName + "_sdp_editrow').click(function () {" + param + ".EditRow();});");
            //table.Append("$('#" + grid.GridGroupName + "_sdp_deletrow').click(function () {" + param + ".DeleteRow();});");
            #endregion

            #region tablemodal脚本
            string mdparam = string.Format("tbmodal{0}", _tableScriptlst.Count + 1);
            string gridid = string.Format("GridGroup_{0}", grid.GridGroupName);
            table.Append(string.Format("var {0}=new libTableModal(\"{1}\");", mdparam, string.Format("sdp_tbmdl_{0}", gridid)));
            table.Append(string.Format("{0}.initialModal();", mdparam));
            table.Append("$('#sdp_tbmodalbtn" + gridid + "').click(function () {" + mdparam + ".Confirm();});");
            #endregion
            _tableScriptlst.Add(table.ToString());
            _tbmodalFormfields.Add(string.Format("GridGroup_{0}", grid.GridGroupName), tbformfield.ToString());

        }

        /// <summary>
        /// 结束视图页
        /// </summary>
        public void EndPage()
        {
            //KeyValuePair<string, bool> item = _panelgroupdic.FirstOrDefault(i => i.Value == false);
            //if (item.Key !=null)
            //{
            //    _page.Append("</div>");
            //    _page.Append("</div>");
            //    _page.Append("</div>");
            //    _page.Append("</div>");
            //}
            EndprePanel();
            #region 页面提交，暂存等尾部按钮
            _page.Append("<div class=\"form-group\" style=\"margin-bottom:0\">");
            _page.Append("<div class=\"col-sm-offset-2 col-sm-10 text-right\">");
            _page.Append("<button type=\"submit\" class=\"btn btn-default\">提交</button>");
            _page.Append("<button type=\"submit\" class=\"btn btn-primary\">暂存</button>");
            _page.Append("<button type=\"reset\" class=\"btn btn-default\">重置</button>");
            _page.Append("</div>");
            _page.Append("</div>");
            #endregion
            //_page.Append("</form>");//
            _page.Append("}");//form  结束

            #region 添加搜索控件的模态框。
            if (this._hasSearchModal)
            {
                //用于搜索控件的模态框
                _page.Append(" <div class=\"modal fade\" id=\"searchModal\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"searchModalLabel\">");
                _page.Append("<div class=\"modal-dialog\" role=\"document\">");
                _page.Append("<div class=\"modal-content\">");
                _page.Append("<div class=\"modal-header\" style=\"background-color:#dff0d8\">");
                _page.Append("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">");
                _page.Append("<span aria-hidden=\"true\">×</span>");
                _page.Append("</button>");
                _page.Append("<h4 class=\"modal-title\" id=\"searchModalLabel\">来源主数据</h4>");
                _page.Append("</div>");
                _page.Append("<div class=\"modal-body\">");
                _page.Append("<form id=\"sdp_smodalform\">");

                _page.Append("<div id=\"sdp_smodalCondition\"  class=\"row\">");
                #region 旧代码
                //_page.Append("<label class=\"form-inline\"> 字段:");

                //_page.Append("<select class=\"form-control\" name=\"sdp_smodalfield1\">");
                //_page.Append("<option value=\"1\">默认选择</option>");
                //_page.Append("</select>");

                //_page.Append("<select class=\"form-control\" name=\"sdp_smodalsymbol1\">");
                //_page.Append("<option value=\"1\">等于</option>");
                //_page.Append("<option value=\"2\">大于</option>");
                //_page.Append("<option value=\"3\">小于</option>");
                //_page.Append("<option value=\"4\">包含</option>");
                //_page.Append("<option value=\"5\">[a,b]之间</option>");
                //_page.Append("</select>");

                //_page.Append("<input type=\"text\" class=\"form-control\" name=\"sdp_smodalval1_1\"/>");
                //_page.Append("<input type=\"text\" class=\"form-control\" name=\"sdp_smodalval1_2\"/>");

                //_page.Append("<select class=\"form-control\" name=\"sdp_smodallogic1\">");
                //_page.Append("<option value=\"1\">and</option>");
                //_page.Append("<option value=\"2\">or</option>");
                //_page.Append("</select>");

                //_page.Append("<button id=\"sdp_smodalcondadd\" class=\"btn btn-default\" type=\"button\">");
                //_page.Append(" <i class=\"glyphicon glyphicon-plus\"></i>");
                //_page.Append("</button>");

                //_page.Append("</label>");
                #endregion
                _page.Append(new ElementCollection().SearchModalCondition(1));
                _page.Append("</div>");
                _page.Append("<button id=\"sdp_smodalbtnSearch\" type=\"button\" class=\"btn btn-primary\">"+AppCom.GetFieldDesc((int)Language ,string.Empty ,string.Empty , "sdp_btnselect") +"</button>");
                _page.Append("<table id=\"sdp_smodaldata\"></table>");

                _page.Append("</form>");
                _page.Append("</div>");
                _page.Append("<div class=\"modal-footer\">");
                _page.Append("<button type=\"button\" class=\"btn btn-primary\">"+ AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, "sdp_btnConfirm") + "</button>");
                _page.Append("<button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">" + AppCom.GetFieldDesc((int)Language, string.Empty, string.Empty, "sdp_btnClose") + "</button>");
                _page.Append("</div>");
                _page.Append("</div>");
                _page.Append("</div>");
                _page.Append("</div>");
            }

            #endregion

            #region 添加表格模态框
            if (this._gridGroupdic.Count > 0)
            {
                foreach (KeyValuePair<string, bool> keyval in this._gridGroupdic)
                {
                    //用于表格新增，编辑等操作的模态框
                    _page.Append(" <div class=\"modal fade\" id=\"sdp_tbmdl_" + keyval.Key + "\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"sdp_tbmdlbody_" + keyval.Key + "\">");
                    _page.Append("<div class=\"modal-dialog\" role=\"document\">");
                    _page.Append("<div class=\"modal-content\">");
                    _page.Append("<div class=\"modal-header\" style=\"background-color:#dff0d8\">");
                    _page.Append("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">");
                    _page.Append("<span aria-hidden=\"true\">×</span>");
                    _page.Append("</button>");
                    _page.Append("<h4 class=\"modal-title\" id=\"sdp_tbmdlbody_" + keyval.Key + "\">来源主数据</h4>");
                    _page.Append("</div>");
                    _page.Append("<div class=\"modal-body\">");
                    _page.Append("<form id=\"sdp_" + keyval.Key + "_form\" class=\"form-horizontal\">");
                    _page.Append(_tbmodalFormfields[keyval.Key]);
                    _page.Append("</form>");
                    _page.Append("</div>");
                    _page.Append("<div class=\"modal-footer\">");
                    _page.Append("<button id=\"sdp_tbmodalbtn" + keyval.Key + "\" type=\"button\" class=\"btn btn-primary\">确定</button>");
                    _page.Append("<button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">关闭</button>");
                    _page.Append("</div>");
                    _page.Append("</div>");
                    _page.Append("</div>");
                    _page.Append("</div>");
                }
            }
            #endregion
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
            //_script.Append("$.validator.setDefaults({ submitHandler: function() {alert(\"提交事件!\")} });");
            _script.Append("$().ready(function() { $('#sdp_form').validate();});");
            #endregion
            _script.Append("$(function (){");

            //_script.Append("$('form').validate();");
            #region 禁用页面的enter建
            _script.Append("$(window).keydown(function (e) {");
            _script.Append("var key = window.event ? e.keyCode : e.which;");
            _script.Append("if (key.toString()== \"13\") {");
            _script.Append(" return false;}});");
            #endregion

            _script.Append("$('#bwysdp_progid').val(\"" + this._progid + "\");");
            _script.Append("$('#bwysdp_dsid').val(\"" + this.DSID + "\");");

            #region pageload
            _script.Append("$.ajax({url: \" /" + (string.IsNullOrEmpty(this.ControlClassNm) ? this.Package : this.ControlClassNm) + "/BasePageLoad\",data: \"\",type: 'Post',async: false,success: function (obj) {},");
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

            #region 页面按钮组 事件绑定
            string tbs = "[";
            for (int i = 1; i <= _tableScriptlst.Count; i++)
            {
                if (i == 1)
                { tbs += string.Format("tb{0}", i); continue; }
                tbs += string.Format(",tb{0}", i);
            }
            tbs += "]";
            _script.Append("$('#bwysdp_btnsave').click(function (){" +
                "var objs=" + tbs + "; " +
                "var datastr='['; " +
                "$.each(objs,function(index,o){if(datastr.length==1){ datastr+=Serialobj(o);}else{datastr+=\",\"+Serialobj(o);}});" +
                "datastr+=']';" +
                "Save(datastr,\"" + (string.IsNullOrEmpty(this.ControlClassNm) ? "DataBase" : this.ControlClassNm) + "\");" +
                "});");
            #endregion

            #region 搜索控件 绑定input propertychange 事件
            foreach (string id in this._searchModelIds)
            {
                _script.Append("$('#"+id+"').bind(\"input propertychange\", function () {");
                _script.Append("if (this.value =='') {$('#" + id + "_desc').text(''); }");
                _script.Append("Showfuzzydiv('"+id+"');");
                _script.Append("onpropertychange('"+id+"','"+ (string.IsNullOrEmpty(this.ControlClassNm) ? "DataBase" : this.ControlClassNm) + "')");
                _script.Append("});");
            }
            #endregion

            //#region msgforsave 函数
            //_script.Append("GetMsgForSave();");
            //#endregion

            _script.Append("})");

            #region 日期控件
            foreach (string id in _dateElemlst)//只有日期，没有时间
            {
                _script.AppendLine();
                _script.Append("laydate.render({");
                _script.Append("elem: '#" + id + "'");
                _script.Append(",format: 'yyyy-MM-dd'");
                //_script.Append(",value: new Date().toLocaleDateString().replace(new RegExp(\"/\", \"g\"),'-')");
                _script.Append("});");
            }

            foreach (string id in _datetimeElemlst) //日期和时间
            {
                _script.AppendLine();
                _script.Append("laydate.render({");
                _script.Append("elem: '#" + id + "'");
                _script.Append(",type: 'datetime'");
                _script.Append(",format: 'yyyy-MM-dd HH:mm:ss'");
                //_script.Append(",value: new Date().toLocaleDateString().replace(new RegExp(\"/\", \"g\"),'-')");
                _script.Append("});");
            }
            #endregion
            _script.Append("</script>");
        }

        /// <summary>
        /// 给上一个面板或表格组添加结束标志
        /// </summary>
        private void EndprePanel()
        {
            KeyValuePair<string, bool> panelitem = _panelgroupdic.FirstOrDefault(i => !i.Value);
            if (panelitem.Key != null)
            {
                _page.Append("</div>");
                _page.Append("</div>");
                _page.Append("</div>");
                _page.Append("</div>");
                _panelgroupdic[panelitem.Key] = true;
            }
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

        private LibField GetField(string deftable, string table, string fieldNm)
        {
            var deftb = this.LibDataSource.DefTables.FindFirst("TableName", deftable);
            var tbstruct = deftb.TableStruct.FindFirst("Name", table);
            var field = tbstruct.Fields.FindFirst("Name", fieldNm);
            return field;
        }

        private string GetMastTable()
        {
            foreach (LibDefineTable deftb in this.LibDataSource.DefTables)
            {
                foreach (LibDataTableStruct dt in deftb.TableStruct)
                {
                    if (dt.TableIndex == 0)
                        return dt.Name;
                }
            }
            return string.Empty;
        }

        /// <summary>创建表格模态框的控件</summary>
        /// <param name="fieldsbuilder"></param>
        /// <param name="field"></param>
        /// <param name="fielddisplaynm"></param>
        private void CreatModalFields(StringBuilder fieldsbuilder, LibGridGroupField field,string fielddisplaynm)
        {
            #region 模态框 的控件
            string id = string.Format("{0}_{1}", field.FromTableNm, field.Name);
            string name = string.Format("{0}.{1}", field.FromTableNm, field.Name);

            #region 字段属性验证设置
            StringBuilder validatorAttr = new StringBuilder();
            validatorAttr.AppendFormat(" {0} ", field.ReadOnly ? "readonly" : "");
            #endregion

            fieldsbuilder.Append("<label for=\"" + field.Name + "\" class=\"col-sm-1 control-label\">" + fielddisplaynm + "</label>");
            fieldsbuilder.Append("<div class=\"col-sm-" + field.Width + "\">");
            switch (field.ElemType)
            {
                case ElementType.Date:
                    _dateElemlst.Add(id);
                    fieldsbuilder.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + fielddisplaynm + "\" " + validatorAttr.ToString() + ">");
                    break;
                case ElementType.DateTime:
                    fieldsbuilder.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + fielddisplaynm + "\" " + validatorAttr.ToString() + ">");
                    break;
                case ElementType.Select:
                    break;
                case ElementType.Text:
                    fieldsbuilder.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + fielddisplaynm + "\" " + validatorAttr.ToString() + ">");
                    break;
                case ElementType.Search:
                    LibField libField = GetField(field.FromDefTableNm, field.FromTableNm, field.Name);
                    if (libField.SourceField == null || libField.SourceField.Count == 0)
                    {
                    }
                    fieldsbuilder.Append("<div class=\"input-group\">");
                    fieldsbuilder.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + fielddisplaynm + "\" " + validatorAttr.ToString() + ">");
                    fieldsbuilder.Append("<span class=\"input-group-btn\">");
                    //_page.Append("<button class=\"btn btn-default\" type=\"button\" data-toggle=\"modal\" data-target=\"#searchModal\" data-modalnm=\"" + displaynm + "\" data-fromdsid=\"\" data-deftb=\"\" data-tbstruct=\"" + field.FromTableNm + "\" data-fieldnm=\"" + field.Name + "\"  data-controlnm=\"" + (string.IsNullOrEmpty(this.ControlClassNm) ? this.Package : this.ControlClassNm) + "\"   data-flag=\"2\">");
                    fieldsbuilder.Append("<button class=\"btn btn-default\" type=\"button\" data-toggle=\"modal\" data-target=\"#searchModal\" data-modalnm=\"" + fielddisplaynm + "\" data-fromdsid=\"\" data-deftb=\"\" data-tbstruct=\"" + field.FromTableNm + "\" data-fieldnm=\"" + field.Name + "\"  data-controlnm=\"" + (string.IsNullOrEmpty(this.ControlClassNm) ? this.Package : this.ControlClassNm) + "\"   data-flag=\"" + ((libField.SourceField == null || libField.SourceField.Count == 0) ? 3 : 2) + "\">");
                    fieldsbuilder.Append("<i class=\"glyphicon glyphicon-search\"></i>");
                    fieldsbuilder.Append("</button>");
                    fieldsbuilder.Append("</span>");
                    fieldsbuilder.Append("</div>");
                    this._hasSearchModal = true;
                    break;
            }
            fieldsbuilder.Append("</div>");//结束 col-sm
            #endregion
        }
        #endregion
    }

    public class ElementCollection
    {
        public Language Language { get { return (System.Web.HttpContext.Current.Session[SysConstManage.sdp_userinfo] as UserInfo).Language; } }
        /// <summary>
        /// 搜索模态框的条件元素
        /// </summary>
        /// <param name="condindex"></param>
        /// <returns></returns>
        public string SearchModalCondition(int condindex)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<label class=\"form-inline\"> "+AppCom.GetFieldDesc ((int)Language, string.Empty ,string.Empty , "sdp_labelField") +":");

            str.AppendFormat("<select class=\"form-control\" name=\"{0}{1}\">", SysConstManage.sdp_smodalfield,condindex);
            //str.Append("<option value=\"0\">默认选择</option>");
            str.Append("</select>");

            str.AppendFormat("<select class=\"form-control\" name=\"{0}{1}\">",SysConstManage .sdp_smodalsymbol, condindex);
            foreach (var item in Enum.GetValues(typeof(SmodalSymbol)))
            {
                str.AppendFormat("<option value=\"{0}\">{1}</option>", (int)item, ReSourceManage.GetResource(item));
            }
            str.Append("</select>");

            str.AppendFormat("<input type=\"text\" class=\"form-control\" name=\"{0}{1}_1\"/>",SysConstManage .sdp_smodalval,condindex);
            str.AppendFormat("<input type=\"text\" class=\"form-control\" name=\"{0}{1}_2\"/>", SysConstManage.sdp_smodalval, condindex);

            str.AppendFormat("<select class=\"form-control\" name=\"{0}{1}\">",SysConstManage .sdp_smodallogic,condindex);
            foreach (var item in Enum.GetValues(typeof(Smodallogic)))
            {
                str.AppendFormat("<option value=\"{0}\">{1}</option>",(int)item , ReSourceManage.GetResource(item));
            }
            //str.Append("<option value=\"1\">and</option>");
            //str.Append("<option value=\"2\">or</option>");
            str.Append("</select>");

            str.AppendFormat("<button id=\"sdp_smodalcondadd{0}\" class=\"btn btn-default\" type=\"button\">", condindex);
            str.Append(" <i class=\"glyphicon glyphicon-plus\"></i>");
            str.Append("</button>");

            if (condindex != 1)
            {
                str.AppendFormat("<button id=\"sdp_smodalconddelet{0}\" class=\"btn btn-default\" type=\"button\">", condindex);
                str.Append(" <i class=\"glyphicon glyphicon-minus\"></i>");
                str.Append("</button>");
            }
            str.Append("</label>");
            return str.ToString();
        }
    }
}