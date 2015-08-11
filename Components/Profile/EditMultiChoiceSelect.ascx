<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditMultiChoiceSelect.ascx.cs" Inherits="AspNetDating.Components.Profile.EditMultiChoiceSelect" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc1" %>
<div id="pnlID" runat="server">
    <h4 ID="pnlUser" Runat="server"><uc1:HeaderLine ID="hlName" runat="server" /></h4>
    <h4 ID="pnlAdmin" Runat="server"><%= hlName.Title %></h4>
	<asp:Label ID="lblDescription" Runat="server" />
    <asp:listbox id=lbValues runat="server" CssClass="form-control" Rows="5" SelectionMode="Multiple" Width="100%"/>
	<p class="text-muted">
		<small><i class="fa fa-lightbulb-o"></i>&nbsp;<asp:Label ID="lblHint" Runat="server" /></small>
	</p>
    <INPUT id=hidQuestionId type=hidden runat="server">
</div>