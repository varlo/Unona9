<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimilarProfiles.ascx.cs" Inherits="AspNetDating.Components.SimilarProfiles" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="SmallBoxEnd.ascx" %>
<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:SmallBoxStart ID="SmallBoxStart1" runat="server" />

<asp:DataList  ID="dlSimilarProfiles" CssClass="repeater-horizontal" runat="server" RepeatLayout="Flow" onitemcreated="dlSimilarProfiles_ItemCreated">
    <ItemTemplate>
        <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' class="thumbnail">
            <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 100, 100, "", false, true, true) %>
            <div class="caption">
                <%# Eval("Username") %>
                <div class="text-muted">
                    <span id="pnlGenderAge" runat="server" ><%# Lang.Trans(Eval("Gender").ToString()) %><span id="pnlDelimiter" runat="server">/</span><%# Eval("Age") %><%# Eval("Age2") != String.Empty ? "," + Eval("Age2") : string.Empty %></span>
                </div>
            </div>
        </a>
    </ItemTemplate>
</asp:DataList>
<uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server" />