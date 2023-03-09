<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AcctUC_Item.ascx.cs" Inherits="CDS_FRS_AP_WKF_Fields_Acct_ItemUC" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
		<style type="text/css">
		.rightAlign { text-align:right; }
        </style>	
        <telerik:RadButton ID="lbtnNew" runat="server" Text="新增" CausesValidation="false" UseSubmitBehavior="false" Width="80px" OnClick="lbtnNew_Click">
            <Icon PrimaryIconUrl="~/Common/Images/Icon/icon_m71.png" />
        </telerik:RadButton>
        <asp:CustomValidator ID="CV_FORM" runat="server" ErrorMessage="" Display="Dynamic"></asp:CustomValidator>

        <asp:TextBox ID="A99" runat="server" style="display:none"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RF_Items" runat="server" ErrorMessage="項目尚未新增" ControlToValidate="A99" Display="Dynamic"></asp:RequiredFieldValidator>

        <div>  
            <Ede:Grid ID="gvMain" runat="server" 
                AutoGenerateCheckBoxColumn="False"
                AllowSorting="false" AllowPaging="false" 
                AutoGenerateColumns="false"
                DataKeyOnClientWithCheckBox="False" 
                DefaultSortDirection="Ascending"
                DataKeyNames="Seq"
                ShowHeaderWhenEmpty="true"
                EmptyDataText="沒有資料" 
                EnhancePager="True"
                KeepSelectedRows="False" 
                OnRowDataBound="gvMain_RowDataBound"
                Width="60%" BorderColor="White">
                <Columns>
                    <asp:TemplateField HeaderText="項次" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                        <ItemTemplate>                
                            <asp:Label ID="lbSeq" runat="server" Text='<%#Bind("Seq") %>'></asp:Label>                
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="費用類別" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                        <ItemTemplate>                
                            <asp:Label ID="lbAcctName" runat="server" Text='<%#Bind("AcctName") %>'></asp:Label>
                            <asp:HiddenField ID="hfAcctCode" runat="server" Value='<%#Bind("AcctCode") %>'/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="金額" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPrice" runat="server" Text='<%#Bind("Price") %>' Width="80%" CssClass="rightAlign"></asp:TextBox> <%--不含千分號--%>
                            <asp:Label ID="lbPrice" runat="server" Visible="false"></asp:Label> <%--含千分號--%>
                            <asp:RequiredFieldValidator ID="RF_Price" runat="server" ErrorMessage="必填欄位" ControlToValidate="txtPrice" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="REV_Price" runat="server" ErrorMessage="輸入數字格式錯誤" ValidationExpression="^[0-9]+(.[0-9]{1,3})?$" ControlToValidate="txtPrice" Display="Dynamic"></asp:RegularExpressionValidator>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfJsonRowData" runat="server" /> <%--每列資料(JSON格式)--%>
        <%--                    <img id="Img2" src="~/Common/Images/Icon/icon_m02.png" runat="server" style="margin-left: 4px" border="0"/>
                            <asp:LinkButton ID="lbtnEdit" runat="server" Text="編輯" CausesValidation="false" OnClick="lbtnNew_Click"></asp:LinkButton>--%>
                            <img id="Img3" src="~/Common/Images/Icon/icon_m03.png" runat="server" style="margin-left: 4px" border="0" />
                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="刪除" CommandName="DeleteRow" CommandArgument='<%# Eval("Seq") %>' OnCommand="lbtnDelete_Command" OnClientClick='return deleteConfirm()' CausesValidation="false"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%"/>
                    </asp:TemplateField>
                </Columns>
                <%--<FooterStyle HorizontalAlign="Center" />--%>
            </Ede:Grid>
        </div>
        <%--<div>
            <table class="PopTable">
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label1" runat="server" Text="請購項目預估總金額"></asp:Label>
                    </td>
                    <td class="PopTableRightTD" colspan="3">
                        <asp:Label ID="lbA08" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>--%>


    </ContentTemplate>
</asp:UpdatePanel>



<script>
    function deleteConfirm() {
        return confirm('確認刪除?');
    }
</script>