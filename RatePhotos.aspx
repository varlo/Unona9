<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RatePhotos.aspx.cs" Inherits="AspNetDating.RatePhotos" %>

<%@ Import Namespace="AspNetDating"%>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ MasterType TypeName="AspNetDating.Site" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<asp:UpdatePanel ID="UpdatePanelRatePhotos" runat="server">
	        <ContentTemplate>
            <aside>
	            <uc1:smallboxstart id="SmallBoxStart1" runat="server" />
	                <components:ContentView ID="cvRatePhotos" Key="RatePhotos" runat="server">
			            <p class="text-center help-block">Rate this photo to see more! Use the gender and age filter to narrow down the results</p>
        		    </components:ContentView>
	                <div id="pnlRatedPhoto" runat="server">
	                    <p class="text-center"><%= ImageHandler.RenderImageTag(PrevPhotoId, 100, 100, "img-thumbnail", true, true) %></p>
	                </div>
	                <asp:Panel ID="pnlRating" runat="server" Visible="false">
	                    <ul class="info-header info-header-sm">
	                    <li><%= String.Format("Rating <b>{0}</b> based on {1} votes".Translate(), Rating.ToString("0.00"), Votes)%></li>
	                    <li class="pull-right"><%= "You rated".Translate() + " <b>" + CurrentVote + "</b>" %></li>
	                    </ul>
	                </asp:Panel>
	            <uc1:smallboxend id="SmallBoxEnd1" runat="server" />
	        </aside>
	        <article>
                <uc1:largeboxstart id="LargeBoxStart1" runat="server" />
                <div class="filter-group">
                    <div class="col" ID="pnlGender" runat="server">
                        <div class="input-group input-group-sm filter">
                            <span class="input-group-addon"><%= Lang.Trans("Gender") %></span>
                            <asp:DropDownList ID="ddGender" CssClass="form-control" runat="server" AutoPostBack="true" onselectedindexchanged="ddGender_SelectedIndexChanged" />
                        </div>
                    </div>
                    <div class="col" id="pnlAge" runat="server">
                        <div class="input-group input-group-sm filter">
                            <span class="input-group-addon"><b><%= Lang.Trans("Age") %></b></span>
                            <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" Size="2" AutoPostBack="true" MaxLength="2" ontextchanged="txtFrom_TextChanged" />
                            <span class="input-group-addon"><%= Lang.Trans("to") %></span>
                            <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" Size="2" AutoPostBack="true" MaxLength="2" ontextchanged="txtTo_TextChanged" />
                        </div>
                    </div>
				</div>
                <div class="text-center">
                    <asp:Panel ID="pnlVotes" runat="server">
                        <div class="info-header">
                            <%= Lang.Trans("Rate with:") %>
                            <span class="rates">
                                <asp:Repeater ID="rptVotes" runat="server" onitemcommand="rptRating_ItemCommand">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkVote" runat="server" CommandArgument='<%# Eval("Vote") %>'>
                                            <span class="fa-stack fa-lg fa-badge">
                                                <i class="fa fa-certificate fa-stack-2x"></i>
                                                <i class="fa fa-stack-1x fa-inverse"><%# Eval("Vote") %></i>
                                            </span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </span>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlCurrentPhoto" runat="server">
                        <div class="thumbnail">
                            <asp:Literal ID="ltrPhoto" runat="server" />
                            <div class="caption">
                                <div class="text-muted">
                                    <b><asp:Label ID="lblName" runat="server" /></b>
                                    <div><asp:Label ID="lblDescription" runat="server" /></div>
                                </div>
                                <a id="lnkUser" runat="server" target="_blank"></a>
                                <span class="text-muted"><asp:Label ID="lblAge" runat="server" /></span>
                                <div class="text-muted">
                                    <i class="fa fa-map-marker"></i>&nbsp;<asp:Label ID="lblLocation" runat="server" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False" />
                <uc1:largeboxend id="LargeBoxEnd1" runat="server" />
	        </article>
	    </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
