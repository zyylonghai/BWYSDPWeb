using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BaseController
{
    public class DataBaseController : BaseController
    {
        // GET: DataBase
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public DataBaseController()
        {

        }
        /// <summary>
        /// 菜单跳转功能页
        /// </summary>
        /// <param name="progId"></param>
        /// <returns></returns>
        public ActionResult ConverToPage(string progId)
        {
            return View(progId);
        }
        /// <summary>
        /// 功能搜索
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public ActionResult SearchFunc(string q)
        {
            return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }
    }
}