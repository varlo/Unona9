<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopUsers.ascx.cs" Inherits="AspNetDating.Components.TopUsersCtrl" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Search/SearchResults.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="SmallBoxEnd.ascx" %>
<div class="row">
    <div id="tdTopFemales" class="col-sm-6 hslice" runat="server">
        <h5>
            <span class="entry-title" style="display: none"><%= String.Format(Lang.Trans("Top {0} Females on {1}"), Config.Ratings.TopUsersCount, Config.Misc.SiteTitle) %></span>
            <span class="ttl" style="display: none">120</span>
            <%= String.Format(Lang.Trans("Top {0} Females"), Config.Ratings.TopUsersCount) %>
        </h5>
        <uc1:SearchResults ID="SearchResults1" PaginatorEnabled="false" ShowSlogan="false" ShowRating="true" ShowCity="false" ShowLastOnline="false" UseCache="true" runat="server" />
    </div>
    <div id="tdTopMales" class="col-sm-6 hslice" runat="server">
        <h5>
            <span class="entry-title" style="display: none"><%= String.Format(Lang.Trans("Top {0} Males on {1}"), Config.Ratings.TopUsersCount, Config.Misc.SiteTitle) %></span>
            <span class="ttl" style="display: none">120</span>
            <%= String.Format(Lang.Trans("Top {0} Males"), Config.Ratings.TopUsersCount) %>
        </h5>
        <div class="entry-content">
            <uc1:SearchResults ID="SearchResults2" PaginatorEnabled="false" ShowSlogan="false" ShowRating="true" ShowCity="false" ShowLastOnline="false" UseCache="true" runat="server" />
        </div>
    </div>
    <div id="tdTopCouples" class="col-sm-4 hslice" runat="server">
        <h5>
            <span class="entry-title" style="display: none"><%= String.Format(Lang.Trans("Top {0} Couples on {1}"), Config.Ratings.TopUsersCount, Config.Misc.SiteTitle) %></span>
            <span class="ttl" style="display: none">120</span>
            <%= String.Format(Lang.Trans("Top {0} Couples"), Config.Ratings.TopUsersCount) %>
        </h5>
        <div class="entry-content">
            <uc1:SearchResults ID="SearchResults3" PaginatorEnabled="false" ShowSlogan="false" ShowRating="true" ShowCity="false" ShowLastOnline="false" UseCache="true" runat="server" />
        </div>
    </div>
</div>
<div class="text-muted text-center"><small><asp:Label ID="lblNote" runat="server" /></small></div>