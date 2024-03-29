﻿using System.Web;
using System.Web.Optimization;

namespace BWYSDPWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidate").Include(
                        //"~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/Message_zh.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js",
                      //"~/Scripts/jquery-2.1.3.min.js",
                      "~/Scripts/lib/jquery.mousewheel.min.js",
                      "~/Scripts/lib/jquery.cookie.min.js",
                      "~/Scripts/lib/fastclick.min.js",
                      //"~/Scripts/lib/bootstrap.min.js",
                      "~/Scripts/lib/clearmin.min.js",
                      "~/Scripts/lib/home.js",
                      "~/Scripts/lib/jquery-confirm.min.js",
                      "~/Scripts/lib/ajaxBus.js",
                      "~/Scripts/lib/sdp_com.js"
                      //"~/Scripts/lib/bootstrap-table.js",
                      //"~/Scripts/lib/bootstrap-table-zh-CN.js",
                      //"~/Scripts/lib/colResizable-1.6.js",
                      //"~/Scripts/lib/TableSetting.js",
                      //"~/Scripts/lib/laydate/laydate.js"
                      //"~/Scripts/lib/bootstrap-datetimepicker.js",
                      //"~/Scripts/lib/bootstrap-datetimepicker.zh-CN.js"
                      ));
            #region js控件
            //bootstrapTable
            bundles.Add(new ScriptBundle("~/bundles/bootstrapTable").Include(
                      "~/Scripts/lib/bootstrap-table.js",
                      "~/Scripts/lib/bootstrap-table-zh-CN.js",
                      //"~/Scripts/lib/bootstrap-table-resizable.js",
                      //"~/Scripts/lib/colResizable-1.6.min.js",
                      "~/Scripts/lib/TableSetting.js"
                      
                ));
            //bootstrapTable
            bundles.Add(new ScriptBundle("~/bundles/RptbootstrapTable").Include(
                      "~/Scripts/lib/bootstrap-table.min.js",
                      "~/Scripts/lib/bootstrap-table-zh-CN.js",
                      "~/Scripts/lib/TableSetting.js"

                ));
            //bootstrapTableExport
            bundles.Add(new ScriptBundle("~/bundles/bootstrapTableExport").Include(
                      "~/Scripts/lib/bootstrap-table-export.js",
                      "~/Scripts/lib/tableExport.min.js",
                      "~/Scripts/lib/xlsx.core.min.js"
                     //"~/Scripts/lib/jspdf.plugin.autotable.js",
                     //"~/Scripts/lib/jquery.base64.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/jquerybase64").Include(
                     "~/Scripts/lib/jquery.base64.js"
                ));
            //fixed-columns
            bundles.Add(new ScriptBundle("~/bundles/fixedcolumns").Include(
                     "~/Scripts/lib/bootstrap-table-fixed-columns.min.js"
                ));
            //laydate
            bundles.Add(new ScriptBundle("~/Scripts/lib/laydate/laydate").Include(
                    "~/Scripts/lib/laydate/laydate.js"
                ));
            //searchmodal
            bundles.Add(new ScriptBundle("~/bundles/searchmodal").Include(
                     "~/Scripts/lib/SearchModal.js"

               ));
            //TableModal.js
            bundles.Add(new ScriptBundle("~/bundles/TableModal").Include(
                     "~/Scripts/lib/TableModal.js"

               ));
            #endregion

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      //"~/Content/bootstrap4.3.min.css",
                      "~/Content/bootstrap-clearmin.min.css",
                      "~/Content/roboto.css",
                      "~/Content/material-design.css",
                      "~/Content/small-n-flat.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/jquery-confirm.min.css"
                      //"~/Content/bootstrap-table.css"
                      //"~/Content/bootstrap-datetimepicker.min.css"
                      ));
            #region 控件的css 
            //bootstrapTable
            bundles.Add(new StyleBundle("~/Content/bootstrapTable").Include(
                     "~/Content/bootstrap-table.min.css"
                     
                ));

            //bootstrapTable
            bundles.Add(new StyleBundle("~/Content/fixedcolumns").Include(
                //"~/Content/all.min.css",
                //"~/Content/default.css",
                //"~/Content/hint.css",
                //"~/Content/docs.min.css",
                "~/Content/_float.css",
                "~/Content/bootstrap-table-fixed-columns.min.css"

                ));
            #endregion
        }
    }
}
