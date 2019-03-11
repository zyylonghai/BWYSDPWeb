﻿using SDPCRL.COM.ModelManager.FormTemplate;
using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BWYSDPWeb.Com
{
    public class ViewFactory
    {
        private StringBuilder _page=null;
        private StringBuilder _script=null;
        private Dictionary<string, bool> _panelgroupdic=null;
        private Dictionary<string, bool> _gridGroupdic = null;
        private List<string> _dateElemlst = null;
        private List<string> _tableScriptlst = null;
        private string _progid = null;
        private bool _hasSearchModal = false;// 是否有搜索控件。
        //private Dictionary<string, bool> _fomGroupdic=null;
        public ViewFactory()
        {
            _page = new StringBuilder();
            _script = new StringBuilder();
            _dateElemlst = new List<string>();
            _panelgroupdic = new Dictionary<string, bool>();
            _gridGroupdic = new Dictionary<string, bool>();
            _tableScriptlst = new List<string>();
            //_fomGroupdic = new Dictionary<string, bool>();
        }
        public ViewFactory(string progid) 
            : this()
        {
            this._progid = progid;
        }

        public string PageHtml {
            get {
                StringBuilder jsandcss = new StringBuilder();
                if (_gridGroupdic.Count > 0)
                {
                    jsandcss.Append("@Styles.Render(\"~/Content/bootstrapTable\")");
                    jsandcss.Append("@Scripts.Render(\"~/bundles/bootstrapTable\")");
                    jsandcss.Append("@Scripts.Render(\"~/bundles/bootstrapTableExport\")");
                }
                if (_dateElemlst.Count > 0) //加载日期控件的js，css
                {
                    jsandcss.Append("@Scripts.Render(\"~/Scripts/lib/laydate/laydate\")");
                }
                if (_hasSearchModal)
                {
                    jsandcss.Append("@Scripts.Render(\"~/bundles/searchmodal\")");
                }
                return jsandcss.Append(_page.ToString()).ToString();
            }
        }
        /// <summary>
        /// 开始创建视图页
        /// </summary>
        /// <param name="pagetitle"></param>
        public void BeginPage(string pagetitle)
        {
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
            _page.Append("<button type=\"button\" class=\"btn btn-default\"><i class=\"fa fa-fw fa-save\"></i></button>");
            _page.Append("<button type=\"button\" class=\"btn btn-default\"><i class=\"fa fa-fw fa-cut\"></i></button>");
            _page.Append("<button type=\"button\" class=\"btn btn-default\"><i class=\"fa fa-fw fa-copy\"></i></button>");
            _page.Append("<button type=\"button\" class=\"btn btn-default\"><i class=\"fa fa-fw fa-clipboard\"></i></button>");
            _page.Append("</div>");
            _page.Append("<br /><br />");
        }
        /// <summary>
        /// 创建表格Form
        /// </summary>
        public void CreateForm()
        {
            _page.Append("<form class=\"form-horizontal\" action=\"Save\">");
        }

        /// <summary>
        ///创建面板(PanelGroup)
        /// </summary>
        public void CreatePanelGroup(string title)
        {
            //KeyValuePair<string, bool> panelitem = _panelgroupdic.FirstOrDefault(i => !i.Value);
            //if (panelitem.Key != null)
            //{
            //    _page.Append("</div>");
            //    _page.Append("</div>");
            //    _page.Append("</div>");
            //    _page.Append("</div>");
            //    _panelgroupdic[panelitem.Key] = true;
            //}
            //KeyValuePair<string, bool> griditem = _gridGroupdic.FirstOrDefault(i => !i.Value);
            //if (griditem.Key != null)
            //{
            //    _page.Append("</div>");
            //    _page.Append("</div>");
            //    _page.Append("</div>");
            //    _page.Append("</div>");
            //    _gridGroupdic[griditem.Key] = true;
            //}
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
            foreach (LibFormGroupField field in fields)
            {
                if (colcout%12 == 0)
                {
                    if (colcout != 0)
                    {
                        _page.Append("</div>");
                    }
                    _page.Append("<div class=\"form-group\">");
                }
                string id = string.Format("{0}_{1}", field.FromTableNm, field.Name);
                string name=string.Format("{0}.{1}", field.FromTableNm, field.Name);
                _page.Append("<label for=\"" + field.Name + "\" class=\"col-sm-1 control-label\">" + field.DisplayName + "</label>");
                _page.Append("<div class=\"col-sm-" + field.Width + "\">");
                switch (field.ElemType)
                {
                    case ElementType.Date:
                        _dateElemlst.Add(id);
                        _page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + field.DisplayName + "\">");
                        break;
                    case ElementType.DateTime:
                        _page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + field.DisplayName + "\">");
                        break;
                    case ElementType.Select:
                        break;
                    case ElementType.Text:
                        _page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + field.DisplayName + "\">");
                        break;
                    case ElementType.Search:
                        _page.Append("<div class=\"input-group\">");
                        _page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + name + "\" placeholder=\"" + field.DisplayName + "\">");
                        _page.Append("<span class=\"input-group-btn\">");
                        _page.Append("<button class=\"btn btn-default\" type=\"button\" data-toggle=\"modal\" data-target=\"#searchModal\" data-whatever=\"" + field.DisplayName + "\">");
                        _page.Append("<i class=\"glyphicon glyphicon-search\"></i>");
                        _page.Append("</button>");
                        _page.Append("</span>");
                        _page.Append("</div>");
                        this._hasSearchModal = true;
                        break;
                }
                //_page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + id + "\" placeholder=\""+field.DisplayName+"\">");
                _page.Append("</div>");//结束 col-sm
                colcout += field.Width + 1;
            }
            _page.Append("</div>");// 结束 form-group
        }

        /// <summary>
        /// 创建表格组
        /// </summary>
        /// <param name="title"></param>
        public void CreateGridGroup(LibGridGroup grid,string package)
        {
            EndprePanel();
            string id = string.Format("GridGroup_{0}", grid.GridGroupName);
            string contentid = string.Format("{0}_info", id);
            _page.Append("<div class=\"panel-group\" id=\"" + id + "\">");
            _page.Append("<div class=\"panel panel-default\">");

            //面板标题
            _page.Append("<div class=\"panel-heading\" style=\"background-color:#dff0d8; text-align:left\">");
            _page.Append("<h4 class=\"panel-title\">");
            _page.Append("<a data-toggle=\"collapse\" data-parent=\"#" + id + "\" href=\"#" + contentid + "\">" + grid.GridGroupDisplayNm + "</a>");
            _page.Append("</h4>");
            _page.Append("</div>");

            //面板内容
            _page.Append("<div id=\"" + contentid + "\" class=\"panel-collapse in \">");
            _page.Append("<div class=\"panel-body\">");

            #region toolbar
            _page.Append("<div id=\""+grid .GridGroupName+"_toolbar\" class=\"btn-group\">");
            _page.Append("<button type=\"button\" class=\"btn btn-default\" onclick=\"tablerefresh()\">");
            _page.Append("<i class=\"glyphicon glyphicon-plus\"></i>");
            _page.Append("</button>");
            _page.Append("<button type=\"button\" class=\"btn btn-default\">");
            _page.Append("<i class=\"glyphicon glyphicon-pencil\"></i>");
            _page.Append("</button>");
            _page.Append("<button type=\"button\" class=\"btn btn-default\">");
            _page.Append("<i class=\"glyphicon glyphicon-trash\"></i>");
            _page.Append("</button>");
            _page.Append("</div>");
            #endregion
            _page.Append("<table id=\""+ grid.GridGroupName + "\"></table>");

            _gridGroupdic.Add(id, false);
            AddGridColumns(grid, package);
        }
        /// <summary>
        /// 添加表格列
        /// </summary>
        /// <param name="fields"></param>
        private void AddGridColumns(LibGridGroup grid,string package)
        {
            StringBuilder table = new StringBuilder();
            string param = string.Format("tb{0}", _tableScriptlst.Count + 1);
            table.Append(string.Format("var {0} = new LibTable(\"{1}\");", param, grid.GridGroupName));
            table.Append(string.Format("{0}.$table.url =\"/{1}/BindTableData?progid ={2}\";", param, string.IsNullOrEmpty(grid.ControlClassNm) ? package: grid.ControlClassNm,this._progid));
            table.Append(string.Format("{0}.$table.toolbar =\"#{1}_toolbar\";",param,grid.GridGroupName));
            if (grid.HasSummary)
            {
                table.Append(string.Format("{0}.$table.showFooter={1};",param,grid .HasSummary ? "true" : "false"));
            }
            table.Append(string.Format("{0}.$table.columns = [", param));
            table.Append("{checkbox: true,visible: true }");
            if (grid.GdGroupFields != null)
            {
                //bool flag = false;//用于标识是否已设置了 汇总行。
                foreach (LibGridGroupField field in grid.GdGroupFields)
                {
                    table.Append(",{");
                    table.Append(string.Format("field:'{0}',title: '{1}',align: 'center',sortable:{2}", field.Name, field.DisplayName, field.HasSort ? "true" : "false"));
                    if (grid .HasSummary)
                    {
                        //设置汇总行，
                        table.Append(",footerFormatter: function() {return '汇总'}");
                        grid.HasSummary = false;
                    }
                    table.Append("}");
                }
            }
            table.Append("];");
            table.Append(string.Format("{0}.initialTable();", param));
            _tableScriptlst.Add(table.ToString());

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
            _page.Append("</form>");//

            #region 添加模态框。
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
                _page.Append("<form>");
                _page.Append("<table id=\"searchdata\"></table>");
                _page.Append("</form>");
                _page.Append("</div>");
                _page.Append("<div class=\"modal-footer\">");
                _page.Append("<button type=\"button\" class=\"btn btn-primary\">确定</button>");
                _page.Append("<button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">关闭</button>");
                _page.Append("</div>");
                _page.Append("</div>");
                _page.Append("</div>");
                _page.Append("</div>");
            }
            
            #endregion
            _page.Append("</div>");//panel - body
            _page.Append("</div>");//panel panel - default
            _page.Append("</div>");//container - fluid

            CreateJavaScript();
            _page.Append(_script.ToString());
            
        }

        #region 私有函数
        private void CreateJavaScript()
        {
            _script.Append("<script type=\"text/javascript\">");
            _script.Append("$(function (){");

            #region 表格脚本
            foreach (string script in _tableScriptlst)
            {
                _script.Append(script);
                _script.AppendLine();
            }
            #endregion

            _script.Append("})");

            #region 日期控件
            foreach (string id in _dateElemlst)
            {
                _script.AppendLine();
                _script.Append("laydate.render({");
                _script.Append("elem: '#"+id+"'");
                _script.Append(",format: 'yyyy-MM-dd'");
                _script.Append(",value: new Date().toLocaleDateString().replace(new RegExp(\"/\", \"g\"),'-')");
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
        #endregion
    }
}