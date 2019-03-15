﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public class SQLite
    {
        string connectStr = "Data Source=|DataDirectory|ServerInfo.db;providerName=\"System.Data.SQLite\"; Pooling=true;FailIfMissing=false";
        public SQLite()
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectStr))
            {
                //在打开数据库时，会判断数据库是否存在，如果不存在，则在当前目录下创建一个 
                try
                {
                    cn.Open();
                }
                catch (Exception ex) {
                }
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = cn;
                    try
                    {
                        //判断表是否存在
                        cmd.CommandText = "select * from sqlite_master where type = 'table' and name = 'ServerInfo'";
                        object exits = cmd.ExecuteScalar();
                        if (exits == null)
                        {
                            //建立表，如果表已经存在，则报错 
                            cmd.CommandText = "CREATE TABLE [ServerInfo] (" +
                                " ipAddress nvarchar(50)," +
                                "connectype nvarchar(15)," +
                                "serverNm nvarchar(50) primary key," +
                                "point int," +
                                "accountid nvarchar(36)," +
                                "accountname nvarchar(30)," +
                                "IsCurrentServer bit)";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public bool Insert(ServerInfo info)
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectStr))
            {
                //在打开数据库时，会判断数据库是否存在，如果不存在，则在当前目录下创建一个 
                cn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = cn;
                    try
                    {
                        cmd.CommandText = string.Format("insert into ServerInfo values('{0}','{1}','{2}',{3},'{4}','{5}','{6}')",
                            info.ipAddress,
                            info.connectype,
                            info.serverNm,
                            info.point,
                            info.accountid,
                            info.accountname,
                            info.IsCurrentServer);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }

        public List<ServerInfo> SelectAllServer()
        {
            List<ServerInfo> result = new List<ServerInfo>();
            using (SQLiteConnection cn = new SQLiteConnection(connectStr))
            {
                cn.Open();
                ServerInfo info = null;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = cn;
                    try
                    {
                        cmd.CommandText = string.Format("Select *from ServerInfo");
                        using (SQLiteDataReader read = cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                info = new ServerInfo();
                                info.accountid = read["accountid"].ToString();
                                info.accountname = read["accountname"].ToString();
                                info.connectype = read["connectype"].ToString();
                                info.ipAddress = read["ipAddress"].ToString();
                                info.serverNm = read["serverNm"].ToString();
                                info.IsCurrentServer = (bool)read["IsCurrentServer"];
                                info.point = Convert.ToInt32(read["point"]);
                                result.Add(info);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return result;
        }

        public ServerInfo GetCurrentServer()
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectStr))
            {
                //在打开数据库时，会判断数据库是否存在，如果不存在，则在当前目录下创建一个 
                cn.Open();
                ServerInfo info = null;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = cn;
                    try
                    {
                        cmd.CommandText = string.Format("Select *from ServerInfo where IsCurrentServer='True'");
                        using (SQLiteDataReader read = cmd.ExecuteReader())
                        {
                            if (read.Read())
                            {
                                info = new ServerInfo();
                                info.accountid = read["accountid"].ToString();
                                info.accountname = read["accountname"].ToString();
                                info.connectype = read["connectype"].ToString();
                                info.ipAddress = read["ipAddress"].ToString();
                                info.serverNm = read["serverNm"].ToString();
                                info.IsCurrentServer = (bool)read["IsCurrentServer"];
                                info.point = Convert.ToInt32(read["point"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return info;
            }
        }

        public bool Update(ServerInfo info)
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectStr))
            {
                //在打开数据库时，会判断数据库是否存在，如果不存在，则在当前目录下创建一个 
                cn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = cn;
                    try
                    {
                        cmd.CommandText = string.Format("update ServerInfo set ipAddress='{0}',connectype='{1}',accountid='{2}',point={3},accountname='{4}',IsCurrentServer='{5}' where serverNm='{6}'",
                            info.ipAddress,
                            info.connectype,
                            info.accountid,
                            info.point,
                            info.accountname,
                            info.IsCurrentServer,
                            info.serverNm);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }

        public bool Delete(ServerInfo info)
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectStr))
            {
                //在打开数据库时，会判断数据库是否存在，如果不存在，则在当前目录下创建一个 
                cn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = cn;
                    try
                    {
                        cmd.CommandText = string.Format("delete from ServerInfo  where serverNm='{0}'", info.serverNm);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }
    }

    public class ServerInfo
    {
        public string ipAddress { get; set; }
        public string connectype { get; set; }
        public string serverNm { get; set; }
        public int point { get; set; }
        public string accountid { get; set; }
        public string accountname { get; set; }
        public bool IsCurrentServer { get; set; }

        public override string ToString()
        {
            return string.Format("服务名：{0} IP地址：{1} 端口：{2} 账套：{3} 是否当前链接：{4}", serverNm, ipAddress, point, accountname, IsCurrentServer);
        }
    }
}
