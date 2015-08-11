<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewUsersWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.NewUsersWebPart" %>
<%@ Import namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:DataList CssClass="repeater-horizontal" ID="dlNewMembers" Runat="server" SkinID="NewUsersHome" RepeatLayout="Flow" onitemcreated="dlNewMembers_ItemCreated">
    <ItemTemplate>
        <a class="thumbnail" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
            <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 100, 100, null, false, true, true) %>
            <div class="caption">
                <%# Eval("Username") %>
                <div class="text-muted">
                    <span id="pnlGenderAge" runat="server" ><%# Lang.Trans(Eval("Gender").ToString()) %><span id="pnlDelimiter" runat="server">/</span><%# Eval("Age") %><%# Eval("Age2") != String.Empty ? "," + Eval("Age2") : string.Empty %></span>
                </div>
            </div>
        </a>
    </ItemTemplate>
</asp:DataList>