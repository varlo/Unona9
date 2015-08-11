<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowGroupEvents.aspx.cs"
    Inherits="AspNetDating.ShowGroupEvents" %>

<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditEvent" Src="~/Components/Groups/EditEvent.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewEvents" Src="~/Components/Groups/ViewEvents.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewEvent" Src="~/Components/Groups/ViewEvent.ascx" %>
<%@ MasterType TypeName="AspNetDating.Site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <aside>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"/>
            <ul class="nav">
                <li><asp:LinkButton ID="lnkGroupHome" runat="server" OnClick="lnkGroupHome_Click"/></li>
                <li id="pnlGroupGallery" runat="server"><asp:LinkButton ID="lnkGroupGallery" runat="server" OnClick="lnkGroupGallery_Click"/></li>
                <li id="pnlGroupMembers" runat="server"><asp:LinkButton ID="lnkGroupMembers" runat="server" OnClick="lnkGroupMembers_Click"/></li>
                <li id="pnlMessageBoard" runat="server"><asp:LinkButton ID="lnkMessageBoard" runat="server" OnClick="lnkMessageBoard_Click"/></li>
                <li id="pnlGroupEvents" runat="server"><asp:LinkButton ID="lnkGroupEvents" runat="server" OnClick="lnkGroupEvents_Click"/></li>
                <li id="pnlAjaxChat" runat="server"><asp:HyperLink ID="lnkOpenAjaxChat" runat="server"/></li>
                <li id="pnlAddEvent" runat="server"><asp:LinkButton ID="lnkAddEvent" runat="server" OnClick="lnkAddEvent_Click"/></li>
                <li id="pnlEditEvent" runat="server"><asp:LinkButton ID="lnkEditEvent" runat="server" OnClick="lnkEditEvent_Click"/></li>
                <li id="pnlDeleteEvent" runat="server"><asp:LinkButton ID="lnkDeleteEvent" runat="server" OnClick="lnkDeleteEvent_Click"/></li>
                <div id="divAddThis" visible="false" runat="server"></div>
                <li id="pnlBrowseGroups" runat="server"><asp:LinkButton ID="lnkBrowseGroups" runat="server" OnClick="lnkBrowseGroups_Click"/></li>
            </ul>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <asp:MultiView ID="mvGroupEvents" runat="server">
            <asp:View ID="viewEvents" runat="server">
                <uc1:ViewEvents ID="ViewEvents1" runat="server" />
            </asp:View>
            <asp:View ID="viewEdit" runat="server">
                <uc1:EditEvent ID="EditEvent1" runat="server"/>
            </asp:View>
            <asp:View ID="viewEvent" runat="server">
                <uc1:ViewEvent ID="ViewEvent1" runat="server"/>
            </asp:View>
        </asp:MultiView>
    </article>
</asp:Content>
