<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowGroup.aspx.cs" Inherits="AspNetDating.ShowGroup" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewGroup" Src="~/Components/Groups/ViewGroup.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupMembers" Src="~/Components/Groups/GroupMembers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PendingMembers" Src="~/Components/Groups/PendingMembers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditGroup" Src="~/Components/Groups/EditGroup.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SendAnnouncement" Src="~/Components/Groups/SendAnnouncement.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InviteFriends" Src="~/Components/Groups/InviteFriends.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupBans" Src="~/Components/Groups/GroupBans.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ MasterType TypeName="AspNetDating.Site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
        <uc1:smallboxstart id="SmallBoxStart1" runat="server" />
            <ul class="nav">
                <li id="pnlGroupHome" runat="server"><asp:linkbutton id="lnkGroupHome" Runat="server" OnClick="lnkGroupHome_Click" /></li>
                <li id="pnlGroupGallery" runat="server"><asp:linkbutton id="lnkGroupGallery" Runat="server" OnClick="lnkGroupGallery_Click" /></li>
                <li id="pnlGroupMembers" runat="server"><asp:linkbutton id="lnkGroupMembers" Runat="server" OnClick="lnkGroupMembers_Click" /></li>
                <li id="pnlMessageBoard" runat="server"><asp:linkbutton id="lnkMessageBoard" Runat="server" OnClick="lnkMessageBoard_Click" /></li>
                <li id="pnlGroupEvents" runat="server"><asp:linkbutton id="lnkGroupEvents" Runat="server" OnClick="lnkGroupEvents_Click" /></li>
                <li id="pnlAjaxChat" runat="server"><asp:HyperLink id="lnkOpenAjaxChat" Runat="server" /></li>
                <li id="pnlInviteFriends" runat="server"><asp:linkbutton id="lnkInviteFriends" Runat="server" OnClick="lnkInviteFriends_Click" /></li>
                <li id="pnlPendingMembers" runat="server"><asp:linkbutton id="lnkPendingMembers" Runat="server" OnClick="lnkPendingMembers_Click" /></li>
                <li id="pnlManageGroup" runat="server"><asp:linkbutton id="lnkManageGroup" Runat="server" OnClick="lnkManageGroup_Click" /></li>
                <li id="pnlGroupBans" runat="server"><asp:linkbutton id="lnkGroupBans" Runat="server" OnClick="lnkGroupBans_Click" /></li>
                <li id="pnlSendAnnouncement" runat="server"><asp:linkbutton id="lnkSendAnnouncement" Runat="server" OnClick="lnkSendAnnouncement_Click" /></li>
                <li id="pnlJoinGroup" runat="server"><asp:linkbutton id="lnkJoinGroup" Runat="server" OnClick="lnkJoinGroup_Click" /></li>
                <li id="pnlLeaveGroup" runat="server"><asp:linkbutton id="lnkLeaveGroup" Runat="server" OnClick="lnkLeaveGroup_Click" /></li>
                <li id="pnlBrowseGroups" runat="server"><asp:linkbutton id="lnkBrowseGroups" Runat="server" OnClick="lnkBrowseGroups_Click" /></li>
            </ul>
        <uc1:smallboxend id="SmallBoxEnd1" runat="server" />
    </aside>
    <article>
        <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False" />
        <asp:MultiView ID="mvGroup" runat="server">
            <asp:View ID="viewGroupHome" runat="server">
                <uc1:ViewGroup ID="ViewGroup1" runat="server" />
            </asp:View>
            <asp:View ID="viewGroupMembers" runat="server">
                <uc1:GroupMembers ID="GroupMembers1" runat="server" />
            </asp:View>
            <asp:View ID="viewPendingMembers" runat="server">
                <uc1:PendingMembers ID="PendingMembers1" runat="server" />
            </asp:View>
            <asp:View ID="viewManageGroup" runat="server">
                <uc1:EditGroup ID="EditGroup1" runat="server" />
            </asp:View>
            <asp:View ID="viewGroupBans" runat="server">
                <uc1:GroupBans id="GroupBans1" runat="server" />
            </asp:View>
            <asp:View ID="viewSendAnnouncement" runat="server">
                <uc1:SendAnnouncement id="SendAnnoucement1" runat="server" />
            </asp:View>
            <asp:View ID="viewInviteFriends" runat="server">
                <uc1:InviteFriends id="InviteFriends1" runat="server" />
            </asp:View>
            <asp:View ID="viewJoinGroup" runat="server">
                <uc1:largeboxstart id="LargeBoxStart1" runat="server" />
                <asp:Label ID="lblJoinToGroupMessage" CssClass="text-danger" runat="server" EnableViewState="False" />
                <div class="actions">
	                <asp:Button ID="btnJoinToGroup" CssClass="btn btn-default" runat="server" OnClick="btnJoinToGroup_Click" />
                </div>
                <uc1:largeboxend id="LargeBoxEnd1" runat="server" />
            </asp:View>
        </asp:MultiView>
    </article>
</asp:Content>
