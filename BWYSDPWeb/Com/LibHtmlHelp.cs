using BWYSDPWeb.Com;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}