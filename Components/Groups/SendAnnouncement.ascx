<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendAnnouncement.ascx.cs" Inherits="AspNetDating.Components.Groups.SendAnnouncement" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>

<div class="form-horizontal" id="pnlSendAnnoucement" runat="server">
    <div class="form-group" id="pnlGender" runat="server">
        <label class="control-label col-sm-3"><%= Lang.Trans("Send to") %></label>
        <div class="col-sm-9">
            <asp:DropDownList ID="ddGender" CssClass="form-control" runat="server" />
        </div>
    </div>
    <div class="form-group" id="pnlAge" runat="server">
        <label class="control-label col-sm-3"><%= Lang.Trans("Age Range") %></label>
        <div class="col-sm-9">
            <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2" />
            <%= Lang.Trans("to") %>
            <asp:TextBox ID="txtAgeTo" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label col-sm-3"><%= Lang.Trans("Enter your announcement") %></label>
        <div class="col-sm-9"><asp:TextBox ID="txtAnnouncement" runat="server" CssClass="form-control" TextMode="MultiLine"/></div>
    </div>
</div>
<div class="actions">
    <asp:Button CssClass="btn btn-default" ID="btnSendAnnouncement" runat="server" OnClick="btnSendAnnouncement_Click"/>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>