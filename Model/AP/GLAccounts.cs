using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Model.AP
{
    public class GLAccounts
    {
        public string AcctCode { get; set; } //費用類別代碼
        public string AcctName { get; set; } //費用類別名稱
        public DateTime CreateDate { get; set; } //建立時間
        public DateTime UpdateDate { get; set; } //修改時間

    }
}
