using Ede.Uof.Utility.Log;
using Ede.Uof.WKF.Design;
using Ede.Uof.WKF.ExternalUtility;
using Ede.Uof.WKF.Utility;
using FRS.Lib.Model;
using FRS.Lib.Service.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FRS.Lib.Trigger.PO
{
    public class EndFormTrigger : ICallbackTriggerPlugin
    {
        public string GetFormResult(ApplyTask applyTask)
        {
            POService service = new POService();
            var formDoc = applyTask.Task.CurrentDocument;
            //Logger.Write("SW_Requisitions", applyTask.Task.CurrentDocXml);
            if (applyTask.FormResult == Ede.Uof.WKF.Engine.ApplyResult.Adopt)
            {
                SW_ACCEPT_HEAD head = new SW_ACCEPT_HEAD();

                head.Date = DateTime.Now.ToString("yyyy/MM/dd");

                string[] dept = formDoc.Fields["Dept"].RealValue.Split(new string[] { "<", ">" }, StringSplitOptions.RemoveEmptyEntries);
                dept[0] = formDoc.Fields["Dept"].FieldValue;
                head.Dept = dept;//groupid=dept[3]
                head.PODept = formDoc.Fields["PODept"].FieldValue;

                string[] applicant = new string[3];
                applicant[0] = formDoc.Fields["Applicant"].FieldValue.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[0];
                applicant[1] = formDoc.Fields["Applicant"].FieldValue.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1];
                applicant[2] = service.GetUserId(applicant[1]);
                head.Applicant = applicant;//0:USERNAME 1:USERACCOUNT 2:USERGUID

                string REQFieldXml = formDoc.Fields["REQForm"].FieldValue;
                XElement REQForm = XElement.Parse(REQFieldXml);
                head.REQForm = GetAttrValue(REQForm, "taskGuid");
                head.POForm = service.GetTaskId(applyTask.FormNumber);

                /*string VendorFieldXml = formDoc.Fields["Vendor"].FieldValue;
                XElement VendorField = XElement.Parse(VendorFieldXml);
                head.Vendor = GetAttrValue(VendorField, "VendorName");*/
                head.Vendor = formDoc.Fields["Vendor"].FieldValue;
                head.TotalPrice = formDoc.Fields["TotalPrice"].FieldValue;

                FieldDataGrid grid = applyTask.Task.CurrentDocument.Fields["Detail"] as Ede.Uof.WKF.Design.FieldDataGrid;
                List<SW_ACCEPT_GRID> Items = GetItems(grid);

                string[] ACCEPTApplicant = new string[3];
                ACCEPTApplicant[0] = formDoc.Fields["ACCEPTApplicant"].FieldValue.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[0];
                ACCEPTApplicant[1] = formDoc.Fields["ACCEPTApplicant"].FieldValue.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1];
                ACCEPTApplicant[2] = service.GetUserId(ACCEPTApplicant[1]);//0:USERNAME 1:USERACCOUNT 2:USERGUID

                if (applicant[1] == ACCEPTApplicant[1])
                    head.IsApplicant = "1";
                else
                    head.IsApplicant = "0";

                Logger.Write("SW_Form", "驗收單 USERGUID:" + ACCEPTApplicant[2] + " USERACCOUNT:" + ACCEPTApplicant[1] + " USERNAME:" + ACCEPTApplicant[0]);
                /*string USERACCOUNT = "";
                string USERGUID = "";
                string USERNAME = "";
                string FUNCID = "";
                FUNCID = service.GetFUNCID(ACCEPTApplicant);
                USERGUID = service.GetUserIdFUNC(FUNCID);
                USERACCOUNT = service.GetUserAccount(USERGUID);
                USERNAME = service.GetUserName(USERGUID);*/

                string USING_VERSION_ID = service.GetUSINGVERSIONID("驗收單");
                ACCEPTFormXmlService formXml = new ACCEPTFormXmlService(USING_VERSION_ID, UrgentLevel.Normal, ACCEPTApplicant[1], ACCEPTApplicant[2], ACCEPTApplicant[0], head, Items);
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

        private List<SW_ACCEPT_GRID> GetItems(FieldDataGrid grid)
        {
            List<SW_ACCEPT_GRID> lstItems = new List<SW_ACCEPT_GRID>();
            foreach (var row in grid.FieldDataGridValue.RowValueList)
            {
                SW_ACCEPT_GRID item = new SW_ACCEPT_GRID();
                foreach (var cell in row.CellValueList)
                {
                    if (cell.fieldId == "Name")
                    {
                        item.Name = cell.fieldValue;
                        //Logger.Write("SW_Requisitions", "petty_amount" + " " + Convert.ToDecimal(cell.fieldValue).ToString() + " ");
                    }

                    if (cell.fieldId == "Spec")
                    {
                        item.Spec = cell.fieldValue;
                        //Logger.Write("SW_Requisitions", "petty_money" + " " + Convert.ToDecimal(cell.fieldValue).ToString() + " ");
                    }

                    if (cell.fieldId == "Quantity")
                    {
                        item.Quantity = cell.fieldValue;
                        //Logger.Write("SW_Requisitions", "petty_name" + " " + cell.fieldValue + " ");
                    }

                    if (cell.fieldId == "UnitPrice")
                    {
                        item.UnitPrice = cell.fieldValue;
                        //Logger.Write("SW_Requisitions", "petty_unitPrice" + " " + Convert.ToDecimal(cell.fieldValue).ToString() + " ");
                    }

                    if (cell.fieldId == "Amount")
                    {
                        item.Amount = cell.fieldValue;
                        //Logger.Write("SW_Requisitions", "Amount" + " " + Convert.ToDecimal(cell.fieldValue).ToString() + " ");
                    }
                }
                lstItems.Add(item);
            }

            return lstItems;
        }

        string GetAttrValue(XElement section, string attrName)
        {
            if (section == null) return "";
            var attr = section.Attribute(attrName);
            return attr != null ? attr.Value : "";
        }
    }
}
