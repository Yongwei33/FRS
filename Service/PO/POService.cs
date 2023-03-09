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

namespace FRS.Lib.Service.PO
{
    public class POService : BaseService
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

        public string GetUserIdFUNC(string FUNC_ID)
        {
            string sSql = @"SELECT USER_GUID FROM TB_EB_EMPL_FUNC WHERE FUNC_ID= @FUNC_ID";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { FUNC_ID }).FirstOrDefault();
            }
            return item;
        }

        public string GetUserAccount(string USER_GUID)
        {
            string sSql = @"SELECT ACCOUNT FROM TB_EB_USER WHERE USER_GUID= @USER_GUID";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { USER_GUID }).FirstOrDefault();
            }
            return item;
        }

        public string GetUserName(string USER_GUID)
        {
            string sSql = @"SELECT NAME FROM TB_EB_USER WHERE USER_GUID= @USER_GUID";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { USER_GUID }).FirstOrDefault();
            }
            return item;
        }

        public string GetFUNCID(string FUNC_NAME)
        {
            string sSql = @"SELECT FUNC_ID FROM TB_EB_JOB_FUNC WHERE FUNC_NAME= @FUNC_NAME";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { FUNC_NAME }).FirstOrDefault();
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
