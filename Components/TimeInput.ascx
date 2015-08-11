<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeInput.ascx.cs" Inherits="AspNetDating.Components.TimeInput" %>
<asp:TextBox ID="txtTime" CssClass="form-control form-control-inline" Width="60" runat="server"/>
<asp:DropDownList CssClass="form-control form-control-inline" ID="ddAMPM" runat="server">
    <asp:ListItem>AM</asp:ListItem>
    <asp:ListItem>PM</asp:ListItem>
</asp:DropDownList>
<p class="form-control-static form-control-inline text-muted"><small><asp:Label ID="lblExample" runat="server"/></small></p>