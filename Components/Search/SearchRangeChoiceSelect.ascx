<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchRangeChoiceSelect.ascx.cs" Inherits="AspNetDating.Components.Search.SearchRangeChoiceSelect" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" runat="server">
    <b><asp:Label ID="lblName" Runat="server" /></b>
    &nbsp;<%= Lang.Trans("from") %>&nbsp;<asp:DropDownList CssClass="form-control form-control-inline" ID="ddFrom" Runat="server" />
    &nbsp;<%= Lang.Trans("to") %>&nbsp;<asp:DropDownList CssClass="form-control form-control-inline" ID="ddTo" Runat="server" />
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId">
</div>
