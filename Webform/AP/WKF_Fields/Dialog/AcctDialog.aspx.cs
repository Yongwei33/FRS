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

public partial class CDS_FRS_AP_WKF_Fields_Dialog_AcctDialog : BasePage
{
    public string UpdateType // 新增or編輯
    {
        get
        {
            return ViewState["updateType"] as string;
        }
        set
        {
            ViewState["updateType"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_DialogMasterPage)this.Master).Button1OnClick += OKButton1OnClick;
        ((Master_DialogMasterPage)this.Master).Button1AutoCloseWindow = false;
        ((Master_DialogMasterPage)this.Master).Button2Text = "";

        try
        {
            if (!IsPostBack)
            {
                this.UpdateType = Request["updateType"]; //new or edit
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
            var row = gvMain.Rows[gvMain.SelectedIndex];
            if (row != null)
            {
            
                Label lbAcctCode = row.FindControl("lbAcctCode") as Label;
                Label lbAcctName = row.FindControl("lbAcctName") as Label;


                if (Session["AP_GridItem"] != null)
                {
                    List<APGridItem> list = Session["AP_GridItem"] as List<APGridItem>;
                    if (list != null)
                    {
                        var item = list.Where(f => f.AcctCode == lbAcctCode.Text).FirstOrDefault();
                        if (item != null)
                        {
                            throw new Exception(string.Format("費用類別【{0}】已存在明細中。", lbAcctName.Text));
                        }
                    }
                }

                APGridItem acct = new APGridItem();
                acct.AcctCode = lbAcctCode.Text;
                acct.AcctName = lbAcctName.Text;

                Dialog.SetReturnValue2(JsonConvert.SerializeObject(acct));
                Dialog.Close(this);
            }
            else
            {
                throw new Exception("尚未選擇費用類別");
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
            List<GLAccounts> list = service.QueryGLAccountByKeywordService(txtKeyword.Text);

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