using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class Menu
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 功能ID
        /// </summary>
        public string ProgId { get; set; }

        /// <summary>
        /// 所属包
        /// </summary>
        public string Package { get; set; }
        /// <summary>
        /// 菜单显示名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 父节点菜单ID
        /// </summary>
        public long PmenuId { get; set; }

    }
}