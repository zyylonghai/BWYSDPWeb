using BWYSDPBaseDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDPCRL.COM;
using SDPCRL.CORE;

namespace ComDal
{
    public class CheckBillDal: ComDal
    {
        public CheckBillDal()
        {
            
        }

        protected override void BeforeUpdate()
        {
            this.AddMessage("BeforeUpdate  error test",LibMessageType.Warning);
            base.BeforeUpdate();
        }
        protected override void AfterUpdate()
        {
            base.AfterUpdate();
            //this.AddMessage("error test", LibMessageType.Error);
        }


        //public override void Save(LibTable[] libtables)
        //{
        //    //int a =0;
        //    //int b = 8 / a;
        //    this.AddErrorMessage("error test",LibMessageType.Warning);
        //    base.Save(libtables);
        //}

        public string Test(string a,int b)
        {
            return string.Format("{0},{1},{2}", "zyy",a,b);
        }
    }
}
