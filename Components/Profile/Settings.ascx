<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="Settings.ascx.cs" Inherits="AspNetDating.Components.Profile.SettingsCtrl"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register Src="../DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>

<uc1:LargeBoxStart ID="LargeBoxStart" runat="server"/>
    <h4><uc1:HeaderLine ID="hlPersonalSettings" runat="server"/></h4>
    <input id="hidUsername" type="hidden" name="hidUsername" runat="server" />
    <asp:Label CssClass="alert text-danger" ID="lblError" runat="server"/>

    <div class="form-horizontal">
        <asp:PlaceHolder ID="pnlLocation" runat="server">
            <div class="form-group">
                <label class="control-label col-sm-4">
                    <%= Lang.Trans("Country") %>
                </label>
                <div class="col-sm-8">
                    <select ID="dropCountry" enableviewstate="false" class="form-control" runat="server"></select>
                </div>
            </div>
            <div class="form-group" id="trState" runat="server">
                <label class="control-label col-sm-4">
                    <%= Lang.Trans("Region/State") %>
                </label>
                <div class="col-sm-8">
                    <select ID="dropRegion" class="form-control" runat="server"></select>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4">
                    <%= Lang.Trans("City") %>
                </label>
                <div class="col-sm-8">
                    <select ID="dropCity" class="form-control" runat="server"></select>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4">
                    <%= Lang.Trans("Zip/Postal Code") %>
                </label>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtZipCode" CssClass="form-control" runat="server"/>
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Name") %>
            </label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtName" CssClass="form-control" runat="server"/>
            </div>
        </div>
        <div class="form-group" id="trInterestedIn" runat="server">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Interested in") %>
            </label>
            <div class="col-sm-8">
                <asp:DropDownList ID="dropInterestedIn" CssClass="form-control" runat="server"/>
            </div>
        </div>
        <div class="form-group" id="pnlBirthdate" runat="server">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Birthdate") %>
            </label>
            <div class="col-sm-8">
                <uc2:DatePicker ID="datePicker1" CssClass="datepicker" runat="server"/>
            </div>
        </div>
        <div class="form-group" id="trBirthdate2" runat="server">
            <label class="control-label col-sm-4">
                <asp:Label ID="lblBirthDate2" runat="server"/>
            </label>
            <div class="col-sm-8">
                <uc2:DatePicker ID="datePicker2" CssClass="datepicker" runat="server"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("E-Mail") %>
            </label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Current password") %>
            </label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtCurrentPassword" CssClass="form-control" runat="server" TextMode="Password"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("New password") %>
            </label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtNewPassword" CssClass="form-control" runat="server" TextMode="Password"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Confirm new password") %>
            </label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtConfirmNewPassword" CssClass="form-control" runat="server" TextMode="Password"/>
            </div>
        </div>
    </div>

    <h4><uc1:HeaderLine ID="hlAdditionalSettings" runat="server"/></h4>
    <div class="form-horizontal">
        <div class="form-group">
            <div class="col-sm-8 col-sm-offset-4">
                <div class="checkbox"><label><asp:CheckBox ID="cbReceiveEmails" runat="server"/></label></div>
                <div class="checkbox"><label><asp:CheckBox ID="cbProfileVisible" runat="server"/></label></div>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4"><%= Lang.Trans("Receive messages from") %></label>
            <div class="col-sm-8"><asp:DropDownList ID="ddMessagesFrom" Cssclass="form-control" runat="server"/></div>
        </div>
        <div class="form-group" id="pnlAge" runat="server">
            <label class="control-label col-sm-4"><%= Lang.Trans("Members should be from") %></label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2"/>
                <%= Lang.Trans("to") %>
                <asp:TextBox ID="txtAgeTo" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2"/>
                <%= Lang.Trans("years old") %>
            </div>
        </div>
        <div class="form-group" id="pnlProfileSkin" runat="server" visible="false">
            <asp:UpdatePanel ID="updatePanelProfileSkin" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <label class="control-label col-sm-4"><%= "Profile skin".Translate() %></label>
                    <div class="col-sm-5">
                        <asp:DropDownList CssClass="form-control" ID="dropProfileSkin" AutoPostBack="true" OnSelectedIndexChanged="dropProfileSkin_OnSelectedIndexChanged" runat="server"/>
                    </div>
                    <div class="col-sm-3 text-right">
                        <a class="btn btn-default" id="lnkPreviewSkin" target="_blank" visible="false" runat="server"><i class="fa fa-eye"></i>&nbsp;<%= "Preview".Translate() %></a>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="form-group" id="pnlRelationshipStatus" runat="server" visible="false">
            <asp:UpdatePanel ID="UpdatePanelRelationshipStatus" runat="server">
                <ContentTemplate>
                    <label class="control-label col-sm-4"><%= "Relationship status".Translate() %></label>
                    <div class="col-sm-4"><asp:DropDownList ID="ddRelationshipStatus" Cssclass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddRelationshipStatus_SelectedIndexChanged"/></div>
                    <div class="col-sm-4"><asp:DropDownList ID="ddInRelationshipWith" Cssclass="form-control" runat="server" Visible="false"/></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="form-group">
            <div class="col-sm-8 col-sm-offset-4">
                <div class="checkbox">
                    <label><asp:CheckBox ID="cbPhotoRequired" runat="server" /></label>
                </div>
                <div class="checkbox" id="pnlDisableProfileRating" runat="server">
                    <label><asp:CheckBox ID="cbDisableProfileRating" runat="server" /></label>
                </div>
                <div class="checkbox" id="pnlDisableVoting" runat="server">
                    <label><asp:CheckBox ID="cbDisableVoting" runat="server" /></label>
                </div>
                <div class="checkbox" id="pnlDisableProfileComments" runat="server">
                    <label><asp:CheckBox ID="cbDisableProfileComments" runat="server" /></label>
                </div>
                <div class="checkbox" id="pnlDisablePhotoComments" runat="server">
                    <label><asp:CheckBox ID="cbDisablePhotoComments" runat="server" /></label>
                </div>
                <div class="checkbox" id="pnlDisablePhotoRating" runat="server">
                    <label><asp:CheckBox ID="cbDisablePhotoRating" runat="server" /></label>
                </div>
                <div class="checkbox" id="pnlDisableBlogComments" runat="server">
                    <label><asp:CheckBox ID="cbDisableBlogComments" runat="server" /></label>
                </div>
            </div>
        </div>
    </div>
    <div class="actions">
        <asp:Button CssClass="btn btn-default" runat="server" ID="btnSaveChanges"/>
    </div>
<uc1:LargeBoxEnd ID="LargeBoxEnd" runat="server"/>
<div id="divTwitter" runat="server">
    <uc1:LargeBoxStart ID="LargeBoxStart3" runat="server"/>
    <div class="text center">
        <asp:label ID="lblTwitterStatus" runat="server" />
    </div>
    <div id="divEnterTwitterCredentials" runat="server">
        <!--
                    <label><%= Lang.Trans("Twitter username") %></label>
                    <asp:TextBox ID="txtTwitterUsername" CssClass="form-control" runat="server" />
                    <label><%= Lang.Trans("Twitter password") %></label>
                    <asp:TextBox ID="txtTwitterPassword" CssClass="form-control" TextMode="Password" runat="server" />
        <div class="actions">
            <asp:Button CssClass="btn btn-deafult" runat="server" ID="btnSaveTwitterSettings"></asp:Button>
        </div>
        -->
        <div class="actions">
            <asp:Button CssClass="btn btn-default" ID="btnLinkToYourTwitterAccount" runat="server" onclick="btnLinkToYourTwitterAccount_Click" />
        </div>
    </div>
    <div id="divTwitterStatus" runat="server" visible="false">
        <div class="actions">
        <!--<asp:Button runat="server" ID="btnRemoveTwitterCredentials"></asp:Button>-->
            <asp:Button CssClass="btn btn-default" runat="server" id="btnUnlinkFromYourTwitterAccount" onclick="btnUnlinkFromYourTwitterAccount_Click"/>
        </div>
    </div>
    <uc1:LargeBoxEnd runat="server" ID="Largeboxend3"/>
</div>


<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
    <p class="help-block">
        <%= Lang.Trans("We hope you've found the love of your life and this is the reason you've decided to delete your account.")%>
    </p>
    <asp:UpdatePanel ID="UpdatePanelConfirmPassword" runat="server">
        <ContentTemplate>
            <div id="pnlConfirmPasswordForDeletion" runat="server" visible="false">
                <asp:Label CssClass="alert text-danger" ID="lblErrorConfirmPasswordForDeletion" runat="server" EnableViewState="false"/>
                <label><%= "Enter your password".Translate() %></label>&nbsp;<asp:TextBox CssClass="form-control form-control-inline" ID="txtConfirmPasswordForDeletion" runat="server" TextMode="Password"/>
            </div>
            <div class="actions">
                <asp:Button CssClass="btn btn-default" ID="btnDelete" runat="server"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
<uc1:LargeBoxEnd runat="server" ID="Largeboxend1"/>
