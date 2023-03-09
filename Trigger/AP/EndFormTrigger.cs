using Ede.Uof.Utility.Log;
using Ede.Uof.WKF.ExternalUtility;
using Ede.Uof.WKF.Utility;
using FRS.Lib.Model.AP;
using FRS.Lib.Service.AP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FRS.Lib.Trigger.AP
{
    public class EndFormTrigger : ICallbackTriggerPlugin
    {
        public void Finally()
        {
            //throw new NotImplementedException();
        }

        public string GetFormResult(ApplyTask applyTask)
        {
            if (applyTask.FormResult != Ede.Uof.WKF.Engine.ApplyResult.Adopt)
                return "";

            var formDoc = applyTask.Task.CurrentDocument;
            //Logger.Write("FRS_Log", "applyTask: \n" + applyTask.CurrentDocXML);

            //取得表頭
            APInvoiceHeader header = GetHeaderData(formDoc);
            Logger.Write("FRS_Log", "GetHeaderData: \n" + JsonConvert.SerializeObject(header));

            //取得明細列
            List<APInvoiceRow> lstRows = GetRowData(formDoc);
            Logger.Write("FRS_Log", "GetRowData: \n" + JsonConvert.SerializeObject(lstRows));


            //新增至中介表格
            APService service = new APService();
            int result = service.InsertAPInvoiceService(header, lstRows);

            //throw new Exception("故意錯誤");
            return "";
        }

        public void OnError(Exception errorException)
        {
            Logger.Write("FRS_Errorlog", errorException.ToString());
        }


        /// <summary>
        /// 取得表頭資料
        /// </summary>
        /// <param name="formDoc"></param>
        /// <returns></returns>
        private APInvoiceHeader GetHeaderData(Document formDoc)
        {
            try
            {
                APInvoiceHeader obj = new APInvoiceHeader();

                //BPM單據Key值
                obj.BPMKey = formDoc.Fields["BPMKey"].FieldValue;

                //供應商付款資訊
                string vendor = formDoc.Fields["Vendor"].FieldValue;
                XElement xeVendor = XElement.Parse(vendor);
                if (xeVendor.HasAttributes)
                {
                    var bp = xeVendor.Element("BP");
                    obj.CardCode = bp.Attribute("CardCode").Value;
                    obj.CardName = bp.Attribute("CardName").Value;
                    obj.BankCode = bp.Attribute("BankCode").Value;
                    obj.BnkBranch = bp.Attribute("BnkBranch").Value;
                    obj.BnkAccount = bp.Attribute("BnkAccount").Value;
                }
                //入帳日期
                obj.TaxDate = GetDateTime(formDoc.Fields["TaxDate"].FieldValue);
                //憑證日期
                obj.DocDate = GetDateTime(formDoc.Fields["DocDate"].FieldValue);
                //用途說明
                obj.JrnlMemo = formDoc.Fields["JrnlMemo"].FieldValue;
                //該BPMKey的Row筆數
                obj.RowCount = GetRowCount(formDoc.Fields["FeeItems"].FieldValue);
                //界接狀態
                obj.Flag = 0; //0：BPM至中介DB
                              //建立時間
                obj.CreateDate = DateTime.Now;
                obj.UpdateDate = DateTime.Now;

                return obj;
            }
            catch(Exception ex)
            {
                throw new Exception("GetHeaderData: \n" + ex.ToString());
            }
            
        }

        /// <summary>
        /// 取得付款明細
        /// </summary>
        /// <param name="formDoc"></param>
        /// <returns></returns>
        private List<APInvoiceRow> GetRowData(Document formDoc)
        {
            try
            {
                List<APInvoiceRow> lstRows = new List<APInvoiceRow>();
                XElement fieldValue = XElement.Parse(formDoc.Fields["FeeItems"].FieldValue);
                if (fieldValue.HasElements)
                {
                    var feeItems = fieldValue.Element("Acct").Elements("APGridItem");

                    foreach (var item in feeItems)
                    {
                        APInvoiceRow row = new APInvoiceRow();
                        row.BPMKey = formDoc.Fields["BPMKey"].FieldValue;
                        row.LineID = Convert.ToInt32(item.Attribute("Seq").Value) - 1;
                        row.AcctCode = item.Attribute("AcctCode").Value;
                        row.Price = Convert.ToDecimal(item.Attribute("Price").Value);
                        row.CreateDate = DateTime.Now;
                        row.UpdateDate = DateTime.Now;

                        lstRows.Add(row);

                    }
                }
                return lstRows;
            }
            catch (Exception ex)
            {
                throw new Exception("GetRowData: \n" + ex.ToString());
            }
            

            

        }

        private int GetRowCount(string fieldValue)
        {
            int rowCount = 0;
            
            XElement xe = XElement.Parse(fieldValue);
            var feeItems = xe.Element("Acct").Elements("APGridItem");

            if (feeItems != null) rowCount = feeItems.Count();

            return rowCount;
        }

        private DateTime GetDateTime(string fieldValue)
        {
            string str = fieldValue.Replace('/', '-');
            DateTime dateTime = DateTime.ParseExact(str, "yyyy-MM-dd", null);
            return dateTime;
        }
    }
}


/*
 <Form formVersionId="f0271324-0645-4097-ad49-9b93170ccdec">
    <FormFieldValue>
        <FieldItem fieldId="BPMKey" fieldValue="BPM221100003" realValue="" enableSearch="True" />
        <FieldItem fieldId="AppUser" fieldValue="彭祖嗣(tsussu)" realValue="&lt;UserSet&gt;&lt;Element type='user'&gt; &lt;userId&gt;8d53e309-28f3-4290-b942-51c1bdc9f114&lt;/userId&gt;&lt;/Element&gt;&lt;/UserSet&gt;&#xD;&#xA;" enableSearch="True" />
        <FieldItem fieldId="AppDate" fieldValue="2022/11/01" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="AppTime" fieldValue="09:06" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="Application" fieldValue="報銷@報銷" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="Payment" fieldValue="匯款@匯款" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="Amount" fieldValue="50000" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="TaxDate" fieldValue="2022/12/29" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="RefForm" ConditionValue="" realValue="">
            <FormChooseInfo taskGuid="" />
        </FieldItem>
        <FieldItem fieldId="Vendor" ConditionValue="" realValue="">
            <FieldValue AppGroupGuid="ae7a69d3-f291-e61f-7223-c555e073c7ac" AppUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114">
                <BP CardName="味丹企業股份有限公司" CardCode="S010336" BankCode="808" BnkBranch="0990" AcctName="碩益科技股份有限公司" BnkAccount="048108000271" DispCode="8080990" DispName="玉山商業銀行草屯分行" />
            </FieldValue>
        </FieldItem>
        <FieldItem fieldId="Memo" fieldValue="TEST摘要" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="FeeItems" ConditionValue="" realValue="">
            <FieldValue AppGroupGuid="ae7a69d3-f291-e61f-7223-c555e073c7ac" AppUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114">
                <Acct>
                    <APGridItem xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Seq="1" AcctCode="" AcctName="銷貨淨額" Price="35000" Price_Text="35,000" />
                    <APGridItem xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Seq="2" AcctCode="" AcctName=" 銷貨退回" Price="15000" Price_Text="" />
                </Acct>
            </FieldValue>
        </FieldItem>
        <FieldItem fieldId="Notes" fieldValue="TEST提醒事項" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="Confirm" fieldValue="已確認" realValue="" enableSearch="True" customValue="@null" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="DocDate" fieldValue="" realValue="" enableSearch="True" />
        <FieldItem fieldId="TransferDate" fieldValue="" realValue="" enableSearch="True" />
    </FormFieldValue>
</Form>

 */