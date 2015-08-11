<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="c#" EnableViewState="false" AutoEventWireup="True" CodeBehind="NewUsers.ascx.cs" Inherits="AspNetDating.Components.NewUsers" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div class="panel panel-members">
    <div class="panel-heading">
        <h3 class="panel-title"><asp:Label ID="lblNewUsersBoxHeader" runat="server" /></h3>
       <!-- <asp:Label ID="lblNewUsersBoxTitle" runat="server" />-->
    </div>
    <div class="panel-body entry-content">
        <div class="users-wrap">
        <asp:DataList ID="dlNewMembers" CssClass="repeater-horizontal" runat="server" RepeatLayout="Flow" onitemcreated="dlNewMembers_ItemCreated">
            <ItemTemplate>
                <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' class="thumbnail">
                <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 100, 100, null, false, true, true) %>
                <div class="caption">
                    <%# Eval("Username") %>
                    <div class="text-muted">
                        <span id="pnlGenderAge" runat="server" ><%# Lang.Trans(Eval("Gender").ToString()) %></span>/<span id="pnlDelimiter" runat="server"><%# Eval("Age") %><%# Eval("Age2") != String.Empty ? "," + Eval("Age2") : string.Empty %></span>
                    </div>
                </div>
                </a>
            </ItemTemplate>
        </asp:DataList>

    </div>
</div>
