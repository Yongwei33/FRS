using Ede.Uof.Utility.Page.Common;
using FRS.Lib.Utils;
using FRS.Lib.Model.AP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class CDS_FRS_AP_WKF_Fields_Acct_ItemUC : System.Web.UI.UserControl
{
    public bool Enabled
    {
        get
        {
            if (ViewState["Enabled"] != null)
                return Convert.ToBoolean(ViewState["Enabled"]);
            else
                return false;
        }
        set
        {
            ViewState["Enabled"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetButtonLink();
            lbtnNew.Visible = this.Enabled;
        }
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        string dialogRtnVal = Dialog.GetReturnValue();
        UpdateRow(dialogRtnVal); //更新GridView
    }

    protected void lbtnDelete_Command(object sender, CommandEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.CommandArgument as string))
        {
            int seq = Convert.ToInt32(e.CommandArgument);
            List<APGridItem> totalRow = this.GetRowData();
            foreach (var row in totalRow)
            {
                if (row.Seq == seq)
                {
                    totalRow.Remove(row);
                    break;
                }
            }
            BindGrid(totalRow);
        }
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridViewRow row = e.Row;
            APGridItem obj = (APGridItem)row.DataItem;

            //列資料轉換為JSON格式，儲存於HiddenField內
            //HiddenField hfJsonRowData = (HiddenField)row.FindControl("hfJsonRowData");
            //hfJsonRowData.Value = JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            //再將列資料ID儲存於Session內，傳到新增/編輯新窗
            //string dataClientId = hfJsonRowData.ClientID;
            //Session[dataClientId] = hfJsonRowData.Value;
            //LinkButton lbtnEdit = (LinkButton)row.FindControl("lbtnEdit");
            //ExpandoObject param1 = new
            //{
            //    updateType = "edit",
            //    dataClientId
            //}.ToExpando();
            //Dialog.Open2(lbtnEdit, "~/CDS/FRS/AP/WKF_Fields/Dialog/AcctDialog.aspx", "編輯費用項目", 600, 600, Dialog.PostBackType.AfterReturn, param1);

            //金額
            TextBox txtPrice = (TextBox)row.FindControl("txtPrice"); //單價，不含千分號
            Label lbPrice = (Label)row.FindControl("lbPrice"); //單價，含千分號
            txtPrice.Visible = this.Enabled;
            lbPrice.Text = UIUtils.GetAmtText(txtPrice.Text);
            lbPrice.Visible = !this.Enabled;

            //操作欄位
            gvMain.Columns[3].Visible = this.Enabled; //僅申請者看得到
        }
    }

    public XElement GetFormValue()
    {
        XElement xe = new XElement("Acct");
        foreach (GridViewRow row in gvMain.Rows)
        {
            var item = Row2Object(row);
            var el = XmlUtils.ToXElement<APGridItem>(item); // Object2Xml(item);
            xe.Add(el);
        }

        return xe;
    }

    public void SetFormValue(XElement xe)
    {
        if (xe == null)
            xe = new XElement("Acct");

        List<APGridItem> list = this.Xml2Data(xe.ToString());

        BindGrid(list);
    }

    protected void SetButtonLink()
    {
        Dialog.Open2(lbtnNew, "~/CDS/FRS/AP/WKF_Fields/Dialog/AcctDialog.aspx", "新增費用項目", 600, 700, Dialog.PostBackType.AfterReturn, null);

    }

    private void BindGrid(List<APGridItem> list)
    {    
        //資料儲存於SESSION內
        Session["AP_GridItem"] = list;

        ReorderSeq(list);

        gvMain.DataSource = list;
        gvMain.DataBind();

        //判斷細項是否有填,供送出判斷驗證
        A99.Text = (list != null && list.Count > 0) ? Convert.ToString(list.Count) : "";

    }

    

    List<APGridItem> Xml2Data(string xml)
    {
        List<APGridItem> data = new List<APGridItem>();
        if (!string.IsNullOrEmpty(xml))
        {
            XElement xeList = XElement.Parse(xml);
            foreach (XElement row in xeList.Elements())
            {
                data.Add(XmlUtils.FromXElement<APGridItem>(row));
            }
        }
        return data;
    }

    public void UpdateRow(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        APGridItem obj = JsonConvert.DeserializeObject<APGridItem>(json);

        List<APGridItem> totalRow = this.GetRowData();

        obj.Seq = GetMaxSeq(totalRow);
        totalRow.Add(obj);

        BindGrid(totalRow);
    }

    public List<APGridItem> GetRowData()
    {
        var gvData = new List<APGridItem>();

        foreach (GridViewRow row in gvMain.Rows)
        {
            gvData.Add(this.Row2Object(row));
        }
        return gvData;
    }

    private APGridItem Row2Object(GridViewRow row)
    {
        APGridItem obj = new APGridItem();
        obj.Seq = Convert.ToInt32(UIUtils.GetLabelText(row, "lbSeq")); //項次
        obj.AcctCode = UIUtils.GetHiddenValue(row, "hfAcctCode"); //1.品名主項
        obj.AcctName = UIUtils.GetLabelText(row, "lbAcctName"); //2.品名次項
        obj.Price =UIUtils.GetTextBoxValue(row, "txtPrice"); //4.參考單價(不含千分號)

        return obj;
    }

    //新增取得最大序號
    public int GetMaxSeq(List<APGridItem> data)
    {
        int seq = 0;
        foreach (APGridItem row in data)
        {
            if (row.Seq > seq) seq = row.Seq;
        }
        return ++seq;
    }

    //序號重新排序
    private void ReorderSeq(List<APGridItem> list)
    {
        int start = 1;
        foreach (var item in list)
        {
            if (item.Seq != start)
            {
                item.Seq = start;
            }
            start++;
        }
    }
}