using ProgViewModel;
using SDPCRL.COM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class SessionInfo
    {
        /// <summary>当前功能操作动作（新增Add，编辑Edit，删除Delete,） </summary>
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
        /// <summary>
        /// 用于来源数据搜索时，存储来源信息
        /// </summary>
        public FromFieldInfo FromFieldInfo { get; set; }

        /// <summary>
        /// 功能的ViewModel
        /// </summary>
        public ProgBaseViewModel ProgBaseVM { get; set; }

        public object ExtInfo { get; set; }
    }

    public class FromFieldInfo
    {
        /// <summary>当前字段所在的表明</summary>
        public string tableNm { get; set; }
        /// <summary>
        /// 当前字段名
        /// </summary>
        public string FieldNm { get; set; }

        /// <summary>
        /// 来源字段名
        /// </summary>
        public string FromFieldNm { get; set; }
        /// <summary>
        /// 来源字段描述
        /// </summary>
        public string FromFieldDesc { get; set; }

        /// <summary>
        ///关联带出来的字段集合
        /// </summary>
        public List<string> RelateFields { get; set; }
    }
}