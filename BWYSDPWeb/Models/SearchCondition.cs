using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class SearchConditionField
    {
        /// <summary>
        /// 自定义表名
        /// </summary>
        public string DefTableNm { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableNm { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldNm { get; set; }

        public string DisplayNm { get; set; }

        //public int TableIndex { get; set; }
        /// <summary>根据TableIndex取别名</summary>
        public char TBAliasNm { get; set; }

        /// <summary>
        /// 字段别名
        /// </summary>
        public string AliasNm { get; set; }

        public bool Hidden { get; set; }

        public bool IsDateType { get; set; }
    }

}