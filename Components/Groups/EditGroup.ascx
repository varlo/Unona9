<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditGroup.ascx.cs" Inherits="AspNetDating.Components.Groups.EditGroup" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="~/Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="~/Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="~/Components/HeaderLine.ascx" %>

<uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
	<asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
    <asp:MultiView ID="mvEditGroup" runat="server">
        <asp:View ID="viewMain" runat="server">
            <asp:Label ID="lblEditGroup" runat="server" Visible="false"/>
			<div class="actions">
            	<asp:Button CssClass="btn btn-default" ID="btnEditGroup" runat="server" Visible="false" OnClick="btnEditGroup_Click" />
            </div>
			<hr id="divSeparatorEditGroup" runat="server" />
			<div id="divLineEditGroup" runat="server"></div>
            <asp:Label ID="lblTransferOwnership" runat="server" Visible="false"/>
			<div class="actions">
            	<asp:Button CssClass="btn btn-default" ID="btnTransferOwnership" runat="server" Visible="false" OnClick="btnTransferOwnership_Click" />
			</div>
			<hr id="divSeparatorTransferOwnership" runat="server" />
			<div id="divLineTransferOwnership" runat="server"></div>
            <asp:Label ID="lblDeleteGroup" runat="server" Visible="false"/>
			<div class="actions">
            	<asp:Button CssClass="btn btn-default" ID="btnDeleteGroup" runat="server" OnClick="btnDeleteGroup_Click" Visible="false"/>
            </div>
        </asp:View>
        <asp:View ID="viewEditGroup" runat="server">
            <div class="form-group">
                <div class="media">
                    <img class="img-thumbnail media-object pull-left" src="GroupIcon.ashx?groupID=<%= CurrentGroup.ID.ToString() %>&width=100&height=100&diskCache=1" />
                    <div class="media-body">
                        <label><asp:Label ID="lblGroupImage" runat="server"/></label>
                        <asp:FileUpload ID="fuGroupImage" runat="server" />
                    </div>
                </div>
            </div>
            <hr />
            <div class="form-group">
                <div class="form-inline">
                    <label><asp:Label ID="lblAccessLevel" runat="server"/></label>&nbsp;
                    <asp:DropDownList ID="ddAccessLevel" AutoPostBack="true" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddAccessLevel_SelectedIndexChanged"/>
                </div>
            </div>
            <div class="form-group">
                <label><asp:Label ID="lblName" runat="server"/></label>
                <asp:TextBox CssClass="form-control" ID="txtName" runat="server"/>
            </div>
            <div class="form-group">
                <label><asp:Label ID="lblCategories" runat="server"/></label>
                <asp:ListBox CssClass="form-control" ID="lbCategories" runat="server" SelectionMode="Multiple"/>
                <small class="text-muted"><i class="fa fa-lightbulb-o"></i>&nbsp;<asp:Label ID="lblExplanation" runat="server"/></small>
            </div>
            <div class="form-group">
                <label><asp:Label ID="lblDescription" runat="server"/></label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Columns="50"/>
            </div>
            <div id="pnlAgeRestriction" class="form-group" runat="server">
                <div class="form-inline">
                    <label><%= Lang.Trans("Minimum age restriction") %></label>&nbsp;
                    <asp:DropDownList CssClass="form-control" ID="ddAgeRestriction" runat="server"/>
                </div>
            </div>
            <div id="pnlAutomaticallyJoin" class="form-group" runat="server" visible="false">
                <div class="checkbox"><label><%= Lang.Trans("Users automatically join group")%><asp:CheckBox ID="cbAutomaticallyJoin" runat="server" AutoPostBack="true" oncheckedchanged="cbAutomaticallyJoin_CheckedChanged" /></label></div>
            </div>
            <div id="pnlCountry" class="form-group" runat="server" visible="false">
                <div class="form-inline">
                    <label><%= Lang.Trans("Country") %></label>&nbsp;
                    <select ID="dropCountry" enableviewstate="false" class="form-control" runat="server"></select>
                </div>
            </div>
            <div id="pnlState" class="form-group" runat="server" visible="false">
                <div class="form-inline">
                    <label><%= Lang.Trans("Region/State") %></label>&nbsp;
                    <select ID="dropRegion" class="form-control" runat="server"></select>
                </div>
            </div>
            <div id="pnlCity" class="form-group" runat="server" visible="false">
                <div class="form-inline">
                    <label><%= Lang.Trans("City") %></label>&nbsp;
                    <select ID="dropCity" class="form-control" runat="server"></select>
                </div>
            </div>
            <!-- begin group permissions -->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th></th>
                        <th><%= Lang.Trans("Non Members") %></th>
                        <th><%= Lang.Trans("Members") %></th>
                        <th><%= Lang.Trans("V.I.P.") %></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><%= Lang.Trans("View group") %></td>
                        <td><asp:CheckBox ID="cbViewGroupNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewGroupMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewGroupVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("View message board") %></td>
                        <td><asp:CheckBox ID="cbViewMessageBoardNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewMessageBoardMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewMessageBoardVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("View gallery") %></td>
                        <td><asp:CheckBox ID="cbViewGalleryNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewGalleryMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewGalleryVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("View members") %></td>
                        <td><asp:CheckBox ID="cbViewMembersNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewMembersMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewMembersVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("View events") %></td>
                        <td><asp:CheckBox ID="cbViewEventsNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewEventsMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbViewEventsVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Upload photo") %></td>
                        <td><asp:CheckBox ID="cbUploadPhotoNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbUploadPhotoMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbUploadPhotoVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Use chat") %></td>
                        <td><asp:CheckBox ID="cbUseChatNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbUseChatMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbUseChatVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Add topic") %></td>
                        <td><asp:CheckBox ID="cbAddTopicNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbAddTopicMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbAddTopicVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Add post") %></td>
                        <td><asp:CheckBox ID="cbAddPostNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbAddPostMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbAddPostVip" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%= Lang.Trans("Add event") %></td>
                        <td><asp:CheckBox ID="cbAddEventNonMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbAddEventMembers" runat="server" /></td>
                        <td><asp:CheckBox ID="cbAddEventVip" runat="server" /></td>
                    </tr>
                </tbody>
            </table>
                <div class="form-group">
                <label><asp:Label ID="lblTerms" runat="server"/></label>
                <asp:TextBox ID="txtTerms" TextMode="MultiLine" CssClass="form-control" Rows="10" runat="server"/>
            </div>
            <asp:UpdatePanel ID="UpdatePanelPhotoDetails" runat="server">
                <ContentTemplate>
                    <div class="form-group" id="pnlQuestion" Visible="false" runat="server">
                        <label><asp:Label ID="lblQuestion" runat="server"/></label>
                        <asp:TextBox ID="txtQuestion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Columns="50"/>
                        <small class="text-muted"><i class="fa fa-lightbulb-o"></i>&nbsp;<%= Lang.Trans("You may leave this field blank") %></small>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddAccessLevel" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="actions">
                <asp:Button CssClass="btn btn-default" ID="btnCancelUpdate" runat="server" OnClick="btnCancelUpdate_Click" />
                <asp:Button CssClass="btn btn-primary" ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" />
            </div>
		</asp:View>
        <asp:View ID="viewTransferOwnership" runat="server">
        <div class="input-group">
            <asp:DropDownList CssClass="form-control" ID="ddGroupMembers" runat="server"/>
            <div class="input-group-btn"><asp:Button CssClass="btn btn-default" ID="btnCancelTransfer" runat="server" OnClick="btnCancelTransfer_Click" /></div>
            <div class="input-group-btn"><asp:Button CssClass="btn btn-primary" ID="btnTransfer" runat="server" OnClick="btnTransfer_Click" /></div>
        </div>
        </asp:View>
            <asp:View ID="viewDeleteGroup" runat="server">
            <%= Lang.Trans("Do you really want to delete this group? This is an irreversible action!") %>
            <div class="actions">
	            <asp:Button CssClass="btn btn-default" ID="btnCancelDelete" runat="server" OnClick="btnCancelDelete_Click"/>
	            <asp:Button CssClass="btn btn-primary" ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>