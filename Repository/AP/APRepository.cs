using Dapper;
using FRS.Lib.Model.AP;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Repository.AP
{
    public class APRepository
    {

        /// <summary>
        /// 關鍵字搜尋供應商
        /// </summary>
        /// <param name="keyword">供應商代碼或名稱關鍵字</param>
        /// <returns></returns>
        public List<BusinessPartners> QueryBPByKeyworddRepository(SqlConnection conn, string keyword)
        {
            try
            {
                string sql = @"SELECT CardCode, CardName
                                FROM BusinessPartners
                                WHERE 1=1 ";

                DynamicParameters dynPara = new DynamicParameters();
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql += @" AND (CardCode LIKE @KEYWORD OR CardName LIKE @KEYWORD) ";
                    dynPara.Add("KEYWORD", $"%{keyword}%");           
                }
                sql += "ORDER BY CardCode";
                                
                List<BusinessPartners> lstBP = conn.Query<BusinessPartners>(sql, dynPara).ToList();
                return lstBP;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 取得供應商-銀行代碼
        /// </summary>
        /// <param name="cardCode">供應商代碼</param>
        /// <returns></returns>
        public List<VM_BusinessPartner> QueryBPAcctByCardCodedRepository(SqlConnection conn, string cardCode)
        {
            string sql = @" SELECT bp.CardCode, bp.CardName, (bpb.BankCode + bpb.Branch) as DispCode, (ISNULL(bpb.BankName,'') + ISNULL(bpb.BranchName,'')) as DispName, bpb.AcctName, bpb.Account as BnkAccount, bpb.BankCode, bpb.Branch as BnkBranch
FROM BusinessPartnersBank bpb LEFT JOIN BusinessPartners bp ON bpb.CardCode = bp.CardCode WHERE bpb.CardCode = @CardCode ";
            try
            {
                List<VM_BusinessPartner> lstVMBP = conn.Query<VM_BusinessPartner>(sql, new { CardCode = cardCode }).ToList();
                return lstVMBP;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 取得費用類別主檔
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<GLAccounts> QueryGLAccountByKeywordRepository(SqlConnection conn, string keyword)
        {
            string sql = @"SELECT AcctCode, AcctName, CreateDate, UpdateDate
                            FROM GLAccount
                            WHERE 1=1";
                                                      
            DynamicParameters dynPara = new DynamicParameters();
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += @" AND (AcctCode LIKE @KEYWORD OR AcctName LIKE @KEYWORD) ";
                dynPara.Add("KEYWORD", $"%{keyword}%");
            }
            sql += @" ORDER BY AcctCode ";

            try
            {
                List<GLAccounts> lstAcct = conn.Query<GLAccounts>(sql, dynPara).ToList();
                return lstAcct;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 取得APInvoice Header
        /// </summary>
        /// <returns></returns>
        public List<APInvoiceHeader> QueryAPInvoiceRepository(SqlConnection conn)
        {
            string sql = @" SELECT * FROM APInvoiceHeader WHERE Flag = '2' ";
            try
            {
                List<APInvoiceHeader> lstAP = conn.Query<APInvoiceHeader>(sql).ToList();
                return lstAP;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 新增APInvoice Header及Row
        /// </summary>
        /// <param name="header"></param>
        /// <param name="lstRows"></param>
        /// <returns></returns>
        public int InsertAPInvoiceRepository(SqlConnection conn, SqlTransaction tx, APInvoiceHeader header, List<APInvoiceRow> lstRows)
        {
            string sqlHeader = @"INSERT INTO APInvoiceHeader 
                                    (BPMKey, CardCode, CardName, DocDate, TaxDate, 
                                    JrnlMemo, [RowCount], Flag, DocEntry, DocNum, 
                                    ErrorMsg, CreateDate, UpdateDate, BankCode, BnkBranch, BnkAccount) 
                                VALUES (@BPMKey, @CardCode, @CardName, @DocDate, @TaxDate, 
                                    @JrnlMemo, @RowCount, @Flag, @DocEntry, @DocNum, 
                                    @ErrorMsg, @CreateDate, @UpdateDate, @BankCode, @BnkBranch, @BnkAccount)" ;

            string sqlRow = @"INSERT INTO APInvoiceRow 
                                    (BPMKey, LineID, AcctCode, Price, CreateDate, UpdateDate) 
                              VALUES (@BPMKey, @LineID, @AcctCode, @Price, @CreateDate, @UpdateDate) ";


            try
            {
                int result = conn.Execute(sqlHeader, header, tx);
                if (result > 0)
                {
                    foreach (var row in lstRows)
                    {
                        result = conn.Execute(sqlRow, row, tx);
                        if (result == 0)
                        {
                            throw new Exception($"新增費用明細第{row.LineID + 1}筆發生錯誤");
                        }
                    }

                    return result;
                }
                else
                {
                    throw new Exception($"新增表頭發生錯誤");
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除APInvoice Header及Row
        /// </summary>
        /// <param name="header"></param>
        /// <param name="lstRows"></param>
        /// <returns></returns>
        public int DeleteAPInvoiceRepository(SqlConnection conn, SqlTransaction tx, string BPMKey)
        {
            string sqlHeader = @"DELETE FROM APInvoiceHeader WHERE BPMKey=@BPMKey";

            string sqlRow = @"DELETE FROM APInvoiceRow WHERE BPMKey=@BPMKey";


            try
            {
                int result = conn.Execute(sqlHeader, BPMKey, tx);
                if (result > 0)
                {
                    result = conn.Execute(sqlRow, BPMKey, tx);
                    if (result == 0)
                    {
                        throw new Exception($"刪除費用明細發生錯誤");
                    }
                    return result;
                }
                else
                {
                    throw new Exception($"刪除表頭發生錯誤");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}