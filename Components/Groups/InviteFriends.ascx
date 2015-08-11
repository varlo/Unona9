<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InviteFriends.ascx.cs" Inherits="AspNetDating.Components.Groups.InviteFriends" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
<div id="pnlInviteFriends" runat="server">
	<label><%= Lang.Trans("Enter your friends' nicknames:") %></label>
	<div class="form-group">
	    <asp:TextBox CssClass="form-control" ID="txtFriend1" runat="server"/>
    </div>
    <div class="form-group">
	    <asp:TextBox CssClass="form-control" ID="txtFriend2" runat="server"/>
    </div>
    <div class="form-group">
	    <asp:TextBox CssClass="form-control" ID="txtFriend3" runat="server"/>
    </div>
    <div class="form-group">
	    <asp:TextBox CssClass="form-control" ID="txtFriend4" runat="server"/>
    </div>
    <div class="form-group">
	    <asp:TextBox CssClass="form-control" ID="txtFriend5" runat="server"/>
    </div>
    <div class="form-group">
	    <asp:TextBox CssClass="form-control" ID="txtFriend6" runat="server"/>
    </div>
</div>
<div class="actions">
    <asp:Button CssClass="btn btn-default" ID="btnSend" runat="server" OnClick="btnSend_Click" />
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>