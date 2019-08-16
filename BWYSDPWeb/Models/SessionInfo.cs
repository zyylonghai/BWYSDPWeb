using SDPCRL.COM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class SessionInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseController.OperatAction OperateAction { get; set; }

        /// <summary>
        /// 功能的搜索条件
        /// </summary>
        public List<LibSearchCondition> Conds { get; set; }
    }
}