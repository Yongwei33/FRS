using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using FRS.Lib.Model.AP;
using FRS.Lib.Service.AP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CDS_FRS_AP_WKF_Fields_Dialog_ItemDialog : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_DialogMasterPage)this.Master).Button1OnClick += OKButton1OnClick;
        ((Master_DialogMasterPage)this.Master).Button1AutoCloseWindow = false;
        ((Master_DialogMasterPage)this.Master).Button2Text = "";

        try
        {
            if (!IsPostBack)
            {
                SetBtnLink();
                BindNull();
            }
        }
        catch (Exception ex)
        {
            CV_FORM.IsValid = false;
            CV_FORM.ErrorMessage = ex.Message;
        }

    }

    

    private void OKButton1OnClick()
    {
        try
        {
            if (gvMain.SelectedIndex < 0) throw new Exception("尚未選擇");
            var row = gvMain.Rows[gvMain.SelectedIndex];
            if (row != null)
            {
                Label lbCardCode = row.FindControl("lbCardCode") as Label;
                Label lbCardName = row.FindControl("lbCardName") as Label;
                Label lbBankCode = row.FindControl("lbBankCode") as Label;
                Label lbBnkBranch = row.FindControl("lbBnkBranch") as Label;
                Label lbBnkAccount = row.FindControl("lbBnkAccount") as Label;
                Label lbDispCode = row.FindControl("lbDispCode") as Label;
                Label lbDispName = row.FindControl("lbDispName") as Label;
                Label lbAcctName = row.FindControl("lbAcctName") as Label;

                VM_BusinessPartner obj = new VM_BusinessPartner();
                obj.CardName = lbCardName.Text;
                obj.CardCode = lbCardCode.Text;
                obj.BankCode = lbBankCode.Text;
                obj.BnkBranch = lbBnkBranch.Text;
                obj.BnkAccount = lbBnkAccount.Text;
                obj.AcctName = lbAcctName.Text;
                obj.DispCode = lbDispCode.Text;
                obj.DispName = lbDispName.Text;

                Dialog.SetReturnValue2(JsonConvert.SerializeObject(obj));
                Dialog.Close(this);
            }
            else
            {
                throw new Exception("尚未選擇");
            }
        }
        catch (Exception ex)
        {
            CV_FORM.IsValid = false;
            CV_FORM.ErrorMessage = ex.Message;
        }
    }

    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMain.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //加入click點選一欄就可讓整列有select效果
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvMain, "Select$" + e.Row.RowIndex);
        }
    }

    protected void BindGrid()
    {
        try
        {
            APService service = new APService();
            List<VM_BusinessPartner> list = service.QueryBPAcctByCardCodedService(hfCardCode.Value);

            gvMain.DataSource = list;
            gvMain.DataBind();
        }
        catch (Exception ex)
        {
            CV_FORM.IsValid = false;
            CV_FORM.ErrorMessage = ex.Message;
        }

    }

    private void BindNull()
    {
        gvMain.DataSource = new List<VM_BusinessPartner>();
        gvMain.DataBind();
    }

    //開啟對話視窗
    protected void SetBtnLink()
    {
        //ExpandoObject param1 = new
        //{

        //}.ToExpando();
        Dialog.Open2(btnBP, "~/CDS/FRS/AP/WKF_Fields/Dialog/BPDialog.aspx", "查詢供應商", 550, 500, Dialog.PostBackType.AfterReturn, null);
    }

    protected void btnBP_Click(object sender, EventArgs e)
    {
        try
        {
            string dialogRtnVal = Dialog.GetReturnValue();
            if (!String.IsNullOrEmpty(dialogRtnVal) && "NeedPostBack" != dialogRtnVal)
            {
                BusinessPartners bp = JsonConvert.DeserializeObject<BusinessPartners>(dialogRtnVal);

                hfCardName.Value = bp.CardName;
                hfCardCode.Value = bp.CardCode;
                txtBP.Text = bp.CardName;
                BindGrid();
            }

        }
        catch (Exception ex)
        {
            CV_FORM.IsValid = false;
            CV_FORM.ErrorMessage = ex.Message;
        }
    }

}