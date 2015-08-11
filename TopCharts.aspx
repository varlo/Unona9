﻿<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="TopCharts.aspx.cs" Inherits="AspNetDating.TopCharts"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Components/Search/SearchResults.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register Src="Components/TopUsers.ascx" TagName="TopUsers" TagPrefix="uc1" %>
<%@ Register Src="Components/TopPhotos.ascx" TagName="TopPhotos" TagPrefix="uc1" %>
<%@ Register Src="Components/TopModerators.ascx" TagName="TopModerators" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<article class="no-sidebar">
    <div class="panel">
        <div class="panel-body">
            <div class="text-center" id="pnlButtons" runat="server">
                <div class="btn-group">
                    <asp:Button CssClass="btn btn-default" ID="btnShowTopUsers" runat="server" onclick="btnShowTopUsers_Click" />
                    <asp:Button CssClass="btn btn-default" ID="btnShowTopPhotos" runat="server" onclick="btnShowTopPhotos_Click" />
                    <asp:Button CssClass="btn btn-default" ID="btnShowTopModerators" runat="server" onclick="btnShowModerators_Click" />
                </div>
            </div>
            <hr />
            <asp:MultiView ID="mvTopCharts" runat="server">
                <asp:View ID="viewTopUsers" runat="server">
                    <uc1:TopUsers id="TopUsersCtrl" runat="server" />
                </asp:View>
                <asp:View ID="viewTopPhotos" runat="server">
                    <uc1:TopPhotos id="TopPhotosCtrl" runat="server" />
                </asp:View>
                <asp:View ID="viewTopModerators" runat="server">
                    <uc1:TopModerators id="TopModeratorsCtrl" runat="server" />
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</article>
</asp:Content>
