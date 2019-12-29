using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class SearchConditionField
    {
        public string DSID { get; set; }
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

        /// <summary>
        /// 是否作为查询条件
        /// </summary>
        public bool IsCondition { get; set; }

        public bool Hidden { get; set; }
        /// <summary>
        /// 是否日期类型
        /// </summary>
        public bool IsDateType { get; set; }
        /// <summary>
        /// 是否二进制类型
        /// </summary>
        public bool isBinary { get; set; }
    }

}