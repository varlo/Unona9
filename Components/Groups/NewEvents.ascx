<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewEvents.ascx.cs" Inherits="AspNetDating.Components.Groups.NewEvents" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server" />
<asp:Label ID="lblError" CssClass="alert text-info" runat="server" />
<asp:Repeater ID="rptGroupEvents" Runat="server">
    <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
	<ItemTemplate>
        <li class="list-group-item">
            <div class="media">
                <a class="pull-left" href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'>
                    <img class="media-object img-thumbnail" src='GroupEventImage.ashx?id=<%# Eval("ID") %>&width=50&height=50&diskCache=1' alt=""/>
                </a>
                <div class="media-body">
                    <a href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'><%# Eval("Title") %></a>
                    <ul class="info-header info-header-sm">
                        <li><a class="tooltip-link" title="<%= Lang.Trans("Event date") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("Date") %></a></li>
                        <li><a class="tooltip-link" title="<%= Lang.Trans("Created by") %>&nbsp;<%# Eval("Username") %>"><i class="fa fa-user"></i></a>&nbsp;<a href='ShowUser.aspx?uid=<%# Eval("Username") %>'><%# Eval("Username") %></a></li>
                        <li><a class="tooltip-link" title="<%= Lang.Trans("Attenders") %>"><i class="fa fa-users"></i>&nbsp;<asp:Label ID="lblAttenders" runat="server" Text='<%# Eval("Attenders") %>' Visible='<%# (string) Eval("Attenders") != "0" %>' /></a></li>
                    </ul>
                </div>
			</div>
		</li>
	</ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
<div id="pnlMore" runat="server" class="text-right" visible="false">
	<asp:LinkButton CssClass="btn btn-link" ID="lnkMore" runat="server" OnClick="lnkMore_Click" />
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server" />