using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Model.AP
{
    public class BaseBusinessPartners
    {
        public string CardCode { get; set; } //業務夥伴代碼
        public DateTime CreateDate { get; set; } //建立時間
        public DateTime UpdateDate { get; set; } //修改時間
    }

    public class BusinessPartners
    {
        public string CardName { get; set; } //業務夥伴名稱
        public string CardCode { get; set; } //業務夥伴代碼
        public DateTime CreateDate { get; set; } //建立時間
        public DateTime UpdateDate { get; set; } //修改時間
    }

    public class BusinessPartnersBank : BaseBusinessPartners
    {
        public string BankCode { get; set; } //銀行代碼
        public string BankName { get; set; } //銀行名稱
        public string Account { get; set; } //銀行帳戶
        public string AcctName { get; set; } //銀行帳戶名稱
        public string Branch { get; set; } //分行代碼
        public string BranchName { get; set; } //分行名稱

    }

    public class VM_BusinessPartner : BusinessPartners
    {
        public string DispCode { get; set; } //銀行代碼 + 分行代碼
        public string DispName { get; set; } //銀行名稱 + 分行名稱
        public string AcctName { get; set; } //帳戶名稱
        public string BnkAccount { get; set; } //銀行帳戶(回寫ERP)
        public string BankCode { get; set; } //銀行代碼(回寫ERP)
        public string BnkBranch { get; set; } //分行代碼(回寫ERP)

    }
}
