<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="SearchBox.ascx.cs" Inherits="AspNetDating.Components.Search.SearchBox" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>
<div class="panel panel-search">
    <div class="panel-heading">
        <h3 class="panel-title"><%= Lang.Trans("Search") %></h3>
    </div>
    <div class="panel-body">
        <asp:Panel CssClass="search-wrap" ID="pnlDefaultButton" runat="server" DefaultButton="fbBasicSearchGo">
            <div class="form-horizontal form-sm">
                <div class="form-group" id="pnlGender" runat="server">
                    <label class="control-label col-sm-5"><%= Lang.Trans("Gender") %></label>
                    <div class="col-sm-7"><asp:DropDownList ID="dropGender" CssClass="form-control" runat="server" /></div>
                </div>
                <div class="form-group" id="trCountry" runat="server">
                    <label class="control-label col-sm-5"><%= Lang.Trans("Country") %></label>
                    <div class="col-sm-7"><select ID="dropCountry" EnableViewState="false" class="form-control" runat="server" /></div>
                </div>
                <div class="form-group" id="trState" runat="server">
                    <label class="control-label col-sm-5"><%= Lang.Trans("Region/State") %></label>
                    <div class="col-sm-7"><select id="dropRegion" class="form-control" runat="server" /></div>
                </div>
                <div class="form-group" id="trCity" runat="server">
                    <label class="control-label col-sm-5"><%= Lang.Trans("City") %></label>
                    <div class="col-sm-7"><select id="dropCity" class="form-control" runat="server" /></div>
                </div>
                <div class="form-group form-group-range" id="pnlAge" runat="server">
                    <label class="control-label col-sm-5"><%= Lang.Trans("Age") %></label>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtAgeFrom" CssClass="form-control" runat="server" Size="2" MaxLength="2" />
                        <p class="form-control-static"><%= Lang.Trans("to") %></p>
                        <asp:TextBox ID="txtAgeTo" CssClass="form-control" runat="server" Size="2" MaxLength="2" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-5"><%= Lang.Trans("Username") %></label>
                    <div class="col-sm-7"><asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" /></div>
                </div>
                <asp:PlaceHolder ID="phProfileQuestions" runat="server" />
            </div>
            <div class="actions">
                <uc1:FlexButton ID="fbBasicSearchGo" CssClass="btn btn-secondary" runat="server" OnClick="fbBasicSearchGo_Click" RenderAs="Button" SkinID="Search" />
            </div>

        </asp:Panel>
    </div><!--- /.panel-body -->

    <script type="text/javascript">
        function showhide(dropdown) {
            switch (dropdown.val()) {
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
                default:
                    $(".visibleformale").show();
                    $(".visibleforfemale").show();
                    $(".visibleforcouple").show();
                    break;
            }
            SetHeight();
        }

        $(function () {
            showhide($(".dropgender"));
        });

        $(".dropgender").change(function () {
            showhide($(this));
        });
    </script>
</div>
