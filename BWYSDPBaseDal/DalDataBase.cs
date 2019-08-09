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
            this.AddErrorMessage("AfterUpdate has do run error",LibMessageType.Warning);
            base.AfterUpdate();
        }

        public DataTable InternalSearch(string tbnm, string[] fields,List<LibSearchCondition> conds)
        {
            object[] values = { };
            StringBuilder whereformat = new StringBuilder();
            int n = 0;
            foreach (LibSearchCondition item in conds)
            {
                if (whereformat.Length > 0)
                {
                    whereformat.AppendFormat(" {0} ", item.Logic .ToString());
                    //switch (logic)
                    //{
                    //    case Smodallogic.And:

                    //        break;
                    //    case Smodallogic.Or:
                    //        break;
                    //}
                }
                switch (item.Symbol)
                {
                    case SmodalSymbol.Equal:
                        whereformat.Append("" + item.FieldNm + "={" + n + "}");
                        Array.Resize(ref values, values.Length + 1);
                        values[values.Length - 1] = item.Values[0];
                        break;
                    case SmodalSymbol.MoreThan:
                        whereformat.Append("" + item.FieldNm + ">{" + n + "}");
                        break;
                    case SmodalSymbol.LessThan:
                        whereformat.Append("" + item.FieldNm + "<{" + n + "}");
                        break;
                    case SmodalSymbol.Contains:
                        whereformat.Append("" + item.FieldNm + " like (%{" + n + "}%)");
                        break;
                    case SmodalSymbol.Between:
                        whereformat.Append("" + item.FieldNm + "between {" + n + "} and {" + (n++) + "}");
                        break;
                    case SmodalSymbol.NoEqual:
                        whereformat.Append("" + item.FieldNm + "!={" + n + "}");
                        break;
                }
                n++;
                if (item.Values != null)
                {
                    foreach (object o in item.Values)
                    {
                        if (LibSysUtils.IsNULLOrEmpty(o)) continue;
                        Array.Resize(ref values, values.Length + 1);
                        values[values.Length - 1] = o;
                    }
                }
            }
            string sql = this.SQLBuilder.GetSQL(tbnm, fields, new WhereObject { WhereFormat = whereformat.ToString (), Values = values });
            return this.DataAccess.GetDataTable(sql);
            //return null;
        }
    }
}
