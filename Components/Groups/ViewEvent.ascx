<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEvent.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewEvent" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="~/Components/DatePicker.ascx" %>
<%@ Register Src="~/Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="~/Components/HeaderLine.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"/>
<asp:Label ID="Label1" CssClass="alert text-danger" runat="server" EnableViewState="false"/>
<div class="media">
    <img class="pull-left img-thumbnail" src='GroupEventImage.ashx?id=<%= EventID %>&width=120&height=120&diskCache=1' alt=""/>
    <div class="media-body">
        <h4 class="media-heading"><asp:Label ID="lblTitle" runat="server"/></h4>
        <ul class="info-header info-header-sm">
            <li><a class="tooltip-link" title="<%= Lang.Trans("Event date") %>"><i class="fa fa-clock-o"></i>&nbsp;<asp:Label ID="lblDate" runat="server"/></a></li>
            <li><a class="tooltip-link" title="<%= Lang.Trans("Created by") %>"><i class="fa fa-user"></i></a>&nbsp;<a href='ShowUser.aspx?uid=<%= CreatedBy %>'><%= CreatedBy %></a></li>
            <li><a class="tooltip-link" title="<%= Lang.Trans("Location") %>"><i class="fa fa-map-marker"></i>&nbsp;<asp:Label ID="lblLocation" runat="server"/></a></li>
        </ul>
        <asp:Image ID="imgGoogleMap" runat="server" Visible="false" />
        <asp:Label ID="lblDescription" runat="server"/>
	</div>
</div>
<div id="divUserFriends" Class="clearfix" runat="server">
    <hr />
    <p class="text-muted"><uc1:HeaderLine ID="hlUserFriends" runat="server"/></p>
    <asp:DataList ID="dlUserFriends" CssClass="repeater-horizontal clearfix" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" SkinID="Friends">
        <ItemTemplate>
            <a class="thumbnail" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' title="<%# Eval("Username") %>">
                <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 40, 40, "", true, true, true) %>
            </a>
        </ItemTemplate>
    </asp:DataList>
</div>
<asp:UpdatePanel Class="clearfix" ID="UpdatePanelAttenders" runat="server">
    <ContentTemplate>
        <hr />
        <p class="pull-right">
            <asp:Label ID="lblAttending" runat="server"/>
            <asp:Button CssClass="btn btn-default btn-xs" ID="btnJoinEvent" runat="server" onclick="btnJoinEvent_Click" />
            <asp:Button CssClass="btn btn-default btn-xs" ID="btnLeaveEvent" runat="server" onclick="btnLeaveEvent_Click" />
        </p>
        <div id="pnlAttenders" runat="server">
            <p class="text-muted"><uc1:HeaderLine ID="hlAttenders" runat="server"/></p>
            <asp:DataList ID="dlAttenders" CssClass="repeater-horizontal clearfix" RepeatLayout="Flow" runat="server" SkinID="Friends">
                <ItemTemplate>
                    <a class="thumbnail" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' title="<%# Eval("Username") %>">
                        <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 40, 40, "", true, true, true) %>
                    </a>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upnlGroupEventsComments" runat="server">
    <ContentTemplate>
    <div id="pnlBlogComments" runat="server">
        <hr />
        <label><uc1:HeaderLine ID="hlGroupEventsComments" runat="server"/></label>
        <span id="spanAddNewComment" runat="server">
            <div id="divAddCommentLink" class="pull-right">
                <a class="btn btn-default btn-xs" onclick="document.getElementById('divAddCommentLink').style.display = 'none'; document.getElementById('divAddCommentBox').style.display = 'block';" href="javascript: void(0)">
                    <i class="fa fa-comment-o"></i>&nbsp;<%= Lang.Trans("Add Comment") %>
                </a>
            </div>
            <div id="divAddCommentBox" style="display: none">
                <asp:TextBox ID="txtNewComment" runat="server" CssClass="form-control" TextMode="MultiLine"/>
                <div class="actions">
                    <asp:Button CssClass="btn btn-default" ID="btnSubmitNewComment" runat="server" onclick="btnSubmitNewComment_Click"/>
                </div>
           </div>
        </span>
        <asp:Repeater ID="rptComments" runat="server" onitemcommand="rptComments_ItemCommand" onitemcreated="rptComments_ItemCreated">
            <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
            <ItemTemplate>
                <li class="list-group-item">
                    <p>
                        <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' target=_blank><%# Eval("Username") %></a>:&nbsp;<%# Eval("Comment") %>
                    </p>
                    <div class="clearfix">
                        <small class="text-muted"><i class="fa fa-clock-o"></i>&nbsp;<%# ((DateTime)Eval("Date")).ToShortDateString() %></small>
                        <span class="pull-right">
                            <asp:LinkButton ID="lnkDeleteComment" CssClass="btn btn-default btn-xs" CommandName="DeleteComment" CommandArgument='<%# Eval("Id") %>' Visible='<%# Eval("CanDelete") %>' runat="server">
                                <i class="fa fa-trash-o"></i>&nbsp;<%# Lang.Trans("Remove") %>
                            </asp:LinkButton>
                        </span>
                    </div>
                </li>
            </ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
	    </asp:Repeater>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>