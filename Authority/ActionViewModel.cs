using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorityViewModel
{
    public class ActionViewModel
    {
        public List<ActionObj> ActObjList { get; set; }
        public List<ActionObj> DataObjList { get; set; }

        public ActionViewModel(List<ActionObj> actionObjs)
        {
            ActObjList = actionObjs.Where(i => i.ObjectType == 1).OrderBy(i=>i.GroupId ).ToList ();
            DataObjList = actionObjs.Where(i => i.ObjectType == 2).OrderBy (i=>i.GroupId ).ToList();
        }
    }

    /// <summary>
    /// 权限对象
    /// </summary>
    public class ActionObj
    {
        /// <summary>权限对象类型(1:操作对象 2:数据对象 )</summary>
        public int ObjectType { get; set; }

        /// <summary> 对象ID（包括按钮id，字段id） </summary>
        public string ObjectId { get; set; }

        public string ObjectNm { get; set; }

        /// <summary>组ID（包括信息组ID，表格组ID）</summary>
        public string GroupId { get; set; }

        public string GroupNm { get; set; }

        /// <summary>是否有权限</summary>
        public bool IsAuthority = true;

    }
}
