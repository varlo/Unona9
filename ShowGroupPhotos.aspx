<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowGroupPhotos.aspx.cs" Inherits="AspNetDating.ShowGroupPhotos" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupMembers" Src="~/Components/Groups/GroupMembers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewGroupPhotos" Src="~/Components/Groups/ViewGroupPhotos.ascx" %>
<%@ Register TagPrefix="uc1" TagName="UploadGroupPhoto" Src="~/Components/Groups/UploadGroupPhoto.ascx" %>
<%@ MasterType TypeName="AspNetDating.Site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server"/>
	        <ul class="nav">
                <li><asp:linkbutton id="lnkGroupHome" Runat="server" OnClick="lnkGroupHome_Click"/></li>
                <li><asp:linkbutton id="lnkGroupGallery" Runat="server" OnClick="lnkGroupGallery_Click"/></li>
                <li id="pnlGroupMembers" runat="server"><asp:linkbutton id="lnkGroupMembers" Runat="server" OnClick="lnkGroupMembers_Click"/></li>
                <li id="pnlMessageBoard" runat="server"><asp:linkbutton id="lnkMessageBoard" Runat="server" OnClick="lnkMessageBoard_Click" /></li>
                <li id="pnlGroupEvents" runat="server"><asp:linkbutton id="lnkGroupEvents" Runat="server" OnClick="lnkGroupEvents_Click"/></li>
                <li id="pnlAjaxChat" runat="server"><asp:HyperLink id="lnkOpenAjaxChat" Runat="server"/></li>
                <li id="pnlUploadPhoto" runat="server"><asp:linkbutton id="lnkUploadPhoto" Runat="server" OnClick="lnkUploadPhoto_Click"/></li>
                <li><asp:linkbutton id="lnkBrowseGroups" Runat="server" OnClick="lnkBrowseGroups_Click"/></li>
            </ul>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <asp:MultiView ID="mvGroup" runat="server">
            <asp:View ID="viewGroupPhotos" runat="server">
                <uc1:ViewGroupPhotos ID="ViewGroupPhotos1" runat="server" />
            </asp:View>
            <asp:View ID="viewUploadPhoto" runat="server">
                <uc1:UploadGroupPhoto ID="UploadGroupPhoto1" runat="server" />
            </asp:View>
            <asp:View ID="viewGroupMembers" runat="server">
                <uc1:GroupMembers ID="GroupMembers1" runat="server" />
            </asp:View>
        </asp:MultiView>
    </article>
</asp:Content>
