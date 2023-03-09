using FRS.Lib.Service.PO;
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
    public class PO_WS : System.Web.Services.WebService
    {
        [WebMethod]
        public string CheckPO(string formInfo)
        {
            string message = string.Empty;
            try
            {
                POService service = new POService();
                XmlDocument rootDoc = new XmlDocument();
                rootDoc.LoadXml(formInfo);
                string strPODept = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='PODept']").Attributes["fieldValue"].Value;
                string strPOSignOff = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='POSignOff']").Attributes["fieldValue"].Value;
                string strPOApplicant = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='POApplicant']").Attributes["fieldValue"].Value;
                string strACCEPTApplicant = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='ACCEPTApplicant']").Attributes["fieldValue"].Value;

                /*string FUNCID = service.GetFUNCID(strACCEPTApplicant);
                string USERGUID = service.GetUserIdFUNC(FUNCID);
                string USERACCOUNT = service.GetUserAccount(USERGUID);*/

                if(strACCEPTApplicant.Contains("、"))
                    message = "驗收人員只能選擇一人\r\n";
                if (strPOSignOff.Contains("、"))
                    message += "行銷議價人員只能選擇一人\r\n";
                if (strPODept.Contains("行政") && strPOApplicant == strACCEPTApplicant)
                    message += "行政的採購人員和驗收人員不可為同一人\r\n";
                if (strPODept.Contains("行政") && !string.IsNullOrEmpty(strPOSignOff))
                    message += "行政流程不用選擇行銷議價人員\r\n";
                /*if (strPODept.Contains("行銷") && string.IsNullOrEmpty(strPOSignOff))
                    message += "行銷流程必須選擇行銷議價人員";
                else*/ if (strPOSignOff == strPOApplicant)
                    message += "行銷的採購人員和行銷議價人員不可為同一人";

                //string strACCEPTAccount = strACCEPTApplicant.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1];
                /*else if (strPOApplicant.Contains(USERACCOUNT))
                    message = "採購人員和驗收人員不可為同一人";*/
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
