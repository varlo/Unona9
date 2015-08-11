<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchSingleChoice.ascx.cs" Inherits="AspNetDating.Components.Search.SearchSingleChoice" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" runat="server">
    <b><asp:Label ID="lblName" Runat="server" /></b>
    <asp:DropDownList id="dropValues" CssClass="form-control" runat="server"/>
    <INPUT id="hidQuestionId" type="hidden" runat="server">
</div>