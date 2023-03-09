using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.EIP.PrivateMessage;
using Ede.Uof.Utility.Log;
using Ede.Uof.WKF.Design;
using Ede.Uof.WKF.ExternalUtility;
using Ede.Uof.WKF.Utility;
using FRS.Lib.Model;
using FRS.Lib.Service.ACCEPT;
using FRS.Lib.Service.REQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FRS.Lib.Trigger.REQ
{
    public class EndFormTrigger : ICallbackTriggerPlugin
    {
        public string GetFormResult(ApplyTask applyTask)
        {
            REQService service = new REQService();
            var formDoc = applyTask.Task.CurrentDocument;
            //Logger.Write("SW_Requisitions", applyTask.Task.CurrentDocXml);
            if(applyTask.FormResult == Ede.Uof.WKF.Engine.ApplyResult.Adopt)
            {
                string category = formDoc.Fields["Category"].FieldValue;
                if(category.Contains("A") || category.Contains("C"))
                {
                    SW_PO_HEAD head = new SW_PO_HEAD();

                    head.Date = DateTime.Now.ToString("yyyy/MM/dd");

                    head.Dept = formDoc.Fields["Dept"].RealValue.Split(',');

                    string[] applicant = new string[3];
                    applicant[0] = formDoc.Fields["Applicant"].FieldValue.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    applicant[1] = formDoc.Fields["Applicant"].FieldValue.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    applicant[2] = service.GetUserId(applicant[1]);
                    head.Applicant = applicant;//0:USERNAME 1:USERACCOUNT 2:USERGUID

                    head.REQForm = service.GetTaskId(applyTask.FormNumber);
                    string POApplicant = formDoc.Fields["POApplicant"].FieldValue.Split('@')[0];

                    /*if (POApplicant.Contains("行銷"))
                    {
                        string[] POSignOff = new string[3];
                        List<string> lstPO = service.GetLstPO();
                        foreach (string po in lstPO)
                        {
                            string userId = service.GetUserIdFUNC(po);
                            if (userId != applicant[2])
                            {
                                POSignOff[2] = userId;
                                break;
                            }
                        }
                        POSignOff[1] = service.GetUserAccount(POSignOff[2]);
                        POSignOff[0] = service.GetUserName(POSignOff[2]);
                        head.POSignOff = POSignOff;
                    }*/

                    head.TotalPrice = formDoc.Fields["TotalPrice"].FieldValue;

                    FieldDataGrid grid = applyTask.Task.CurrentDocument.Fields["Detail"] as Ede.Uof.WKF.Design.FieldDataGrid;
                    List<SW_PO_GRID> Items = GetItems(grid);

                    string USERACCOUNT = "";
                    string USERGUID = "";
                    string USERNAME = "";
                    string FUNCID = "";
                    if (POApplicant.Contains("行政"))
                    {
                        head.PODept = "行政部";
                    }
                    else
                    {
                        head.PODept = "行銷暨數位發展部";
                    }
                    FUNCID = service.GetFUNCID(POApplicant);
                    USERGUID = service.GetUserIdFUNC(FUNCID);
                    USERACCOUNT = service.GetUserAccount(USERGUID);
                    USERNAME = service.GetUserName(USERGUID);
                    Logger.Write("SW_Form", "採購單 USERGUID:" + USERGUID + " USERACCOUNT:" + USERACCOUNT + " USERNAME:" + USERNAME);

                    string USING_VERSION_ID = service.GetUSINGVERSIONID("採購單");
                    POFormXmlService formXml = new POFormXmlService(USING_VERSION_ID, Service.REQ.UrgentLevel.Normal, USERACCOUNT, USERGUID, USERNAME, head, Items);
                    string xml = formXml.ConvertToFormInfoXml();

                    TaskUtilityUCO uco = new TaskUtilityUCO();
                    uco.WebService_CreateTask(xml);
                    //service.SaveTaskData(xml);
                }
                else
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

                    APFormXmlService formXml = new APFormXmlService(USING_VERSION_ID, Service.ACCEPT.UrgentLevel.Normal,  applicant[1],  applicant[2],  applicant[0], head);
                    string xml = formXml.ConvertToFormInfoXml();

                    TaskUtilityUCO uco = new TaskUtilityUCO();
                    uco.WebService_CreateTask(xml);
                    //service.SaveTaskData(xml);
                }

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

        private List<SW_PO_GRID> GetItems(FieldDataGrid grid)
        {
            List<SW_PO_GRID> lstItems = new List<SW_PO_GRID>();
            foreach (var row in grid.FieldDataGridValue.RowValueList)
            {
                SW_PO_GRID item = new SW_PO_GRID();
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
    }
}
