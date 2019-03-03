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
        //private Dictionary<string, bool> _fomGroupdic=null;
        public ViewFactory()
        {
            _page = new StringBuilder();
            _script = new StringBuilder();
            _dateElemlst = new List<string>();
            _panelgroupdic = new Dictionary<string, bool>();
            _gridGroupdic = new Dictionary<string, bool>();
            //_fomGroupdic = new Dictionary<string, bool>();
        }

        public string PageHtml {
            get {
                StringBuilder jsandcss = new StringBuilder();
                if (_dateElemlst.Count > 0) //加载日期控件的js，css
                {
                    jsandcss.Append("@Scripts.Render(\"~/Scripts/lib/laydate/laydate\")");
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
            _page.Append("<form class=\"form-horizontal\" action=\"DoSave\">");
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
                }
                //_page.Append("<input type=\"text\" class=\"form-control\" id=\"" + id + "\" name=\"" + id + "\" placeholder=\""+field.DisplayName+"\">");
                _page.Append("</div>");
                colcout += field.Width + 1;
            }
            _page.Append("</div>");
        }

        /// <summary>
        /// 创建表格组
        /// </summary>
        /// <param name="title"></param>
        public void CreateGridGroup(string tableNm, string title)
        {
            EndprePanel();
            string id = string.Format("GridGroup{0}", _gridGroupdic.Count + 1);
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

            _page.Append("<table id=\""+tableNm+"\"></table>");

            _gridGroupdic.Add(id, false);
        }
        /// <summary>
        /// 添加表格列
        /// </summary>
        /// <param name="fields"></param>
        public void AddGridColumns(string tableNm, LibCollection<LibGridGroupField> fields)
        {

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
            _page.Append("</form>");//
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