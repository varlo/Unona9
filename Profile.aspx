<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="Profile.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.MemberProfile" %>
<%@ Register Src="Components/Profile/Gadgets.ascx" TagName="Gadgets" TagPrefix="uc9" %>
<%@ Register Src="Components/Profile/Billing.ascx" TagName="Billing" TagPrefix="uc6" %>
<%@ Register Src="Components/Profile/RecordVideo.ascx" TagName="RecordVideo" TagPrefix="uc7" %>
<%@ Register Src="Components/Profile/UploadVideo.ascx" TagName="UploadVideo" TagPrefix="uc10" %>
<%@ Register Src="Components/Profile/UploadAudio.ascx" TagName="UploadAudio" TagPrefix="uc12" %>
<%@ Register Src="Components/Profile/Settings.ascx" TagName="Settings" TagPrefix="uc8" %>
<%@ Register Src="Components/Profile/ViewPhotos.ascx" TagName="ViewPhotos" TagPrefix="uc5" %>
<%@ Register Src="Components/Profile/ViewProfile.ascx" TagName="ViewProfile" TagPrefix="uc4" %>
<%@ Register Src="Components/Profile/EditPhotos.ascx" TagName="EditPhotos" TagPrefix="uc3" %>
<%@ Register Src="Components/Profile/EditProfile.ascx" TagName="EditProfile" TagPrefix="uc2" %>
<%@ Register Src="Components/Profile/EditSkin.ascx" TagName="EditSkin" TagPrefix="uc13" %>
<%@ Register Src="~/Components/Profile/CreditHistory.ascx" TagName="CreditHistory" TagPrefix="uc16" %>
<%@ Register Src="~/Components/Profile/PrivacySettings.ascx" TagName="PrivacySettings" TagPrefix="uc14" %>
<%@ Register Src="~/Components/Profile/ViewEvents.ascx" TagName="ViewEvents" TagPrefix="uc15" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ MasterType TypeName="AspNetDating.Site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <aside>
        <uc1:smallboxstart id="SmallBoxStart1" runat="server" />
            <ul class="nav">                        
                <li><asp:linkbutton id="lnkEditProfile" Runat="server" onclick="lnkEditProfile_Click" /></li>
                <li><asp:linkbutton id="lnkViewProfile" Runat="server" onclick="lnkViewProfile_Click" /></li>
                <li><asp:linkbutton id="lnkUploadPhotos" Runat="server" onclick="lnkUploadPhotos_Click" /></li>
                <li><asp:linkbutton id="lnkViewPhotos" Runat="server" onclick="lnkViewPhotos_Click" /></li>
                <li id="pnlSalutePhoto" runat="server" visible="false"><asp:linkbutton id="lnkUploadSalutePhoto" Runat="server" onclick="lnkUploadSalutePhoto_Click" /></li>
                <li id="pnlAudioUpload" runat="server"><asp:linkbutton id="lnkUploadAudio" Runat="server"  onclick="lnkUploadAudio_Click" /></li>
                <li id="pnlUploadVideo" runat="server"><asp:linkbutton id="lnkUploadVideo" Runat="server" onclick="lnkUploadVideo_Click" /></li>
                <li><asp:linkbutton id="lnkViewEvents" Runat="server" onclick="lnkViewEvents_Click" /></li>
                <li id="pnlEditSkin" runat="server"><asp:linkbutton id="lnkEditSkin" Runat="server" onclick="lnkEditSkin_Click" /></li>
                <li><asp:linkbutton id="lnkCreditHistory" Runat="server" onclick="lnkCreditHistory_Click" /></li>
                <li><asp:linkbutton id="lnkPrivacySettings" Runat="server" onclick="lnkPrivacySettings_Click" /></li>
                <li><asp:linkbutton id="lnkSettings" Runat="server" onclick="lnkSettings_Click" /></li>
                <li id="trSubscription" runat="server"><asp:linkbutton id="lnkSubscription" Runat="server" onclick="lnkSubscription_Click" /></li>
		    </ul>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server" />
    </aside>
	<article>
        <uc2:EditProfile ID="EditProfile1" runat="server" />
        <uc3:EditPhotos ID="EditPhotos1" runat="server" />
        <uc4:ViewProfile ID="ViewProfile1" runat="server" />
        <uc5:ViewPhotos ID="ViewPhotos1" runat="server" />
        <uc8:Settings ID="Settings1" runat="server" />
        <uc12:UploadAudio ID="UploadAudio1" runat="server" />
        <uc10:UploadVideo ID="UploadVideo1" runat="server" />
        <uc6:Billing ID="Billing1" runat="server" />
        <uc13:EditSkin ID="EditSkin1" runat="server" />
        <uc16:CreditHistory ID="CreditHistory1" runat="server" />
        <uc14:PrivacySettings ID="PrivacySettings1" runat="server" />
        <uc15:ViewEvents ID="ViewEvents1" runat="server" />
	</article>
</asp:Content>
