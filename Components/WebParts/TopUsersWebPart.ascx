<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopUsersWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.TopUsersWebPart" %>
<%@ Import namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>

<h3 id="h4Daily" Runat="server" class="panel-title"><%= "Top daily".Translate() %></h3>
<asp:DataList ID="dlTopUsersDaily" Runat="server" SkinID="TopUsersHome" Width="100%" cellpadding="0" cellspacing="0" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="6">
    <ItemTemplate>
        <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
        <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a><br><%# Eval("Rating") %>
    </ItemTemplate>
</asp:DataList>
<h3 id="h4Weekly" Runat="server" class="panel-title"><%= "Top weekly".Translate() %></h3>
<asp:DataList ID="dlTopUsersWeekly" Runat="server" SkinID="TopUsersHome" Width="100%" cellpadding="0" cellspacing="0" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="6">
    <ItemTemplate>
        <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
        <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a><br><%# Eval("Rating") %>
    </ItemTemplate>
</asp:DataList>
<h3 id="h4Monthly" Runat="server" class="panel-title"><%= "Top monthly".Translate() %></h3>
<asp:DataList ID="dlTopUsersMonthly" Runat="server" SkinID="TopUsersHome" Width="100%" cellpadding="0" cellspacing="0" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="6">
    <ItemTemplate>
        <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
        <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a><br><%# Eval("Rating") %>
    </ItemTemplate>
</asp:DataList>
<h3 id="h4Yearly" Runat="server" class="panel-title"><%= "Top yearly".Translate() %></h3>
<asp:DataList ID="dlTopUsersYearly" Runat="server" SkinID="TopUsersHome" Width="100%" cellpadding="0" cellspacing="0" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="6">
    <ItemTemplate>
        <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
        <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a><br><%# Eval("Rating") %>
    </ItemTemplate>
</asp:DataList>

