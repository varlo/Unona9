<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewVideosWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.NewVideosWebPart" %>
<asp:MultiView ID="mvNewVideos" ActiveViewIndex="0" runat="server">
    <asp:View ID="vNewVideos" runat="server">
    <asp:DataList CssClass="repeater-horizontal" RepeatLayout="Flow" ID="dlNewVideos" Runat="server" SkinID="NewVideosHome" onitemcreated="dlNewVideos_ItemCreated">
	    <ItemTemplate>
		    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
		        <img src='<%# Eval("ThumbnailUrl") %>' style="behavior: none" />
                    <div class="caption">
                    <%# Eval("Username") %>
                    <div class="text-muted">
                        <span id="pnlGenderAge" runat="server" ><%# Lang.Trans(Eval("Gender").ToString()) %><span id="pnlDelimiter" runat="server">/</span><%# Eval("Age") %></span>
                    </div>
                </div>
		    </a>
	    </ItemTemplate>
    </asp:DataList>
    </asp:View>
    <asp:View ID="vNoNewVideos" runat="server">
        <div class="text-center">
            <%= Lang.Trans("There are no new videos!") %>
        </div>
    </asp:View>
</asp:MultiView>