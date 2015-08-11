<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AddPost" Src="AddPost.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ViewBlog.ascx.cs" Inherits="AspNetDating.Components.Blog.ViewBlogCtrl"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<uc1:LargeBoxStart ID="LargeBoxStart" runat="server"/>
<components:BannerView id="bvBlogRightTop" runat="server" Key="BlogRightTop"/>
<h4><uc1:HeaderLine ID="hlBlog" runat="server"/></h4>
<div id="divViewBlog" runat="server">
    <asp:Repeater ID="rptBlogPosts" runat="server">
        <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
        <ItemTemplate>
            <li class="list-group-item">
                <h5><asp:LinkButton ID="lnkViewPost" CommandName="ViewPost" CommandArgument='<%# Eval("Id") %>' Runat="server"><%# Eval("Title") %></asp:LinkButton></h5>
                <p>
                    <%# Eval("Content") %><asp:LinkButton ID="lnkViewPost2" CommandName="ViewPost" CommandArgument='<%# Eval("Id") %>' runat="server"><%# Lang.Trans("more") %>&nbsp;<i class="fa fa-angle-double-right"></i></asp:LinkButton>
                </p>
                <small class="text-muted"><a class="tooltip-link" title="<%= Lang.Trans("Date Posted") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# ((DateTime)Eval("DatePosted")).ToShortDateString() %></a></small>
            </li>
        </ItemTemplate>
        <FooterTemplate></ul></FooterTemplate>
    </asp:Repeater>
</div>

<div id="divViewPost" runat="server" visible="false">
    <div class="info-header">
        <asp:LinkButton ID="lnkBackToBlog2" CssClass="btn btn-default btn-sm" runat="server" OnClick="lnkBackToBlog_Click"/>
        <div id="divManagePost" class="pull-right btn-group btn-group-sm" runat="server" visible="false">
            <asp:LinkButton CssClass="btn btn-default" ID="lnkEditPost" runat="server" OnClick="lnkEditPost_Click"/>
            <asp:LinkButton CssClass="btn btn-default" ID="lnkDeletePost" runat="server" OnClick="lnkDeletePost_Click"/>
        </div>
    </div>
    <label><asp:Label ID="lblTitle" runat="server"/></label>
    <p class="text-muted"><i class="fa fa-link"></i>&nbsp;<asp:Label ID="lblDirectLink" runat="server"/></p>
    <p><asp:Label ID="lblContent" runat="server"/></p>
    <small class="text-muted"><a class="tooltip-link" title="<%= Lang.Trans("Date Posted") %>"><i class="fa fa-clock-o"></i>&nbsp;<asp:Label ID="lblDate" runat="server"/></a></small>
    <!--<asp:LinkButton ID="lnkBackToBlog" CssClass="btn btn-default btn-sm" runat="server" OnClick="lnkBackToBlog_Click"/>-->
    <div id="pnlBlogComments" runat="server">
        <hr />
        <label><uc1:HeaderLine ID="hlUserComments" runat="server"/></label>
        <span id="spanAddNewComment" runat="server">
            <div id="divAddCommentLink" class="pull-right">
                <a class="btn btn-default btn-xs" onclick="document.getElementById('divAddCommentLink').style.display = 'none'; document.getElementById('divAddCommentBox').style.display = 'block';" href="javascript: void(0)">
                    <i class="fa fa-comment-o"></i>&nbsp;<%= Lang.Trans("Add Comment") %>
                </a>
            </div>
            <div id="divAddCommentBox" style="display: none">
                <asp:TextBox ID="txtNewComment" CssClass="form-control" Rows="3" runat="server" TextMode="MultiLine"/>
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
                        <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' target=_blank onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()"><%# Eval("Username") %></a>&nbsp;
                        <%# ((string)Eval("CommentText")).Length > 250 ?
                        ((string)Eval("CommentText")).Substring(0, 200) + "<span id=\"spanCommentsMore" + Eval("Id") + "\">... [ <a href=\"javascript: void 0;\" onclick=\"document.getElementById('spanCommentsFull" + Eval("Id") + "').style.display = 'inline';document.getElementById('spanCommentsMore" + Eval("Id") + "').style.display = 'none';\">" + Lang.Trans("more") + "</a> ]</span><span id=\"spanCommentsFull" + Eval("Id") + "\" style=\"display: none;\">" + ((string)Eval("CommentText")).Substring(200) + "</span>"  :
                        Eval("CommentText") %>
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
    </div>
</div>
<uc1:AddPost ID="AddPost1" runat="server" Visible="False"/>
<components:BannerView id="bvBlogRightBottom" runat="server" Key="BlogRightBottom"/>
<uc1:LargeBoxEnd runat="server" ID="Largeboxend1"/>
