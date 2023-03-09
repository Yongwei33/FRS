<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AcctUC.ascx.cs" Inherits="WKF_OptionalFields_AcctUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/CDS/FRS/AP/WKF_Fields/AcctUC_Item.ascx" TagPrefix="uc1" TagName="UC_Item" %>

<uc1:UC_Item runat="server" ID="UC_Item"/>

<asp:Label ID="lbAppGroupGuid" runat="server" Text="" Visible="False"></asp:Label>
<asp:Label ID="lbAppUserGuid" runat="server" Text="" Visible="False"></asp:Label>


<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>