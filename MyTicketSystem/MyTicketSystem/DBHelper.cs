using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MyTicketSystem
{
    class DBHelper
    {
        //数据库链接字符串
        private string connString = @"Data Source =(local);Initial Catalog=MyTicket;User Id=sa;Pwd=123456";
        private SqlConnection connecion;

        public SqlConnection Connecion
        {
            get
            {
                if(connecion==null)
                {
                    connecion = new SqlConnection(connString);
                }
                return connecion;
            }
        }
        /// <summary>
        /// 打开数据库
        /// </summary>
        public void  OpenConnection()
        {
            if(connecion.State==ConnectionState.Closed)//当数据库在关闭的时候就打开数据库
            {
                connecion.Open();
            }
            else if(connecion.State==ConnectionState.Broken)//当数据库链接中断
            {
                connecion.Close();
                connecion.Open();
            }
        }
        /// <summary>
        /// 打开数据库链接
        /// </summary>
        public void CloseConnection()
        {
            if(connecion.State==ConnectionState.Open||connecion.State==ConnectionState.Broken)
            {
                connecion.Close();
            }
        }
    }
}
