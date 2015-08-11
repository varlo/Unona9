<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrivacySettings.ascx.cs" Inherits="AspNetDating.Components.Profile.PrivacySettings" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
    <h4><uc1:HeaderLine ID="hlVisibilitySettings" runat="server"/></h4>
    <asp:Label CssClass="alert text-danger" ID="lblError" runat="server"/>
	<div class="form-horizontal">
		<div class="form-group">
			<label class="control-label col-sm-4">
				<%= "My profile is visible for".Translate() %>
			</label>
			<div class="col-sm-7">
				<asp:DropDownList CssClass="form-control" ID="ddViewProfile" runat="server"/>
			</div>
		</div>
		<div class="form-group">
			<label class="control-label col-sm-4">
				<%= "My photos are visible for".Translate()%>
			</label>
			<div class="col-sm-7">
				<asp:DropDownList CssClass="form-control" ID="ddViewPhotos" runat="server"/>
			</div>
		</div>
		<div class="form-group">
			<label class="control-label col-sm-4">
				<%= "My videos are visible for".Translate()%>
			</label>
			<div class="col-sm-7">
				<asp:DropDownList CssClass="form-control" ID="ddViewVideos" runat="server"/>
			</div>
		</div>
		<div class="form-group">
			<label class="control-label col-sm-4">
				<%= "My friends are visible for".Translate()%>
			</label>
			<div class="col-sm-7">
				<asp:DropDownList CssClass="form-control" ID="ddViewFriends" runat="server"/>
			</div>
		</div>
		<div class="form-group">
			<label class="control-label col-sm-4">
				<%= "My blog is visible for".Translate()%>
			</label>
			<div class="col-sm-7">
				<asp:DropDownList CssClass="form-control" ID="ddViewBlog" runat="server"/>
			</div>
		</div>
        <div class="form-group">
            <div class="col-sm-7 col-sm-offset-4">
                <div class="checkbox" id="pnlDisableLevelIcon" runat="server"><label><asp:CheckBox ID="cbDisableLevelIcon" runat="server" /></label></div>
                <div class="checkbox"><label><asp:CheckBox ID="cbDisableProfileViews" runat="server" /></label></div>
                <div class="checkbox"><label><asp:CheckBox ID="cbHideFriends" runat="server" /></label></div>
                <div class="checkbox" id="pnlHideGroupMembership" runat="server"><label><asp:CheckBox ID="cbHideGroupMembership" runat="server" /></label></div>
            </div>
        </div>
	</div>
    <h4><uc1:HeaderLine ID="hlEventsSettings" runat="server"/></h4>
    <asp:CheckBoxList ID="cblEventsSettings" CssClass="checkboxlist" RepeatColumns="2" runat="server"/>
    <hr />
    <div class="actions">
        <asp:Button CssClass="btn btn-default" runat="server" ID="btnSaveChanges" onclick="btnSaveChanges_Click"/>
    </div>
<uc1:LargeBoxEnd runat="server" ID="Largeboxend1"/>