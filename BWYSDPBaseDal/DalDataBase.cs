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
        private SDPCRL.DAL.COM.SQLBuilder _sQLBuilder = null;
        #endregion 
        #region 属性
        public  SDPCRL.DAL.COM.SQLBuilder SQLBuilder { get
            {
                if (this._sQLBuilder == null) this._sQLBuilder = new SDPCRL.DAL.COM.SQLBuilder(this.ProgId);
                return this._sQLBuilder;
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

        protected  string GetFieldDesc(string dsid, string tablenm, string fieldnm)
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

        public DataTable InternalSearch(string tbnm, string[] fields,List<LibSearchCondition> conds)
        {
            object[] values = { };
            StringBuilder whereformat = new StringBuilder();
            AnalyzeSearchCondition(conds, whereformat,ref values);
            string sql = this.SQLBuilder.GetSQL(tbnm, fields, new WhereObject { WhereFormat = whereformat.ToString (), Values = values });
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
            string sql = sQLBuilder.GetSQLByPage(tbnm, fields, new WhereObject { WhereFormat = whereformat.ToString(), Values = values },pageindex,pagesize,true ,false);
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
                foreach (DataTable dt in libdt.Tables)
                {
                    TableExtendedProperties tbextprop = this.JsonToObj<TableExtendedProperties>(dt.ExtendedProperties[SysConstManage.ExtProp].ToString());
                    if (!tbextprop.Ignore) continue;
                    Array.Resize(ref dts, dts.Length + 1);
                    dts[dts.Length - 1] =new DataTable (dt.TableName);

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
                        sql.Append(this.SQLBuilder.GetSQL(dt.TableName, null, new WhereObject { WhereFormat=where.ToString (),Values=valus },false,false));
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
            SQLBuilder sQLBuilder = new SQLBuilder("Account");
            string sql = sQLBuilder.GetSQL("UserRole",null, sQLBuilder.Where("B.UserId={0}", userid));
            return this.DataAccess.GetDataTable(sql);
        }

        #region 根据规则生成编码 相关函数
        public DataTable GetRuleDataByProgid(string progid)
        {
            SQLBuilder sQLBuilder = new SQLBuilder("CodeRuleConfig");
            string sql = sQLBuilder.GetSQL("CodeRuleConfig", null, sQLBuilder.Where("A.ProgId={0}", progid));
            return this.DataAccess.GetDataTable(sql);
        }

        public string GenerateNo(string progid, string ruleid)
        {
            DataTable dt = GetRuleDataByProgid(progid);
            dt.DefaultView.Sort = "RuleId,SeqNo";
            if (dt != null)
            {
                DataRow[] rows = dt.Select(string.Format("RuleId='{0}'", ruleid));
                return DoGenerateNo(rows);
                //if (rows != null && rows.Length > 0)
                //{
                //    string currdate = rows[0]["CurrDate"].ToString();
                //    int currserial = Convert.ToInt32(rows[0]["CurrSerial"]);
                //    string module = string.Empty;
                //    int serialen = -1;
                //    int index = 0;
                //    //string suffix = string.Empty;
                //    StringBuilder dateformat = new StringBuilder();
                //    StringBuilder format = new StringBuilder();
                //    foreach (DataRow dr in rows)
                //    {
                //        module = dr["ModuleId"].ToString();
                //        switch (module)
                //        {
                //            case "yy":
                //            case "yyyy":
                //            case "MM":
                //            case "dd":
                //                dateformat.Append(module);
                //                format.Append(module);
                //                break;
                //            case "prefix":
                //            case "suffix":
                //                format.Append(dr["FixValue"].ToString());
                //                break;
                //            case "serial":
                //                format.Append("{" + index + "}");
                //                index++;
                //                serialen = Convert.ToInt32(dr["SeriaLen"]);
                //                break;

                //        }
                //    }
                //    if (serialen != -1)
                //    {
                //        currserial = DateTime.Now.ToString(dateformat.ToString()) == currdate ? currserial++ : 1;
                //        //format.(format.ToString(), currserial.ToString().PadLeft(serialen, '0'));
                //    }
                //    return DateTime.Now.ToString(string.Format(format.ToString(), currserial.ToString().PadLeft(serialen, '0')));
                //}
            }
            return string.Empty;
        }
        public string GenerateNoByprogid(string progid)
        {
            DataTable dt = GetRuleDataByProgid(progid);
            dt.DefaultView.Sort = "RuleId,SeqNo";
            if (dt != null)
            {
                DataRow[] rows = dt.Select(string.Format("IsDefault={0}", true));
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
                string currdate = rows[0]["CurrDate"].ToString();
                int currserial = Convert.ToInt32(rows[0]["CurrSerial"]);
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
                SQLBuilder builder = new SQLBuilder("CodeRuleConfig");
                string sql= builder.GetUpdateSQL("CodeRuleConfig", builder.UpdateField("CurrDate={0},CurrSerial={1}", DateTime.Now.ToString(dateformat.ToString()), currserial), 
                                                       builder.Where("ProgId={0} and RuleId={1}", rows[0]["ProgId"].ToString(), rows[0]["RuleId"].ToString()));
                this.DataAccess.ExecuteNonQuery(sql);
                return DateTime.Now.ToString(string.Format(format.ToString(), currserial.ToString().PadLeft(serialen, '0')));
            }
            return string.Empty;
        }
        #endregion
    }
}
