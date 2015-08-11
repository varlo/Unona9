<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpcomingEventsWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.UpcomingEventsWebPart" %>
<%@ Import namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>

<asp:MultiView ID="mvUpcomingEvents" ActiveViewIndex="0" runat="server">
<asp:View ID="viewUpcomingEvents" runat="server">
<asp:Repeater ID="rptGroupEvents" Runat="server">
    <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
	<ItemTemplate>
	    <li class="list-group-item">
            <div class="media">
                <a class="pull-left" href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'>
                    <img src='GroupEventImage.ashx?id=<%# Eval("ID") %>&width=50&height=50&diskCache=1' class="media-object img-thumbnail" alt=""/>
                </a>
                <div class="media-body">
                    <a href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'><%# Eval("Title") %></a>
                    <ul class="info-header info-header-sm">
                        <li><a class="tooltip-link" title="<%= Lang.Trans("Event date") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("Date") %></a></li>
                        <li><a class="tooltip-link" title="<%= Lang.Trans("Created by") %>"><i class="fa fa-user"></i></a>&nbsp;<a href='ShowUser.aspx?uid=<%# Eval("Username") %>'><%# Eval("Username") %></a></li>
                    </ul>
                </div>
            </div>
        </li>
	</ItemTemplate>
	<FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
</asp:View>
<asp:View ID="viewNoUpcomingEvents" runat="server">
    <div class="text-center">
        <%= Lang.Trans("There are no upcoming events!") %>
	</div>
</asp:View>
</asp:MultiView>