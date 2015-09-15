<%@ Import namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Header.ascx.cs" Inherits="AspNetDating.Components.Header" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<%@ Register TagPrefix="uc1" TagName="LoginBox" Src="LoginBox.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>

<header>
    <div id="masthead" class="clearfix">
        <div class="logo">
            <a href='<%= ((PageBase)Page).CurrentUserSession == null? "Default.aspx":"Home.aspx" %>' title=''><img id="imgLogo" src="~/images/logo.png" runat="server"></a>
        </div>
        <div id="pnlLogin" class="pull-right" Runat="server">
            <uc1:LoginBox id="LoginBox1" runat="server" />
        </div>
        <div id="pnlLogout" class="pull-right" Runat="server">
            <ul class="nav navbar-nav navbar-right">
                <li id="liBroadcast" Visible="false" runat="server">
                    <a href="BroadcastVideo.aspx" title='<%= Lang.Trans("Broadcast Video") %>'><i class="fa fa-video-camera"></i>&nbsp;<span><%= Lang.Trans("Broadcast Video") %></span></a>
                </li>
                <li id="liAjaxChat" Visible="false" runat="server">
                    <asp:LinkButton ID="lnkAjaxChatPay" runat="server" onclick="lnkAjaxChatPay_Click"><i class="fa fa-comment"></i>&nbsp;<span><%= Lang.Trans("Chat") %></span></asp:LinkButton>
                    <a id="lnkAjaxChat" href="~/AjaxChat/ChatWindow.aspx" target="_ajaxchat" runat="server"><i class="fa fa-comment"></i>&nbsp;<span><%= Lang.Trans("Chat") %></span></a>
                </li>
                <li id="liBlog" runat="server">
                    <a href="Blogs.aspx"><i class="fa fa-rss"></i>&nbsp;<%= Lang.Trans("Blog") %></a>
                </li>
                <li id="liMailbox" runat="server">
                    <a href="Mailbox.aspx" title='<%= Lang.Trans("Mailbox") %>'><i class="fa fa-envelope"></i>&nbsp;<span><%= Lang.Trans("Mailbox") %><asp:Literal ID="ltrNewMessages" runat="server" /></span></a>
                    
                </li>
                <li id="liProfile" runat="server" class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown"><asp:Label id="lblWelcome" runat="server" />&nbsp;
                        <small class="text-muted"><asp:Label ID="lblCredits" runat="server" /></small>&nbsp;<i class="fa fa-caret-down"></i>
                    </a>
                    <ul class="dropdown-menu">
                        <li><i class="fa fa-caret-up fa-lg"></i></li>
                        <li><a href="Profile.aspx" title='<%= Lang.Trans("Profile") %>'><%= Lang.Trans("Profile") %></a></li>
                        <li><a href="Profile.aspx?sel=photos"><%= Lang.Trans("Manage Photos") %></a></li>
                        <li class="divider"></li>
                        <li id="liFavorites" runat="server"><a href="Favorites.aspx"><%= Lang.Trans("Favorites") %></a></li>
                        <li id="liFriends" runat="server"><a href="Friends.aspx"><%= Lang.Trans("Friends") %></a></li>
                        <li class="divider"></li>
                        <li><a href="Profile.aspx?sel=set"><%= Lang.Trans("Settings") %></a></li>
                        <li><a href="Profile.aspx?sel=credits"><%= Lang.Trans("Credits History") %></a></li>
                        <li><a href="Profile.aspx?sel=privacy"><%= Lang.Trans("Privacy Settings") %></a></li>
                        <li id="liSubscription" runat="server"><a href="Profile.aspx?sel=payment"><%= Lang.Trans("Billing") %></a></li>
                        <li class="divider"></li>
                        <li><asp:LinkButton id="lnkLogout" runat="server" onclick="lnkLogout_Click"><%= Lang.Trans("Log Out") %>&nbsp;&nbsp;<i class="fa fa-sign-out"></i></asp:LinkButton></li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
    <nav class="navbar navbar-default" role="navigation">
        <div class="container-fluid">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#header-navbar-collapse">
                   <span class="text-muted">toggle menu</span>&nbsp;<i class="fa fa-bars"></i>
                </button>
                <span>
                    <a class="navbar-brand" href='<%= ((PageBase)Page).CurrentUserSession == null? "Default.aspx":"Home.aspx" %>' title="<%= Lang.Trans("Home") %>"><i class="fa fa-home"></i></a>
                </span>
            </div>
            <!-- Collect the nav links, forms, and other content for toggling -->
            <div class="collapse navbar-collapse" id="header-navbar-collapse">
                <ul class="nav navbar-nav">
                    <li id="liHome" runat="server">
                        <a href='<%= ((PageBase)Page).CurrentUserSession == null? "Default.aspx":"Home.aspx" %>' title="<%= Lang.Trans("Home") %>"><i class="fa fa-home"></i></a>
                    </li>
                    <li id="liSearch" runat="server">
                        <a href="<%= Config.BackwardCompatibility.UseClassicSearchPage ? "Search.aspx" : "Search2.aspx" %>"><%= Lang.Trans("Search") %></a>
                    </li>
                    <li id="liTopFavorites" runat="server">
                        <a href="Favorites.aspx"><%= Lang.Trans("Favorites") %></a>
                    </li>
                    <li id="liGroups" runat="server">
                        <a href="Groups.aspx"><%= Lang.Trans("Groups") %></a>
                    </li>
                    <li id="liVideos" runat="server">
                        <a href="BrowseVideos.aspx"><%= Lang.Trans("Videos") %></a>
                    </li>
                    <li id="liRatePhotos" runat="server">
                        <a href="RatePhotos.aspx"><%= Lang.Trans("Rate photos") %></a>
                    </li>
                    <li id="liContests" Visible="false" runat="server">
                        <a href="PhotoContests.aspx"><%= Lang.Trans("Contests") %></a>
                    </li>
                    <li id="liTopCharts" runat="server">
                        <a href="TopCharts.aspx"><%= Lang.Trans("Top Charts") %></a>
                    </li>
                    <li id="liReviewNewProfiles" Visible="false" runat="server">
                        <a href="ReviewNewUsers.aspx"><%= Lang.Trans("Approve profiles") %></a>
                    </li>
                    <li id="liReviewNewPhotos" Visible="false" runat="server">
                        <a href="ReviewNewPhotos.aspx"><%= Lang.Trans("Approve photos") %></a>
                    </li>
                    <li id="liAds" runat="server">
                        <a href="Ads.aspx"><%= Lang.Trans("Classified Ads") %></a>
                    </li>
                    <asp:Repeater ID="rptPages" Runat="server">
                        <ItemTemplate>
                            <li id="liContentPage" data-id='<%# Eval("ID") %>' runat="server">
                                <a href='<%# Eval("URL") == null ? UrlRewrite.CreateContentPageUrl((int)Eval("ID")) : (string) Eval("URL")%>'><%# Eval("LinkText")%></a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div class="navbar-form navbar-right" id="pnlLanguage" runat="server">
                    <div class="form-group">
                        <asp:DropDownList ID="ddLanguages" CssClass="form-control input-sm" Runat="server" AutoPostBack="True" onselectedindexchanged="ddLanguages_SelectedIndexChanged" />
                    </div>
                </div>
            </div><!-- /.navbar-collapse -->
        </div><!-- /.container-fluid -->
    </nav>
</header>