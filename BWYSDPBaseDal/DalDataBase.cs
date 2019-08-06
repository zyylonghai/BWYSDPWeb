using SDPCRL.DAL.BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDPCRL.COM;
using SDPCRL.CORE;

namespace BWYSDPBaseDal
{
    public class DalDataBase : DALBase
    {
        public DalDataBase()
        {
            
        }

        protected override void AfterUpdate()
        {
            this.AddErrorMessage("AfterUpdate has do run error",LibMessageType.Warning);
            base.AfterUpdate();
        }
    }
}
