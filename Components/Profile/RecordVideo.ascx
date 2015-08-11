<%@ Control Language="c#" AutoEventWireup="True" Codebehind="RecordVideo.ascx.cs" Inherits="AspNetDating.Components.Profile.RecordVideo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="VideoRecorder" Src="../../VR/VideoRecorder.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<input type="hidden" id="hidUsername" runat="server" NAME="hidUsername" />
<h4><uc1:HeaderLine id="HeaderLine1" runat="server" /></h4>
<p>
	<%= Lang.Trans("Use this section to record a short video that will be a part of your profile.") %>
</p>
<div class="text-center">
	<uc1:VideoRecorder id="VideoRecorder1" runat="server"/>
    <asp:UpdatePanel ID="UpdatePanelGridMessages" runat="server">
        <ContentTemplate>
            <asp:LinkButton CssClass="btn btn-default" ID="lnkSetPublic" runat="server" OnClick="lnkSetPublic_Click"/>&nbsp;&nbsp;
            <asp:LinkButton CssClass="btn btn-default" ID="lnkSetPrivate" runat="server" OnClick="lnkSetPrivate_Click"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="false"/>
</div>