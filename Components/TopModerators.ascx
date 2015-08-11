<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopModerators.ascx.cs" Inherits="AspNetDating.Components.TopModerators" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Search/SearchResults.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="SmallBoxEnd.ascx" %>
<div class="row">
    <div id="tdTopFemales" class="col-sm-6" runat="server">
        <h5><%= String.Format(Lang.Trans("Top {0} Females Moderators"), Config.CommunityModeratedSystem.TopModeratorsCount) %></h5>
        <uc1:SearchResults id="SearchResults1" PaginatorEnabled="false" ShowSlogan="false" ShowCity="false" ShowModerationScore="true" ShowLastOnline="false" UseCache="true" runat="server"/>
    </div>
    <div id="tdTopMales" class="col-sm-6" runat="server">
        <h5><%= String.Format(Lang.Trans("Top {0} Males Moderators"), Config.CommunityModeratedSystem.TopModeratorsCount) %></h5>
        <uc1:SearchResults id="SearchResults2" PaginatorEnabled="false" ShowSlogan="false" ShowCity="false" ShowModerationScore="true" ShowLastOnline="false" UseCache="true" runat="server"/>
    </div>
</div>