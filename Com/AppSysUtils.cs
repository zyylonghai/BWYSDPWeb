using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com
{
    public class AppSysUtils
    {
        public static int CalculateTotal(DataTable dt)
        {
            int total = 0;
            foreach (DataRow  row in dt.Rows)
            {
                if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached) continue;
                total++;
            }
            return total;
        }
    }
}
