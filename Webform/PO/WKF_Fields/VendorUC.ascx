<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VendorUC.ascx.cs" Inherits="WKF_OptionalFields_VendorUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
		<style type="text/css">
		.rightAlign { text-align:right; }
        </style>
        <div>
            <telerik:RadButton ID="btnBP" runat="server" Text="查詢供應商" CausesValidation="false" UseSubmitBehavior="false" OnClick="btnBP_Click" Width="130px">
                <Icon PrimaryIconUrl="~/Common/Images/Icon/icon_m39.gif" />
            </telerik:RadButton>
            <asp:TextBox ID="txtBP" runat="server" Text="" ReadOnly="true" Width="200px"></asp:TextBox>
        </div>
        <asp:CustomValidator ID="CV_FORM" runat="server" ErrorMessage="" Display="Dynamic"></asp:CustomValidator>
        </ContentTemplate>
    </asp:UpdatePanel>


<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>