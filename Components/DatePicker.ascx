<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="DatePicker.ascx.cs" Inherits="AspNetDating.Components.DatePicker" %>
<div id="divDatePicker" runat="server">
    <asp:DropDownList CssClass="form-control form-control-inline" ID="dropDay" runat="server" />
    <asp:DropDownList CssClass="form-control form-control-inline" ID="dropMonth" runat="server" />
    <asp:DropDownList CssClass="form-control form-control-inline" ID="dropYear" runat="server" />
</div>
