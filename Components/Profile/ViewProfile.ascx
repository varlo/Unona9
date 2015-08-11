<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="ViewProfile.ascx.cs" Inherits="AspNetDating.Components.Profile.ViewProfile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register Src="ViewVideo.ascx" TagName="ViewVideo" TagPrefix="uc2" %>
<%@ Register Src="ViewVideoBroadcast.ascx" TagName="ViewVideoBroadcast" TagPrefix="uc2" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>

<input id="hidUsername" type="hidden" name="hidUsername" runat="server">
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
<components:BannerView id="bvShowUserRightTop" runat="server" Key="ShowUserRightTop"/>
    <h4><uc1:HeaderLine ID="hlSlogan" CssClass="translatable" runat="server" /></h4>
    <div class="media">
        <div class="pull-left">
            <asp:Literal ID="ltrPhoto" runat="server"/>
            <div class="text-center"><a class="btn btn-default btn-xs btn-block" id="lnkViewAllPhotos" runat="server"><%= "all photos".Translate() %></a></div>
        </div>
        <div class="media-body">
            <div class="clearfix">
                <h4 class="media-heading pull-left">
                    <asp:Label ID="lblUsername" runat="server"/>
                </h4>
                <div class="pull-right" id="pnlIcons" runat="server">
                    <%= Config.UserScores.ShowLevelIcons && !User.IsOptionEnabled(eUserOptions.DisableLevelIcon) && User.Level != null ?
                        String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{2}\"><span class=\"fa-stack fa-lg fa-badge\"><i class=\"fa fa-certificate fa-stack-2x\"></i><i class=\"fa fa-stack-1x fa-inverse\">{1}</i></span></a>",
                                                                        User.Level.GetIconUrl(), String.Format(Lang.Trans("{0}"), User.Level.LevelNumber), User.Level.Name) : ""%>
                    <% if (Config.Users.EnableZodiacSign && !Config.Users.DisableAgeInformation)
                       { %>
                       <% if (!zodiacSign2.HasValue) %>
                       <% { %>
                               <%= String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{1}\"><img src=\"{0}\"/></a>",
                                User.GetZodiacImageUrl(zodiacSign1), User.GetZodiacTooltip(zodiacSign1))%>
                       <% } %>
                       <% else
                          {%>
                               <%= String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{1}\"><img src=\"{0}\"/></a>",
                                    User.GetZodiacImageUrl(zodiacSign1), User.GetZodiacTooltip(zodiacSign1)) %>
                               <%= String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{1}\"><img src=\"{0}\"/></a>",
                                    User.GetZodiacImageUrl(zodiacSign2.Value), User.GetZodiacTooltip(zodiacSign2.Value)) %>
                       <% }%>
                    <% } %>
                    <%= blocked ? String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{0}\"><i class=\"fa fa-ban\"></i></a>", Lang.Trans("Blocked")) : "" %>
                    <%= messageHistoryExists ? String.Format("<a class=\"tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" href=\"Mailbox.aspx?uid={0}\" title=\"{1}\" ><i class=\"fa fa-comments\"></i></a>", User.Username, Lang.Trans("Message History")) : "" %>
                    <%= verifiedByUsers ? String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{0}\"><i class=\"fa fa-check\"></i></a>", Lang.Trans("Verified")) : "" %>
                    <%= verifiedByAdmin ? String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{0}\"><i class=\"fa fa-check-square\"></i></a>", Lang.Trans("Verified By Admin")) : "" %>
                    <%= hasPrivatePhoto ? String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{0}\"><i class=\"fa fa-lock\"></i></a>", Lang.Trans("Private Photo")) : "" %>
                    <%= hasVideoProfile ? String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{0}\"><i class=\"fa fa-file-video-o\"></i></a>", Lang.Trans("Video Profile")) : "" %>
                    <%= hasBlog ? String.Format("<a class=\"tooltip-link tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"{0}\"><i class=\"fa fa-rss\"></i></a>", Lang.Trans("Blog")) : "" %>
                    <asp:Literal ID="ltrSkype" runat="server"/>
                </div>
            </div>
            <ul class="info-header info-header-sm">
                <li id="pnlAge" runat="server">
                    <a class="tooltip-link" title="<%= Lang.Trans("Age") %>"><i class="fa fa-gift"></i>&nbsp;<asp:Label ID="lblAge" runat="server"/></a>
                </li>
                <li id="pnlGender" runat="server">
                    <a class="tooltip-link" title="<%= Lang.Trans("Gender") %>"><asp:Label ID="lblGender" runat="server"/></a>
                </li>
                <li id="pnlLocation" runat="server">
                    <a class="tooltip-link" title="<%= Lang.Trans("Location") %>"><i class="fa fa-map-marker"></i>&nbsp;<%= User.LocationString %></a>
                </li>
                <li id="pnlDistance" runat="server">
                    <a class="tooltip-link" title="<%= Lang.Trans("Distance") %>"><i class="fa fa-road"></i>&nbsp;<asp:Label ID="lblDistance" runat="server"/></a>
                </li>
            </ul>
            <ul class="list-group list-group-sm list-group-itags">
                <li class="list-group-item" id="pnlStatusText" runat="server" visible="false">
                    <i class="fa fa-info-circle tag"></i>&nbsp;<label><%= Lang.Trans("Status") %></label>&nbsp;<asp:Label ID="lblStatusText" runat="server"/>
                </li>
                <li class="list-group-item" id="pnlProfileMatch" runat="server">
                    <i class="fa fa-check-square tag"></i>&nbsp;<label><%= Lang.Trans("Match") %></label>&nbsp;<asp:Label ID="lblMatchedPercentage" runat="server"/>
                </li>
                <asp:UpdatePanel ID="UpdatePanelRating" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div id="pnlRating" runat="server">
                            <li class="list-group-item">
                                <i class="fa fa-star tag"></i>&nbsp;<label><%= Lang.Trans("Average Rating") %></label>&nbsp;<asp:Label ID="lblRatingAverage" runat="server"/>
                            </li>
                            <li class="list-group-item" id="pnlRateUser" runat="server">
                                <i class="fa fa-star-o tag"></i>&nbsp;<label><%= Lang.Trans("Your Rating") %></label>&nbsp;<asp:DropDownList CssClass="form-control form-control-inline input-sm" ID="ddRating" runat="server"/>
                                <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnRateUser" runat="server" OnClick="btnRateUser_Click"/>
                            </li>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanelVoting" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <li class="list-group-item" id="pnlVoting" runat="server">
                            <i class="fa fa-thumbs-o-up tag"></i>&nbsp;<label><%= Lang.Trans("Votes Score") %></label>&nbsp;<asp:Label ID="lblVotesScore" runat="server"/>
                            &nbsp;<span class="text-muted">|</span>&nbsp;
                            <span id="pnlVoteUser" runat="server">
                                <%= Lang.Trans("Your Vote") %>&nbsp;
                                <div class="btn-group btn-group-sm">
                                    <asp:LinkButton CssClass="btn btn-default" ID="btnVoteDown" runat="server" OnClick="btnVote_Click"><i class="fa fa-thumbs-down"></i></asp:LinkButton>&nbsp;
                                    <asp:LinkButton CssClass="btn btn-default" ID="btnVoteUp" runat="server" OnClick="btnVote_Click"><i class="fa fa-thumbs-up"></i></asp:LinkButton>
                                </div>
                            </span>
                        </li>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <li class="list-group-item" id="divMemberOf" visible="false" runat="server">
                    <i class="fa fa-users tag"></i>&nbsp;<label><%= Lang.Trans("Member of") %></label>&nbsp;<asp:PlaceHolder ID="plhGroupsLinks" runat="server" />
                </li>
                <li class="list-group-item" id="pnlRelationship" visible="false" runat="server">
                    <i class="fa fa-heart tag"></i>&nbsp;<label id="spanRelationship" runat="server"></label>&nbsp;<a id="lnkUsername" runat="server"></a>&nbsp;<span class="text-muted"><asp:Label ID="lblRelationship" runat="server"/></span>
                </li>
            </ul><!-- /.list-group -->
            <div class="text-right text-muted small">
                <i class="fa fa-sign-in"></i>&nbsp;<label><%= Lang.Trans("last online") %></label>:&nbsp;<asp:Label ID="lblLastOnline" runat="server"/>
            </div>
        </div>
    </div><!-- /.media -->
    <div id="pnlVideoBroadcast" visible="false" runat="server">
        <h4><uc1:HeaderLine ID="hlVideoBroadcast" runat="server"/></h4>
        <div id="pnlSubscribe" runat="server" visible="false">
            <a href='<%= Config.Urls.Home %>/Profile.aspx?sel=payment'><%= "You need to upgrade your plan in order to view video broadcast".Translate() %></a>
        </div>
        <div id="pnlVideoStream" runat="server">
            <uc2:ViewVideoBroadcast ID="ViewVideoBroadcast1" runat="server" />
        </div>
    </div>
    <div id="pnlUnlockVideoStream" runat="server" visible="false">
        <components:ContentView ID="cvUnlockVideoStream" Key="UnlockVideoStream" runat="server"/>
        <div class="actions"><asp:Button CssClass="btn btn-default" ID="btnUnlockVideoStream" runat="server" onclick="btnUnlockVideoStream_Click" /></div>
    </div>
    <asp:UpdatePanel ID="UpdatePanelVideo" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <h4 id="pnlVideosHeaderLine" runat="server"><uc1:HeaderLine ID="hlVideos" runat="server"/></h4>
            <asp:Label ID="lblVideoPrivacySettingsError" runat="server" EnableViewState="false"/>
            <div id="pnlVideos" runat="server" class="SectionContent">
                <div id="pnlRecordedVideo" runat="server" visible="false">
                    <uc2:ViewVideo ID="ViewVideo1" runat="server" />
                </div>
                <div id="pnlVideoUpload" runat="server" visible="false">
                    <div class="text-center">
                        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0" width="325" height="262" id="flvplayer">
                            <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf">
                            <param name="quality" value="high">
                            <param name="bgcolor" value="#FFFFFF">
                            <param name="wmode" value="transparent">
                            <param name="allowfullscreen" value="true">
                            <param name="flashvars" value="width=325&height=262&file=<%= VideoUploadUrl %>&image=<%= VideoUploadUrl.Replace(".flv", ".png") %>&logo=<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/watermark2.png" />
                            <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf"
                                quality="high" wmode="transparent" bgcolor="#FFFFFF" width="325" height="262"
                                name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="width=325&height=262&file=<%= VideoUploadUrl %>&image=<%= VideoUploadUrl.Replace(".flv", ".png") %>&logo=<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/watermark2.png"></embed></object>
                    </div>
                    <hr />
                </div>
                <div id="pnlVideoUnlock" runat="server">
                    <components:ContentView ID="cvUnlockVideo" Key="UnlockVideo" runat="server"/>
                    <div class="actions"><asp:Button CssClass="btn btn-default" ID="btnUnlockVideo" runat="server" onclick="btnUnlockVideo_Click" /></div>
                </div>
                <div id="pnlVideoEmbed" runat="server" visible="false">
                    <div class="text-center">
                        <asp:Literal ID="ltrVideoEmbed" runat="server"/>
                        <div><asp:Label ID="lblVideoEmbedName" runat="server"/></div>
                    </div>
                    <hr />
                </div>
                <div id="divVideoThumbnails" runat="server">
                    <asp:DataList ID="dlVideos" CssClass="repeater-horizontal videos" RepeatLayout="Flow" runat="server" OnItemCommand="dlVideos_ItemCommand">
                        <ItemTemplate>
                            <div class="video-thumbnail">
                                <asp:ImageButton ID="ImageButton1" ImageUrl='<%# Eval("ThumbnailUrl") %>' CommandName="SelectVideo" CommandArgument='<%# Eval("ThumbnailUrl") + "|" + Eval("VideoUrl") + "|" + Eval("Title") %>' Height="100" runat="server" />
                            </div>
                            <div class="caption small">
                                <asp:LinkButton ID="LinkButton1" Text='<%# Eval("Title") %>' CommandName="SelectVideo" CommandArgument='<%# Eval("ThumbnailUrl") + "|" + Eval("VideoUrl") + "|" + Eval("Title") %>' runat="server" />
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                    </div>
                    <div id="pnlViewAllVideos" runat="server" class="text-right">
                        <asp:LinkButton ID="lnkViewAllVideos" runat="server" onclick="lnkViewAllVideos_Click"/>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelAudioUploads" runat="server">
        <ContentTemplate>
            <h4 id="pnlAudioHeaderLine" runat="server"><uc1:HeaderLine ID="hlAudio" runat="server"/></h4>
            <div id="pnlAudioUploads" runat="server">
                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0" width="450" height="134" id="Object1">
                    <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf">
                    <param name="quality" value="high">
                    <param name="bgcolor" value="#FFFFFF">
                    <param name="wmode" value="transparent">
                    <param name="allowfullscreen" value="true">
                    <param name="flashvars" value="width=450&height=134&playlist=right&playlistsize=220&file=Components/Profile/<%= Server.UrlEncode("UserAudioData.ashx?uid=" + User.Username + "&vid=" + (((PageBase)Page).CurrentUserSession != null ? ((PageBase)Page).CurrentUserSession.Username : String.Empty)) %>&searchbar=false&logo=<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/watermark2.png" />
                    <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf"
                        quality="high" wmode="transparent" bgcolor="#FFFFFF" width="450" height="134"
                        name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                        pluginspage="http://www.macromedia.com/go/getflashplayer" allowscriptaccess="always"
                        flashvars="width=450&height=134&playlist=right&playlistsize=220&file=Components/Profile/<%= Server.UrlEncode("UserAudioData.ashx?uid=" + User.Username + "&vid=" + (((PageBase)Page).CurrentUserSession != null ? ((PageBase)Page).CurrentUserSession.Username : String.Empty)) %>&searchbar=false&logo=<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/watermark2.png"></embed>
                </object>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:PlaceHolder ID="plhProfile" runat="server"/>
    <asp:UpdatePanel ID="UpdatePanelFriends" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div id="divUserFriends" runat="server">
                <h4><uc1:HeaderLine ID="hlUserFriends" runat="server"/></h4>
                <asp:Label ID="lblFriendsPrivacySettingsError" runat="server" EnableViewState="false"/>
                <div id="pnlFriends" runat="server">
                    <asp:DataList ID="dlUserFriends" CssClass="repeater-horizontal" RepeatLayout="Flow" runat="server" GridLines="None" SkinID="Friends">
                        <ItemTemplate>
                            <a class="thumbnail" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                                <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 50, 50, "", true, true, true) %>
                                <div class="caption">
                                    <%# Eval("Username") %>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:DataList>
                    <div id="divViewAllFriends" class="text-right" runat="server">
                        <asp:LinkButton ID="lnkViewAllFriends" runat="server" OnClick="lnkViewAllFriends_Click"/>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="pnlUserComments" runat="server">
        <hr />
        <asp:UpdatePanel ID="UpdatePanelComments" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <label><uc1:HeaderLine ID="hlUserComments" runat="server"/></label>
                <span id="spanAddNewComment" runat="server">
                    <div id="divAddCommentLink" class="pull-right">
                        <a class="btn btn-default btn-xs" href="javascript: void(0)" onclick="document.getElementById('divAddCommentLink').style.display = 'none'; document.getElementById('divAddCommentBox').style.display = 'block';">
                            <i class="fa fa-comment-o"></i>&nbsp;<%= Lang.Trans("Add Comment") %>
                        </a>
                    </div>
                    <div id="divAddCommentBox" style="display: none">
                        <asp:TextBox ID="txtNewComment" CssClass="form-control" MaxLength="200" Rows="3" runat="server" TextMode="MultiLine"/>
                        <div class="actions">
                            <asp:Button CssClass="btn btn-default" ID="btnSubmitNewComment" runat="server" OnClick="btnSubmitNewComment_Click"/>
                        </div>
                    </div>
                </span>
                <asp:Repeater ID="rptComments" runat="server">
                    <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
                    <ItemTemplate>
                        <li class="list-group-item">
                            <p>
                                <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("FromUsername"))%>' target="_blank" onmouseover="showUserPreview('<%# Eval("FromUsername") %>')" onmouseout="hideUserPreview()"><%# Eval("FromUsername") %></a>&nbsp;
                                <%# Eval("CommentText") %>
                            </p>
                            <div class="clearfix">
                                <small class="text-muted"><i class="fa fa-clock-o"></i>&nbsp;<%# ((DateTime)Eval("DatePosted")).ToShortDateString() %></small>
                                <span class="pull-right">
                                    <asp:LinkButton ID="lnkDeleteComment" CssClass="btn btn-default btn-xs" CommandName="DeleteComment" CommandArgument='<%# Eval("Id") %>' Visible='<%# Eval("CanDelete") %>' runat="server">
                                        <i class="fa fa-trash-o"></i>&nbsp;<%# Lang.Trans("Delete") %>
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate></ul></FooterTemplate>
                </asp:Repeater>
                <div id="divViewAllComments" runat="server" class="text-right">
                    <asp:LinkButton ID="lnkViewAllComments" runat="server"/>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
<components:BannerView id="bvShowUserRightBottom" runat="server" Key="ShowUserRightBottom" />
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server" />
<script type="text/javascript">
    $(document).ready(function() {
        $('.tooltip-go').tooltip({
            animation: false
        });
    });
</script>