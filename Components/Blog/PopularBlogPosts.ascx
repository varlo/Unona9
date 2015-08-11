<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopularBlogPosts.ascx.cs" Inherits="AspNetDating.Components.Blog.PopularBlogPosts" %>

<%@ Import namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"/>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server"/>
<asp:Repeater ID="rptNewBlogs" runat="server">
    <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
    <ItemTemplate>
        <li class="list-group-item">
            <div class="media">
                <a class="pull-left" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' title='<%# Eval("Username") %>'><%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 50, 50, "img-thumbnail", true, true, true) %></a>
                <div class="media-body">
                    <div class="form-group">
                        <label><%= Lang.Trans("Blog") %></label>
                        <%# Eval("BlogName") %>
                    </div>
                    <div class="form-group">
                        <label><%= Lang.Trans("Blog Post") %></label>
                        <a href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username"), (int) Eval("BlogPostID")) %>'><%# Eval("BlogPostTitle") %></a>
                    </div>
                    <small class="text-muted"><a class="tooltip-link" title="<%= Lang.Trans("Date Posted") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("DatePosted") %></a></small>
                </div>
            </div>
	    </li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>