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

public partial class CDS_FRS_AP_WKF_Fields_Dialog_BPDialog : BasePage
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
                BindGrid();
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
                Label lbCode = row.FindControl("lbCardCode") as Label;
                Label lbNAME = row.FindControl("lbCardName") as Label;

                BusinessPartners bp = new BusinessPartners();
                bp.CardCode = lbCode.Text;
                bp.CardName = lbNAME.Text;

                Dialog.SetReturnValue2(JsonConvert.SerializeObject(bp));
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid();
        }
        catch (Exception ex)
        {
            CV_FORM.IsValid = false;
            CV_FORM.ErrorMessage = ex.Message;
        }
    }

    protected void BindGrid()
    {
        try
        {
            APService service = new APService();
            List<BusinessPartners> list = service.QueryBPByKeyworddService(txtKeyword.Text);

            gvMain.DataSource = list;
            gvMain.DataBind();
        }
        catch (Exception ex)
        {
            CV_FORM.IsValid = false;
            CV_FORM.ErrorMessage = ex.Message;
        }

    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //加入click點選一欄就可讓整列有select效果
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvMain, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMain.PageIndex = e.NewPageIndex;
        BindGrid();
    }

}