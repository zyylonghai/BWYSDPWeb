using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgViewModel
{
    public class ProgBaseViewModel
    {
        public List<AuthorityObj> AuthorityObjs { get; set; }
        public bool IsTrans = false;
        public string TransModelId = string.Empty;

        public ProgBaseViewModel()
        {
            AuthorityObjs = new List<AuthorityObj>();
        }
    }

    public class AuthorityObj
    {
        /// <summary>权限对象类型(1:操作对象 2:数据对象 )</summary>
        public int ObjectType { get; set; }

        /// <summary> 对象ID（包括按钮id，字段id） </summary>
        public string ObjectId { get; set; }
        /// <summary>组ID（包括信息组ID，表格组ID）</summary>
        public string GroupId { get; set; }
    }
}
