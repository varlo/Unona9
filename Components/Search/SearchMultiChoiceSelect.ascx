<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchMultiChoiceSelect.ascx.cs" Inherits="AspNetDating.Components.Search.SearchMultiChoiceSelect" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" runat="server">
	<b><asp:Label ID="lblName" Runat="server" /></b>
	<asp:ListBox id="lbValues" CssClass="form-control" SelectionMode=Multiple runat="server"/>
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId">
</div>
