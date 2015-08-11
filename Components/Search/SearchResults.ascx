<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchResults.ascx.cs" Inherits="AspNetDating.Components.Search.SearchResults" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes" %>

<% if (this.Page is Search2 || this.Page is Search) { %>
<script type="text/javascript">
    function pageLoad() {
        $('.thumb').mouseover(function() {
            $('.thumb-icons').hide();
            $(this).find('.thumb-icons').show();
        });
        $('.thumb').mouseout(function() {
            $(this).find('.thumb-icons').hide();
        });
    }
</script>
<% } %>
<asp:UpdatePanel ID="UpdatePanelSearchResults" runat="server">
    <ContentTemplate>
        <div id="divSwitchModes" class="text-right" runat="server" visible="false">
            <asp:LinkButton ID="lnkShowGrid" runat="server" OnClick="lnkShowGrid_Click"><i class="fa fa-th"></i>&nbsp;<%= Lang.Trans("Show as Photo Grid") %></asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="lnkShowDetails" runat="server" Enabled="False" OnClick="lnkShowDetails_Click"><i class="fa fa-th-list"></i>&nbsp;<%= Lang.Trans("Show as Profile List") %></asp:LinkButton>
            <hr />
        </div>

	    <asp:DataList id="dlUsers" runat="server" CssClass="table table-striped" OnItemCommand="dlUsers_ItemCommand" OnItemCreated="dlUsers_ItemCreated" OnItemDataBound="dlUsers_ItemDataBound">
		    <ItemTemplate>
		        <div class="media">
                    <a class="pull-left" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server">
                        <%# Photo.HasViewPhotoPermission(CurrentUserSession, (string)Eval("Username"), false) ?
                        ImageHandler.RenderImageTag((int)Eval("PhotoId"), 110, 110, "media-object img-thumbnail", true, true, true) :
                         String.Format("<span class=\"no-photo img-thumbnail\" style=\"width:115px; height: 115px; line-height: 105px\"></span>") %>
                        <%-- ImageHandler.RenderImageTag((AspNetDating.Classes.User.eGender)Enum.Parse(typeof(AspNetDating.Classes.User.eGender), (string)Eval("Gender")), 110, 110, "media-object img-thumbnail", false, true)--%>
                    </a>
                    <div class="media-body">
                        <div class="clearfix">
                            <h4 class="media-heading pull-left">
                                <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a>
                            </h4>
                            <% if (GroupID != null) { %>
                                <div class="pull-right"><%# Lang.Trans((string)Eval("GroupMemberType"))%></div>
                            <% } %>
                            <div id="pnlIcons" class="pull-right" runat="server">
                                <%# Config.UserScores.ShowLevelIcons && (!(bool) Eval("HideUserLevelIcon")) && Eval("UserLevel") is UserLevel ? String.Format("<img src=\"{0}\" title=\"{1} - {2}\" />",
                                                                ((UserLevel)Eval("UserLevel")).GetIconUrl(), String.Format(Lang.Trans("Level {0}"), ((UserLevel)Eval("UserLevel")).LevelNumber), ((UserLevel)Eval("UserLevel")).Name) : ""%>
                                <% if (Config.Users.EnableZodiacSign && ShowZodiacSign)
                                   { %>
                                   <%# (DataBinder.Eval(Container.DataItem, "ZodiacSign2") == DBNull.Value)
                                       ?
                                       String.Format("<img src=\"{0}\" title=\"{1}\" />", User.GetZodiacImageUrl((User.eZodiacSign)Eval("ZodiacSign1")), User.GetZodiacTooltip((User.eZodiacSign)Eval("ZodiacSign1")))
                                       :
                                       String.Format("<img src=\"{0}\" title=\"{1}\" />", User.GetZodiacImageUrl((User.eZodiacSign)Eval("ZodiacSign1")), User.GetZodiacTooltip((User.eZodiacSign)Eval("ZodiacSign1")))
                                       + String.Format("<img src=\"{0}\" title=\"{1}\" />", User.GetZodiacImageUrl(((User.eZodiacSign)Eval("ZodiacSign2"))), User.GetZodiacTooltip(((User.eZodiacSign)Eval("ZodiacSign2"))))
                                   %>
                                <% } %>
                                <%# (bool)Eval("Blocked")? String.Format("<i class=\"fa fa-ban\" title=\"{0}\" /></i>", Lang.Trans("Blocked")) : "" %>
                                <%# (bool)Eval("MessageHistory")? String.Format("<i class=\"fa fa-comments\" title=\"{0}\" /></i>", Lang.Trans("Message History")) : "" %>
                                <%# (bool)Eval("VerifiedByUsers")? String.Format("<i class=\"fa fa-check\" title=\"{0}\" /></i>", Lang.Trans("Verified")) : "" %>
                                <%# (bool)Eval("VerifiedByAdmin")? String.Format("<i class=\"fa fa-check-square\" title=\"{0}\" /></i>", Lang.Trans("Verified By Admin")) : "" %>
                                <%# (bool)Eval("PrivatePhoto")? String.Format("<i class=\"fa fa-lock\" title=\"{0}\" /></i>", Lang.Trans("Private Photo")) : "" %>
                                <%# (bool)Eval("VideoProfile")? String.Format("<i class=\"fa fa-file-video-o\" title=\"{0}\" /></i>", Lang.Trans("Video Profile")) : "" %>
                                <%# (bool)Eval("HasBlog")? String.Format("<i class=\"fa fa-rss\" title=\"{0}\" /></i>", Lang.Trans("Blog")) : "" %>
                                <%# (bool)Eval("IsBroadcastingVideo")? String.Format("<i class=\"fa fa-video-camera\" title=\"{0}\" /></i>", Lang.Trans("Currently Broadcasting Video")) : "" %>
                            </div><!-- /#pnlIcons -->
                        </div>
                        <ul class="info-header info-header-sm">
                            <% if (ShowAge) { %>
                            <li>
                               <a class="tooltip-link" title="<%= Lang.Trans("Age") %>"><i class="fa fa-gift"></i>&nbsp;<%# Eval("Age") %></a>
                            </li>
                            <% } %>
                            <% if (ShowGender) { %>
                            <li>
                               <a class="tooltip-link" title="<%= Lang.Trans("Gender") %>"><%# Lang.Trans((string)Eval("Gender")) %></a>
                            </li>
                            <% } %>
                            <% if (showCity) { %>
                            <li>
                                <a class="tooltip-link" title="<%= Lang.Trans("Location") %>"><i class="fa fa-map-marker"></i>&nbsp;<%# Eval("Location") %></a>
                            </li>
                            <% } %>
                            <% if (ShowDistance) { %>
                            <li id="pnlDistance" visible='<%# ((string)Eval("Distance") != "") %>' runat="server">
                                <a class="tooltip-link" title="<%= Lang.Trans("Distance") %>"><i class="fa fa-road"></i>&nbsp;<%# Eval("Distance") %></a>
                            </li>
                            <% } %>
                            <% if (ShowModerationScore) { %>
                            <li>
                                <a class="tooltip-link" title="<%= Lang.Trans("Moderation score") %>"><i class="fa fa-certificate"></i>&nbsp;<%# (int)Eval("ModerationScore") %></a>
                            </li>
                            <% } %>
                            <% if (showRating) { %>
                            <li>
                                <a class="tooltip-link" title="<%= Lang.Trans("Rating") %>"><i class="fa fa-star"></i>&nbsp;<%# Eval("Rating") %></a>
                            </li>
                            <% } %>
                            <% if (GroupID != null) { %>
                            <li>
                                <a class="tooltip-link" title="<%= Lang.Trans("Member since") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("GroupMemberJoinDate")%></a>
                            </li>
                            <% } %>
                        </ul>
                        <% if (showSlogan) { %>
                            <p><%#Server.HtmlEncode((((string) Eval("Slogan")).Length > 80 ? ((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan")) as string) %></p>
                        <% } %>
                        <div class="text-right text-muted small">
                            <% if (ShowViewedOn) { %>
                                <i class="fa fa-eye"></i>&nbsp;<label><%= Lang.Trans("last viewed") %></label>:&nbsp;<%# Eval("ViewedOnString") %>&nbsp;&nbsp;
                            <% } %>
                            <% if (ShowLastOnline) { %>
                                <i class="fa fa-sign-in"></i>&nbsp;<label><%= Lang.Trans("last online") %></label>:&nbsp;<%# Eval("LastOnlineString") %>
                            <% } %>
                        </div>
                        <div id="pnlManageGroupMembers" class="btn-group pull-right" runat="server" visible='<%# GroupID != null && CurrentUserSession != null ? true : false %>'>
                            <span class="btn-group btn-group-xs" id="liMakeAdmin" runat="server">
                                <asp:LinkButton class="btn btn-default" ID="lnkMakeAdmin" CommandName="MakeAdmin" CommandArgument='<%# Eval("Username") %>' Runat="server"/>
                            </span>
                            <span class="btn-group btn-group-xs" id="liRemoveAdmin" runat="server">
                                <asp:LinkButton class="btn btn-default" ID="lnkRemoveAdmin" CommandName="RemoveAdmin" CommandArgument='<%# Eval("Username") %>' Runat="server"/>
                            </span>
                            <span class="btn-group btn-group-xs" id="liMakeModerator" runat="server">
                                <asp:LinkButton class="btn btn-default" ID="lnkMakeModerator" CommandName="MakeModerator" CommandArgument='<%# Eval("Username") %>' Runat="server"/>
                            </span>
                            <span class="btn-group btn-group-xs" id="liRemoveModerator" runat="server">
                                <asp:LinkButton class="btn btn-default" ID="lnkRemoveModerator" CommandName="RemoveModerator" CommandArgument='<%# Eval("Username") %>' Runat="server"/>
                            </span>
                            <span class="btn-group btn-group-xs" id="liMakeVip" runat="server">
                                <asp:LinkButton class="btn btn-default" ID="lnkMakeVip" CommandName="MakeVip" CommandArgument='<%# Eval("Username") %>' Runat="server"/>
                            </span>
                            <span class="btn-group btn-group-xs" id="liRemoveVip" runat="server">
                                <asp:LinkButton class="btn btn-default" ID="lnkRemoveVip" CommandName="RemoveVip" CommandArgument='<%# Eval("Username") %>' Runat="server"/>
                            </span>
                            <span class="btn-group btn-group-xs" id="liDeleteMember" runat="server">
                                <asp:LinkButton class="btn btn-default" ID="lnkDeleteMember" CommandName="DeleteMember" CommandArgument='<%# Eval("Username") %>' Runat="server"/>
                            </span>
                        </div>
		        	</div><!-- /.media-body -->
		        </div><!-- /.media -->
                <% if (showFriendsPath) { %><small class="text-muted"><i class="fa fa-long-arrow-down"></i> <%= "knows".Translate() %></small><% } %>
		    </ItemTemplate>
	    </asp:DataList>

	    <asp:DataList id="dlUsersGrid" CssClass="repeater-horizontal" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server" EnableViewState="false" Visible="false" onitemcreated="dlUsersGrid_ItemCreated">
	        <ItemTemplate>
            	<div class="thumb" style="background-image:url(<%# Config.Photos.EnablePhotoStack ?
ImageHandler.CreateImageStackUrl((string) Eval("Username"), 180, 150) :
ImageHandler.CreateImageUrl((int)Eval("PhotoId"), 180, 150, false, true, true) %>)" onclick="location.href = '<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username")) %>';">
                    <div class="thumb-icons" style="display: none;">
                        <a href="<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username")) %>" title="<%= "View user profile".Translate() %>" >
                            <i class="fa fa-eye"></i>
                        </a>
                       	<a href="<%# "SendMessage.aspx?to_user=" + (string) Eval("Username") + "&src=search" %>" title="<%= "Send a message".Translate() %>">
                       	    <i class="fa fa-envelope"></i>
                       	</a>
                        <a href='<%# "~/SendEcard.aspx?uid=" + (string) Eval("Username") + "&src=search" %>' runat="server" visible='<%# CurrentUserSession != null && CurrentUserSession.CanSendEcards() != PermissionCheckResult.No %>' title="Send an e-card">
                            <i class="fa fa-picture-o"></i>
                        </a>
                        <a href='<%# "~/AddRemoveFavourite.aspx?uid=" + (string) Eval("Username") + "&cmd=add&src=search" %>' runat="server" visible='<%# Config.Users.EnableFavorites %>' title="Add to favorites">
                            <i class="fa fa-star"></i>
                        </a>
                    </div>
                    <span class="thumb-info">
                        <a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'><%# Eval("Username") %></a>
                        <div id="pnlGenderAge" class="text-muted" runat="server">
                            <%# Lang.Trans((string) Eval("Gender")) %><span id="pnlDelimiter" runat="server">/</span><%# Eval("Age") %>
                        </div>
                	</span>
                </div>
	        </ItemTemplate>
	    </asp:DataList>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="dlUsers" />
    </Triggers>
</asp:UpdatePanel>
<asp:Panel ID="pnlPaginator" Visible="True" Runat="server">
    <asp:UpdatePanel ID="UpdatePanelPaginator" runat="server">
        <ContentTemplate>
		    <ul class="pager">
			    <li><asp:LinkButton id="lnkFirst" runat="server"/></li>
			    <li><asp:LinkButton id="lnkPrev" runat="server"/></li>
			    <li class="text-muted"><asp:Label id="lblPager" runat="server"/></li>
			    <li><asp:LinkButton id="lnkNext" runat="server"/></li>
			    <li><asp:LinkButton id="lnkLast" runat="server"/></li>
		    </ul>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
