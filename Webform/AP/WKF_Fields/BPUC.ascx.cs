using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Ede.Uof.WKF.Design;
using System.Collections.Generic;
using Ede.Uof.WKF.Utility;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.WKF.Design.Data;
using Ede.Uof.WKF.VersionFields;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using Ede.Uof.Utility.Log;
using System.Dynamic;
using Ede.Uof.Utility.Page.Common;
using FRS.Lib.Model.AP;
using Newtonsoft.Json;

public partial class WKF_OptionalFields_BPUC : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
{

	#region ==============公開方法及屬性==============
    //表單設計時
	//如果為False時,表示是在表單設計時
    private bool m_ShowGetValueButton = true;
    public bool ShowGetValueButton
    {
        get { return this.m_ShowGetValueButton; }
        set { this.m_ShowGetValueButton = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
		//這裡不用修改
		//欄位的初始化資料都到SetField Method去做
        SetField(m_versionField);
    }    

    /// <summary>
    /// 外掛欄位的條件值
    /// </summary>
    public override string ConditionValue
    {
        get
        {
			//回傳字串
			//此字串的內容將會被表單拿來當做條件判斷的值
			return String.Empty;
        }
    }

    /// <summary>
    /// 是否被修改
    /// </summary>
    public override bool IsModified
    {
        get
        {
			//請自行判斷欄位內容是否有被修改
			//有修改回傳True
			//沒有修改回傳False
            //若實作產品標準的控制修改權限必需實作
            //一般是用 m_versionField.FieldValue (表單開啟前的值)
            //      和this.FieldValue (當前的值) 作比對
			return false;
        }
    }

    /// <summary>
    /// 查詢顯示的標題
    /// </summary>
    public override string DisplayTitle
    {
        get
        {
			//表單查詢或WebPart顯示的標題
			//回傳字串
            return String.Empty;
        }
    }

    /// <summary>
    /// 訊息通知的內容
    /// </summary>
    public override string Message
    {
        get
        {
			//表單訊息通知顯示的內容
			//回傳字串
            return String.Empty;
        }
    }


    /// <summary>
    /// 真實的值
    /// </summary>
    public override string RealValue
    {
        get
        {
            //回傳字串
			//取得表單欄位簽核者的UsetSet字串
            //內容必須符合EB UserSet的格式
			return String.Empty;
        }
        set
        {
			//這個屬性不用修改
            base.m_fieldValue = value;
        }
    }


    /// <summary>
    /// 欄位的內容
    /// </summary>
    public override string FieldValue
    {
        get
        {
            //回傳字串
            //取得表單欄位填寫的內容
            return BuildFieldValue();
        }
        set
        {
			//這個屬性不用修改
            base.m_fieldValue = value;
        }
    }

    /// <summary>
    /// 是否為第一次填寫
    /// </summary>
    public override bool IsFirstTimeWrite
    {
        get
        {
            //這裡請自行判斷是否為第一次填寫
            //若實作產品標準的控制修改權限必需實作
            //實作此屬性填寫者可修改也才會生效
            //一般是用 m_versionField.Filler == null(沒有記錄填寫者代表沒填過)
            //      和this.FieldValue (當前的值是否為預設的空白) 作比對
            return false;
        }
        set
        {
            //這個屬性不用修改
            base.IsFirstTimeWrite = value;
        }
    }

    /// <summary>
    /// 設定元件狀態
    /// </summary>
    /// <param name="Enabled">是否啟用輸入元件</param>
    public void EnabledControl(bool Enabled)
    {
        btnSearch.Visible = Enabled;
    }

    /// <summary>
    /// 顯示時欄位初始值
    /// </summary>
    /// <param name="versionField">欄位集合</param>
    public override void SetField(Ede.Uof.WKF.Design.VersionField versionField)
    {
        FieldOptional fieldOptional = versionField as FieldOptional;

        if (fieldOptional != null)
        {

            //若有擴充屬性，可以用該屬性存取
            // fieldOptional.ExtensionSetting

            
            //草稿
            if(!fieldOptional.IsAudit)
            {
                if(fieldOptional.HasAuthority)
                {
                    //有填寫權限的處理
                    EnabledControl(true);
                }
                else
                {
                    //沒填寫權限的處理
                    EnabledControl(false);
                }
            }
            else
            {
                //己送出

                //有填過
                if(fieldOptional.Filler != null)
                {
                    //判斷填寫的站點和當前是否相同
                    if(base.taskObj != null && base.taskObj.CurrentSite != null &&
                        base.taskObj.CurrentSite.SiteId == fieldOptional.FillSiteId && fieldOptional.Filler.UserGUID == Ede.Uof.EIP.SystemInfo.Current.UserGUID)
                    {
                        //判斷填寫權限
                        if (fieldOptional.HasAuthority)
                        {
                            //有填寫權限的處理
                            EnabledControl(true);
                        }
                        else
                        {
                            //沒填寫權限的處理
                            EnabledControl(false);
                        }
                    }
                    else
                    {
                        //判斷修改權限
                        if (fieldOptional.AllowModify)
                        {
                            //有修改權限的處理
                            EnabledControl(true);
                        }
                        else
                        {
                            //沒修改權限的處理
                            EnabledControl(false);
                        }

                    }
                }
                else
                {
                    //判斷填寫權限
                    if (fieldOptional.HasAuthority)
                    {
                        //有填寫權限的處理
                        EnabledControl(true);
                    }
                    else
                    {
                        //沒填寫權限的處理
                        EnabledControl(false);
                    }

                }
            }



            switch(fieldOptional.FieldMode)
            {
                case FieldMode.Applicant:
                case FieldMode.ReturnApplicant:
                    EnabledControl(true);

                    break;
                case FieldMode.Signin:
                    if (this.ApplicantGuid == base.taskObj.CurrentNode.OriginalSignerId)
                        EnabledControl(true);
                    else
                        EnabledControl(false);
                    break;
                case FieldMode.Print:
                case FieldMode.View:
                    //觀看和列印都需作沒有權限的處理
                    EnabledControl(false);
                    break;

            }
            
            #region ==============屬性說明==============『』
			//fieldOptional.IsRequiredField『是否為必填欄位,如果是必填(True),如果不是必填(False)』
			//fieldOptional.DisplayOnly『是否為純顯示,如果是(True),如果不是(False),一般在觀看表單及列印表單時,屬性為True』
			//fieldOptional.HasAuthority『是否有填寫權限,如果有填寫權限(True),如果沒有填寫權限(False)』
			//fieldOptional.FieldValue『如果已有人填寫過欄位,則此屬性為記錄其內容』
			//fieldOptional.FieldDefault『如果欄位有預設值,則此屬性為記錄其內容』
			//fieldOptional.FieldModify『是否允許修改,如果允許(fieldOptional.FieldModify=FieldModifyType.yes),如果不允許(fieldOptional.FieldModify=FieldModifyType.no)』
			//fieldOptional.Modifier『如果欄位有被修改過,則Modifier的內容為EBUser,如果沒有被修改過,則會等於Null』
            #endregion

            #region ==============如果有修改，要顯示修改者資訊==============
            if (fieldOptional.Modifier != null)
            {
                lblModifier.Visible = true;
                lblModifier.ForeColor = System.Drawing.Color.FromArgb(0x52, 0x52, 0x52);
                lblModifier.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(fieldOptional.Modifier.Name, true);
            } 
            #endregion
        }

        BindData(fieldOptional);
        SetBtnLink();
    }

    private void BindData(FieldOptional fieldOptional)
    {
        if (!IsPostBack)
        {
            string fieldValueXml = fieldOptional.FieldValue;// 目前DB存的值(第一次為空)

            if (String.IsNullOrEmpty(fieldValueXml)) //第一次開單時            
            {
                lbAppGroupGuid.Text = this.ApplicantGroupId;
                lbAppUserGuid.Text = this.ApplicantGuid;

                //初始化表單負責人及建立日期
                this.InitFormData();
            }
            else
            {
                //從草稿開單或簽核途中開啟表單
                SetFormValue(fieldValueXml);
            }
        }
        else
        {
            //申請者切換部門或代申請下拉bar時會觸發postback
            if (lbAppGroupGuid.Text != this.ApplicantGroupId || lbAppUserGuid.Text != this.ApplicantGuid)
            {
                //初始化表單負責人及建立日期
                this.InitFormData();
            }
        }
    }

    private void InitFormData()
    {

    }


    protected string BuildFieldValue()
    {
        XElement xe = new XElement("FieldValue",
            new XAttribute("AppGroupGuid", lbAppGroupGuid.Text),
            new XAttribute("AppUserGuid", lbAppUserGuid.Text),
            BuildBPInfo()
        );

        //Logger.Write("FRS_Log", "BPUC: \n" + xe);

        return xe.ToString();
    }

    public void SetFormValue(string fieldValueXml)
    {
        if (!string.IsNullOrEmpty(fieldValueXml))
        {

            XElement xe = XElement.Parse(fieldValueXml);
            lbAppGroupGuid.Text = xe.Attribute("AppGroupGuid").Value;
            lbAppUserGuid.Text = xe.Attribute("AppUserGuid").Value;

            XElement xeBP = xe.Element("BP");
            lbCardName.Text = xeBP.Attribute("CardName").Value;
            hfCardCode.Value = xeBP.Attribute("CardCode").Value;
            hfBnkBranch.Value = xeBP.Attribute("BnkBranch").Value;
            hfBankCode.Value = xeBP.Attribute("BankCode").Value;
            lbAcctName.Text = xeBP.Attribute("AcctName").Value;
            lbBnkAccount.Text = xeBP.Attribute("BnkAccount").Value;
            lbDispCode.Text = xeBP.Attribute("DispCode").Value;
            lbDispName.Text = xeBP.Attribute("DispName").Value;

        }
    }

    private XElement BuildBPInfo()
    {
        XElement xe = new XElement("BP",
            new XAttribute("CardName", lbCardName.Text),
            new XAttribute("CardCode", hfCardCode.Value),
            new XAttribute("BankCode", hfBankCode.Value),
            new XAttribute("BnkBranch", hfBnkBranch.Value),
            new XAttribute("AcctName", lbAcctName.Text),
            new XAttribute("BnkAccount", lbBnkAccount.Text),
            new XAttribute("DispCode", lbDispCode.Text),
            new XAttribute("DispName", lbDispName.Text)
        );

        return xe;
    }

    

    //開啟對話視窗
    protected void SetBtnLink()
    {
        Dialog.Open2(btnSearch, "~/CDS/FRS/AP/WKF_Fields/Dialog/ItemDialog.aspx", "新增供應商付款資訊", 900, 600, Dialog.PostBackType.AfterReturn, null);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string dialogRtnVal = Dialog.GetReturnValue();
            if (!String.IsNullOrEmpty(dialogRtnVal) && "NeedPostBack" != dialogRtnVal)
            {
                VM_BusinessPartner bp = JsonConvert.DeserializeObject<VM_BusinessPartner>(dialogRtnVal);

                BindContorl(bp);
            }

        }
        catch (Exception ex)
        {
            CV_FORM.IsValid = false;
            CV_FORM.ErrorMessage = ex.Message;
        }
    }

    private void BindContorl(VM_BusinessPartner bp)
    {
        lbCardName.Text = bp.CardName;
        hfCardCode.Value = bp.CardCode;
        hfBankCode.Value = bp.BankCode;
        hfBnkBranch.Value = bp.BnkBranch;
        lbAcctName.Text = bp.AcctName;
        lbBnkAccount.Text = bp.BnkAccount;
        lbDispCode.Text = bp.DispCode;
        lbDispName.Text = bp.DispName;
    }
}