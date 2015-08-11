<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditSingleChoiceRadio.ascx.cs" Inherits="AspNetDating.Components.Profile.EditSingleChoiceRadio" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
	<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc1" %>
<div id="pnlID" runat="server">
	<div ID="pnlUser" Runat="server">
		<h4><uc1:HeaderLine ID="hlName" runat="server" /></h4>
	</div>
	<div ID="pnlAdmin" Runat="server">
		<h4><%= hlName.Title %></h4>
	</div>

    <asp:Label ID="lblDescription" Runat="server" />
    <asp:radiobuttonlist RepeatColumns="3" id="rbValues" runat="server"/>

    <INPUT id="hidQuestionId" type="hidden" runat="server">
	<p class="text-muted">
		<small><i class="fa fa-lightbulb-o"></i>&nbsp;<asp:Label ID="lblHint" Runat="server" /></small>
	</p>
</div>
