<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopularBlogPostsWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.PopularBlogPostsWebPart" %>
<%@ Import namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server"/>
<asp:MultiView ID="mvNewBlogs" ActiveViewIndex="0" runat="server">
    <asp:View ID="vNewBlogs" runat="server">
    <ul class="list-group list-group-striped">
    <asp:Repeater ID="rptNewBlogs" runat="server">
        <ItemTemplate>
            <li class="list-group-item">
                <a class="tooltip-link" title="<%= Lang.Trans("Blog Name") %>"><b><%# Eval("BlogName") %></b></a>&nbsp;<i class="fa fa-angle-right text-muted"></i>&nbsp;<a href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username"), (int) Eval("BlogPostID")) %>' title="<%= Lang.Trans("Blog Post") %>"><%# Eval("BlogPostTitle") %></a>
                <ul class="info-header info-header-sm">
                    <li><a class="tooltip-link" title="<%= Lang.Trans("Date posted") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("DatePosted") %></a></li>
                    <li><a class="tooltip-link" title="<%= Lang.Trans("Posted by") %>"><i class="fa fa-user"></i></a>&nbsp;<a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a></li>
                </ul>
            </li>
        </ItemTemplate>
    </asp:Repeater>
    </ul>
    </asp:View>
    <asp:View ID="vNoNewBlogs" runat="server">
        <div class="text-center">
            <%= Lang.Trans("There are no blog posts!") %>
        </div>
    </asp:View>
</asp:MultiView>