<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="PhotoContests.aspx.cs"
    Inherits="AspNetDating.PhotoContestsPage" %>

<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register src="Components/HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server" />
            <ul class="nav">
                <li><asp:LinkButton ID="lnkViewActiveContests" Runat="server" Enabled="False" OnClick="lnkViewActiveContests_Click" /></li>
                <li><asp:LinkButton ID="lnkViewPastContests" Runat="server" OnClick="lnkViewPastContests_Click" /></li>
		    </ul>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server" />
    </aside>
    <article>
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server" />
        <asp:Repeater ID="rptContests" runat="server">
            <ItemTemplate>
                <h4><uc2:HeaderLine ID="hlName" Title='<%# Eval("Name")  %>' runat="server" /></h4>
                <div class="media">
                    <div class="pull-left">
                        <%# ImageHandler.RenderImageTag((int)Eval("TopPhotoId1"), 90, 90, "img-thumbnail", false, false)%>
                        <%# ImageHandler.RenderImageTag((int)Eval("TopPhotoId2"), 90, 90, "img-thumbnail", false, false)%>
                        <%# ImageHandler.RenderImageTag((int)Eval("TopPhotoId3"), 90, 90, "img-thumbnail", false, false)%>
                    </div>
                    <div class="media-body">
                        <p><%# Eval("Description") %></p>
                        <ul class="info-header info-header-sm">
                            <li><%= Lang.Trans("Entries") %>:&nbsp;<b><%# Eval("Entries") %></b></li>
                            <li class="pull-right">
                                <span runat="server" visible='<%# !lnkViewActiveContests.Enabled %>'>
                                    <a class="btn btn-default btn-xs" href="PhotoContest.aspx?cid=<%# Eval("ContestId") %>"><%= Lang.Trans("View Contest") %></a>
                                </span>
                                <span runat="server" visible='<%# (int) Eval("Entries") > Config.Ratings.MinPhotosToStartContest %>'>
                                    <a class="btn btn-default btn-xs" href="PhotoContest.aspx?cid=<%# Eval("ContestId") %>&top=1"><%= Lang.Trans("View Top Entries") %></a>
                                </span>
                            </li>
                        </ul>
                	</div>
                </div>
            </ItemTemplate>
            <SeparatorTemplate><hr /></SeparatorTemplate>
        </asp:Repeater>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server" />
    </article>
</asp:Content>
