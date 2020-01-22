using SDPCRL.CORE;
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

        /// <summary>
        /// 分页取数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="page">页码</param>
        /// <param name="rows">每页条数</param>
        /// <returns></returns>
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

        public static DataTable GetData(DataTable dt, string whereExpress)
        {
            DataTable resultdt = dt.Clone();
            DataRow[] rows = dt.Select(whereExpress);
            foreach (DataRow row in rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                resultdt.ImportRow(row);
            }
            return resultdt;
        }

        public static DataRow GetRowByRowId(DataTable dt, int rowId)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                if (Convert.ToInt32(row[SysConstManage.sdp_rowid]) == rowId)
                    return row;
            }
            return null;
        }
    }
}
