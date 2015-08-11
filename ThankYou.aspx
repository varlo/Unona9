<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="ThankYou.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.ThankYou" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <uc1:WideBoxStart id="WideBoxStart1" runat="server"/>
	    <asp:Label ID="lblMessage" runat="server"/>
    <uc1:WideBoxEnd id="WideBoxEnd1" runat="server"/>
</asp:Content>