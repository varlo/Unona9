<%@ Page Language="c#" MasterPageFile="Site.Master" CodeBehind="Home.aspx.cs" AutoEventWireup="True"
    Inherits="AspNetDating.Home" %>

<%@ Register Src="Components/Groups/NewGroups.ascx" TagName="NewGroups" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="SearchBox" Src="Components/Search/SearchBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewsBox" Src="Components/NewsBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BirthdayBox" Src="Components/BirthdayBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FriendsOnlineBox" Src="Components/FriendsOnlineBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Src="Components/WebParts/NewUsersWebPart.ascx" TagName="NewUsersWebPart"
    TagPrefix="uc3" %>
<%@ Register Src="Components/WebParts/NewVideosWebPart.ascx" TagName="NewVideosWebPart"
    TagPrefix="uc3" %>
<%@ Register Src="Components/WebParts/PopularBlogPostsWebPart.ascx" TagName="PopularBlogPostsWebPart"
    TagPrefix="uc3" %>
<%@ Register Src="Components/WebParts/NewGroupsWebPart.ascx" TagName="NewGroupsWebPart"
    TagPrefix="uc3" %>
<%@ Register TagPrefix="CustomWebPartManager" Namespace="AspNetDating.CustomWebPartManager"
    Assembly="AspNetDating" %>
<%@ Register TagPrefix="CustomWebPartZone" Namespace="AspNetDating.CustomWebPartZone"
    Assembly="AspNetDating" %>
<%@ Register TagPrefix="CustomEditorZone" Namespace="AspNetDating.CustomEditorZone"
    Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<CustomWebPartManager:CustomWebPartManager ID="WebPartManager1" Personalization-InitialScope="User" Personalization-Enabled="true" runat="server" />
    <aside>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <CustomWebPartZone:CustomWebPartZone TabIndex="0" BorderStyle="None" BorderWidth="0" ID="wpzHomePageLeftZone" WebPartVerbRenderMode="TitleBar" HeaderText="" Padding="0" PartChromePadding="0" runat="server">
                    <PartStyle CssClass="panel-body"></PartStyle>
                    <PartTitleStyle CssClass="panel-heading"></PartTitleStyle>
                    <PartChromeStyle CssClass="panel"></PartChromeStyle>
                    <TitleBarVerbStyle CssClass="verb"></TitleBarVerbStyle>
                    <ZoneTemplate ></ZoneTemplate>
                    <CloseVerb Text="Close" Visible="false"></CloseVerb>
                    <MinimizeVerb Visible="false"></MinimizeVerb>
                    <RestoreVerb Visible="false"></RestoreVerb>
                    <DeleteVerb Visible="false"></DeleteVerb>
                    <EditVerb Visible="false"></EditVerb>
                </CustomWebPartZone:CustomWebPartZone>
                <div id="pnlEditorZoneLeft" runat="server">
                    <CustomEditorZone:CustomEditorZone CssClass="panel" Padding="0" ID="ezEditorZoneLeft" PartChromePadding="0" runat="server">
                       <HeaderStyle CssClass="panel-heading"></HeaderStyle>
                        <HeaderVerbStyle CssClass="verb"></HeaderVerbStyle>
                        <HeaderCloseVerb></HeaderCloseVerb>
                        <ZoneTemplate></ZoneTemplate>
                    </CustomEditorZone:CustomEditorZone>
                </div>
                <div class="actions">
                    <asp:LinkButton CssClass="btn btn-default" ID="lnkCatalogForLeftParts" runat="server" OnClick="lnkCatalogForLeftParts_Click"><i class="fa fa-plus-square"></i>&nbsp;<%= Lang.Trans("Add Components") %></asp:LinkButton>
                </div>
                <components:BannerView id="bvHomeLeftBottom" runat="server" Key="HomeLeftBottom" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </aside>
    <article>
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server" />
        <components:BannerView id="bvHomeBeforeProfile" runat="server" Key="HomeBeforeProfile" />
        <div class="media">
            <a href="Profile.aspx" class="pull-right"><asp:Image ID="imgPhoto" runat="server" /></a>
            <div class="media-body">
            <ul class="list-group list-group-sm list-group-itags">
                    <li class="list-group-item" id="liWhoViewedMyProfile" runat="server">
                        <i class="fa fa-eye tag"></i>&nbsp;
                        <%= Lang.Trans("Your profile has been viewed") %>&nbsp;<asp:LinkButton ID="lnkViewProfileViewers" runat="server" OnClick="lnkViewProfileViewers_Click"><asp:Label ID="lblProfileViews" runat="server" /></asp:LinkButton>&nbsp;<%= Lang.Trans("times") %>
                    </li>
                    <li class="list-group-item" id="pnlRating" runat="server">
                        <i class="fa fa-star tag"></i>&nbsp;
                        <%= Lang.Trans("Average rating") %>:&nbsp;<b><asp:Label ID="lblRating" runat="server" /></b>
                    </li>
                    <li class="list-group-item" id="pnlVotes" runat="server">
                        <i class="fa fa-thumbs-up tag"></i>&nbsp;
                        <%= Lang.Trans("Your votes") %>:&nbsp;<asp:LinkButton ID="lnkViewMutualVotes" runat="server" OnClick="lnkViewMutualVotes_Click"><asp:Label ID="lblVotes" runat="server" /></asp:LinkButton>
                    </li>
                    <li class="list-group-item">
                        <i class="fa fa-users tag"></i>&nbsp;
                        <span id="pnlNewUsers" runat="server">
                            <asp:LinkButton ID="lnkNewUsers" runat="server" OnClick="lnkNewUsers_Click"/>&nbsp;<%= Lang.Trans("new users since your last visit") %>&nbsp;
                        </span>
                        <span id="pnlUsersOnline" runat="server">
                            <asp:LinkButton ID="lnkUsersOnline" runat="server" OnClick="lnkUsersOnline_Click"/>&nbsp;<asp:Label ID="lblUsersOnline" runat="server" />&nbsp;
                        </span>
                        <span id="pnlUsersBroadcasting" runat="server">
                            <asp:LinkButton ID="lnkUsersBroadcasting" runat="server" OnClick="lnkUsersBroadcasting_Click"/>&nbsp;<asp:Label ID="lblUsersBroadcasting" runat="server" />
                        </span>
                    </li>
                    <li class="list-group-item" id="pnlNewMessages" runat="server">
                        <i class="fa fa-envelope tag"></i>&nbsp;
                        <%= Lang.Trans("You have") %>&nbsp;<asp:LinkButton ID="lnkNewMessages" runat="server" OnClick="lnkNewMessages_Click" />&nbsp;<asp:Label ID="lblNewMessages" runat="server" />
                    </li>
                    <li class="list-group-item" id="pnlRelationshipRequests" runat="server">
                        <i class="fa fa-heart tag"></i>&nbsp;
                        <asp:LinkButton ID="lnkRelationshipRequests" runat="server" onclick="lnkRelationshipRequests_Click" />&nbsp;<asp:Label ID="lblRelationshipRequests" runat="server" />
                    </li>
                    <li class="list-group-item" id="pnlFriendsRequests" runat="server">
                        <i class="fa fa-user tag"></i>&nbsp;
                        <asp:LinkButton ID="lnkFriendsRequests" runat="server" onclick="lnkFriendsRequests_Click"/>&nbsp;<asp:Label ID="lblFriendsRequests" runat="server" />
                    </li>
                    <li class="list-group-item" id="pnlNewEcards" runat="server">
                        <i class="fa fa-picture-o tag"></i>&nbsp;
                        <asp:LinkButton ID="lnkNewEcards" runat="server" onclick="lnkNewEcards_Click" />&nbsp;<asp:Label ID="lblNewEcards" runat="server" />
                    </li>
                    <li class="list-group-item" id="pnlStatusText" runat="server" visible="false">
                        <asp:UpdatePanel ID="UpdatePanelStatusText" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-info-circle tag"></i>&nbsp;
                                <%= Lang.Trans("Your status")%>:&nbsp;<span id="pnlViewStatusText" runat="server"><b><asp:Label ID="lblStatusText" runat="server" /></b>&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkEditStatusText" runat="server" onclick="lnkEditStatusText_Click"><i class="fa fa-pencil"></i></asp:LinkButton></span>
                                <span id="pnlEditStatusText" runat="server" visible="false">
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox CssClass="form-control" ID="txtStatusText" runat="server" />
                                        <span class="input-group-btn"><asp:LinkButton CssClass="btn btn-default" ID="lnkUpdateStatusText" runat="server" onclick="lnkUpdateStatusText_Click"><i class="fa fa-share"></i></asp:LinkButton></span>
                                    </div>
                                </span>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </li>
                    <li class="list-group-item" id="pnlBlockedUsers" runat="server">
                        <i class="fa fa-ban tag"></i>&nbsp;
                        <a href="BlockedUsers.aspx"><asp:Label ID="lblBlockedUsers" runat="server" /></a>&nbsp;<%= Lang.Trans("blocked users") %>
                    </li>
                    <li class="list-group-item" id="pnlPendingInvitations" runat="server" visible="false">
                        <i class="fa fa-envelope-square tag"></i>&nbsp;
                        <asp:LinkButton ID="lnkPendingInvitations" runat="server" OnClick="lnkPendingInvitations_Click" />&nbsp;<asp:Label ID="lblPendingInvitatinos" runat="server" />
                    </li>
                    <asp:Repeater ID="rptContestsRanks" Visible="False" runat="server">
                        <ItemTemplate>
                            <li class="list-group-item">
                                <i class="fa fa-trophy tag"></i>&nbsp;
                                <%# String.Format(Lang.Trans("Your entry is ranked #{0} in the \"{1}\" contest"), Eval("Rank"), Eval("ContestName")) %>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="rptGroupTopicSubscriptions" runat="server" Visible="false">
                        <ItemTemplate>
                            <li class="list-group-item">
                                <i class="fa fa-comment-o tag"></i>&nbsp;
                                <%# Lang.Trans("There are new posts in topic ")%>
                                <a href='<%# UrlRewrite.CreateShowGroupTopicsUrl((string) Eval("GroupID"), (string) Eval("GroupTopicID")) %>'><%# String.Format(Lang.Trans("{0}"), Eval("GroupTopicName"))%></a>
                                <%# String.Format(Lang.Trans(" in group <b>{0}</b>"), Eval("GroupName"))%>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <li class="list-group-item" id="pnlInviteAFriend" runat="server">
                        <asp:Label ID="lblInviteAFriend" runat="server" />
                        <a class="btn btn-default btn-xs" href="<%= Config.Mailing.EnableAddressBookImporter ? "ImportFriends.aspx" : "InviteFriend.aspx" %>"><i class="fa fa-plus"></i>&nbsp;<%= Lang.Trans("Invite a friend") %></a>
                    </li>
                    <li class="list-group-item" id="pnlInviteFriendsFromFacebook" visible="false" runat="server">
                        <i class="fa fa-facebook tag"></i>&nbsp;
                        <a href='<%= String.Format("//www.facebook.com/dialog/apprequests?app_id={0}&redirect_uri={1}&message={2}", FacebookAPIKey, Server.UrlEncode(Config.Urls.Home + "/FacebookInviteFriendsHandler.aspx"), Server.UrlEncode(String.Format("Come and join me at {0}".Translate(), Config.Urls.Home))) %>'>
                            <%= Lang.Trans("Invite friends from Facebook") %></a>
                    </li>
                </ul>
            </div>
        </div>
        <components:BannerView id="bvHomeAfterProfile" runat="server" Key="HomeAfterProfile" />
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server" />
        <asp:UpdatePanel ID="upWebParts" runat="server">
            <ContentTemplate>
                <CustomWebPartZone:CustomWebPartZone TabIndex="1" Padding="0" PartChromePadding="0" BorderStyle="None" ID="wpzHomePageRightZone" WebPartVerbRenderMode="TitleBar" runat="server">
                    <PartStyle CssClass="panel-body"></PartStyle>
                    <PartTitleStyle CssClass="panel-heading"></PartTitleStyle>
                    <PartChromeStyle CssClass="panel"></PartChromeStyle>
                    <TitleBarVerbStyle CssClass="verb"></TitleBarVerbStyle>
                    <ZoneTemplate></ZoneTemplate>
                    <CloseVerb Visible="false"></CloseVerb>
                    <RestoreVerb Visible="false"></RestoreVerb>
                    <MinimizeVerb Visible="false"></MinimizeVerb>
                    <EditVerb Visible="false"></EditVerb>
                    <DeleteVerb Visible="false"></DeleteVerb>
                </CustomWebPartZone:CustomWebPartZone>
                <div id="pnlEditorZoneRight" runat="server">
                    <CustomEditorZone:CustomEditorZone BorderStyle="None" CssClass="panel" ID="ezEditorZoneRight" runat="server">
                        <HeaderStyle CssClass="panel-heading"></HeaderStyle>
                        <HeaderVerbStyle CssClass="verb"></HeaderVerbStyle>
                        <HeaderCloseVerb></HeaderCloseVerb>
                        <PartStyle CssClass="panel-body"></PartStyle>
                        <ZoneTemplate></ZoneTemplate>
                    </CustomEditorZone:CustomEditorZone>
                </div>
                <asp:Panel ID="pnlCatalog" Visible="false" runat="server">
                    <div class="panel">
                        <div class="panel-heading">
                            <h3 class="panel-title"><%= Lang.Trans("Add Components") %>
                                <asp:LinkButton CssClass="pull-right verb" ID="imgbCloseCatalog" runat="server" OnClick="imgbCloseCatalog_Click"><i class="fa fa-times"></i></asp:LinkButton>
                            </h3>
                        </div>
                        <div class="panel-body">
                            <ul class="list-group list-group-striped">
                            <asp:Repeater ID="rptCatalogWebParts" runat="server" OnItemCommand="rptCatalogWebParts_ItemCommand">
                                <ItemTemplate>
                                    <li class="list-group-item">
                                        <div class="media">
                                            <span class="pull-left">
                                                <span class="fa-stack fa-3x">
                                                    <i class="fa fa-square fa-stack-2x"></i>
                                                    <i class='fa <%# ((string)Eval("ThumbnailIcon")).Translate() %> fa-stack-1x fa-inverse'></i>
                                                </span>
                                            </span>
                                            <div class="media-body">
                                                <h4><%# ((string)Eval("Name")).Translate() %></h4>
                                                <div class="row">
                                                    <div class="col-sm-10">
                                                        <%# ((string)Eval("Description")).Translate() %>
                                                    </div>
                                                    <div class="col-sm-2 text-right">
                                                        <asp:LinkButton ID="btnAdd" CssClass="btn btn-default btn-sm" CommandName="Add" CommandArgument='<%# Eval("ControlPath") %>' runat="server">
                                                            <i class="fa fa-plus-square"></i>&nbsp;<%# "Add".Translate() %>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                            </ul>
                            <div class="actions">
                                <asp:Button CssClass="btn btn-default" ID="btnCancelCatalog" runat="server" OnClick="btnCancelCatalog_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="actions">
                    <asp:LinkButton ID="lnkCatalogForRightParts" CssClass="btn btn-default" runat="server" OnClick="lnkCatalogForRightParts_Click"><i class="fa fa-plus-square"></i>&nbsp;<%= Lang.Trans("Add Component") %></asp:LinkButton>
                </div>
                <components:BannerView id="bvHomeRightBottom" runat="server" Key="HomeRightBottom" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </article>
</asp:Content>
