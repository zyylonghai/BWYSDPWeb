using SDPCRL.DAL.BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDPCRL.COM;
using SDPCRL.CORE;
using System.Data;
using SDPCRL.DAL.COM;

namespace BWYSDPBaseDal
{
    public class DalDataBase : DALBase
    {
        #region 私有属性
        //private SDPCRL.DAL.COM.SQLBuilder _sQLBuilder = null;
        private LibDSContext _dSContext = null;
        #endregion 
        #region 属性
        //public  SDPCRL.DAL.COM.SQLBuilder SQLBuilder { get
        //    {
        //        if (this._sQLBuilder == null) this._sQLBuilder = new SDPCRL.DAL.COM.SQLBuilder(this.ProgId);
        //        return this._sQLBuilder;
        //    }
        //}
        public LibDSContext DSContext {
            get {
                if (this._dSContext == null) this._dSContext = new LibDSContext(this.ProgId);
                return this._dSContext;
            }
        }
        #endregion 
        public DalDataBase()
        {
            
        }

        protected override void AfterUpdate()
        {
            //this.AddMessage("AfterUpdate has do run error",LibMessageType.Warning);
            base.AfterUpdate();
        }
        #region 字段或信息的多语言描述 相关
        protected string GetFieldDesc(string dsid, string tablenm, string fieldnm)
        {
            CachHelp cachelp = new CachHelp();
            DataTable dt = cachelp.GetCach(dsid) as DataTable;
            if (dt == null)
            {
                dt = (DataTable)this.ExecuteSysDalMethod("TestFunc", "GetFieldDescByDSID", dsid);
                cachelp.AddCachItem(dsid, dt, DateTimeOffset.Now.AddMinutes(2));
            }
            if (dt != null)
            {
                DataRow[] dr = dt.Select(string.Format("LanguageId={0} and DSID='{1}' and FieldNm='{2}' and TableNm='{3}'",
                                                     (int)this.Language, dsid, fieldnm, tablenm));
                if (dr != null && dr.Length > 0)
                {
                    return dr[0]["Vals"].ToString();
                }
            }
            return (string)this.ExecuteSysDalMethod("TestFunc", "InternalGetFieldDesc", (int)this.Language, dsid, tablenm, fieldnm);

        }

        protected string GetMessageDesc(string msgid)
        {
            return this.GetFieldDesc(string.Empty, string.Empty, msgid);
        }
        protected void AddMessage(string msgid, object[] parms, LibMessageType type = LibMessageType.Error )
        {
            if (parms != null)
            {
                this.AddMessage(string.Format(this.GetMessageDesc(msgid), parms), type);
            }
            else
                this.AddMessage(this.GetMessageDesc(msgid), type);
        }
        #endregion 

        #region  锁相关
        protected void AddDataLock(string tablenm, DataRow row, DataColumn[] primarykey)
        {
            //DataLock dataLock = new DataLock(tablenm, row, primarykey);
            //dataLock.ClientSessionId = this.LibClient.SessionId;
            //LockHelp.AddLock<DataLock>(tablenm, this.LibClient .SessionId ,tablenm, row, primarykey);
            LockHelp<DataLock>.AddLock(tablenm, this.LibClient.SessionId, tablenm, row, primarykey);
        }
        protected void AddDataLock(string tablenm, DataRow row)
        {
            LockHelp<DataLock>.AddLock(tablenm, this.LibClient.SessionId, row);
        }
        protected void RemoveDataLock(string tablenm, DataRow row)
        {
            List<DataLock> locks = LockHelp<DataLock>.GetLock(tablenm);
            if (locks != null)
            {
                DataLock l = locks.FirstOrDefault(i => i.ClientSessionId == this.LibClient.SessionId && i.Status == LibLockStatus.Lock && i.HasExist(row));
                if (l!=null)
                {
                    LockHelp<DataLock>.RemoveLock(l);
                }
            }
        }

        protected bool ExistDataLock(string tablenm, DataRow row)
        {
            List<DataLock> locks = LockHelp<DataLock>.GetLock(tablenm);
            if (locks != null)
            {
                if (locks.FirstOrDefault(i => i.ClientSessionId == this.LibClient.SessionId && i.Status == LibLockStatus.Lock && i.HasExist(row)) != null)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion 

        public DataTable InternalSearch(string tbnm, string[] fields,List<LibSearchCondition> conds)
        {
            object[] values = { };
            StringBuilder whereformat = new StringBuilder();
            AnalyzeSearchCondition(conds, whereformat,ref values);
            //string sql = this.SQLBuilder.GetSQL(tbnm, fields, new WhereObject { WhereFormat = whereformat.ToString (), Values = values });
            string sql = this.DSContext.GetSQL(tbnm, fields, new WhereObject { WhereFormat = whereformat.ToString(), Values = values });
            return this.DataAccess.GetDataTable(sql);
            //return null;
        }
        public DataTable InternalSearchByPage(string dsid,string tbnm, string[] fields, List<LibSearchCondition> conds, int pageindex, int pagesize)
        {
            object[] values = { };
            StringBuilder whereformat = new StringBuilder();
            AnalyzeSearchCondition(conds, whereformat,ref values);
            SDPCRL.DAL.COM.SQLBuilder sQLBuilder = null;
            if (string.IsNullOrEmpty(dsid))
            {
                sQLBuilder = new SDPCRL.DAL.COM.SQLBuilder();
            }
            else
            {
                sQLBuilder = new SDPCRL.DAL.COM.SQLBuilder(dsid);
            }
            //LibDSContext dSContext = new LibDSContext(dsid);
            //string sql = dSContext.GetSQLByPage(tbnm, fields, new WhereObject { WhereFormat = whereformat.ToString(), Values = values }, pageindex, pagesize, true, false);
            string sql = sQLBuilder.GetSQLByPage(tbnm, fields, new WhereObject { WhereFormat = whereformat.ToString(), Values = values }, pageindex, pagesize, true, false);
            return this.DataAccess.GetDataTable(sql);
        }

        public DataTable[] InternalFillData(List<string> whereformat,object[] valus )
        {
            StringBuilder sql = new StringBuilder();
            DataTable[] dts = { };
            StringBuilder where = new StringBuilder();
            //DataTable mdt = null;
            //Dictionary<int, int> dic = new Dictionary<int, int>();
            //SDPCRL.DAL.COM.SQLBuilder sQLBuilder = new SDPCRL.DAL.COM.SQLBuilder();
            foreach (var libdt in this.LibTables)
            {
                foreach (LibTableObj dtobj in libdt.Tables)
                {
                    TableExtendedProperties tbextprop = this.JsonToObj<TableExtendedProperties>(dtobj.DataTable.ExtendedProperties[SysConstManage.ExtProp].ToString());
                    if (!tbextprop.Ignore) continue;
                    Array.Resize(ref dts, dts.Length + 1);
                    dts[dts.Length - 1] =new DataTable (dtobj.TableName);

                    if (tbextprop.TableIndex == 0 || tbextprop .TableIndex !=tbextprop .RelateTableIndex)
                    {
                        where.Clear();
                        foreach (string item in whereformat)
                        {
                            if (where.Length > 0)
                            {
                                where.Append(" And ");
                            }
                            where.AppendFormat("{0}.{1}", LibSysUtils.ToCharByTableIndex(tbextprop.TableIndex), item);
                        }
                        //sql.Append(this.SQLBuilder.GetSQL(dtobj.TableName, null, new WhereObject { WhereFormat=where.ToString (),Values=valus },false,false));
                        sql.Append(this.DSContext.GetSQL(dtobj.TableName, null, new WhereObject { WhereFormat = where.ToString(), Values = valus }, false, false));
                        sql.AppendLine();
                    }
                }
            }
            this.DataAccess.GetDatatTables(sql.ToString(), ref dts);
            return dts;
           
        }

        /// <summary>
        /// 根据账户id 获取账户下的角色及权限对象。
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetAuthority(string userid)
        {
            //SQLBuilder sQLBuilder = new SQLBuilder("Account");
            LibDSContext dSContext = new LibDSContext("Account");
            var userRole = dSContext["UserRole"];
            string sql = dSContext.GetSQL("UserRole",null, dSContext.Where(userRole.Columns.UserId + "={0}", userid));
            return this.DataAccess.GetDataTable(sql);
        }

        #region 根据规则生成编码 相关函数
        public DataTable GetRuleDataByProgid(string progid)
        {
            //SQLBuilder sQLBuilder = new SQLBuilder("CodeRuleConfig");
            LibDSContext dSContext = new LibDSContext("CodeRuleConfig");
            var ruleconfig = dSContext["CodeRuleConfig"];
            //string sql = sQLBuilder.GetSQL("CodeRuleConfig", null, sQLBuilder.Where("A.ProgId={0}", progid));
            string sql = dSContext.GetSQL("CodeRuleConfig", null, dSContext.Where(ruleconfig.Columns.ProgId + "={0}", progid));
            return this.DataAccess.GetDataTable(sql);
        }

        public string GenerateNo(string progid, string ruleid)
        {
            DataTable dt = GetRuleDataByProgid(progid);
            //dt.DefaultView.Sort = "RuleId,SeqNo";
            if (dt != null)
            {
                LibTableObj tbobj = new LibTableObj(dt);
                var rows = tbobj.Rows.Where(i => i.RuleId == ruleid).OrderBy(i=>new { i.RuleId, i.SeqNo }).ToList();
                return DoGenerateNo(rows);
                //DataRow[] rows = dt.Select(string.Format("RuleId='{0}'", ruleid));
                //return DoGenerateNo(rows);
            }
            return string.Empty;
        }
        public string GenerateNoByprogid(string progid)
        {
            DataTable dt = GetRuleDataByProgid(progid);
            //dt.DefaultView.Sort = "RuleId,SeqNo";
            if (dt != null)
            {
                LibTableObj tbobj = new LibTableObj(dt);
                var rows = tbobj.Rows.Where(i => i.IsDefault && ((DataRowObj)i).DataRowState!=DataRowState.Deleted).ToList();
                return DoGenerateNo(rows);
            }
            return string.Empty;
        }
        #endregion 
        #region 私有函数
        private void AnalyzeSearchCondition(List<LibSearchCondition> conds, StringBuilder whereformat,ref object[] values)
        {
            SearchConditionHelper.AnalyzeSearchCondition(conds, whereformat, ref values);
            //int n = 0;
            //LibSearchCondition precond=null;
            //int len = 0;
            //foreach (LibSearchCondition item in conds)
            //{
            //    if (whereformat.Length > 0)
            //    {
            //        if (precond != null)
            //            whereformat.AppendFormat(" {0} ", precond.Logic.ToString());
            //    }
            //    switch (item.Symbol)
            //    {
            //        case SmodalSymbol.Equal:
            //            whereformat.Append("" + item.FieldNm + "={" + n + "}");
            //            len = 1;
            //            break;
            //        case SmodalSymbol.MoreThan:
            //            whereformat.Append("" + item.FieldNm + ">{" + n + "}");
            //            len = 1;
            //            break;
            //        case SmodalSymbol.LessThan:
            //            whereformat.Append("" + item.FieldNm + "<{" + n + "}");
            //            len = 1;
            //            break;
            //        case SmodalSymbol.Contains:
            //            whereformat.Append("" + item.FieldNm + " like {" + n + "}");
            //            item.Values[0] = string.Format("%{0}%", item.Values[0]);
            //            len = 1;
            //            break;
            //        case SmodalSymbol.Between:
            //            whereformat.Append("" + item.FieldNm + " between {" + n + "} and {" +(n=n+1) + "}");
            //            len = 2;
            //            break;
            //        case SmodalSymbol.NoEqual:
            //            whereformat.Append("" + item.FieldNm + "!={" + n + "}");
            //            len = 1;
            //            break;
            //    }
            //    n++;
            //    if (item.Values != null)
            //    {
            //        for(int i=0;i<len;i++)
            //        {
            //            //if (LibSysUtils.IsNULLOrEmpty(o)) continue;
            //            Array.Resize(ref values, values.Length + 1);
            //            values[values.Length - 1] =item .Values[i];
            //        }
            //    }
            //    precond = item;
            //}
        }
        private string DoGenerateNo(DataRow[] rows)
        {
            //DataRow[] rows = dt.Select(string.Format("RuleId='{0}'", ruleid));
            if (rows != null && rows.Length > 0)
            {
                LibTableObj config = new LibTableObj(rows[0].Table);
                config.FillData(rows);
                
                #region 判断是否有加锁
                if (this.ExistDataLock("CodeRuleConfig", rows[0]))
                {
                    //该功能和规则编号，正被锁着。
                    //throw new LibExceptionBase("该功能和规则编号，正被锁着。");
                    //this.AddMessage("该功能和规则编号，正被锁着。", LibMessageType.Error);
                    //msg000000011 该功能和规则编号，正被锁着。
                    this.AddMessage("msg000000011", null);
                }

                #endregion
                #region 加锁，防止生成重复序列号
                dynamic firstdr = config.FindRow(0);
                this.AddDataLock("CodeRuleConfig", rows[0], new DataColumn[] { rows[0].Table.Columns[config .Columns .ProgId], rows[0].Table.Columns[config.Columns.RuleId] });
                #endregion 
                //string currdate = rows[0][config.Columns .CurrDate].ToString();
                //int currserial = Convert.ToInt32(rows[0][config.Columns .CurrSerial]);
                string currdate = firstdr.CurrDate;
                int currserial = firstdr.CurrSerial;
                string module = string.Empty;
                int serialen = -1;
                int index = 0;
                //string suffix = string.Empty;
                StringBuilder dateformat = new StringBuilder();
                StringBuilder format = new StringBuilder();
                foreach (DataRow dr in rows)
                {
                    module = dr["ModuleId"].ToString();
                    switch (module)
                    {
                        case "yy":
                        case "yyyy":
                        case "MM":
                        case "dd":
                            dateformat.Append(module);
                            format.Append(module);
                            break;
                        case "prefix":
                        case "suffix":
                            format.Append(dr["FixValue"].ToString());
                            break;
                        case "serial":
                            format.Append("{" + index + "}");
                            index++;
                            serialen = Convert.ToInt32(dr["SeriaLen"]);
                            break;

                    }
                }
                if (serialen != -1)
                {
                    currserial = DateTime.Now.ToString(dateformat.ToString()) == currdate ? (currserial+1) : 1;
                    //format.(format.ToString(), currserial.ToString().PadLeft(serialen, '0'));
                }
                #region 更新CodeRuleConfig表的当前日期和流水号
                //SQLBuilder builder = new SQLBuilder("CodeRuleConfig");
                //string sql= builder.GetUpdateSQL("CodeRuleConfig", builder.UpdateField("CurrDate={0},CurrSerial={1}", DateTime.Now.ToString(dateformat.ToString()), currserial), 
                //                                       builder.Where("ProgId={0} and RuleId={1}", rows[0]["ProgId"].ToString(), rows[0]["RuleId"].ToString()));
                //this.DataAccess.ExecuteNonQuery(sql);
                config = new LibTableObj("CodeRuleConfig", "CodeRuleConfig");
                config.FillData(new DataRow[] { rows[0] });
                dynamic rw = config.FindRow(0);
                rw.CurrDate = DateTime.Now.ToString(dateformat.ToString());
                rw.CurrSerial = currserial;
                this.DataAccess.SaveChange(new LibTableObj[] { config });
                #endregion 
                #region 从容器中移除锁
                RemoveDataLock("CodeRuleConfig", rows[0]);
                #endregion
                return DateTime.Now.ToString(string.Format(format.ToString(), currserial.ToString().PadLeft(serialen, '0')));
            }
            return string.Empty;
        }

        private string DoGenerateNo(List<dynamic> rows)
        {
            if (rows != null && rows.Count > 0)
            {
                #region 判断是否有加锁
                dynamic firstdr = rows[0];
                DataRowObj rowobj = (DataRowObj)firstdr;
                if (this.ExistDataLock("CodeRuleConfig", rowobj.Row))
                {
                    //该功能和规则编号，正被锁着。
                    //throw new LibExceptionBase("该功能和规则编号，正被锁着。");
                    //msg000000011       该功能和规则编号，正被锁着。
                    this.AddMessage("msg000000011",null);
                }

                #endregion
                #region 加锁，防止生成重复序列号

                this.AddDataLock("CodeRuleConfig", rowobj.Row, 
                    new DataColumn[] { 
                        rowobj.Row.Table.Columns[rowobj.TableObj.Columns.ProgId], 
                        rowobj.Row.Table.Columns[rowobj.TableObj.Columns.RuleId] }
                    );
                #endregion 
                string currdate = firstdr.CurrDate;
                int currserial = firstdr.CurrSerial;
                string module = string.Empty;
                int serialen = -1;
                int index = 0;
                StringBuilder dateformat = new StringBuilder();
                StringBuilder format = new StringBuilder();
                foreach (var dr in rows)
                {
                    module = dr.ModuleId;
                    switch (module)
                    {
                        case "yy":
                        case "yyyy":
                        case "MM":
                        case "dd":
                            dateformat.Append(module);
                            format.Append(module);
                            break;
                        case "prefix":
                        case "suffix":
                            format.Append(dr.FixValue);
                            break;
                        case "serial":
                            format.Append("{" + index + "}");
                            index++;
                            serialen = Convert.ToInt32(dr.SeriaLen);
                            break;

                    }
                }
                if (serialen != -1)
                {
                    currserial = DateTime.Now.ToString(dateformat.ToString()) == currdate ? (currserial + 1) : 1;
                }
                #region 更新CodeRuleConfig表的当前日期和流水号
                LibTableObj config = new LibTableObj("CodeRuleConfig", "CodeRuleConfig");
                config.FillData(new DataRow[] { rowobj.Row });
                dynamic rw = config.FindRow(0);
                rw.CurrDate = DateTime.Now.ToString(dateformat.ToString());
                rw.CurrSerial = currserial;
                this.DataAccess.SaveChange(new LibTableObj[] { config });
                #endregion 
                #region 从容器中移除锁
                RemoveDataLock("CodeRuleConfig", rowobj.Row);
                #endregion
                return DateTime.Now.ToString(string.Format(format.ToString(), currserial.ToString().PadLeft(serialen, '0')));
            }
            else
            {
                //msg000000012        找不到或未配置编码规则，请确认
                this.AddMessage("msg000000012",null);
            }
            return string.Empty;
        }
        #endregion
    }
}
