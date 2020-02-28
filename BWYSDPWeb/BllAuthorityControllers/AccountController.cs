using BWYSDPWeb.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BWYSDPWeb.BaseController;

namespace BWYSDPWeb.BllAuthorityControllers
{
    public class AccountController : AuthorityController
    {
        // GET: Account
        //public ActionResult Index()
        //{
        //    return View();
        //}

        protected override void BeforeSave()
        {
            base.BeforeSave();
            //string pwd = this.LibTables[0].Tables[0].DataTable .Rows[0]["Password"].ToString();
            //string confirmpwd = this.LibTables[0].Tables[0].DataTable.Rows[0]["Confirmpwd"].ToString();
            var firstrow = this.LibTables[0].Tables[0].FindRow(0);
            string pwd = firstrow.Password;
            string confirmpwd = firstrow.Confirmpwd;
            if (this.OperatAction == OperatAction.Add)
            {
                if (string.Compare(pwd, confirmpwd, false) != 0)
                {
                    //msg000000010 两次输入的密码不一致
                    //this.ThrowErrorException(AppCom.GetMessageDesc("msg000000010"));
                    this.AddMessage(AppCom.GetMessageDesc("msg000000010"));
                }
            }
        }
    }
}