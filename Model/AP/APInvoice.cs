using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Model.AP
{
    public class APInvoiceHeader
    {
        public string BPMKey { get; set; } //BPM單據Key值
        public string CardCode{ get; set; } //供應商代碼
        public string CardName { get; set; } //供應商名稱
        public DateTime DocDate { get; set; } //入帳日期 ex:2022-10-26
        public DateTime TaxDate { get; set; } //憑證日期
        public string JrnlMemo { get; set; } //用途說明
        public int RowCount { get; set; } //Row數量
        public int Flag { get; set; } //界接狀態 0：BPM至中介DB, 1：B1新增完成, 2：B1新增異常
        public int DocEntry { get; set; } //B1 DocEntry
        public int DocNum { get; set; } //B1 DocNum
        public string BankCode { get; set; } //銀行代碼
        public string BnkAccount { get; set; } //銀行帳戶
        public string BnkBranch { get; set; } //分行代碼
        public string ErrorMsg { get; set; } //錯誤訊息
        public DateTime CreateDate { get; set; } //建立時間
        public DateTime UpdateDate { get; set; } //修改時間

    }

    public class APInvoiceRow
    {
        public string BPMKey { get; set; } //BPM單據Key值
        public int LineID { get; set; } //項次 從0開始
        public string AcctCode { get; set; } //費用類別代碼
        public decimal Price { get; set; } //價格
        public DateTime CreateDate { get; set; } //建立時間
        public DateTime UpdateDate { get; set; } //修改時間

    }
}
