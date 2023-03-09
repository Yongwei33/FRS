<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="ItemDialog.aspx.cs" Inherits="CDS_FRS_AP_WKF_Fields_Dialog_ItemDialog" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        .tableField {
            width: 350px;
            margin: 0 auto;
            padding: 10px;
        }7
    </style>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div>
            <telerik:RadButton ID="btnBP" runat="server" Text="查詢供應商" CausesValidation="false" UseSubmitBehavior="false" OnClick="btnBP_Click" Width="130px">
                <Icon PrimaryIconUrl="~/Common/Images/Icon/icon_m39.gif" />
            </telerik:RadButton>
            <asp:TextBox ID="txtBP" runat="server" Text="" ReadOnly="true" Width="200px" style="border:none"></asp:TextBox>
            <asp:HiddenField ID="hfCardName" runat="server" />
            <asp:HiddenField ID="hfCardCode" runat="server" />
        </div>
        <div style="padding-top:10px">
            <table width="100%">
                <tr>
                    <td>
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
                                PageSize="10"
                                OnRowDataBound="gvMain_RowDataBound"
                                OnPageIndexChanging="gvMain_PageIndexChanging"
                                Width="100%" BorderColor="White">
                            <SelectedRowStyle BackColor="#00ff00" ForeColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="匯入銀行" Headerstyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDispCode" runat="server" Text='<%#Bind("DispCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="150px"/>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="匯入銀行名稱" Headerstyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDispName" runat="server" Text='<%#Bind("DispName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px"/>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="匯入戶名" Headerstyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbAcctName" runat="server" Text='<%#Bind("AcctName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px"/>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="匯入帳號" Headerstyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbBnkAccount" runat="server" Text='<%#Bind("BnkAccount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="250px"/>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="隱藏欄位" Headerstyle-HorizontalAlign="Center" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbCardCode" runat="server" Text='<%#Bind("CardCode") %>'/>
                                        <asp:Label ID="lbCardName" runat="server" Text='<%#Bind("CardName") %>'/>
                                        <asp:Label ID="lbBnkBranch" runat="server" Text='<%#Bind("BnkBranch") %>'/>
                                        <asp:Label ID="lbBankCode" runat="server" Text='<%#Bind("BankCode") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="200px"/>
                                </asp:TemplateField>
                            </Columns>
                        </Ede:Grid>
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="CV_FORM" runat="server" ErrorMessage="" Display="Dynamic"></asp:CustomValidator>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>






</asp:Content>

