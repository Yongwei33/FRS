using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using FRS.Lib.Service.REQ;
using FRS.Lib.Model;

namespace FRS.Lib.Service.ACCEPT
{
    public class ACCEPTService : BaseService
    {
        public string GetUserId(string ACCOUNT)
        {
            string sSql = @"SELECT USER_GUID FROM TB_EB_USER WHERE ACCOUNT= @ACCOUNT";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { ACCOUNT }).FirstOrDefault();
            }
            return item;
        }

        public string GetTaskId(string formNo)
        {
            string sSql = @"SELECT TASK_ID FROM TB_WKF_TASK WHERE DOC_NBR= @formNo";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { formNo }).FirstOrDefault();
            }
            return item;
        }

        public string GetUSINGVERSIONID(string FORM_NAME)
        {
            string sSql = @"SELECT USING_VERSION_ID FROM TB_WKF_FORM WHERE FORM_NAME= @FORM_NAME";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { FORM_NAME }).FirstOrDefault();
            }
            return item;
        }

        public bool SaveTaskData(string formXml)
        {
            string sSql = @"INSERT INTO [dbo].[TB_WKF_EXTERNAL_TASK] ([EXTERNAL_TASK_ID], [FORM_INFO], [STATUS]) VALUES (newid(), @formXml, '2')";
            bool result = false;
            using (var conn = GetConnection())
            {
                conn.Open();
                var tx = conn.BeginTransaction();
                try
                {
                    conn.Execute(sSql, new { formXml }, transaction: tx);
                    result = true;
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    throw ex;
                }
            }
            return result;
        }

    }
}
