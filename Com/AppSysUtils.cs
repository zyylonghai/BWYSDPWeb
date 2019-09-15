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

        public static DataTable GetDataByPage(DataTable dt, int page, int rows)
        {
            DataTable resultdt = dt.Clone();
            for (int index = (page - 1) * rows; index < page * rows; index++)
            {
                if (index >= dt.Rows.Count) break;
                if (dt.Rows[index].RowState == DataRowState.Deleted) continue;
                resultdt.ImportRow(dt.Rows[index]);
            }
            return resultdt;
        }
    }
}
