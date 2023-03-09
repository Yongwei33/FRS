<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BPUC.ascx.cs" Inherits="WKF_OptionalFields_BPUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>


<telerik:RadButton ID="btnSearch" runat="server" Text="新增付款資訊" CausesValidation="false" UseSubmitBehavior="false" Width="130px" OnClick="btnSearch_Click">
    <Icon PrimaryIconUrl="~/Common/Images/Icon/icon_m71.png" />
</telerik:RadButton>
<asp:RequiredFieldValidator ID="RF_Items" runat="server" ErrorMessage="尚未選擇供應商" ControlToValidate="lbCardName" Display="Dynamic"></asp:RequiredFieldValidator>
<asp:CustomValidator ID="CV_FORM" runat="server" ErrorMessage="" Display="Dynamic"></asp:CustomValidator>


<table class="PopTable" style="width:80%">
    <tr>
        <td class="PopTableLeftTD" style="text-align:center; width:20%">
            <asp:Label ID="Label1" runat="server" Text="供應商"></asp:Label>
        </td>
        <td class="PopTableRightTD" colspan="3" style="width:80%">
            <asp:TextBox ID="lbCardName" runat="server" Text="　" ReadOnly="true" style="border:none"></asp:TextBox>
            <asp:HiddenField ID="hfCardCode" runat="server" />
            <asp:HiddenField ID="hfBankCode" runat="server" />
            <asp:HiddenField ID="hfBnkBranch" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="PopTableLeftTD" style="text-align:center; width:20%">
            <asp:Label ID="Label2" runat="server" Text="匯入銀行"></asp:Label>
        </td>
        <td class="PopTableLeftTD" style="text-align:center; width:20%">
            <asp:Label ID="Label4" runat="server" Text="匯入銀行名稱"></asp:Label>
        </td>
        <td class="PopTableLeftTD" style="text-align:center; width:20%">
            <asp:Label ID="Label5" runat="server" Text="匯入戶名"></asp:Label>
        </td>
        <td class="PopTableLeftTD" style="text-align:center; width:20%">
            <asp:Label ID="Label3" runat="server" Text="匯入帳號"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="PopTableRightTD" style="text-align:center; width:20%">
            <asp:Label ID="lbDispCode" runat="server" Text="　"></asp:Label>
        </td>
        <td class="PopTableRightTD" style="text-align:center; width:20%">
            <asp:Label ID="lbDispName" runat="server" Text="　"></asp:Label>
        </td>
        <td class="PopTableRightTD" style="text-align:center; width:20%">
            <asp:Label ID="lbAcctName" runat="server" Text="　"></asp:Label>
        </td>
        <td class="PopTableRightTD" style="text-align:center; width:20%">
            <asp:Label ID="lbBnkAccount" runat="server" Text="　"></asp:Label>
        </td>
    </tr>
</table>

<asp:Label ID="lbAppGroupGuid" runat="server" Text="" Visible="False"></asp:Label>
<asp:Label ID="lbAppUserGuid" runat="server" Text="" Visible="False"></asp:Label>


<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>

<%--<table class="PopTable">
    <tr>
        <td class="PopTableLeftTD">
            <asp:Label ID="Label6" runat="server" Text="供應商"></asp:Label>
        </td>
        <td class="PopTableRightTD" colspan="5">
            <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
            <asp:HiddenField ID="HiddenField1" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="PopTableLeftTD">
            <asp:Label ID="Label8" runat="server" Text="銀行代號"></asp:Label>
        </td>
        <td class="PopTableRightTD">
            <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
        </td>
        <td class="PopTableLeftTD">
            <asp:Label ID="Label10" runat="server" Text="分行"></asp:Label>
        </td>
        <td class="PopTableRightTD">
            <asp:Label ID="Label11" runat="server" Text=""></asp:Label>
        </td>        
    </tr>
    <tr>
         <td class="PopTableLeftTD">
            <asp:Label ID="Label12" runat="server" Text="銀行帳戶"></asp:Label>
        </td>
        <td class="PopTableRightTD">
            <asp:Label ID="Label13" runat="server" Text=""></asp:Label>
        </td>
        <td class="PopTableLeftTD">
            <asp:Label ID="Label14" runat="server" Text="國家"></asp:Label>
        </td>
        <td class="PopTableRightTD">
            <asp:Label ID="Label15" runat="server" Text=""></asp:Label>
        </td>
    </tr>
</table>--%>