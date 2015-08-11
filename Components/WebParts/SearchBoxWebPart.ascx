<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchBoxWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.SearchBoxWebPart" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>

<div class="form-horizontal form-sm">
    <div class="form-group" id="pnlGender" runat="server">
        <label class="control-label col-sm-5"><%= Lang.Trans("Gender") %></label>
        <div class="col-sm-7">
            <asp:DropDownList ID="dropGender" CssClass="form-control" runat="server"/>
        </div>
    </div>
    <div class="form-group" id="trCountry" runat="server">
        <label class="control-label col-sm-5"><%= Lang.Trans("Country") %></label>
        <div class="col-sm-7">
            <asp:DropDownList ID="dropCountry" CssClass="form-control" AutoPostBack="true" runat="server" onselectedindexchanged="dropCountry_SelectedIndexChanged"/>
        </div>
    </div>
    <div class="form-group" id="trState" runat="server">
        <label class="control-label col-sm-5"><%= Lang.Trans("Region/State") %></label>
        <div class="col-sm-7">
            <asp:DropDownList ID="dropRegion" CssClass="form-control" AutoPostBack="true" runat="server" onselectedindexchanged="dropRegion_SelectedIndexChanged"/>
        </div>
    </div>
    <div class="form-group" id="trCity" runat="server">
        <label class="control-label col-sm-5"><%= Lang.Trans("City") %></label>
        <div class="col-sm-7">
            <asp:DropDownList ID="dropCity" CssClass="form-control" AutoPostBack="true" runat="server"/>
        </div>
    </div>
    <div class="form-group" id="pnlAge" runat="server">
        <label class="control-label col-sm-5"><%= Lang.Trans("Age") %></label>
        <div class="col-sm-7">
            <asp:TextBox ID="txtAgeFrom" CssClass="form-control form-control-inline" runat="server" Size="2" MaxLength="2"/>
            <%= Lang.Trans("to") %>
            <asp:TextBox ID="txtAgeTo" CssClass="form-control form-control-inline" runat="server" Size="2" MaxLength="2"/>
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-5"><%= Lang.Trans("Username") %></label>
        <div class="col-sm-7">
            <asp:TextBox ID="txtUsername" CssClass="form-control" runat="server"/>
        </div>
    </div>
    <asp:PlaceHolder ID="phProfileQuestions" runat="server"/>
</div>
<div class="actions">
    <uc1:FlexButton ID="fbBasicSearchGo" CssClass="btn btn-default" runat="server" OnClick="fbBasicSearchGo_Click" SkinID="InnerSearch"/>
</div>
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