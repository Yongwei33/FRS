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

namespace FRS.Lib.Service.REQ
{
    public class REQService : BaseService
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

        public string GetGroupId(string USER_GUID)
        {
            string sSql = @"SELECT GROUP_ID FROM TB_EB_EMPL_DEP WHERE USER_GUID = @USER_GUID AND ORDERS = 0";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { USER_GUID }).FirstOrDefault();
            }
            return item;
        }

        public string GetGroupName(string GROUP_ID)
        {
            string sSql = @"SELECT GROUP_NAME FROM TB_EB_GROUP WHERE GROUP_ID = @GROUP_ID";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { GROUP_ID }).FirstOrDefault();
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

        public List<string> GetLstFUNCID(string USER_GUID)
        {
            using (var cnn = GetConnection())
            {
                List<string> listResult = new List<string>();
                try
                {
                    cnn.Open();
                    string sSql = @"SELECT FUNC_ID FROM TB_EB_EMPL_FUNC WHERE USER_GUID= @USER_GUID";
                    listResult = cnn.Query<string>(sSql, new { USER_GUID }).ToList();
                }
                catch
                {
                    throw;
                }
                return listResult;
            }
        }

        public string GetFUNCNAME(string FUNC_ID)
        {
            string sSql = @"SELECT FUNC_NAME FROM TB_EB_JOB_FUNC WHERE FUNC_ID= @FUNC_ID";
            string item = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                item = conn.Query<string>(sSql, new { FUNC_ID }).FirstOrDefault();
            }
            return item;
        }

        public List<string> GetLstPO()
        {
            using (var cnn = GetConnection())
            {
                string FUNC_NAME = "行銷採購%";
                List<string> listResult = new List<string>();
                try
                {
                    cnn.Open();
                    string sSql = @"SELECT FUNC_ID FROM TB_EB_JOB_FUNC WHERE FUNC_NAME like @FUNC_NAME";
                    listResult = cnn.Query<string>(sSql, new { FUNC_NAME }).ToList();
                }
                catch
                {
                    throw;
                }
                return listResult;
            }
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
