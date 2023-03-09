<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="BPDialog.aspx.cs" Inherits="CDS_FRS_AP_WKF_Fields_Dialog_BPDialog" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <table class="PopTable tableField" style="width:100%" >
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label1" runat="server" Text="供應商"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:TextBox ID="txtKeyword" runat="server" Width="100%" PlaceHolder="輸入供應商代號或名稱關鍵字"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableRightTD" style="text-align:left" colspan="2"> 
                            <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" CausesValidation="false"/>
                            <asp:CustomValidator ID="CV_FORM" runat="server" ErrorMessage="" Display="Dynamic"></asp:CustomValidator>
                        </td>
                    </tr>
                </table>
            </div>

        <div style="padding-top:20px">
            <Ede:Grid ID="gvMain" runat="server" 
                AutoGenerateCheckBoxColumn="False"
                AllowSorting="false" AllowPaging="false" 
                AutoGenerateColumns="false"
                DataKeyOnClientWithCheckBox="False" 
                DefaultSortDirection="Ascending"
                DataKeyNames="CardCode"
                ShowHeaderWhenEmpty="true"
                EmptyDataText="沒有資料" 
                EnhancePager="True"
                KeepSelectedRows="False" 
                OnRowDataBound="gvMain_RowDataBound"
                OnPageIndexChanging="gvMain_PageIndexChanging"
                Width="100%" BorderColor="White">
                <SelectedRowStyle BackColor="#00ff00" ForeColor="White" />
                <Columns>

                    <asp:TemplateField HeaderText="代碼" Headerstyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbCardCode" runat="server" Text='<%#Bind("CardCode") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="15%"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="名稱" Headerstyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbCardName" runat="server" Text='<%#Bind("CardName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="15%"/>
                    </asp:TemplateField>
                </Columns>
            </Ede:Grid>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>







</asp:Content>

