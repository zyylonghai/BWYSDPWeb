using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public class TempHelp
    {
        private static SqlConnection _cn = null;
        //private static SQLiteConnection _cn = null;
        private static readonly object locker = new object();
        private static readonly object locker2 = new object();
        private static readonly object locker3 = new object();
        public static string ConnectStr
        {
            get; set;
        }

        public TempHelp(string dbNm)
        {
            ConnectStr = string.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=True;Min Pool Size=2;Max Pool Size=100;App=Index Web ERP;AttachDbFilename=|DataDirectory|\{0}.mdf;", dbNm);
        }

        public void Update(List<string> commandtextlst)
        {
            using (SqlConnection cn = new SqlConnection(ConnectStr))
            {
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                //StringBuilder str = new StringBuilder();
                //str.Append("begin;");
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.Transaction = transaction;
                    try
                    {
                        foreach (string command in commandtextlst)
                        {
                            cmd.CommandText = command;
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                    //cmd.Connection.Close();
                }

            }

        }

        public void Delete(string commandtext)
        {
            using (SqlConnection cn = new SqlConnection(ConnectStr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = commandtext;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                    //cmd.Connection.Close();
                }
            }
        }

        public Dictionary<string, List<string>> SelectFormfield(string progid)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            List<string> values = null;
            using (SqlConnection cn = new SqlConnection(ConnectStr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    try
                    {
                        cmd.CommandText = string.Format("select tableNm,fieldNm from formfields where progid='{0}'", progid);
                        using (SqlDataReader read = cmd.ExecuteReader())
                        {
                            string tbnm = null;
                            while (read.Read())
                            {
                                tbnm = read["tableNm"].ToString();
                                if (!dic.TryGetValue(tbnm, out values))
                                {
                                    values = new List<string>();
                                    dic.Add(tbnm, values);
                                }
                                values.Add(read["fieldNm"].ToString());

                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    //cmd.Connection.Close();
                }
            }
            return dic;
        }

        public void ClearTempData(string sessionid, string progid)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@sessionid",System.Data.SqlDbType.VarChar),
                new SqlParameter("@progid",System.Data.SqlDbType.VarChar)
            };
            parameters[0].Value = sessionid;
            parameters[1].Value = progid;
            using (SqlConnection cn = new SqlConnection(ConnectStr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "p_cleartemp";
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                    //cmd.Connection.Close();
                }
            }
        }

        public void ClearTempBysessionid(string sessionid)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@sessionid",System.Data.SqlDbType.VarChar)
            };
            parameters[0].Value = sessionid;
            using (SqlConnection cn = new SqlConnection(ConnectStr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "p_clearSessionData";
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                    //cmd.Connection.Close();
                }
            }
        }

        public DataTable GetTempData(string sessionid, string progid, string tbNm, ref int rowcout)
        {
            DataTable dt = new DataTable();
            SqlParameter[] parameters = {
                new SqlParameter("@sessionid",System.Data.SqlDbType.VarChar),
                new SqlParameter("@progid",System.Data.SqlDbType.VarChar),
                new SqlParameter("@tbNm",System.Data.SqlDbType.VarChar),
                new SqlParameter ("@rowCout",System.Data.SqlDbType .Int)
            };
            parameters[0].Value = sessionid;
            parameters[1].Value = progid;
            parameters[2].Value = tbNm;
            parameters[3].Direction = ParameterDirection.Output;
            using (SqlConnection cn = new SqlConnection(ConnectStr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "p_Gettempdata";
                        cmd.Parameters.AddRange(parameters);
                        SqlDataAdapter dbAdapter = new SqlDataAdapter(cmd);
                        dbAdapter.Fill(dt);
                        rowcout = (int)parameters[3].Value;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return dt;
        }

        public void SqlBulkUpdate(DataTable dt)
        {
            using (SqlConnection cn = new SqlConnection(ConnectStr))
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {


                    SqlBulkCopy bulkCopy = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, transaction);

                    bulkCopy.DestinationTableName = "temp";
                    bulkCopy.BatchSize = dt.Rows.Count;
                    try
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            bulkCopy.WriteToServer(dt);
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

    }
}
