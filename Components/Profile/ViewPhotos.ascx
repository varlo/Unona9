<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="ViewPhotos.ascx.cs"
    Inherits="AspNetDating.Components.Profile.ViewPhotos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Src="../ReportAbuse.ascx" TagName="ReportAbuse" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<uc1:LargeBoxStart ID="LargeBoxStart" runat="server"/>
<input type="hidden" id="hidUsername" runat="server" name="hidUsername" />
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
            <div id="divSlideshowLink" class="text-right" runat="server">
                <asp:LinkButton ID="lnkRunSlideshow" runat="server" OnClick="lnkRunSlideshow_Click"/>
            </div>
            <hr />
            <components:BannerView id="bvShowUserPhotosAbovePhoto" runat="server" Key="ShowUserPhotosAbovePhoto"/>
            <div id="divSlideshow" class="text-center" runat="server" visible="false">
                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0" width="<%= Config.Photos.PhotoMaxWidth %>" height="<%= Config.Photos.PhotoMaxHeight %>" id="flvplayer">
                    <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/ImageRotator/imagerotator.swf">
                    <param name="quality" value="high">
                    <param name="bgcolor" value="#FFFFFF">
                    <param name="wmode" value="transparent">
                    <param name="allowfullscreen" value="true">
                    <param name="menu" value="false">
                    <param name="flashvars" value="width=<%= Config.Photos.PhotoMaxWidth %>&height=<%= Config.Photos.PhotoMaxHeight %>&file=Components/Profile/<%= "UserPhotoData.ashx?uid=" + User.Username %>&shuffle=false" />
                    <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/ImageRotator/imagerotator.swf"
                        quality="high" wmode="transparent" bgcolor="#FFFFFF" width="<%= Config.Photos.PhotoMaxWidth %>"
                        height="<%= Config.Photos.PhotoMaxHeight %>" name="flvplayer" align="" type="application/x-shockwave-flash"
                        allowfullscreen="true" menu="false" pluginspage="http://www.macromedia.com/go/getflashplayer"
                        flashvars="width=<%= Config.Photos.PhotoMaxWidth %>&height=<%= Config.Photos.PhotoMaxHeight %>&file=Components/Profile/<%= "UserPhotoData.ashx?uid=" + User.Username %>&shuffle=false"></embed>
                    </object>
            </div>
            <div id="pnlPhotos" runat="server">
                <asp:UpdatePanel ID="UpdatePanelPhotos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="pnlPhotoAlbums" runat="server" Visible="false">
                            <label><%= "Album name".Translate() %></label>&nbsp;
                            <asp:DropDownList ID="ddPhotoAlbums" CssClass="form-control form-control-inline" runat="server" AutoPostBack="true" onselectedindexchanged="ddPhotoAlbums_SelectedIndexChanged"/>
                        </div>
                        <br />
                        <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" />
                        <div class="text-center">
                            <asp:LinkButton CssClass="img-thumbnail" ID="lnkPhoto" runat="server" OnClick="lnkPhoto_Click">
                                <asp:Literal ID="ltrPhoto" runat="server"/>
                            </asp:LinkButton>
                            <div id="pnlUnlockPhotos" runat="server">
                                <components:ContentView ID="cvUnlockPhotos" Key="UnlockPhotos" runat="server"/>
                                <asp:Button CssClass="btn btn-default" ID="btnUnlockPhotos" runat="server" onclick="btnUnlockPhotos_Click" />
                            </div>
                            <div id="divUsersInPhoto" visible="false" runat="server" enableviewstate="false">
                                <label><%= "In this photo:".Translate() %></label>&nbsp;<asp:Literal ID="ltrUsersInPhoto" runat="server" />
                            </div>
                            <components:BannerView id="bvShowUserPhotosUnderPhoto" runat="server" Key="ShowUserPhotosUnderPhoto"/>
                            <div id="pnlPhotoRating" runat="server">
                                <div class="small text-muted">
                                    <label><asp:Label ID="lblRating" runat="server"/></label>
                                    <asp:Label ID="lblRatingAverage" runat="server"/>
                                </div>
                                <asp:Panel ID="pnlRatePhoto" runat="server" Visible="False" HorizontalAlign="Center">
                                    <div class="input-group input-group-sm col-sm-4 col-sm-offset-4">
                                        <span class="input-group-addon"><%= "Your Rating".Translate() %></span>
                                        <asp:DropDownList CssClass="form-control" ID="ddRating" runat="server"/>
                                        <span class="input-group-btn"><asp:Button CssClass="btn btn-default" ID="btnRatePhoto" runat="server"/></span>
                                    </div>
                                    <input id="hidcurrentPhotoID" type="hidden" runat="server" name="hidcurrentPhotoID" />
                                </asp:Panel>
                            </div>
                            <label><asp:Label ID="lblName" runat="server" EnableViewState="false" /></label>
                            <div><asp:Label ID="lblDescription" runat="server" EnableViewState="false" /></div>
                            <div id="pnlReportAbuseLink" runat="server">
                                <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkReportAbuse" OnClick="lnkReportAbuse_Click" runat="server"/>
                            </div>
                        </div>
                        <div id="pnlUserComments" runat="server">
                            <hr />
                            <label><uc1:HeaderLine ID="hlUserComments" runat="server"/></label>
                            <span id="spanAddNewComment" runat="server">
                                <div id="divAddCommentLink" class="pull-right">
                                    <a class="btn btn-default btn-xs" href="javascript: void(0)" onclick="document.getElementById('divAddCommentLink').style.display = 'none'; document.getElementById('divAddCommentBox').style.display = 'block';">
                                        <i class="fa fa-comment-o"></i>&nbsp;<%= Lang.Trans("Add Comment") %>
                                    </a>
                                </div>
                                <div id="divAddCommentBox" style="display: none">
                                    <asp:TextBox ID="txtNewComment" CssClass="form-control" Rows="3" MaxLength="200" runat="server" TextMode="MultiLine"/>
                                    <div class="actions">
                                        <asp:Button CssClass="btn btn-default" ID="btnSubmitNewComment" runat="server" OnClick="btnSubmitNewComment_Click"/>
                                    </div>
                                </div>
                            </span>
                            <asp:Repeater ID="rptComments" runat="server" OnItemCommand="rptComments_ItemCommand">
                                <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
                                <ItemTemplate>
                                    <li class="list-group-item">
                                        <p>
                                            <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username"))%>' target="_blank" onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()">
                                                <%# Eval("Username") %>
                                            </a>&nbsp;
                                            <%# Eval("Comment") %>
                                        </p>
                                        <div class="clearfix">
                                            <small class="text-muted"><i class="fa fa-clock-o"></i>&nbsp;<%# ((DateTime)Eval("Date")).ToShortDateString() %></small>
                                            <span class="pull-right">
                                                <asp:LinkButton ID="lnkDeleteComment" CssClass="btn btn-default btn-xs" CommandName="DeleteComment" CommandArgument='<%# Eval("ID") %>' Visible='<%# Eval("CanDelete") %>' runat="server">
                                                    <i class="fa fa-trash-o"></i>&nbsp;<%# Lang.Trans("Delete") %>
                                                </asp:LinkButton>
                                            </span>
                                        </div>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate></ul></FooterTemplate>
                            </asp:Repeater>
                            <div id="divViewAllComments" runat="server"  class="text-right">
                                <asp:LinkButton ID="lnkViewAllComments" runat="server" OnClick="lnkViewAllComments_Click"/>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="dlPhotos" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
                <hr />
                <asp:UpdatePanel ID="UpdatePanelThumbnails" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <asp:DataList ID="dlPhotos" runat="server" SkinID="UserPhotos" CssClass="repeater-horizontal" RepeatLayout="Flow" ShowFooter="False" ShowHeader="False">
                            <ItemTemplate>
                            	<asp:LinkButton CssClass="thumbnail" ID="lnkShowPhoto" CommandName="ShowPhoto" CommandArgument='<%# Eval("PhotoId") %>' runat="server">
								    <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 100, 100, "", true, true, false) %>
								</asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </ContentTemplate>
                      <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddPhotoAlbums" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <uc2:ReportAbuse ID="ReportAbuse1" Visible="false" runat="server" />
<uc1:LargeBoxEnd ID="LargeBoxEnd" runat="server"/>
