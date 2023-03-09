using Ede.Uof.Utility.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
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
    public class AP_WS : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloUOF(string formInfo)
        {
            //Logger.Write("FRS_Log", formInfo);
            return BuildResult(1, "");
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

            if (pymtAmount > 3000 && pymtMethod.Contains("現金"))
            {
                return "付款金額3000元以下，付款方式才能選擇【現金】。";
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





/*
 
 
 <Form formVersionId="f0271324-0645-4097-ad49-9b93170ccdec">
    <FormFieldValue>
        <FieldItem fieldId="BPMKey" fieldValue="" realValue="" enableSearch="True" />
        <FieldItem fieldId="AppUser" fieldValue="彭祖嗣(tsussu)" realValue="＆lt;UserSet＆gt;＆lt;Element type='user'＆gt; ＆lt;userId＆gt;8d53e309-28f3-4290-b942-51c1bdc9f114＆lt;/userId＆gt;＆lt;/Element＆gt;＆lt;/UserSet＆gt;＆#xD;＆#xA;" enableSearch="True" />
        <FieldItem fieldId="AppDate" fieldValue="2022/10/31" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="AppTime" fieldValue="15:54" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="Application" fieldValue="報銷@報銷" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="Payment" fieldValue="匯款@匯款" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="Amount" fieldValue="99999" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="TaxDate" fieldValue="2022/11/30" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="RefForm" ConditionValue="" realValue="">
            <FormChooseInfo taskGuid="" />
        </FieldItem>
        <FieldItem fieldId="Vendor" ConditionValue="" realValue="">
            <FieldValue AppGroupGuid="ae7a69d3-f291-e61f-7223-c555e073c7ac" AppUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114">
                <BP CardName="台灣區模具工業同業公會" CardCode="台灣區模具工業同業公會" BankCode="7000021" Branch="KH2" Account="7000021-9872314" Country="TW" />
            </FieldValue>
        </FieldItem>
        <FieldItem fieldId="Memo" fieldValue="TEST" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="FeeItems" ConditionValue="" realValue="">
            <FieldValue AppGroupGuid="ae7a69d3-f291-e61f-7223-c555e073c7ac" AppUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114">
                <Acct>
                    <APGridItem xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Seq="1" AcctCode="" AcctName="銷貨淨額" Price="55000" Price_Text="55,000" />
                    <APGridItem xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Seq="2" AcctCode="" AcctName="銷貨收入" Price="65000" Price_Text="65,000" />
                </Acct>
            </FieldValue>
        </FieldItem>
        <FieldItem fieldId="Notes" fieldValue="TEST" realValue="" enableSearch="True" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="Confirm" fieldValue="已確認" realValue="" enableSearch="True" customValue="@null" fillerName="彭祖嗣" fillerUserGuid="8d53e309-28f3-4290-b942-51c1bdc9f114" fillerAccount="tsussu" fillSiteId="" />
        <FieldItem fieldId="DocDate" fieldValue="" realValue="" enableSearch="True" />
        <FieldItem fieldId="TransferDate" fieldValue="" realValue="" enableSearch="True" />
    </FormFieldValue>
    <SignInfo SignResult="Approve" Signer="8d53e309-28f3-4290-b942-51c1bdc9f114" />
</Form>
 
 
 */