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
    public class ACCEPT_WS : System.Web.Services.WebService
    {
        [WebMethod]
        public string CheckACCEPT(string formInfo)
        {
            string message = string.Empty;
            try
            {
                POService service = new POService();
                XmlDocument rootDoc = new XmlDocument();
                rootDoc.LoadXml(formInfo);
                string strPODept = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='PODept']").Attributes["fieldValue"].Value;
                string strACCEPTSignOff = rootDoc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='ACCEPTSignOff']").Attributes["fieldValue"].Value;

                if (strPODept.Contains("行政") && string.IsNullOrEmpty(strACCEPTSignOff))
                    message = "行政流程必須選擇行政驗收簽核人員\r\n";
                else if (strPODept.Contains("行銷") && !string.IsNullOrEmpty(strACCEPTSignOff))
                    message = "行銷流程不用選擇行政驗收簽核人員\r\n";
                if (strACCEPTSignOff.Contains("、"))
                    message += "行政驗收簽核人員只能選擇一人";
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
