using Ede.Uof.Utility.Log;
using Ede.Uof.WKF.Design;
using Ede.Uof.WKF.ExternalUtility;
using Ede.Uof.WKF.Utility;
using FRS.Lib.Model;
using FRS.Lib.Service.ACCEPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FRS.Lib.Trigger.ACCEPT
{
    public class EndFormTrigger : ICallbackTriggerPlugin
    {
        public string GetFormResult(ApplyTask applyTask)
        {
            ACCEPTService service = new ACCEPTService();
            var formDoc = applyTask.Task.CurrentDocument;
            //Logger.Write("SW_Requisitions", applyTask.Task.CurrentDocXml);
            if(applyTask.FormResult == Ede.Uof.WKF.Engine.ApplyResult.Adopt)
            {
                SW_AP_HEAD head = new SW_AP_HEAD();

                string[] applicant = new string[3];
                applicant[0] = formDoc.Fields["Applicant"].FieldValue.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[0];
                applicant[1] = formDoc.Fields["Applicant"].FieldValue.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1];
                applicant[2] = service.GetUserId(applicant[1]);
                head.Applicant = applicant;//0:USERNAME 1:USERACCOUNT 2:USERGUID

                head.Date = DateTime.Now.ToString("yyyy/MM/dd");
                head.Time = DateTime.Now.ToString("HH:mm");

                head.REQForm = service.GetTaskId(applyTask.FormNumber);

                string USING_VERSION_ID = service.GetUSINGVERSIONID("付款單");

                Logger.Write("SW_Form", "付款單 USERGUID:" + applicant[2] + " USERACCOUNT:" + applicant[1] + " USERNAME:" + applicant[0] + " VERSION_ID:" + USING_VERSION_ID);

                APFormXmlService formXml = new APFormXmlService(USING_VERSION_ID, UrgentLevel.Normal, applicant[1], applicant[2], applicant[0], head);
                string xml = formXml.ConvertToFormInfoXml();

                TaskUtilityUCO uco = new TaskUtilityUCO();
                uco.WebService_CreateTask(xml);
                //service.SaveTaskData(xml);
            }

            /*else if (applyTask.FormResult == Ede.Uof.WKF.Engine.ApplyResult.Reject)
                */

            Logger.Write("SW_InfoLog", string.Format("{0} 申請結案: {1}", applyTask.Task.FormName, applyTask.FormNumber));

            return "";
        }

        public void OnError(Exception errorException)
        {
            Logger.Write("SW_ErrorLog", errorException.ToString());
        }
        public void Finally()
        {
           
        }

    }
}
