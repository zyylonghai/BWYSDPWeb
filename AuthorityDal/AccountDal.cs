using SDPCRL.COM;
using SDPCRL.CORE;
using SDPCRL.DAL.COM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorityDal
{
    public class AccountDal:AuthorityDal
    {
        string _pwdkeyEncrykey = "bwyAccount";
        protected override void BeforeUpdate()
        {
            base.BeforeUpdate();
            #region 产生密码秘钥并加密密码
            //string pwd = this.LibTables[0].Tables[0].DataTable .Rows[0]["Password"].ToString();
            //string pwdkey = DesCryptFactory.GenerateKey();
            //pwd = DesCryptFactory.EncryptString(pwd, pwdkey);
            //this.LibTables[0].Tables[0].DataTable .Rows[0]["Password"] = pwd;
            //this.LibTables[0].Tables[0].DataTable .Rows[0]["PasswordKey"] = DesCryptFactory.AESEncrypt(pwdkey, _pwdkeyEncrykey);
            var firstrow = this.LibTables[0].Tables[0].FindRow(0);
            string pwd = firstrow.Password;
            string pwdkey = DesCryptFactory.GenerateKey();
            pwd = DesCryptFactory.EncryptString(pwd, pwdkey);
            firstrow.Password = pwd;
            firstrow.PasswordKey = DesCryptFactory.AESEncrypt(pwdkey, _pwdkeyEncrykey);
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns>返回1表示登录成功，2表示已登录，3表示密码错误,0表示登录失败</returns>
        public int Login(string userid,string password)
        {
            //SQLBuilder builder = new SQLBuilder("Account");
            //string sql = builder.GetSQL("Account", new string[] { "A.UserId,A.Password,A.PasswordKey,A.loginIP,A.LoginDT,A.IsLogin" }, builder.Where("A.UserId={0}", userid));
            //DataRow row = this.DataAccess.GetDataRow(sql);
            LibTableObj account = this.DSContext["Account"];
            
            this.DataAccess.FillTableObj(account.Where(account.Columns.UserId + "={0}",  userid));
            dynamic row = account.FindRow(0);
            if (row != null)
            {
                if (row.IsLogin)
                {
                    //this.AddMessage(string.Format(this.GetMessageDesc("msg000000001")), LibMessageType.Prompt);
                    //return 2;
                }
                string pwd = row.Password;
                string pwdkey = row.PasswordKey;
                pwdkey = DesCryptFactory.AESDecrypt(pwdkey, _pwdkeyEncrykey);
                pwd = DesCryptFactory.DecryptString(pwd, pwdkey);
                //this.AddMessage("test", LibMessageType.Error);
                if (pwd == password)
                {
                    //sql = builder.GetUpdateSQL("Account", builder.UpdateField("IsLogin={0},loginIP={1}", true, "192.168.1.5"), builder.Where("UserId={0}", userid));
                    //int result = this.DataAccess.ExecuteNonQuery(sql);
                    //return result > 0 ? 1 : 0;
                }
                else
                    return 3;
                //return pwd == password;
            }
            return 1;
        }
    }
}
