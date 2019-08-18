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

        public DataTable InternalSearch(string tbnm, string[] fields,List<LibSearchCondition> conds)
        {
            object[] values = { };
            StringBuilder whereformat = new StringBuilder();
            AnalyzeSearchCondition(conds, whereformat,ref values);
            string sql = this.SQLBuilder.GetSQL(tbnm, fields, new WhereObject { WhereFormat = whereformat.ToString (), Values = values });
            return this.DataAccess.GetDataTable(sql);
            //return null;
        }
        public DataTable InternalSearchByPage(string tbnm, string[] fields, List<LibSearchCondition> conds, int pageindex, int pagesize)
        {
            object[] values = { };
            StringBuilder whereformat = new StringBuilder();
            AnalyzeSearchCondition(conds, whereformat,ref values);
            string sql = this.SQLBuilder.GetSQLByPage(tbnm, fields, new WhereObject { WhereFormat = whereformat.ToString(), Values = values },pageindex,pagesize);
            return this.DataAccess.GetDataTable(sql);
        }

        public DataTable[] InternalFillData(string whereformat,object[] valus )
        {
            StringBuilder sql = new StringBuilder();
            DataTable[] dts = { };
            //DataTable mdt = null;
            //Dictionary<int, int> dic = new Dictionary<int, int>();
            SDPCRL.DAL.COM.SQLBuilder sQLBuilder = new SDPCRL.DAL.COM.SQLBuilder();
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
                        sql.Append(sQLBuilder.GetSQL(dt.TableName, null, new WhereObject { WhereFormat=whereformat,Values=valus }));
                        sql.AppendLine();
                    }
                }
            }
            this.DataAccess.GetDatatTables(sql.ToString(), ref dts);
            return dts;
           
        }

        #region 私有函数
        private void AnalyzeSearchCondition(List<LibSearchCondition> conds, StringBuilder whereformat,ref object[] values)
        {
            int n = 0;
            LibSearchCondition precond=null;
            int len = 0;
            foreach (LibSearchCondition item in conds)
            {
                if (whereformat.Length > 0)
                {
                    if (precond != null)
                        whereformat.AppendFormat(" {0} ", precond.Logic.ToString());
                }
                switch (item.Symbol)
                {
                    case SmodalSymbol.Equal:
                        whereformat.Append("" + item.FieldNm + "={" + n + "}");
                        len = 1;
                        break;
                    case SmodalSymbol.MoreThan:
                        whereformat.Append("" + item.FieldNm + ">{" + n + "}");
                        len = 1;
                        break;
                    case SmodalSymbol.LessThan:
                        whereformat.Append("" + item.FieldNm + "<{" + n + "}");
                        len = 1;
                        break;
                    case SmodalSymbol.Contains:
                        whereformat.Append("" + item.FieldNm + " like {" + n + "}");
                        item.Values[0] = string.Format("%{0}%", item.Values[0]);
                        len = 1;
                        break;
                    case SmodalSymbol.Between:
                        whereformat.Append("" + item.FieldNm + " between {" + n + "} and {" +(n=n+1) + "}");
                        len = 2;
                        break;
                    case SmodalSymbol.NoEqual:
                        whereformat.Append("" + item.FieldNm + "!={" + n + "}");
                        len = 1;
                        break;
                }
                n++;
                if (item.Values != null)
                {
                    for(int i=0;i<len;i++)
                    {
                        //if (LibSysUtils.IsNULLOrEmpty(o)) continue;
                        Array.Resize(ref values, values.Length + 1);
                        values[values.Length - 1] =item .Values[i];
                    }
                }
                precond = item;
            }
        }
        #endregion
    }
}
