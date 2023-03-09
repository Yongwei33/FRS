using Ede.Uof.Utility.Log;
using FRS.Lib.Model.AP;
using FRS.Lib.Repository.AP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Service.AP
{
    public class APService : BaseService
    {
        /// <summary>
        /// 關鍵字搜尋供應商
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<BusinessPartners> QueryBPByKeyworddService(string keyword)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();

                    APRepository repo = new APRepository();
                    List<BusinessPartners> list = repo.QueryBPByKeyworddRepository(conn, keyword);
                    
                    return list;
                }

            }
            catch (Exception ex)
            {
                Logger.Write(this.ProjName + "_Errorlog", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 取得供應商-銀行代碼
        /// </summary>
        /// <param name="cardCode"></param>
        /// <returns></returns>
        public List<VM_BusinessPartner> QueryBPAcctByCardCodedService(string cardCode)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();

                    APRepository repo = new APRepository();
                    List<VM_BusinessPartner> list = repo.QueryBPAcctByCardCodedRepository(conn, cardCode);
                    
                    return list;

                }

            }
            catch (Exception ex)
            {
                Logger.Write(this.ProjName + "_Errorlog", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 取得費用類別主檔
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<GLAccounts> QueryGLAccountByKeywordService(string keyword)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();

                    APRepository repo = new APRepository();
                    List<GLAccounts> list = repo.QueryGLAccountByKeywordRepository(conn, keyword);

                    return list;
                }

            }
            catch (Exception ex)
            {
                Logger.Write(this.ProjName + "_Errorlog", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 取得APInvoice Header
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<APInvoiceHeader> QueryAPInvoiceService()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();

                    APRepository repo = new APRepository();
                    List<APInvoiceHeader> list = repo.QueryAPInvoiceRepository(conn);

                    return list;
                }

            }
            catch (Exception ex)
            {
                Logger.Write(this.ProjName + "_Errorlog", ex.ToString());
                throw;
            }
        }

        public int InsertAPInvoiceService(APInvoiceHeader header, List<APInvoiceRow> lstRows)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        APRepository repo = new APRepository();
                        int result = repo.InsertAPInvoiceRepository(conn, tx, header, lstRows);

                        tx.Commit();

                        return result;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        Logger.Write(this.ProjName + "_Errorlog", ex.ToString());
                        throw;
                    }
                }
            }

        }

        public int DeleteAPInvoiceService(string BPMKey)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        APRepository repo = new APRepository();
                        int result = repo.DeleteAPInvoiceRepository(conn, tx, BPMKey);

                        tx.Commit();

                        return result;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        Logger.Write(this.ProjName + "_Errorlog", ex.ToString());
                        throw;
                    }
                }
            }

        }

    }
}
