using Ede.Uof.Utility.Log;
using Ede.Uof.WKF.ExternalUtility;
using FRS.Lib.Service.AP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Trigger.AP
{
    public class RecoveryTrigger : ICallbackTriggerPlugin
    {
        public string GetFormResult(ApplyTask applyTask)
        {
            APService service = new APService();
            service.DeleteAPInvoiceService(applyTask.FormNumber);

            Logger.Write("SW_InfoLog", string.Format("{0} 申請結案復原: {1}", applyTask.Task.FormName, applyTask.FormNumber));

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
