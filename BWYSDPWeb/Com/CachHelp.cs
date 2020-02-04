using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
using SDPCRL.COM;
using System.Data;
using SDPCRL.CORE;
using Bll;

namespace BWYSDPWeb.Com
{
    //public class CachHelp
    //{
    //    ObjectCache cache = MemoryCache.Default;

    //    public CachHelp()
    //    {

    //    }

    //    public void AddCachItem(string key, object val, string progid)
    //    {
    //        var exist = cache[key];
    //        if (exist == null)
    //        {
    //            var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddSeconds(30) };
    //            policy.ChangeMonitors.Add(new LibTableChangeMonitor(key, val, progid, policy));
    //            //policy.UpdateCallback = updatecallback;
    //            cache.Set(key, val, policy);
    //        }

    //    }

    //    public void AddCachItem(string key, object val, DateTimeOffset dtoffset)
    //    {
    //        var exist = cache[key];
    //        if (exist == null)
    //        {
    //            cache.Set(key, val, dtoffset);
    //        }
    //    }

    //    public object GetCach(string key)
    //    {
    //        //if (cache.Contains(key))
    //        //{
    //        //    return cache[key];
    //        //}
    //        return cache[key];
    //    }

    //    public void RemoveCache(string key)
    //    {
    //        cache.Remove(key);
    //    }
    //}

    //public class LibTableChangeMonitor : ChangeMonitor
    //{
    //    private string _uniqueid = string.Empty;
    //    private string _progid = string.Empty;
    //    private LibTable[] _table = null;
    //    private CacheItemPolicy _policy;
    //    public LibTableChangeMonitor(string uniqueid, object table, string progid, CacheItemPolicy policy)
    //    {
    //        this._uniqueid = uniqueid;
    //        this._progid = progid;
    //        this._table = (LibTable[])table;
    //        this._policy = policy;
    //        CacheEntryUpdateCallback updatecallback = new CacheEntryUpdateCallback(CacheUpdateCallBack);
    //        this._policy.UpdateCallback = updatecallback;
    //        //if (this._table != null)
    //        //{
    //        //    foreach (LibTable item in this._table)
    //        //    {
    //        //        foreach (DataTable dt in item.Tables)
    //        //        {
    //        //            dt.RowChanged += Dt_RowChanged;
    //        //            //dt.ColumnChanged += Dt_ColumnChanged;

    //        //        }
    //        //    }
    //        //}
    //        this.InitializationComplete();
    //    }

    //    private void Dt_ColumnChanged(object sender, DataColumnChangeEventArgs e)
    //    {

    //    }

    //    private void Dt_RowChanged(object sender, DataRowChangeEventArgs e)
    //    {

    //        Bll.DelegateFactory df = new Bll.DelegateFactory();
    //        df.SaveRowChange(System.Web.HttpContext.Current.Session.SessionID, this._progid, e.Row, e.Action, (int)System.Web.HttpContext.Current.Session[SysConstManage.OperateAction]);
    //    }

    //    public override string UniqueId { get { return this._uniqueid; } }

    //    protected override void Dispose(bool disposing)
    //    {
    //        //throw new NotImplementedException();
    //    }


    //    private void CacheUpdateCallBack(CacheEntryUpdateArguments arg)
    //    {
    //        if (this._table != null)
    //        {
    //            string sessionid = this._uniqueid.Split('_')[0];
    //            List<string> commandlst = new List<string>();
    //            TempHelp sQLiteHelp = new TempHelp("TempData");
    //            int rowindex = -1;
    //            int action = -1;
    //            #region 
    //            DataTable temp = new DataTable();
    //            temp.Columns.AddRange(new DataColumn[] {
    //                new DataColumn("sessionid",typeof(string)),
    //                new DataColumn("progid",typeof(string)),
    //                new DataColumn("tableNm",typeof(string)),
    //                new DataColumn("rowid",typeof(int)),
    //                new DataColumn("fieldnm",typeof(string)),
    //                new DataColumn("fieldvalue",typeof(string)),
    //                new DataColumn("actions",typeof(int)),
    //                new DataColumn ("oldfieldvalue",typeof(string))
    //            });
    //            DataRow row = null;
    //            #endregion
    //            string a = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
    //            commandlst.Add(string.Format("EXEC sp_executesql N'" +
    //                       "delete from [temp] where sessionid=@sessionid and progid=@progid  '," +
    //                       "N'@sessionid nchar(35),@progid nvarchar(35)'," +
    //                       "@sessionid='{0}',@progid='{1}' ",
    //                       sessionid, this._progid));
    //            sQLiteHelp.Update(commandlst);
    //            try
    //            {
    //                foreach (var tb in this._table)
    //                {
    //                    foreach (var t in tb.Tables)
    //                    {
    //                        foreach (DataRow dr in t.Rows)
    //                        {
    //                            rowindex = t.Rows.IndexOf(dr);
    //                            switch (dr.RowState)
    //                            {
    //                                case DataRowState.Added:
    //                                    action = 0;
    //                                    break;
    //                                case DataRowState.Deleted:
    //                                    action = 2;
    //                                    break;
    //                                case DataRowState.Modified:
    //                                    action = 1;
    //                                    break;
    //                                case DataRowState.Unchanged:
    //                                    action = -1;
    //                                    break;
    //                            }
    //                            object val = null;
    //                            object oldval = null;
    //                            foreach (DataColumn col in t.Columns)
    //                            {
    //                                if (action != 2)
    //                                {
    //                                    val = col.DataType == typeof(byte[]) && dr[col] != DBNull.Value ? Convert.ToBase64String((byte[])dr[col]) : dr[col].ToString();
    //                                }
    //                                if (action == 1 || action == 2)
    //                                {
    //                                    oldval = col.DataType == typeof(byte[])&&dr[col, DataRowVersion.Original] != DBNull.Value ? Convert.ToBase64String((byte[])dr[col, DataRowVersion.Original]) : dr[col, DataRowVersion.Original].ToString();
    //                                    //if (col.DataType == typeof(byte[]))
    //                                    //{
    //                                    //    oldval = dr[col, DataRowVersion.Original] != DBNull.Value ? Convert .ToBase64String((byte[])dr[col, DataRowVersion.Original]) : dr[col, DataRowVersion.Original].ToString();
    //                                    //}
    //                                    //else
    //                                    //    oldval = dr[col, DataRowVersion.Original].ToString();
    //                                }
    //                                row = temp.NewRow();
    //                                row[0] = sessionid;
    //                                row[1] = _progid;
    //                                row[2] = t.TableName;
    //                                row[3] = rowindex;
    //                                row[4] = col.ColumnName;
    //                                row[5] = action == 2 ? oldval : val;
    //                                row[6] = action;
    //                                row[7] = (action == 1 || action == 2) ? oldval : val;
    //                                temp.Rows.Add(row);
    //                                //     commandlst.Add(string.Format("EXEC sp_executesql N'" +
    //                                //"insert into [temp] values(@sessionid,@progid,@tbnm,@rwindx,@fieldnm,@fieldvalu,@actions)   '," +
    //                                //"N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int,@fieldnm nvarchar(35),@fieldvalu ntext,@actions smallint'," +
    //                                //"@sessionid='{0}',@progid='{1}',@tbnm='{2}',@rwindx={3},@fieldnm='{4}',@fieldvalu=N'{5}',@actions={6}  ",
    //                                //sessionid, this._progid, t.TableName, rowindex, col.ColumnName, action==-1?"":dr[col].ToString(), action));
    //                            }
    //                        }
    //                    }
    //                }
    //                string j = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
    //                //sQLiteHelp.Update(commandlst);
    //                sQLiteHelp.SqlBulkUpdate(temp);
    //                string b = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
    //            }
    //            catch (Exception ex)
    //            {

    //            }

    //        }
    //    }


    //}

    public class LibTableChangeMonitor2 : LibChangeMonitorBase
    {
        //private string _uniqueid = string.Empty;
        private string _progid = string.Empty;
        private LibTable[] _table = null;
        //private CacheItemPolicy _policy;
        public LibTableChangeMonitor2(string uniqueid, object table, string progid)
        {
            this._uniqueid = uniqueid;
            this._progid = progid;
            this._table = (LibTable[])table;
            this.InitializationComplete();
        }

        public override string UniqueId { get { return this._uniqueid; } }

        protected override void Dispose(bool disposing)
        {
            //throw new NotImplementedException();
        }

        public override void CacheRemoveCallBack(CacheEntryUpdateArguments arg)
        {
            if (this._table != null)
            {
                string sessionid = this._uniqueid.Split('_')[0];
                List<string> commandlst = new List<string>();
                TempHelp sQLiteHelp = new TempHelp("TempData");
                int rowindex = -1;
                int action = -1;
                #region 
                DataTable temp = new DataTable();
                temp.Columns.AddRange(new DataColumn[] {
                    new DataColumn("sessionid",typeof(string)),
                    new DataColumn("progid",typeof(string)),
                    new DataColumn("tableNm",typeof(string)),
                    new DataColumn("rowid",typeof(int)),
                    new DataColumn("fieldnm",typeof(string)),
                    new DataColumn("fieldvalue",typeof(string)),
                    new DataColumn("actions",typeof(int)),
                    new DataColumn ("oldfieldvalue",typeof(string))
                });
                DataRow row = null;
                #endregion
                string a = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                commandlst.Add(string.Format("EXEC sp_executesql N'" +
                           "delete from [temp] where sessionid=@sessionid and progid=@progid  '," +
                           "N'@sessionid nchar(35),@progid nvarchar(35)'," +
                           "@sessionid='{0}',@progid='{1}' ",
                           sessionid, this._progid));
                sQLiteHelp.Update(commandlst);
                try
                {
                    foreach (var tb in this._table)
                    {
                        foreach (var t in tb.Tables)
                        {
                            foreach (DataRow dr in t.Rows)
                            {
                                rowindex = t.Rows.IndexOf(dr);
                                switch (dr.RowState)
                                {
                                    case DataRowState.Added:
                                        action = 0;
                                        break;
                                    case DataRowState.Deleted:
                                        action = 2;
                                        break;
                                    case DataRowState.Modified:
                                        action = 1;
                                        break;
                                    case DataRowState.Unchanged:
                                        action = -1;
                                        break;
                                }
                                object val = null;
                                object oldval = null;
                                foreach (DataColumn col in t.Columns)
                                {
                                    if (action != 2)
                                    {
                                        val = col.DataType == typeof(byte[]) && dr[col] != DBNull.Value ? Convert.ToBase64String((byte[])dr[col]) : dr[col].ToString();
                                    }
                                    if (action == 1 || action == 2)
                                    {
                                        oldval = col.DataType == typeof(byte[]) && dr[col, DataRowVersion.Original] != DBNull.Value ? Convert.ToBase64String((byte[])dr[col, DataRowVersion.Original]) : dr[col, DataRowVersion.Original].ToString();
                                        //if (col.DataType == typeof(byte[]))
                                        //{
                                        //    oldval = dr[col, DataRowVersion.Original] != DBNull.Value ? Convert .ToBase64String((byte[])dr[col, DataRowVersion.Original]) : dr[col, DataRowVersion.Original].ToString();
                                        //}
                                        //else
                                        //    oldval = dr[col, DataRowVersion.Original].ToString();
                                    }
                                    row = temp.NewRow();
                                    row[0] = sessionid;
                                    row[1] = _progid;
                                    row[2] = t.TableName;
                                    row[3] = rowindex;
                                    row[4] = col.ColumnName;
                                    row[5] = action == 2 ? oldval : val;
                                    row[6] = action;
                                    row[7] = (action == 1 || action == 2) ? oldval : val;
                                    temp.Rows.Add(row);
                                    //     commandlst.Add(string.Format("EXEC sp_executesql N'" +
                                    //"insert into [temp] values(@sessionid,@progid,@tbnm,@rwindx,@fieldnm,@fieldvalu,@actions)   '," +
                                    //"N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int,@fieldnm nvarchar(35),@fieldvalu ntext,@actions smallint'," +
                                    //"@sessionid='{0}',@progid='{1}',@tbnm='{2}',@rwindx={3},@fieldnm='{4}',@fieldvalu=N'{5}',@actions={6}  ",
                                    //sessionid, this._progid, t.TableName, rowindex, col.ColumnName, action==-1?"":dr[col].ToString(), action));
                                }
                            }
                        }
                    }
                    string j = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                    //sQLiteHelp.Update(commandlst);
                    sQLiteHelp.SqlBulkUpdate(temp);
                    string b = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                }
                catch (Exception ex)
                {

                }

            }
        }
    }
}