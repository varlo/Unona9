<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopPhotos.ascx.cs" Inherits="AspNetDating.Components.TopPhotos" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Search/SearchResults.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="SmallBoxEnd.ascx" %>
<div class="row">
    <div id="tdTopFemalePhotos" class="col-sm-6" runat="server">
        <h5>
            <%= String.Format(Lang.Trans("Top {0} Female Photos"), Config.Ratings.TopPhotosCount) %>
        </h5>
        <uc1:SearchResults ID="SearchResults1" PaginatorEnabled="false" ShowSlogan="false" ShowRating="true" ShowCity="false" ShowLastOnline="false" runat="server" />
    </div>
    <div id="tdTopMalePhotos" class="col-sm-6" runat="server">
        <div class="media">
            <h5>
                <%= String.Format(Lang.Trans("Top {0} Male Photos"), Config.Ratings.TopPhotosCount) %>
            </h5>
            <uc1:SearchResults ID="SearchResults2" PaginatorEnabled="false" ShowSlogan="false" ShowRating="true" ShowCity="false" ShowLastOnline="false" runat="server" />
        </div>
    </div>
    <div id="tdTopCouplePhotos" class="col-sm-4" runat="server">
        <h5>
            <%= String.Format(Lang.Trans("Top {0} Couple Photos"), Config.Ratings.TopPhotosCount) %>
        </h5>
        <uc1:SearchResults ID="SearchResults3" PaginatorEnabled="false" ShowSlogan="false" ShowRating="true" ShowCity="false" ShowLastOnline="false" runat="server" />
    </div>
</div>
<div class="text-muted text-center"><small><asp:Label ID="lblNote" runat="server" /></small></div>