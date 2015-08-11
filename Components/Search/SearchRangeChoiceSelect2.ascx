<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="SearchRangeChoiceSelect2.ascx.cs"
    Inherits="AspNetDating.Components.Search.SearchRangeChoiceSelect2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<div id="pnlID" enableviewstate="false" runat="server">
    <a href="#" onclick="return false;" class="expander">
        <asp:Literal ID="ltrName" runat="server" />
    </a>
    <div id="divExpandee" class="expandee" style="display: none" runat="server">
        <%= Lang.Trans("from") %><asp:DropDownList CssClass="form-control form-control-inline" ID="ddFrom" runat="server" />
        <%= Lang.Trans("to") %><asp:DropDownList CssClass="form-control form-control-inline" ID="ddTo" runat="server" />
    </div>
    <input type="hidden" id="hidQuestionId" runat="server" name="hidQuestionId" />
</div>
