using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;



namespace POS_MANAGEMENT_SYSTEM___Cashier__
{
    public class SqlHelper
    {
        SqlConnection cn;

       public SqlHelper(string connectionstring)
        {
            cn = new SqlConnection(connectionstring);
        }

        public bool isConnection
        {
            get
            {
                try
                {
                    if (cn.State == System.Data.ConnectionState.Closed)
                    {
                        cn.Open();
                    }

                    cn.Close();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

    }

}
