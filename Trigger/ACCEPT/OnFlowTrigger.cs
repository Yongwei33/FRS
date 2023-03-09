using Ede.Uof.Utility.Log;
using Ede.Uof.WKF.Engine;
using Ede.Uof.WKF.ExternalUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FRS.Lib.Trigger.ACCEPT
{
    public class OnFlowTrigger : ICallbackTriggerPlugin
    {
        public string GetFormResult(ApplyTask applyTask)
        {
            /*if (applyTask.SignResult == SignResult.Approve && applyTask.SiteCode == "poa")
            {
                var formDoc = applyTask.Task.CurrentDocument;
                string ACCEPTApplicant = formDoc.Fields["ACCEPTApplicant"].FieldValue;
                if (!string.IsNullOrEmpty(ACCEPTApplicant))
                {
                    applyTask.SetFormValue("IsSignOff", "0");
                    applyTask.SaveFormValue();
                }
                else
                {
                    applyTask.SetFormValue("IsSignOff", "1");
                    applyTask.SaveFormValue();
                }
            }*/
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
