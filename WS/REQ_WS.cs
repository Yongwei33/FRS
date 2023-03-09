using Ede.Uof.EIP.Organization.Util;
using FRS.Lib.Service.REQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;

namespace FRS.Lib.WS
{
    /// <summary>
    /// 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class REQ_WS : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloUOF(string formInfo)
        {
            //Logger.Write("FRS_Log", formInfo);
            return BuildResult(1, "");
        }

        [WebMethod]
        public string CheckREQ(string formInfo)
        {
            string message = string.Empty;
            try
            {
                REQService service = new REQService();

                XmlDocument rootDoc = new XmlDocument();
                rootDoc.LoadXml(formInfo);
                string strTotalPrice = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='TotalPrice']").Attributes["fieldValue"].Value;
                double totalPrice = Convert.ToDouble(strTotalPrice);
                string strPayment = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='Payment']").Attributes["fieldValue"].Value;

                if (totalPrice > 3000 && strPayment.Contains("現金"))
                    message = "總價3000元以下，預計付款方式才能選現金\r\n";

                string strCategory = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='Category']").Attributes["fieldValue"].Value;
                string strPOApplicant = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='POApplicant']").Attributes["fieldValue"].Value;
                string strPODept = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='PODept']").Attributes["fieldValue"].Value;
                string strApplicant = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='Applicant']").Attributes["fieldValue"].Value.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1];

                if (strCategory.Contains("A") && string.IsNullOrEmpty(strPOApplicant))
                    message += "請購類別A必須選擇採購人員\r\n";
                else if (strCategory.Contains("C") && string.IsNullOrEmpty(strPOApplicant))
                    message += "請購類別C必須選擇採購人員\r\n";
                else if (strCategory.Contains("A") && strPOApplicant.Contains("行銷"))
                    message += "請購類別A必須選擇行政採購人員\r\n";
                else if (strCategory.Contains("C") && strPOApplicant.Contains("行政"))
                    message += "請購類別C必須選擇行銷採購人員\r\n";
                else if (!strCategory.Contains("A") && !strCategory.Contains("C") && !string.IsNullOrEmpty(strPOApplicant))
                    message += "請購類別A和C以外不用選擇採購人員\r\n";

                if (strCategory.Contains("A") && string.IsNullOrEmpty(strPODept))
                    message += "請購類別A必須選擇採購單位";
                else if (strCategory.Contains("C") && string.IsNullOrEmpty(strPODept))
                    message += "請購類別C必須選擇採購單位";
                else if (strCategory.Contains("A") && strPODept.Contains("行銷"))
                    message += "請購類別A必須選擇採購單位為行政部";
                else if (strCategory.Contains("C") && strPODept.Contains("行政"))
                    message += "請購類別C必須選擇採購單位為行銷暨數位發展部";
                /*else if (!strCategory.Contains("A") && !strCategory.Contains("C") && !string.IsNullOrEmpty(strPODept))
                    message += "請購類別A和C以外不用選擇採購單位";*/

                if (strCategory.Contains("A"))
                {
                    string applicant = service.GetUserId(strApplicant);
                    var lstFunc = service.GetLstFUNCID(applicant);
                    foreach (var func in lstFunc)
                    {
                        string funcName = service.GetFUNCNAME(func);
                        if (strPOApplicant.Contains(funcName))
                        {
                            message += "請購類別A的申請人與採購人不可相同";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            if (string.IsNullOrEmpty(message))
            {
                return BuildResult(1, ""); //驗證成功
            }
            else
            {
                return BuildResult(0, message);//驗證失敗
            }
        }

        [WebMethod]
        public string CheckAP(string formInfo)
        {
            XElement root = XElement.Parse(formInfo);


            //1. 付款金額小於3000以下才能選現金
            //2.【明細金額】欄位數字合計要與【付款金額】欄位數字一致才能送出表單。
            string result = CheckAmount(root);
            if (!string.IsNullOrEmpty(result))
            {
                return BuildResult(0, result);
            }

            //確認資料須選擇已確認才可送出
            string confirm = GetFieldItem(root, "Confirm").Attribute("fieldValue").Value;
            if (!"已確認".Equals(confirm))
            {
                return BuildResult(0, "確認資料欄位未選擇【已確認】");
            }
            else
            {
                return BuildResult(1, "");
            }
        }


        public string CheckAmount(XElement xe)
        {
            string strResult = string.Empty;
            //付款金額
            var xePymtAmount = GetFieldItem(xe, "Amount");
            decimal pymtAmount = Convert.ToDecimal(xePymtAmount.Attribute("fieldValue").Value);
            //付款方式
            var xePymtMethod = GetFieldItem(xe, "Payment");
            string pymtMethod = xePymtMethod.Attribute("fieldValue").Value;

            if (pymtAmount <= 3000 && pymtMethod.Contains("匯款"))
            {
                return "付款金額3000元以下，付款方式僅可選擇【現金】。";
            }

            //費用明細總金額
            decimal total = 0;
            var apGridItems = GetFieldItem(xe, "FeeItems").Element("FieldValue").Element("Acct").Elements("APGridItem");
            if (apGridItems != null)
            {
                foreach (var item in apGridItems)
                {
                    total += Convert.ToDecimal(item.Attribute("Price").Value);
                }

                if (pymtAmount != total)
                {
                    return $"付款金額 {pymtAmount} 與費用明細金額加總 {total} 不符，請再確認";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "無法取得費用明細內容";
            }
        }



        private XElement GetFieldItem(XElement xe, string fieldId)
        {
            var fieldItem = xe.Element("FormFieldValue")
                              .Elements("FieldItem")
                              .Where(f => f.Attribute("fieldId").Value == fieldId).FirstOrDefault();

            return fieldItem;
        }


        private string BuildResult(int status, string message)
        {
            XElement result = new XElement("ReturnValue",
                new XElement("Status", status),
                new XElement("Exception",
                      new XElement("Message", message)
                )
            );
            return result.ToString();
        }

    }
}
