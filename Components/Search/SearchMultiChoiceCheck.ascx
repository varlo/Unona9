<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchMultiChoiceCheck.ascx.cs" Inherits="AspNetDating.Components.Search.SearchMultiChoiceCheck" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" runat="server">
    <b><asp:Label ID="lblName" Runat="server" /></b>
    <div class="checkbox"><asp:CheckBoxList id="cbValues" runat="server" Width="100%" RepeatColumns="3"/></div>
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId">
</div>