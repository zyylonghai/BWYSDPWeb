﻿using BWYSDPBaseDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDPCRL.COM;

namespace ComDal
{
    public class CheckBillDal: DalDataBase
    {
        public CheckBillDal()
        {
            
        }

        protected override void BeforeUpdate()
        {
            this.AddErrorMessage("BeforeUpdate  error test");
            base.BeforeUpdate();
        }


        //public override void Save(LibTable[] libtables)
        //{
        //    this.AddErrorMessage("error test");
        //    base.Save(libtables);
        //}

        public string Test(string a,int b)
        {
            return string.Format("{0},{1},{2}", "zyy",a,b);
        }
    }
}