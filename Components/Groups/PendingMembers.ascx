<%@ Import namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PendingMembers.ascx.cs" Inherits="AspNetDating.Components.Groups.PendingMembers" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
<div class="table-responsive">
<asp:DataList ID="dlPendingMembers" runat="server" CssClass="table table-striped" Width="100%" OnItemCommand="dlPendingMembers_ItemCommand" OnItemDataBound="dlPendingMembers_ItemDataBound">
    <ItemTemplate>
            <div class="media">
				<a class="pull-left thumbnail media-object" href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'><%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 90, 90, null, false, true) %></a>
				<div class="media-body">
					<label><asp:Label ID="lblUsernameText" runat="server"/>:</label>&nbsp;<asp:Label ID="lblUsername" runat="server"/>
					<% if (showAge) { %>
					    <label><asp:Label ID="lblAgeText" runat="server"/>:</label>&nbsp;<asp:Label ID="lblAge" runat="server"/>
					<% } %>
					<% if (showGender) { %>
				    <label><asp:Label ID="lblGenderText" runat="server"/>:</label>&nbsp;<asp:Label ID="lblGender" runat="server"/>
				    <% } %>
					<label id="pnlAnswer" runat="server"><asp:Label ID="lblAnswerText" runat="server"/>:</label>&nbsp;<asp:Label ID="lblAnswer" runat="server"/>
					<div class="actions">
                        <asp:LinkButton CssClass="btn btn-default" ID="lnkApprove" runat="server" Text='<%# Lang.Trans("Approve") %>' CommandName="Approve" CommandArgument='<%# Eval("Username") %>' />
                        <asp:LinkButton CssClass="btn btn-default" ID="lnkReject" runat="server" Text='<%# Lang.Trans("Reject") %>' CommandName="Reject" CommandArgument='<%# Eval("Username") %>' />
					</div>
				</div>
			</div>
	</ItemTemplate>
</asp:DataList>
</div>
<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>