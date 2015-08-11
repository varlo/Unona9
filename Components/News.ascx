<%@ Control Language="c#" AutoEventWireup="True" Codebehind="News.ascx.cs" Inherits="AspNetDating.Components.News" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Repeater ID="rptNews" Runat="server" EnableViewState="false">
    <HeaderTemplate><ul class="list-group"></HeaderTemplate>
    <ItemTemplate>
        <li class="list-group-item">
            <a href='<%# "News.aspx?id=" + Eval("NID") %>'><%# Eval("Title") %></a>
            <div><small class="text-muted"><%# ((DateTime)Eval("Date")).ToShortDateString() %></small></div>
        </li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
