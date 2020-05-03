using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWYSDPWeb.BllComController
{
    public class exportTestController : ComController
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            var mtb = this.LibTables[0].Tables[0];
            var detail = this.LibTables[0].Tables[1];
            dynamic mrow = mtb.Rows[0];
            //dynamic mrow = mtb.NewRow();
            mrow.matid = "matid";
            mrow.plantId = "1000";
            mrow.staffid = "staffid";
            mrow.Hetong = "hetong2";
            mrow.address = "fujianshenglonghai";
            mrow.lianxiren = "13526253664";
            mrow.Field8 = "Field8";
            mrow.Field9 = "Field9";
            mrow.Field10 = "Field10";
            mrow.Field11 = "Field11";
            mrow.Field12 = "Field12";
            mrow.Field13 = "Field13";
            mrow.Field14 = "Field14";
            mrow.Field15 = "Field15";
            mrow.Field16 = "Field16";
            mrow.Field17 = "Field17";
            mrow.Field18 = "Field18";
            mrow.Field19 = "Field19";
            mrow.Field20 = "Field20";
            mrow.billstatu = 2;

            for (int i = 0; i < 10000; i++)
            {
                dynamic drow = detail.NewRow();
                drow.matid = "matidzyy"+i.ToString ();
                drow.rowstatu = 1;
                drow.DField5 = "Field5" + i.ToString();
                drow.DField6 = "Field6" + i.ToString();
                drow.DField7 = "Field7" + i.ToString();
                drow.DField8 = "Field8" + i.ToString();
                drow.DField9 = "Field9" + i.ToString();
                drow.DField10 = "Field10" + i.ToString();
                drow.DField11 = "Field11" + i.ToString();
                drow.DField12 = "Field12" + i.ToString();
                drow.DField13 = "Field13" + i.ToString();
                drow.DField14 = "Field14" + i.ToString();
                drow.DField15 = "Field15" + i.ToString();
                drow.DField16 = "Field16" + i.ToString();
                drow.DField17 = "Field17" + i.ToString();
                drow.DField18 = "Field18" + i.ToString();
                drow.DField19 = "Field19" + i.ToString();
            }
        }
    }
}