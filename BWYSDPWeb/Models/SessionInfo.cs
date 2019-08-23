using SDPCRL.COM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class SessionInfo
    {
        /// <summary> </summary>
        public BaseController.OperatAction OperateAction { get; set; }

        /// <summary>当前语言</summary>
        public Language Language { get; set; }
        /// <summary>
        /// 功能的搜索条件
        /// </summary>
        public List<LibSearchCondition> Conds { get; set; }

        /// <summary>
        /// 用于存储保存时产生的信息。
        /// </summary>
        public List<LibMessage> MsgforSave { get; set; }
    }
}