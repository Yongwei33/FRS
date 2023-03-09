using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace FRS.Lib.Service.REQ
{
    public class AutoNbrREQ : Ede.Uof.WKF.ExternalUtility.IFormAutoNumber
    {
        public string GetFormNumber(string formId, string userGroupId, string formValueXML)
        {
            //   throw new NotImplementedException();

            //return System.Guid.NewGuid().ToString();
            //

            formValueXML = HttpUtility.UrlDecode(formValueXML);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(formValueXML);
            return xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='type']").Attributes["fieldValue"].Value.Substring(1, 2) + DateTime.Now.ToString("yyyyMMdd");
        }

        public void Finally()
        {
            //   throw new NotImplementedException();
        }

        public void OnError()
        {
            //     throw new NotImplementedException();
        }

        public void OnExecption(Exception errorException)
        {
            //   throw new NotImplementedException();
        }
    }
}
