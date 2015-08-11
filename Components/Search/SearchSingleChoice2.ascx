<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchSingleChoice2.ascx.cs" Inherits="AspNetDating.Components.Search.SearchSingleChoice2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" enableviewstate="false" runat="server">
    <a href="#" onclick="return false;" class="expander">
        <asp:Literal ID="ltrName" Runat="server" />
    </a>
    <div id="divExpandee" class="expandee" style="display: none" runat="server">
        <asp:DropDownList CssClass="form-control" id="dropValues" runat="server"/>
    </div>
    <INPUT id="hidQuestionId" type="hidden" runat="server">
</div>