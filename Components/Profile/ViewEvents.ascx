<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEvents.ascx.cs" Inherits="AspNetDating.Components.Profile.ViewEvents" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
    <asp:UpdatePanel ID="UpdatePanelEvents" runat="server">
        <ContentTemplate>
            <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
            <div class="list-group list-group-striped"><asp:PlaceHolder ID="plhEvents" runat="server" /></div>
        </ContentTemplate>
    </asp:UpdatePanel>
<uc1:LargeBoxEnd runat="server" ID="Largeboxend1"/>