using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com
{
    public class FormEnum
    {
    }

    public enum FormAction
    {
        Add=1,
        Edit=2,
        Delete=3
    }

    public enum ProgType
    {
        /// <summary>单据功能</summary>
        Form=1,
        /// <summary>报表功能</summary>
        Report=2
    }
}
