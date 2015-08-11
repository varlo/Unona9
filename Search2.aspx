<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Search2.aspx.cs" Inherits="AspNetDating.Search2" %>

<%@ MasterType TypeName="AspNetDating.Site" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Components/Search/SearchResults.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            $(".nav > div > .expandee:hidden").each(function() {
                $(this).prev().addClass("collapsed");
            });
            $(".expander").click(function() {
                $(this).next(".expandee").toggle();
                SetHeight();
            });
            $(".nav > .expander, .nav > div > .expander").click(function() {
                $(this).toggleClass("collapsed");
            });
            $(".dropgender").change(function() {
                switch ($(this).val()) {
                    case "1":
                        $(".visibleformale").show();
                        $(".invisibleformale").hide();
                        break;
                    case "2":
                        $(".visibleforfemale").show();
                        $(".invisibleforfemale").hide();
                        break;
                    case "3":
                        $(".visibleforcouple").show();
                        $(".invisibleforcouple").hide();
                        break;
                }
                SetHeight();
            });
        });
    </script>

    <aside>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server" />
            <div class="nav panel-group" id="SearchOptions">
                <div class="panel">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" href="#BasicOptions"><%= "Basic Options".Translate() %></a>
                        </h4>
                    </div>
                    <div id="BasicOptions" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <div id="divGender" runat="server">
                                <a href="#" onclick="return false;" class="expander"><%= Lang.Trans("I am looking for") %></a>
                                <div class="expandee">
                                    <div class="form-group">
                                        <asp:DropDownList CssClass="form-control dropgender" ID="dropGender" runat="server"/>
                                    </div>
                                    <div id="divInterestedIn" class="form-group" runat="server">
                                        <label><%= Lang.Trans("who is seeking") %></label>
                                        <asp:DropDownList CssClass="form-control" ID="dropInterestedIn" runat="server"/>
                                    </div>
                                </div>
                            </div>
                            <div id="divAge" runat="server">
                                <a href="#" onclick="return false;" class="expander"><%= "Age range".Translate() %></a>
                                <div class="expandee">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2" />
                                        <%= Lang.Trans("to") %>
                                        <asp:TextBox ID="txtAgeTo" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2" />
                                        <%= Lang.Trans("years old") %>
                                    </div>
                                </div>
                            </div>
                            <div id="divLocation" runat="server">
                                <a href="#" onclick="return false;" class="expander"><%= "Location".Translate() %></a>
                                <div id="divLocationExpandee" class="expandee form-sm" style="display: none" runat="server">
                                    <div class="form-group">
                                        <label><%= Lang.Trans("Country") %></label>
                                        <select ID="dropCountry" EnableViewState="false" runat="server" class="form-control"></select>
                                    </div>
                                    <div class="form-group">
                                        <label><%= Lang.Trans("Region/State") %></label>
                                        <select ID="dropRegion" runat="server" class="form-control"></select>
                                    </div>
                                    <div class="form-group">
                                        <label><%= Lang.Trans("City") %></label>
                                        <select ID="dropCity" runat="server" class="form-control"></select>
                                    </div>
                                    <div class="form-group" runat="server" visible="false">
                                        <label><%= Lang.Trans("Zip/Postal Code") %></label>
                                        <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label><%= Lang.Trans("Distance") %></label>
                                        <asp:DropDownList ID="dropDistance" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                            <a href="#" onclick="return false;" class="expander"><%= "Photos".Translate() %></a>
                            <div class="expandee">
                                <div class="checkbox"><label><asp:CheckBox ID="cbPhotoReq" runat="server" Checked="True"/></label></div>
                            </div>
                            <a href="#" onclick="return false;" class="expander"><%= "Username".Translate() %></a>
                            <div class="expandee">
                                <div class="form-group">
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                                    <div class="checkbox"><label><asp:CheckBox ID="cbOnlineOnly" runat="server" /></label></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:PlaceHolder ID="plhCustomSearch" runat="server" />
                <div class="panel">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a class="collapsed" id="lnkSavedSearches" visible="false" data-toggle="collapse" href="#SavedSearches" runat="server"><%= "Saved searches".Translate() %></a>
                        </h4>
                    </div>
                    <div id="SavedSearches" class="panel-collapse collapse">
                        <div class="panel-body">
                            <asp:DataList RepeatLayout="Flow" CssClass="vertical-repeater nav" ID="dlSavedSearches" OnItemCommand="dlSavedSearches_ItemCommand" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSavedSearch" runat="server" CommandName="SavedSearchView" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-eye"></i>&nbsp;<%# Eval("Name") %></asp:LinkButton>
                                    <asp:LinkButton CssClass="pull-right" ID="lnkDeleteSavedSearch" runat="server" CommandName="SavedSearchDelete" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-trash-o"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>
                    </div>
                </div>
                <div class="panel">
                    <div class="panel-body">
                        <div class="checkbox"><label><asp:CheckBox ID="cbSaveSearch" onclick="$('#divSaveSearch').toggle(); SetHeight();" runat="server" /></label></div>
                        <div id="divSaveSearch" style="display: none">
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" ID="txtSavedSearchName" runat="server" />
                            </div>
                            <div class="form-group">
                                <div class="checkbox"><label><asp:CheckBox ID="cbEmailSavedSearch" onclick="$('#divEmailFrequency').toggle(); SetHeight();" runat="server" /></label></div>
                                <div id="divEmailFrequency" style="display: none">
                                    <asp:DropDownList CssClass="form-control" ID="ddEmailFrequency" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="actions">
                    <asp:Button CssClass="btn btn-default" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                </div>
            </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server" />
    </aside>
    <article>
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server" />
        <uc1:SearchResults ID="SearchResults" runat="server" EnableGridSupport="true" PaginatorEnabled="True" GridMode="true">
        </uc1:SearchResults>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server" />
    </article>
</asp:Content>
