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
    public class CachHelp
    {
        ObjectCache cache = MemoryCache.Default;

        public CachHelp()
        {

        }

        public void AddCachItem(string key, object val, string progid)
        {
            var exist = cache[key];
            if (exist == null)
            {
                var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddSeconds(30) };
                policy.ChangeMonitors.Add(new LibTableChangeMonitor(key, val, progid, policy));
                //policy.UpdateCallback = updatecallback;
                cache.Set(key, val, policy);
            }

        }

        public void AddCachItem(string key, object val, DateTimeOffset dtoffset)
        {
            var exist = cache[key];
            if (exist == null)
            {
                cache.Set(key, val, dtoffset);
            }
        }

        public object GetCach(string key)
        {
            //if (cache.Contains(key))
            //{
            //    return cache[key];
            //}
            return cache[key];
        }

        public void RemoveCache(string key)
        {
            cache.Remove(key);
        }
    }

    public class LibTableChangeMonitor : ChangeMonitor
    {
        private string _uniqueid = string.Empty;
        private string _progid = string.Empty;
        private LibTable[] _table = null;
        private CacheItemPolicy _policy;
        public LibTableChangeMonitor(string uniqueid, object table, string progid, CacheItemPolicy policy)
        {
            this._uniqueid = uniqueid;
            this._progid = progid;
            this._table = (LibTable[])table;
            this._policy = policy;
            CacheEntryUpdateCallback updatecallback = new CacheEntryUpdateCallback(CacheUpdateCallBack);
            this._policy.UpdateCallback = updatecallback;
            //if (this._table != null)
            //{
            //    foreach (LibTable item in this._table)
            //    {
            //        foreach (DataTable dt in item.Tables)
            //        {
            //            dt.RowChanged += Dt_RowChanged;
            //            //dt.ColumnChanged += Dt_ColumnChanged;

            //        }
            //    }
            //}
            this.InitializationComplete();
        }

        private void Dt_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {

        }

        private void Dt_RowChanged(object sender, DataRowChangeEventArgs e)
        {

            Bll.DelegateFactory df = new Bll.DelegateFactory();
            df.SaveRowChange(System.Web.HttpContext.Current.Session.SessionID, this._progid, e.Row, e.Action, (int)System.Web.HttpContext.Current.Session[SysConstManage.OperateAction]);
        }

        public override string UniqueId { get { return this._uniqueid; } }

        protected override void Dispose(bool disposing)
        {
            //throw new NotImplementedException();
        }


        private void CacheUpdateCallBack(CacheEntryUpdateArguments arg)
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
                                        oldval = col.DataType == typeof(byte[])&&dr[col, DataRowVersion.Original] != DBNull.Value ? Convert.ToBase64String((byte[])dr[col, DataRowVersion.Original]) : dr[col, DataRowVersion.Original].ToString();
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


    //public partial class MemoryCacheHelper
    //{
    //    private static object _locker = new object();
    //    private static MemoryCacheHelper instance;
    //    //委托移除以后
    //    private CacheEntryRemovedCallback _RemovedCallback;
    //    //委托即将移除时
    //    private CacheEntryUpdateCallback _UpdateCallback;

    //    private MemoryCacheHelper()
    //    {

    //    }

    //    public static MemoryCacheHelper Instance
    //    {
    //        get
    //        {
    //            if (instance == null)
    //            {
    //                lock (_locker)
    //                {
    //                    if (instance == null)
    //                    {
    //                        instance = new MemoryCacheHelper();
    //                    }
    //                }
    //            }
    //            return instance;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取缓存中的值
    //    /// </summary>
    //    /// <typeparam name="T">类型</typeparam>
    //    /// <param name="key">标识的Key(保持唯一)</param>
    //    /// <param name="value">需要缓存的缓存项的值</param>
    //    /// <param name="slidingExpiration">该值指示如果某个缓存项在给定时段内未被访问，是否应被逐出</param>
    //    /// <param name="absoluteExpiration">该值指示是否应在指定持续时间过后逐出某个缓存项</param>
    //    /// <returns></returns>
    //    public T GetCacheItem<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
    //    {
    //        return GetCacheItem<T>(key, value, null, slidingExpiration, absoluteExpiration);
    //    }

    //    /// <summary>
    //    /// 获取缓存中的值
    //    /// </summary>
    //    /// <typeparam name="T">类型</typeparam>
    //    /// <param name="key">标识的Key(保持唯一)</param>
    //    /// <param name="value">需要缓存的缓存项的值</param>
    //    /// <param name="Command">SQL执行的Command</param>
    //    /// <param name="slidingExpiration">该值指示如果某个缓存项在给定时段内未被访问，是否应被逐出</param>
    //    /// <param name="absoluteExpiration">该值指示是否应在指定持续时间过后逐出某个缓存项</param>
    //    /// <returns></returns>
    //    public T GetCacheItem<T>(string key, T value, SqlCommand Command = null, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
    //    {
    //        if (string.IsNullOrWhiteSpace(key))
    //            throw new ArgumentException("Invalid cache key");
    //        if (value == null)
    //            throw new ArgumentNullException("value");
    //        if (slidingExpiration == null && absoluteExpiration == null)
    //            throw new ArgumentException("Either a sliding expiration or absolute must be provided");
    //        ObjectCache cachelist = MemoryCache.Default;
    //        if (cachelist[key] == null)
    //        {
    //            lock (_locker)
    //            {
    //                var item = new CacheItem(key, value);
    //                var policy = CreatePolicy(_RemovedCallback, _UpdateCallback, Command, slidingExpiration, absoluteExpiration);
    //                MemoryCache.Default.Add(item, policy);
    //            }
    //        }
    //        return (T)Convert.ChangeType(cachelist[key], typeof(T));
    //    }

    //    /// <summary>
    //    /// 获取缓存中的值
    //    /// </summary>
    //    /// <typeparam name="T">类型</typeparam>
    //    /// <param name="key">标识的Key(保持唯一)</param>
    //    public T GetCacheItem<T>(string key)
    //    {
    //        if (string.IsNullOrWhiteSpace(key))
    //            throw new ArgumentException("Invalid cache key");
    //        ObjectCache cachelist = MemoryCache.Default;
    //        return (T)Convert.ChangeType(cachelist[key], typeof(T));
    //    }

    //    /// <summary>
    //    /// 创建指定缓存项的一组逐出和过期详细信息
    //    /// </summary>
    //    /// <param name="RemovedCallback">在从缓存中移除某个项后将调用该委托</param>
    //    /// <param name="UpdateCallback">在从缓存中移除某个缓存项之前将调用该委托</param>
    //    /// <param name="slidingExpiration">该值指示如果某个缓存项在给定时段内未被访问，是否应被逐出</param>
    //    /// <param name="absoluteExpiration">该值指示是否应在指定持续时间过后逐出某个缓存项</param>
    //    /// <returns></returns>
    //    private CacheItemPolicy CreatePolicy(CacheEntryRemovedCallback RemovedCallback, CacheEntryUpdateCallback UpdateCallback, SqlCommand Command, TimeSpan? slidingExpiration, DateTime? absoluteExpiration)
    //    {
    //        var policy = new CacheItemPolicy();
    //        if (absoluteExpiration.HasValue)
    //            policy.AbsoluteExpiration = absoluteExpiration.Value;
    //        else if (slidingExpiration.HasValue)
    //            policy.SlidingExpiration = slidingExpiration.Value;
    //        policy.Priority = CacheItemPriority.Default;
    //        if (RemovedCallback != null)
    //            policy.RemovedCallback = RemovedCallback;
    //        if (UpdateCallback != null)
    //            policy.UpdateCallback = UpdateCallback;
    //        if (Command != null)
    //        {
    //            System.Data.SqlClient.SqlDependency mSqlDependency = new System.Data.SqlClient.SqlDependency(Command);
    //            policy.ChangeMonitors.Add(new SqlChangeMonitor(mSqlDependency));
    //        }
    //        return policy;
    //    }

    //    /// <summary>
    //    /// 事件定义在从缓存中移除某个缓存项后触发
    //    /// </summary>
    //    public event CacheEntryRemovedCallback RemovedCallback
    //    {
    //        add
    //        {
    //            lock (_locker)
    //            {
    //                _RemovedCallback += value;
    //            }
    //        }
    //        remove
    //        {
    //            lock (_locker)
    //            {
    //                _RemovedCallback -= value;
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// 事件定义在从缓存中即将移除某个缓存项时触发
    //    /// </summary>
    //    public event CacheEntryUpdateCallback UpdateCallback
    //    {
    //        add
    //        {
    //            lock (_locker)
    //            {
    //                _UpdateCallback += value;
    //            }
    //        }
    //        remove
    //        {
    //            lock (_locker)
    //            {
    //                _UpdateCallback -= value;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 定义在从缓存中移除某个缓存项后触发
    //    /// </summary>
    //    /// <param name="args">有关已从缓存中移除的缓存项的信息</param>
    //    protected virtual void OnRemovedCallback(CacheEntryRemovedArguments args)
    //    {
    //        if (_RemovedCallback != null)
    //            _RemovedCallback(args);
    //    }
    //    /// <summary>
    //    /// 定义在从缓存中即将移除某个缓存项时触发
    //    /// </summary>
    //    /// <param name="args">有关将从缓存中移除的缓存项的信息</param>
    //    protected virtual void OnUpdateCallback(CacheEntryUpdateArguments args)
    //    {
    //        if (_UpdateCallback != null)
    //            _UpdateCallback(args);
    //    }

    //}
}