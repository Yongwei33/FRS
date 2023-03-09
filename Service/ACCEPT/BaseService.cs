using System.Configuration;
using System.Data.SqlClient;

namespace FRS.Lib.Service.ACCEPT
{
    public class BaseService
    {
        protected string dbConnStr = string.Empty;

        public BaseService()
        {
            // UOF MSSQL Connection String
            this.dbConnStr = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(this.dbConnStr);
        }      


    }
}
