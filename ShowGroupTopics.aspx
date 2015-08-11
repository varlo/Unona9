<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowGroupTopics.aspx.cs" Inherits="AspNetDating.ShowGroupTopics" %>
<%@ Register TagPrefix="uc1" TagName="ViewTopics" Src="~/Components/Groups/ViewTopics.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditPost" Src="~/Components/Groups/EditPost.ascx"%>
<%@ Register TagPrefix="uc1" TagName="ViewPosts" Src="~/Components/Groups/ViewPosts.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchTopicResults" Src="~/Components/Groups/SearchTopicResults.ascx" %>
<%@ MasterType TypeName="AspNetDating.Site" %>
<%@ Import namespace="AspNetDating.Classes"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server"/>
	        <ul class="nav">
                <li><asp:linkbutton id="lnkGroupHome" Runat="server" OnClick="lnkGroupHome_Click"/></li>
                <li id="pnlGroupGallery" runat="server"><asp:linkbutton id="lnkGroupGallery" Runat="server" OnClick="lnkGroupGallery_Click" /></li>
                <li id="pnlGroupMembers" runat="server"><asp:linkbutton id="lnkGroupMembers" Runat="server" OnClick="lnkGroupMembers_Click" /></li>
                <li id="pnlMessageBoard" runat="server"><asp:linkbutton id="lnkMessageBoard" Runat="server" OnClick="lnkMessageBoard_Click"/></li>
                <li id="pnlGroupEvents" runat="server"><asp:linkbutton id="lnkGroupEvents" Runat="server" OnClick="lnkGroupEvents_Click"/></li>
                <li id="pnlAjaxChat" runat="server"><asp:HyperLink id="lnkOpenAjaxChat" Runat="server" /></li>
                <li id="pnlStartNewTopic" runat="server"><asp:linkbutton id="lnkStartNewTopic" Runat="server" OnClick="lnkStartNewTopic_Click"/></li>
                <li id="pnlSubscribeForTopic" runat="server"><asp:linkbutton id="lnkSubscribeForTopic" Runat="server" OnClick="lnkSubscribeForTopic_Click"/></li>
                <li id="pnlUnsubscribeFromTopic" runat="server"><asp:linkbutton id="lnkUnsubscribeFromTopic" Runat="server" OnClick="lnkUnsubscribeFromTopic_Click"/></li>
                <li id="pnlAddNewPost" runat="server"><asp:linkbutton id="lnkAddNewPost" Runat="server" OnClick="lnkAddNewPost_Click"/></li>
                <li id="pnlEditTopic" runat="server"><asp:linkbutton id="lnkEditTopic" Runat="server" OnClick="lnkEditTopic_Click"/></li>
                <li id="pnlDeleteTopic" runat="server"><asp:linkbutton id="lnkDeleteTopic" Runat="server" OnClick="lnkDeleteTopic_Click"/></li>
                <li><asp:linkbutton id="lnkBrowseGroups" Runat="server" OnClick="lnkBrowseGroups_Click" /></li>
            </ul>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <asp:MultiView ID="mvTopics" runat="server">
            <asp:View ID="viewMain" runat="server">
                <uc1:ViewTopics ID="ViewTopics1" runat="server" />
            </asp:View>
            <asp:View ID="viewStartNewTopic" runat="server">
                <uc1:EditPost ID="EditPost1" runat="server" />
            </asp:View>
            <asp:View ID="viewPosts" runat="server">
                <uc1:ViewPosts ID="ViewPosts1" runat="server" />
            </asp:View>
            <asp:View ID="viewSearchResults" runat="server">
			        <uc1:SearchTopicResults id="SearchTopicResults1" runat="server"/>
			</asp:View>
        </asp:MultiView>
    </article>
</asp:Content>
