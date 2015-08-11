<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Friends.aspx.cs" Inherits="AspNetDating.Friends" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
	    <components:BannerView id="bvFriendsLeft" runat="server" Key="FriendsLeft"/>&nbsp;
	</aside>
	<article>
	    <div id="pnlRelationshipRequests" runat="server">
	        <uc1:LargeBoxStart ID="LargeBoxStart3" runat="server" />
		    <asp:DataList ID="dlRelationshipRequests" Runat="server" CssClass="table table-striped" onitemcommand="dlRelationshipRequests_ItemCommand" onitemcreated="dlRelationshipRequests_ItemCreated">
			    <ItemTemplate>
			        <div class="media">
                        <a class="pull-left thumbnail media-object" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server" id="A1">
                            <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 100, 100, "", true, true, true) %>
                        </a>
                        <div class="media-body">
                            <div class="clearfix">
                                <h4 class="media-heading pull-left"><%# Eval("Username") %></h4>
                                <div class="pull-right">
                                    <asp:LinkButton ID="lnkAccept" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Accept" class="tooltip-go" data-toggle="tooltip" data-placement="bottom">
                                        <i class="fa fa-check-square fa-lg"></i>
                                    </asp:LinkButton>&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkReject" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Reject" class="tooltip-go" data-toggle="tooltip" data-placement="bottom">
                                        <i class="fa fa-times fa-lg"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <ul class="info-header info-header-sm">
                                <li>
                                   <label id="pnlAge" runat="server"><%= Lang.Trans("Age") %></label>&nbsp;<span id="pnlAgeValue" runat="server"><%# Eval("Age") %></span>
                                </li>
                            <% if (showRating) { %>
                                <li>
                                    <label><%= Lang.Trans("Rating") %></label>&nbsp;<%# Eval("Rating") %>
                                </li>
                                <li>
                                    <label><%= Lang.Trans("Relationship") %></label>&nbsp;<%# Eval("Type") %>
                                </li>
                            <% } %>
                            </ul>

                            <% if (showSlogan) { %>
                                <%# ((string) Eval("Slogan")).Length > 80 ? ((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan") %>
                            <% } %>

                            <% if (showLastOnline) { %>
                            <div class="text-right text-muted small">
                                 <i class="fa fa-sign-in"></i>&nbsp;<label><%= Lang.Trans("Last Online") %></label>&nbsp;<%# Eval("LastOnlineString") %>
                            </div>
                            <% } %>
                        </div>
		    	    </div>
			    </ItemTemplate>
		    </asp:DataList>
		    <uc1:LargeBoxEnd ID="LargeBoxEnd3" runat="server" />
		</div>
	    <div id="pnlFriendsRequests" runat="server">
	        <uc1:LargeBoxStart ID="LargeBoxStart2" runat="server" />
		    <asp:DataList ID="dlPendingFriendsRequests" Runat="server" CssClass="table table-striped" onitemcommand="dlPendingFriendsRequests_ItemCommand" onitemcreated="dlPendingFriendsRequests_ItemCreated">
			    <ItemTemplate>
			        <div class="media">
                        <a class="pull-left thumbnail media-object" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server" id="A1">
                            <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 100, 100, "", true, true, true) %>
                        </a>
                        <div class="media-body">
                            <div class="clearfix">
                                <h4 class="media-heading pull-left"><%# Eval("Username") %></h4>
                                <div class="pull-right">
                                    <asp:LinkButton ID="lnkAccept" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Accept" class="tooltip-go" data-toggle="tooltip" data-placement="bottom">
                                        <i class="fa fa-check-square fa-lg"></i>
                                    </asp:LinkButton>&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkReject" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Reject" class="tooltip-go" data-toggle="tooltip" data-placement="bottom">
                                        <i class="fa fa-times fa-lg"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <ul class="info-header info-header-sm">
                                <li><a class="tooltip-link" title="<%= Lang.Trans("Added to Friends on") %>"><i class="fa fa-star"></i>&nbsp;<%# Eval("AddedToFriendsOn") %></a></li>
                                <li>
                                   <label id="pnlAge" runat="server"><%= Lang.Trans("Age") %></label>&nbsp;<span id="pnlAgeValue" runat="server"><%# Eval("Age") %></span>
                                </li>
                            <% if (showRating) { %>
                                <li>
                                    <label><%= Lang.Trans("Rating") %></label>&nbsp;<%# Eval("Rating") %>
                                </li>
                            <% } %>
                            </ul>
                            <% if (showSlogan) { %>
                                <%# ((string) Eval("Slogan")).Length > 80 ? ((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan") %>
                            <% } %>
                            <% if (showLastOnline) { %>
                            <div class="text-right text-muted small">
                                 <i class="fa fa-sign-in"></i>&nbsp;<label><%= Lang.Trans("Last Online") %></label>&nbsp;<%# Eval("LastOnlineString") %>
                            </div>
                            <% } %>

                        </div>
		    	    </div>
			    </ItemTemplate>
		    </asp:DataList>
		    <uc1:LargeBoxEnd ID="LargeBoxEnd2" runat="server" />
		</div>
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
			<asp:Label ID="lblMessage" Runat="server" EnableViewState="false"/>
            <asp:DataList ID="dlFriends" Runat="server" CssClass="table table-striped" onitemcreated="dlFriends_ItemCreated"  onitemdatabound="dlFriends_ItemDataBound">
                <ItemTemplate>
                    <div class="media">
                        <a class="pull-left thumbnail media-object" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server" id="A1">
                            <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 100, 100, "", true, true, true) %>
                        </a>
                        <div class="media-body">
                            <div class="clearfix">
                                <h4 class="media-heading pull-left"><%# Eval("Username") %></h4>
                                <div class="pull-right">
                                    <asp:HyperLink ID="lnkSendMessage" Runat="server" class="tooltip-go" data-toggle="tooltip" data-placement="bottom"><i class="fa fa-envelope fa-lg"></i></asp:HyperLink>&nbsp;&nbsp;
                                    <asp:HyperLink ID="lnkRemoveFromFriends" Runat="server" class="tooltip-go" data-toggle="tooltip" data-placement="bottom"><i class="fa fa-trash-o fa-lg"></i></asp:HyperLink>
                                </div>
                            </div>
                            <ul class="info-header info-header-sm">
                                <li><a class="tooltip-link" title="<%= Lang.Trans("Added to Friends on") %>"><i class="fa fa-star"></i>&nbsp;<%# Eval("AddedToFriendsOn") %></a></li>
                                <li>
                                   <label id="pnlAge" runat="server"><%= Lang.Trans("Age") %></label>&nbsp;<span id="pnlAgeValue" runat="server"><%# Eval("Age") %></span>
                                </li>
                            <% if (showRating) { %>
                                <li>
                                    <label><%= Lang.Trans("Rating") %></label>&nbsp;<%# Eval("Rating") %>
                                </li>
                            <% } %>
                            </ul>
                            <% if (showSlogan) { %>
                                <%# ((string) Eval("Slogan")).Length > 80 ? ((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan") %>
                            <% } %>
                            <% if (showLastOnline) { %>
                            <div class="text-right text-muted small">
                                 <i class="fa fa-sign-in"></i>&nbsp;<label><%= Lang.Trans("Last Online") %></label>&nbsp;<%# Eval("LastOnlineString") %>
                            </div>
                            <% } %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
	</article>
</asp:Content>