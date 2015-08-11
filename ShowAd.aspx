<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowAd.aspx.cs" Inherits="AspNetDating.ShowAd" %>

<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="~/Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server"/>
	        <ul class="nav">
                <li><a href='Ads.aspx'><%= "All Classifieds".Translate() %></a></li>
                <li><a ID="lnkBackToCategory" runat="server"></a></li>
                <li><a href='Ads.aspx?uid=<%= PostedBy %>'><%= "User other listings".Translate()%></a></li>
                <li id="pnlEdit" runat="server"><a ID="lnkEdit" runat="server"></a></li>
                <li id="pnlDelete" runat="server"><asp:LinkButton ID="lnkDelete" runat="server" onclick="lnkDelete_Click"/></li>
                <li id="pnlMyAds" runat="server"><a href='Ads.aspx?show=ma'><%= "My Classifieds".Translate()%></a></li>
            </ul>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <asp:UpdatePanel ID="UpdatePanelPostAd" runat="server">
            <ContentTemplate>
                <uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
                    <div class="input-group input-group-sm filter">
                        <span class="input-group-addon"><%= "Keyword".Translate() %>:</span>
                        <asp:TextBox ID="txtKeyword" CssClass="form-control" runat="server" />
                        <span class="input-group-btn"><uc1:FlexButton ID="fbSearch" CssClass="btn btn-default" runat="server" RenderAs="Button" OnClick="fbSearch_Click"/></span>
                    </div>
                    <asp:Label ID="lblError" CssClass="alert text-danger" runat="server" EnableViewState="False"/>
                    <h4><asp:Label ID="lblSubject" runat="server"/></h4>
                    <ul class="info-header info-header-sm">
                        <li><a class="tooltip-link" title="<%= Lang.Trans("Posted On") %>"><i class="fa fa-clock-o"></i>&nbsp;<asp:Label ID="lblDate" runat="server"/></a></li>
                        <li><a class="tooltip-link" title="<%= Lang.Trans("Posted By") %>"><i class="fa fa-user"></i></a>&nbsp;<a id="lnkPostedBy" runat="server"><asp:Label ID="lblPostedBy" runat="server"/></a></li>
                        <li><a class="tooltip-link" title="<%= Lang.Trans("Location") %>"><i class="fa fa-map-marker"></i>&nbsp;<asp:Label ID="lblLocation" runat="server"/></a></li>
                    </ul>
                    <p><asp:Label ID="lblDescription" runat="server"/></p>
                    <p class="text-center">
                        <asp:LinkButton ID="lnkPhoto" runat="server" onclick="lnkPhoto_Click">
                            <asp:Literal ID="ltrPhoto" runat="server"/>
                        </asp:LinkButton>
                        <asp:Label ID="lblAdPhotoDescription" runat="server"/>
                    </p>
                    <p class="clearfix">
                    <asp:DataList ID="dlPhotos" runat="server" SkinID="UserPhotos" CssClass="repeater-horizontal" RepeatLayout="Flow" ShowFooter="False" ShowHeader="False" onitemcommand="dlPhotos_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkShowPhoto" CommandName="ShowPhoto" CommandArgument='<%# Eval("ID") %>' runat="server">
                            <%# AspNetDating.AdPhoto.RenderImageTag((int)Eval("ID"), 100, 100, "img-thumbnail", true) %></asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList>
                    </p>
                    <div id="pnlUserComments" runat="server">
                        <label><uc1:HeaderLine ID="hlUserComments" runat="server"/></label>
                        <span id="spanAddNewComment" runat="server">
                            <div id="divAddCommentLink" class="pull-right">
                                <a class="btn btn-default btn-xs" href="javascript: void(0)" onclick="document.getElementById('divAddCommentLink').style.display = 'none'; document.getElementById('divAddCommentBox').style.display = 'block';">
                                    <i class="fa fa-comment-o"></i>&nbsp;<%= Lang.Trans("Add Comment") %>
                                </a>
                            </div>
                            <div id="divAddCommentBox" style="display: none">
                                <asp:TextBox ID="txtNewComment" CssClass="form-control" Rows="5" MaxLength="200" runat="server" TextMode="MultiLine"/>
                                <div class="actions">
                                    <asp:Button CssClass="btn btn-default" ID="btnSubmitNewComment" runat="server" OnClick="btnSubmitNewComment_Click" />
                                </div>
                            </div>
                        </span>
                        <asp:Repeater ID="rptComments" runat="server" OnItemCommand="rptComments_ItemCommand">
                            <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
                            <ItemTemplate>
                                <li class="list-group-item">
                                    <p><a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username"))%>' target="_blank" onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()"><%# Eval("Username") %></a>&nbsp;
                                    <%# Eval("Comment") %></p>
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
                        <div id="divViewAllComments" runat="server" class="text-right">
                            <asp:LinkButton ID="lnkViewAllComments" runat="server" OnClick="lnkViewAllComments_Click"/>
                        </div>
                    </div>
                <uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
            </ContentTemplate>
        </asp:UpdatePanel>
    </article>
</asp:Content>
