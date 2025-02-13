using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace POS_MANAGEMENT_SYSTEM___Cashier__
{
   public class AppSetting
    {
        Configuration config;

        public AppSetting()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
        public string Getconnetionstring(string key)
        {
            return config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
        }

        public void SaveConnectionstring(string key,string Value)
        {
            config.ConnectionStrings.ConnectionStrings[key].ConnectionString = Value;

            config.ConnectionStrings.ConnectionStrings[key].ProviderName = "System.Data.SqlClient";

            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
