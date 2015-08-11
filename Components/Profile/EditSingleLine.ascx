<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditSingleLine.ascx.cs" Inherits="AspNetDating.Components.Profile.EditSingleLine" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
	<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc1" %>
<div id="pnlID" runat="server">
	<h4 id="tdUser" runat="server"><uc1:HeaderLine ID="hlName" runat="server" /></h4>
	<h4 id="tdAdmin" runat="server"><%= hlName.Title %></h4>
	<asp:Label ID="lblDescription" Runat="server" />
	<asp:TextBox ID="txtValue" TextMode="SingleLine" MaxLength="80" CssClass="form-control" Runat="server" />
    <input type="hidden" id="hidQuestionId" runat="server">
	<p class="text-muted">
		<small><i class="fa fa-lightbulb-o"></i>&nbsp;<asp:Label ID="lblHint" Runat="server" /></small>
	</p>
</div>
