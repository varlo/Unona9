<%@ Page Language="c#" MasterPageFile="~/Site.Master" CodeBehind="BroadcastVideo.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.BroadcastVideo" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">
        function WebcamDetected(webcamdetected) {
            var divNoWebCam = $get("nowebcam");
            var divWebCamPresent = $get("webcampresent");
            if (webcamdetected) {
                if (divWebCamPresent != null) {
                    divWebCamPresent.style.display = '';
                    divNoWebCam.style.display = 'none';
                }
            }
            else {
                if (divWebCamPresent != null) {
                    divWebCamPresent.style.display = 'none';
                    divNoWebCam.style.display = '';
                } 
            }
        }
    </script>
    <article class="no-sidebar">
        <uc1:WideBoxStart ID="WideBoxStart1" runat="server"/>
            <object id="webcamdetect" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="1" height="1">
                <param name="movie" value="FlashResource.ashx?resname=DetectWebcam">
                <param name="quality" value="high">
                <embed name="webcamdetect" src="FlashResource.ashx?resname=DetectWebcam" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="1" height="1"></embed>
            </object>
            <div id="webcampresent" class="text-center" style="display:none">
                <components:ContentView ID="cvBroadcastVideoNotes" Key="BroadcastVideoNotes" runat="server">
                        <p>Click the button below to start the live video broadcast on your profile.
                        Make sure to read the terms of use before broadcasting.</p>
                </components:ContentView>
                <div class="actions"><asp:Button CssClass="btn btn-primary btn-lg" ID="btnStartBroadcast" runat="server" /></div>
            </div>
            <div id="nowebcam" class="text-center">
                <%= Lang.Trans("This feature requires web camera and the site cannot detect one. Please connect your web camera and try again!") %>
            </div>
        <uc1:WideBoxEnd ID="WideBoxEnd1" runat="server"/>
	</article>
</asp:Content>