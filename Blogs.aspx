<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="Blogs.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.MemberBlog" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Settings" Src="Components/Blog/Settings.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AddPost" Src="Components/Blog/AddPost.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewBlog" Src="Components/Blog/ViewBlog.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
		<uc1:smallboxstart id="SmallBoxStart1" runat="server"/>
		<ul class="nav">
            <li><asp:linkbutton id="lnkNewPost" Runat="server" onclick="lnkNewPost_Click"/></li>
            <li><asp:linkbutton id="lnkViewBlog" Runat="server" onclick="lnkViewBlog_Click"/></li>
            <li><asp:linkbutton id="lnkEditBlog" Runat="server" onclick="lnkEditBlog_Click"/></li>
		</ul>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"/>
	</aside>
	<article>
		<uc1:settings id="Settings" runat="server"/>
		<uc1:addpost id="AddPost" runat="server"/>
		<uc1:viewblog id="ViewBlog" runat="server"/>
	</article>
</asp:Content>