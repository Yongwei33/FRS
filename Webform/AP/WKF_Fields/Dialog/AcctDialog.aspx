<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="AcctDialog.aspx.cs" Inherits="CDS_FRS_AP_WKF_Fields_Dialog_AcctDialog" %>
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
            <asp:TextBox ID="txtKeyword" runat="server" Width="200px" PlaceHolder="輸入會科代碼或名稱關鍵字"></asp:TextBox>
            <telerik:RadButton ID="btnSearch" runat="server" Text="查詢" CausesValidation="false" UseSubmitBehavior="false" OnClick="btnSearch_Click">
                <%--<Icon PrimaryIconUrl="~/Common/Images/Icon/icon_m39.gif" />--%>
            </telerik:RadButton>
            <asp:CustomValidator ID="CV_FORM" runat="server" ErrorMessage="" Display="Dynamic"></asp:CustomValidator>
            <asp:HiddenField ID="hfAcctName" runat="server" />
            <asp:HiddenField ID="hfAcctCode" runat="server" />
        </div>
        <div style="padding-top:10px">
            <table width="100%">
                <tr>
                    <td>
                        <Ede:Grid ID="gvMain" runat="server" 
                                AutoGenerateCheckBoxColumn="False"
                                AllowPaging="true" 
                                PageSize="15"
                                AutoGenerateColumns="false"
                                DataKeyOnClientWithCheckBox="False" 
                                DataKeyNames="AcctCode"
                                ShowHeaderWhenEmpty="true"
                                EmptyDataText="沒有資料" 
                                EnhancePager="True"
                                KeepSelectedRows="False" 
                                OnRowDataBound="gvMain_RowDataBound"
                                OnPageIndexChanging="gvMain_PageIndexChanging"
                                Width="100%" >
                            <SelectedRowStyle BackColor="#00ff00" ForeColor="White" />
                            <Columns>

                                <asp:TemplateField HeaderText="會科代碼" Headerstyle-HorizontalAlign="Center" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbAcctCode" runat="server" Text='<%#Bind("AcctCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="200px"/>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="會科名稱" Headerstyle-HorizontalAlign="Center" HeaderStyle-Width="70%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbAcctName" runat="server" Text='<%#Bind("AcctName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="150px"/>
                                </asp:TemplateField>

                            </Columns>
                        </Ede:Grid>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>



</asp:Content>

