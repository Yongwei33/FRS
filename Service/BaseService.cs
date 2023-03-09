using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Service
{
    public class BaseService
    {
        protected string ProjName = string.Empty;
        protected string dbConnStr = string.Empty;
        public BaseService()
        {
            this.ProjName = ConfigurationManager.AppSettings["ProjectName"];
            this.dbConnStr = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(this.dbConnStr);
        }
    }
}
