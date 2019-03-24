using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public class DelegateFactory
    {
        public delegate void TempDataDelegate(string sessionid, string progid, DataRow dr, DataRowAction action, int operataction);
        delegate void TempDataClearDelegate(string sessionid, string progid);
        public string _sessionid = string.Empty;
        public string _progid = string.Empty;
        public DataRow _dr = null;
        public DataRowAction _action;
        public int _operataction = -1;
        private static readonly object templock = new object();
        static Dictionary<string, List<DelegateFactory>> clearTempDelegatdic = null;



        //public TempDataDelegate compressfile;
        //public AsyncCallback callback;
        public DelegateFactory()
        {

        }

        public void SaveRowChange(string sessionid, string progid, DataRow dr, DataRowAction action, int operataction)
        {
            TempDataDelegate compressfile = new TempDataDelegate(InsertTemp);
            AsyncCallback callback = new AsyncCallback(CallBackMethod);
            //this._sessionid = sessionid;
            //this._progid = progid;
            //this._dr = dr;
            //this._action = action;
            //this._operataction = operataction;
            //List<DelegateFactory> val=null;
            //if (clearTempDelegatdic == null)
            //{
            //    lock (templock)
            //    {
            //        if (clearTempDelegatdic == null)
            //        {
            //            clearTempDelegatdic = new Dictionary<string, List<DelegateFactory>>();
            //        }
            //    }
            //}
            //if (!clearTempDelegatdic.TryGetValue(string.Format("{0}_{1}", sessionid, progid), out val))
            //{
            //    val = new List<DelegateFactory>();
            //    clearTempDelegatdic[string.Format("{0}_{1}", sessionid, progid)] = val;
            //}
            //val.Add(this);
            IAsyncResult iar = compressfile.BeginInvoke(sessionid, progid, dr, action, operataction, callback, compressfile);
        }

        public void ClearTempDataByProgid(string sessionid, string progid)
        {
            this._sessionid = sessionid;
            this._progid = progid;
            TempDataClearDelegate compressfile = new TempDataClearDelegate(ClearTempData);
            AsyncCallback callback = new AsyncCallback(ClearCallBackMethod);
            IAsyncResult iar = compressfile.BeginInvoke(sessionid, progid, callback, compressfile);
        }

        private void InsertTemp(string sessionid, string progid, DataRow dr, DataRowAction action, int operataction)
        {
            List<string> commandlst = new List<string>();
            //string sessionid = System.Web.HttpContext.Current.Session.SessionID;
            string tbNm = dr.Table.TableName;
            int rowindex = dr.Table.Rows.IndexOf(dr);
            //Bll.SQLiteHelp sQLiteHelp = new Bll.SQLiteHelp("TempData");
            TempHelp sQLiteHelp = new TempHelp("TempData");
            switch (action)
            {
                case DataRowAction.Add:
                    foreach (DataColumn col in dr.Table.Columns)
                    {
                        //commandlst.Add(string.Format("EXEC sp_executesql N'if exists(select *from [temp] where sessionid=@sessionid and progid=@progid and tableNm=@tbnm and rowid=@rwindx and fieldnm=@fieldnm) begin  " +
                        //    "delete from [temp] where sessionid=@sessionid and progid=@progid and tableNm=@tbnm and rowid=@rwindx and fieldnm=@fieldnm  end  " +
                        //    "insert into [temp] values(@sessionid,@progid,@tbnm,@rwindx,@fieldnm,@fieldvalu,@actions)   '," +
                        //    "N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int,@fieldnm nvarchar(35),@fieldvalu text,@actions bit'," +
                        //    "@sessionid='{0}',@progid='{1}',@tbnm='{2}',@rwindx={3},@fieldnm='{4}',@fieldvalu='{5}',@actions={6}  ", 
                        //    sessionid, progid, tbNm, rowindex, col.ColumnName, dr[col].ToString(),operataction));
                        commandlst.Add(string.Format("EXEC sp_executesql N'" +
                            "insert into [temp] values(@sessionid,@progid,@tbnm,@rwindx,@fieldnm,@fieldvalu,@actions)   '," +
                            "N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int,@fieldnm nvarchar(35),@fieldvalu ntext,@actions smallint'," +
                            "@sessionid='{0}',@progid='{1}',@tbnm='{2}',@rwindx={3},@fieldnm='{4}',@fieldvalu=N'{5}',@actions={6}  ",
                            sessionid, progid, tbNm, rowindex, col.ColumnName, dr[col].ToString(), operataction == 1 ? 0 : operataction));
                    }
                    break;
                case DataRowAction.Change:
                    //commandlst.Add(string.Format("EXEC sp_executesql N'" +
                    //       "delete from  [temp] where sessionid=@sessionid and progid=@progid and tableNm=@tbnm and rowid=@rwindx   '," +
                    //       "N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int'," +
                    //       "@sessionid='{0}',@progid='{1}',@tbnm='{2}',@rwindx={3}  ",
                    //       sessionid, progid, tbNm, rowindex));
                    foreach (DataColumn col in dr.Table.Columns)
                    {
                        //commandlst.Add(string.Format("EXEC sp_executesql N' " +
                        //   "insert into [temp] values(@sessionid,@progid,@tbnm,@rwindx,@fieldnm,@fieldvalu,@actions)   '," +
                        //   "N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int,@fieldnm nvarchar(35),@fieldvalu ntext,@actions smallint'," +
                        //   "@sessionid='{0}',@progid='{1}',@tbnm='{2}',@rwindx={3},@fieldnm='{4}',@fieldvalu=N'{5}',@actions={6}  ",
                        //   sessionid, progid, tbNm, rowindex, col.ColumnName, dr[col].ToString(), operataction));

                        commandlst.Add(string.Format("EXEC sp_executesql N'" +
                            "update [temp] set fieldvalue=@fieldvalu,actions=@actions where sessionid=@sessionid and progid=@progid and tableNm=@tbnm and rowid=@rwindx and fieldnm=@fieldnm   '," +
                            "N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int,@fieldnm nvarchar(35),@fieldvalu ntext,@actions smallint'," +
                            "@sessionid='{0}',@progid='{1}',@tbnm='{2}',@rwindx={3},@fieldnm='{4}',@fieldvalu=N'{5}',@actions={6}  ",
                            sessionid, progid, tbNm, rowindex, col.ColumnName, dr[col].ToString(), operataction));
                    }
                    break;
                case DataRowAction.Delete:
                    foreach (DataColumn col in dr.Table.Columns)
                    {
                        commandlst.Add(string.Format("EXEC sp_executesql N'" +
                            "update [temp] set actions=@actions where sessionid=@sessionid and progid=@progid and tableNm=@tbnm and rowid=@rwindx and fieldnm=@fieldnm   '," +
                            "N'@sessionid nchar(35),@progid nvarchar(35),@tbnm nvarchar(35),@rwindx int,@fieldnm nvarchar(35),@actions smallint'," +
                            "@sessionid='{0}',@progid='{1}',@tbnm='{2}',@rwindx={3},@fieldnm='{4}',@actions={5}  ",
                            sessionid, progid, tbNm, rowindex, col.ColumnName, 2));
                    }
                    break;
            }
            sQLiteHelp.Update(commandlst);
        }

        private void ClearTempData(string sessionid, string progid)
        {
            //Bll.SQLiteHelp sQLiteHelp = new Bll.SQLiteHelp("TempData");
            TempHelp sQLiteHelp = new TempHelp("TempData");
            //sQLiteHelp.Delete(string.Format("delete from [temp] where sessionid='{0}' and progid='{1}'", sessionid, progid));
            sQLiteHelp.ClearTempData(sessionid, progid);
        }

        private void CallBackMethod(IAsyncResult ar)
        {
            //TempDataDelegate temp = ar.AsyncState as TempDataDelegate;
            //temp.EndInvoke(ar);
        }

        private void ClearCallBackMethod(IAsyncResult ar)
        {
            TempDataClearDelegate tempDataClear = ar.AsyncState as TempDataClearDelegate;
            tempDataClear.EndInvoke(ar);
            //List< DelegateFactory> val = null;
            //if (clearTempDelegatdic.TryGetValue(string.Format("{0}_{1}", _sessionid, _progid), out val))
            //{
            //    foreach (DelegateFactory item in val)
            //    {
            //        item.compressfile.BeginInvoke(item._sessionid, item._progid, item._dr, item._action, item._operataction, item.callback, item.compressfile);
            //    }
            //}
        }
    }
}
