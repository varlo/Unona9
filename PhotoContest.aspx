<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" Codebehind="PhotoContest.aspx.cs"
    Inherits="AspNetDating.PhotoContestPage" %>

<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register src="Components/HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanelLastRanked" runat="server">
            <ContentTemplate>
                <p class="text-muted" id="divNoRankedMessage" runat="server">
                    <%= Lang.Trans("Begin voting to compare your choices with the community!") %>
                </p>
                <div id="divLastRankedMessage" visible="false" runat="server">
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="thumbnail">
                                <asp:Image ID="imgLastLeft" runat="server" />
                                <div class="caption text-center small"><a id="linkLastLeft" runat="server"></a></div>
                            </div>
                        </div>
                        <div class="col-sm-2 col-vs text-muted"><%= Lang.Trans("VS") %></div>
                        <div class="col-sm-5">
                            <div class="thumbnail">
                                <asp:Image ID="imgLastRight" runat="server" />
                                <div class="caption text-center small"><a id="linkLastRight" runat="server"></a></div>
                            </div>
                        </div>
                    </div>
                    <div class="text-center text-muted small"><asp:Label ID="lblVotersAgree" runat="server" /></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanelFavouriteEntries" runat="server">
            <ContentTemplate>
                <div id="divPersonalFavs" visible="false" runat="server">
                	<h4><%= Lang.Trans("Your favourite entries") %></h4>
                    <asp:DataList ID="dlFavouriteEntries" CssClass="table table-striped" runat="server" RepeatDirection="Vertical" RepeatColumns="1" ShowFooter="False" ShowHeader="False">
                        <ItemStyle VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <span class="fa-stack text-muted">
                              <i class="fa fa-star fa-stack-2x"></i>
                              <i class="fa fa-stack-1x fa-inverse"><%# Eval("Rank") %></i>
                            </span>
                            <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 50, 50, "img-thumbnail", false, true, false) %>
                            &nbsp;<a href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username")) %>' target="_blank"><%# Eval("Username") %></a>
                        </ItemTemplate>
                    </asp:DataList>
                    <div class="actions">
                        <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkViewAllFavourites" runat="server" OnClick="lnkViewAllFavourites_Click" />
                        <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkViewTopEntries" Visible="False" runat="server" OnClick="lnkViewTopEntries_Click" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkViewAllFavourites" />
                <asp:PostBackTrigger ControlID="lnkViewTopEntries" />
            </Triggers>
        </asp:UpdatePanel>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server" />
    </aside>
    <article>
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server" />
        <h4><uc2:HeaderLine ID="hlContestName" runat="server" /></h4>
        <p><asp:Label ID="lblContestDescription" runat="server" /></p>
        <div id="divPhotos" runat="server">
            <asp:UpdatePanel ID="UpdatePanelPhotos" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="thumbnail">
                                <div class="caption text-center"><a id="linkLeftUsername" runat="server"></a></div>
                                <a id="lnkLeftPhoto" rel="lightbox" target="_blank" runat="server"><asp:Image ID="imgLeftPhoto" runat="server" /></a>
                            </div>
                            <asp:Button CssClass="btn btn-default btn-block" ID="btnPickLeft" runat="server" OnClick="btnPickLeft_Click" />
                        </div>
                        <div class="col-sm-2 col-vs"><%= Lang.Trans("VS") %></div>
                        <div class="col-sm-5">
                            <div class="thumbnail">
                                <div class="caption text-center"><a id="linkRightUsername" runat="server"></a></div>
                                <a id="lnkRightPhoto" rel="lightbox" target="_blank" runat="server"><asp:Image ID="imgRightPhoto" runat="server" /></a>
                            </div>
                            <asp:Button CssClass="btn btn-default btn-block" ID="btnPickRight" runat="server" OnClick="btnPickRight_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <p id="divNotEnoughPhotos" visible="false" runat="server"><%= Lang.Trans("There are not enough photos to start the contest!") %></p>
        <p id="divContestHasEnded" visible="false" runat="server"><%= Lang.Trans("This contest has ended!") %></p>
        <div id="divAllFavourites" align="center" visible="false" runat="server">
            <h4><%= String.Format(Lang.Trans("Your personal top {0} list!"), Config.Ratings.FavoriteEntriesCount) %></h4>
            <asp:DataList ID="dlAllFavourites" runat="server"  CssClass="repeater-horizontal" RepeatLayout="Flow" ShowFooter="False" ShowHeader="False">
                <ItemTemplate>
                    <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 100, 100, null, false, true, false) %>
                    <%# Eval("Rank") %>.&nbsp; <a href="<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username")) %>" target="_blank">
                    <%# Eval("Username") %></a>
                </ItemTemplate>
            </asp:DataList>
            <asp:Button CssClass="btn btn-default" ID="btnBackToPhotos" runat="server" OnClick="btnBackToPhotos_Click" />
        </div>
        <div id="divViewTopEntries" align="center" visible="false" runat="server">
            <h4><%= String.Format(Lang.Trans("Top {0} entries!"), Config.Ratings.TopEntriesCount) %></h4>
            <asp:DataList ID="dlTopEntries" runat="server" CssClass="repeater-horizontal" RepeatLayout="Flow" ShowFooter="False" ShowHeader="False">
                <ItemTemplate>
                    <div class="thumbnail">
                        <div class="watermark over-top">
                            <span class="fa-stack">
                              <i class="fa fa-trophy fa-stack-2x"></i>
                              <i class="fa fa-stack-1x fa-inverse"><%# Eval("Rank") %></i>
                            </span>
                        </div>
                        <a href='<%# String.Format("Image.ashx?id={0}", Eval("PhotoId")) %>' target="_blank"><%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 120, 120, null, false, true, false) %></a>
                        <div class="caption text-center small">
                            <a href="<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username"))%>" target="_blank"><%# Eval("Username") %></a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <asp:Button CssClass="btn btn-default" ID="btnBackToPhotos2" runat="server" OnClick="btnBackToPhotos_Click" />
        </div>
        <hr />
        <div class="clearfix">
            <asp:Button CssClass="btn btn-default btn-sm" ID="btnViewTopEntries" runat="server" OnClick="lnkViewTopEntries_Click" />
            <div id="divContestRank" class="pull-right" visible="false" runat="server">
                <asp:Label ID="lblCurrentRank" runat="server" />
                <asp:Button CssClass="btn btn-default btn-sm" ID="btnLeaveContest" runat="server" OnClick="btnLeaveContest_Click" />
            </div>
            <div class="pull-right">
                <asp:Button CssClass="btn btn-primary btn-sm" ID="btnEnterContest" runat="server" OnClick="btnEnterContest_Click" />
            </div>
        </div>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server" />
    </article>
</asp:Content>