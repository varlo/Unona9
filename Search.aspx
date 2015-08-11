<%@ Page Language="c#" MasterPageFile="Site.Master" Codebehind="Search.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Search" %>

<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Components/Search/SearchResults.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <aside>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"/>
            <ul class="nav">
                <li><asp:LinkButton ID="lnkQuickSearch" runat="server" OnClick="lnkQuickSearch_Click" /></li>
                <li><asp:LinkButton ID="lnkAdvancedSearch" runat="server" OnClick="lnkAdvancedSearch_Click" /></li>
                <li id="trDistanceSearch" runat="server"><asp:LinkButton ID="lnkDistanceSearch" runat="server" OnClick="lnkDistanceSearch_Click" /></li>
                <li><asp:LinkButton ID="lnkOnlineSearch" runat="server" OnClick="lnkOnlineSearch_Click" /></li>
                <li><asp:LinkButton ID="lnkShowProfileViewers" runat="server" OnClick="lnkShowProfileViewers_Click" /></li>
            </ul>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"/>
        <uc1:SmallBoxStart ID="SmallBoxStart2" runat="server"/>
        	<div id="divSavedSearches" runat="server">
                <asp:DataList CssClass="vertical-repeater nav" RepeatLayout="Flow" ID="dlSavedSearches" runat="server">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSavedSearch" runat="server" CommandName="SavedSearchView" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-eye"></i>&nbsp;<%# Eval("Name") %></asp:LinkButton>
                        <asp:LinkButton CssClass="pull-right" ID="lnkDeleteSavedSearch" runat="server" CommandName="SavedSearchDelete" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-trash-o"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd2" runat="server"/>
    </aside>
    <article>
        <asp:Panel ID="pnlQuickSearch" DefaultButton="btnBasicSearchGo" runat="server">
            <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
                    <h4><uc1:HeaderLine ID="BasicSearchHeaderLine" runat="server"/></h4>
                    <p class="text-muted"><%= Lang.Trans("This feature allows you to search based on terms such as age, relationship, and the "+ "presence of a photo.") %></p>
                    <div class="well">
                        <div class="form-horizontal form-sm small-width center-block">
                            <div class="form-group" id="pnlGenderQuickSearch" runat="server">
                                <label class="control-label col-sm-5"><%= Lang.Trans("I am looking for") %></label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="dropGender" runat="server" Cssclass="form-control"/>
                                </div>
                            </div>
                            <div class="form-group" id="trInterestedIn" runat="server">
                                <label class="control-label col-sm-5"><%= Lang.Trans("who is seeking") %></label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="dropInterestedIn" runat="server" CssClass="form-control"/>
                                </div>
                            </div>
                            <div class="form-group" id="pnlCountry" runat="server">
                                <label class="control-label col-sm-5"><%= Lang.Trans("Country") %></label>
                                <div class="col-sm-7">
                                    <select ID="dropCountry" enableviewstate="false" runat="server" class="form-control"></select>
                                </div>
                            </div>
                            <div class="form-group" id="pnlState" runat="server">
                                <label class="control-label col-sm-5"><%= Lang.Trans("Region/State") %></label>
                                <div class="col-sm-7">
                                    <select ID="dropRegion" runat="server" class="form-control"></select>
                                </div>
                            </div>
                            <div class="form-group" id="pnlCity" runat="server">
                                <label class="control-label col-sm-5"><%= Lang.Trans("City") %></label>
                                <div class="col-sm-7">
                                    <select ID="dropCity" class="form-control" runat="server"></select>
                                </div>
                            </div>
                            <div class="form-group" id="pnlZip" runat="server">
                                <label class="control-label col-sm-5"><%= Lang.Trans("Zip/Postal Code") %></label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtZip" runat="server" CssClass="form-control"/>
                                </div>
                            </div>
                            <div class="form-group" id="pnlAgeRangeQuickSearch" runat="server">
                                <label class="control-label col-sm-5"><%= Lang.Trans("Age Range") %></label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2"/>
                                    <%= Lang.Trans("to") %>
                                    <asp:TextBox ID="txtAgeTo" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-5"><%= Lang.Trans("Photo Required") %></label>
                                <div class="col-sm-7">
                                    <div class="checkbox"><label><asp:CheckBox ID="cbPhotoReq" runat="server" Checked="True"/></label></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="actions">
                        <asp:Button CssClass="btn btn-default" ID="btnBasicSearchGo" runat="server" OnClick="btnBasicSearchGo_Click"/>
                    </div>
                    <asp:Panel ID="pnlUsernameSearch" DefaultButton="btnUsernameSearchGo" runat="server">
                        <h4><uc1:HeaderLine ID="UsernameSearchHeaderLine" runat="server"/></h4>
                        <div class="input-group">
                            <span class="input-group-addon"><%= Lang.Trans("Username") %></span>
                            <asp:TextBox ID="txtUsername" cssclass="form-control" runat="server"/>
                            <span class="input-group-btn">
                                <asp:Button CssClass="btn btn-default" ID="btnUsernameSearchGo" runat="server" OnClick="btnUsernameSearchGo_Click"/>
                            </span>
                        </div>
                        <small class="text-muted"><i class="fa fa-lightbulb-o"></i>&nbsp;<%= Lang.Trans("Search	for another member by their Username. For example: sillybilly or simone221.") %></small>
                    </asp:Panel>
                    <asp:Panel ID="pnlKeywordSearch" DefaultButton="btnKeywordSearchGo" runat="server">
                        <h4><uc1:HeaderLine ID="KeywordSearchHeaderLine" runat="server"/></h4>
                        <div class="input-group">
                            <span class="input-group-addon"><%= Lang.Trans("Keyword") %></span>
                            <asp:TextBox ID="txtKeyword" runat="server" cssclass="form-control"/>
                            <span class="input-group-btn">
                                <asp:Button CssClass="btn btn-default" ID="btnKeywordSearchGo" runat="server" OnClick="btnKeywordSearchGo_Click"/>
                            </span>
                        </div>
                        <small class="text-muted"><i class="fa fa-lightbulb-o"></i>&nbsp;<%= Lang.Trans("Use keywords to uncover matches with similar hobbies and interests.") %></small>
                    </asp:Panel>

        	<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
		</asp:Panel>
        <asp:Panel ID="pnlCustomSearch" runat="server" DefaultButton="btnCustomSearchGo" Visible="False">
            <uc1:LargeBoxStart ID="LargeBoxStart3" runat="server"/>
                <h4><uc1:HeaderLine ID="CustomSearchHeaderLine" runat="server"/></h4>
                <p class="text-muted"><%= Lang.Trans("This feature allows you to make more customizable search based on terms such as eye color, social status, bad habits, etc.") %></p>
                <div class="well">
                    <div class="form-horizontal form-sm small-width center-block">
                        <div class="form-group" id="pnlGenderCustomSearch" runat="server">
                            <label class="control-label col-sm-5"><%= Lang.Trans("I am looking for") %></label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="dropGender2" Cssclass="form-control" runat="server" AutoPostBack="True"/>
                            </div>
                        </div>
                        <div class="form-group" id="trInterestedIn2" runat="server">
                            <label class="control-label col-sm-5"><%= Lang.Trans("seeking") %></label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="dropInterestedIn2" Cssclass="form-control" runat="server"/>
                            </div>
                        </div>
                        <div class="form-group" id="pnlCountry2" runat="server">
                            <label class="control-label col-sm-5"><%= Lang.Trans("Country") %></label>
                            <div class="col-sm-7">
                                <select ID="dropCountry2" class="form-control" enableviewstate="false" runat="server"></select>
                            </div>
                        </div>
                        <div class="form-group" id="pnlState2" runat="server">
                            <label class="control-label col-sm-5"><%= Lang.Trans("Region/State") %></label>
                            <div class="col-sm-7">
                                <select ID="dropRegion2" class="form-control" runat="server"></select>
                            </div>
                        </div>
                        <div class="form-group" id="pnlCity2" runat="server">
                            <label class="control-label col-sm-5"><%= Lang.Trans("City") %></label>
                            <div class="col-sm-7">
                                <select ID="dropCity2" class="form-control" runat="server"></select>
                            </div>
                        </div>
                        <div class="form-group" id="pnlZip2" runat="server">
                            <label class="control-label col-sm-5"><%= Lang.Trans("Zip/Postal Code") %></label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtZip2" CssClass="form-control" runat="server"/>
                            </div>
                        </div>
                        <div class="form-group" id="pnlAgeCustomSearch" runat="server">
                            <label class="control-label col-sm-5"><%= Lang.Trans("Age Range") %></label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtAgeFrom2" CssClass="form-control form-control-inline" runat="server" Size="2" MaxLength="2"/>
                                <%= Lang.Trans("to") %>
                                <asp:TextBox ID="txtAgeTo2" CssClass="form-control form-control-inline" runat="server" Size="2" MaxLength="2"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-5"><%= Lang.Trans("Photo Required") %></label>
                            <div class="col-sm-7">
                                <div class="checkbox"><label><asp:CheckBox ID="cbPhotoReq2" runat="server" Checked="True"/></label></div>
                            </div>
                        </div>
                    </div>
                </div>
            	<div class="list-group list-group-striped">
                    <asp:PlaceHolder ID="plhMaleSearchTerms" runat="server"/>
                    <asp:PlaceHolder ID="plhFemaleSearchTerms" runat="server"/>
                    <asp:PlaceHolder ID="plhCoupleSearchTerms" runat="server"/>
                </div>
                <asp:Label ID="lblError" CssClass="alert text-danger" runat="server"/>
                <asp:UpdatePanel id="ajaxRegionEmailMe" runat="server">
                    <ContentTemplate>
                        <div id="pnlSavedSearchOptions" runat="server">
                            <h4><%= Lang.Trans("Save search") %></h4>
                            <p class="text-muted"><%= Lang.Trans("This feature allows you to save the prefered search options") %></p>
                            <div class="input-group">
			                    <span class="input-group-addon">
						            <div class="checkbox"><label>
                                    <asp:CheckBox ID="cbSaveSearch" runat="server" AutoPostBack="true" oncheckedchanged="cbSaveSearch_CheckedChanged"/>
                                    <asp:Label ID="lblSavedSearchText" runat="server"/>
                                    </label></div>
                                </span>
                                <asp:TextBox ID="txtSavedSearchName" CssClass="form-control" runat="server"/>
                            </div>
                            <div id="pnlEmailMe" runat="server" visible="false">
                                <br />
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <div class="checkbox"><label>
                                            <asp:CheckBox ID="cbEmailMe" Runat="server"/>
                                            <%= Lang.Trans("Email me the latest matches") %>
                                        </label></div>
                                    </span>
                                    <asp:DropDownList CssClass="form-control" ID="ddEmailFrequency" runat="server"/>
                                </div>
					        </div>
                    	</div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="actions">
	                <asp:Button CssClass="btn btn-default" ID="btnCustomSearchGo" runat="server" OnClick="btnCustomSearchGo_Click"/>
                </div>
            <uc1:LargeBoxEnd ID="Largeboxend3" runat="server"/>
        </asp:Panel>
        <asp:Panel ID="pnlDistanceSearch" Visible="False" DefaultButton="btnDistanceSearchGo" runat="server">
            <uc1:LargeBoxStart ID="LargeBoxStart4" runat="server"/>
            <h4><uc1:HeaderLine ID="DistanceSearchHeaderLine" runat="server"/></h4>
            <div class="text-muted"><%= Lang.Trans("This feature will help you to find users near you") %></div>
            <div class="well">
                <div class="form-horizontal form-sm small-width center-block">
                    <div class="form-group" id="pnlGenderDistanceSearch" runat="server">
                        <label class="control-label col-sm-7"><%= Lang.Trans("I am looking for") %></label>
                        <div class="col-sm-5">
                            <asp:DropDownList ID="dropGender3" runat="server" Cssclass="form-control"/>
                        </div>
                    </div>
                    <div class="form-group" id="pnlAgeDistanceSearch" runat="server">
                        <label class="control-label col-sm-7"><%= Lang.Trans("Age Range") %></label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtAgeFrom3" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2"/>
                            <%= Lang.Trans("to") %>
                            <asp:TextBox ID="txtAgeTo3" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-7"><asp:Label ID="lblDistanceFromUser" runat="server"/></label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtDistanceFromUser" runat="server" CssClass="form-control"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-7"><%= Lang.Trans("Photo Required") %></label>
                        <div class="col-sm-5">
                            <div class="checkbox"><label><asp:CheckBox ID="cbPhotoReq3" runat="server" Checked="True"/></label></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="actions">
                <asp:Button CssClass="btn btn-default" ID="btnDistanceSearchGo" runat="server" OnClick="btnDistanceSearchGo_Click"/>
            </div>
            <uc1:LargeBoxEnd ID="Largeboxend4" runat="server"/>
        </asp:Panel>
        <asp:Panel ID="pnlSearchResults" runat="server" Visible="False">
            <uc1:LargeBoxStart ID="LargeBoxStart2" runat="server"/>
            <div id="pnlFilterOnline" runat="server" class="UserFilter">
                <span id="pnlGenderFilterOnline" runat="server"><%= Lang.Trans("Gender") %>&nbsp;<asp:DropDownList ID="ddFilterByGender" cssclass="form-control" runat="server"/>&nbsp;</span>
                <span id="pnlAgeFilterOnline" runat="server">
                    <%= Lang.Trans("Age") %>
                    <asp:TextBox ID="txtFromFilterOnline" runat="server" CssClass="smalltextbox" MaxLength="2"/>
                    <%= Lang.Trans("to") %>
                    <asp:TextBox ID="txtToFilterOnline" runat="server" CssClass="smalltextbox" MaxLength="2"/>
                </span>
                <asp:Button ID="btnFilterOnline" runat="server" OnClick="btnFilterOnline_Click"/>
            </div>
            <uc1:SearchResults ID="SearchResults" runat="server" PaginatorEnabled="True" EnableGridSupport="True"/>
            <uc1:LargeBoxEnd ID="LargeBoxEnd2" runat="server"/>
        </asp:Panel>
        <asp:Label ID="lblError2" CssClass="alert text-danger" runat="server"/>
    </article>
</asp:Content>
