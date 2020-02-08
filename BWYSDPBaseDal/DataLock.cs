using SDPCRL.CORE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYSDPBaseDal
{
    /// <summary>
    /// 数据锁
    /// </summary>
    public class DataLock : LibLock
    {
        DataRow _row = null;
        DataColumn[] _primarykey = null;
        #region 公共属性
        public string PrimaryValues { get; set; }
        public string ClientSessionId { get; set; }
        #endregion

        public DataLock(string clientid, string tablenm,DataRow row,DataColumn[] primarykey)
        {
            this.ClientSessionId = clientid;
            this.Key = tablenm;
            this._row = row;
            this._primarykey = primarykey;
            this.PrimaryValues = string.Empty;
        }
        public DataLock(string clientid, DataRow row)
            :this(clientid , row.Table .TableName ,row,row .Table .PrimaryKey)
        {
            if (row == null)
            {
                throw new LibExceptionBase("参数row不能为空");
            }
            if (string.IsNullOrEmpty(row.Table.TableName))
            {
                //
                throw new LibExceptionBase("该行的所在表的名称为空。");
            }
            if (row.Table.PrimaryKey == null || row.Table.PrimaryKey.Length == 0)
            {
                throw new LibExceptionBase("该行的所在表 未设置主键。");
            }
            //this.Key = row.Table.TableName;
            //this._row = row;
            //this._primarykey = row.Table.PrimaryKey;
        }
        public override void Lock()
        {
            foreach (DataColumn col in this._primarykey)
            {
                if (this.PrimaryValues.Length > 0)
                    this.PrimaryValues += ",";
                this.PrimaryValues+=this._row[col].ToString();
            }
            this.Status = LibLockStatus.Lock;
        }

        public override void UnLock()
        {
            this.PrimaryValues = string.Empty;
            this.Status = LibLockStatus.UnLock;
        }

        public bool HasExist(DataRow row)
        {
            string values = string.Empty;
            foreach (DataColumn col in this._primarykey)
            {
                if (values.Length > 0)
                    values += ",";
                values+=(row[col.ColumnName].ToString());
            }
            return values == this.PrimaryValues;
        }
    }
}
