using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FRS.Lib.Utils
{
   public static class UIUtils
    {

        public static void AddBlank(this DropDownList dd)
        {
            dd.Items.Clear();
            dd.Items.Add(new ListItem() { Value = "", Text = "--" });
        }

        //金額加千分位,小數後兩點
        public static string GetAmtText(string text)
        {
            return GetFormatText(text, "#,0.##");
        }

        //數量加千分位,無小數
        public static string GetQtyText(string text)
        {
            return GetFormatText(text, "#,0");
        }

        public static string GetFormatText(string text, string format)
        {
            decimal amount;
            string result = "";
            if (decimal.TryParse(text, out amount))
            {
                result = string.Format("{0:" + format + "}", amount);
            }

            return result;
        }

        //回傳金額無千分位
        public static string RtnValueWithoutComma(string text)
        {
            if (text.Contains(','))
            {
                text = text.Replace(",", "");
            }
            return text;
        }


        public static string GetLabelText(GridViewRow row, string lableName)
        {
            Label lb = row.FindControl(lableName) as Label;
            if (lb == null)
            {
                if (lableName == "lbSeq")
                {
                    return "0";
                }
                else
                    return "";
            }

            return lb.Text;
        }

        public static string GetHiddenValue(GridViewRow row, string hfName)
        {
            HiddenField hf = row.FindControl(hfName) as HiddenField;
            if (hf == null)
            {
                return "";
            }
            return hf.Value;
        }

        public static string GetDropDownListValue(GridViewRow row, string ddlName)
        {
            DropDownList ddl = row.FindControl(ddlName) as DropDownList;
            if (ddlName == null)
            {
                return "";
            }
            return ddl.SelectedValue;
        }

        public static string GetTextBoxValue(GridViewRow row, string ctrlName)
        {
            TextBox txt = row.FindControl(ctrlName) as TextBox;
            if (txt == null)
            {
                return "";
            }
            return txt.Text;
        }

        public static DateTimeOffset String2DateTimeOffSet(string dateTime)
        {

            DateTimeOffset result;
            if (DateTimeOffset.TryParse(dateTime, out result))
            {
                result = DateTimeOffset.Parse(dateTime);
            }
            return result;

        }

        public static string DateTimeOffSet2String(DateTimeOffset dateTime)
        {
            if (dateTime != null)
                return dateTime.ToString("yyyy/MM/dd HH:mm");
            else
                return "";

        }

    }


}
