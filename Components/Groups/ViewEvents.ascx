<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEvents.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewEvents" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="~/Components/DatePicker.ascx" %>
<%@ Register Src="~/Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBox" runat="server"/>
<asp:UpdatePanel ID="pnlCalendar" runat="server">
    <ContentTemplate>
        <asp:Calendar SkinID="TitleStyle" ID="calendar1" runat="server" CssClass="table calendar" DayHeaderStyle-CssClass="day-header" DayStyle-CssClass="day" NextPrevStyle-CssClass="next-prev" OtherMonthDayStyle-CssClass="other-month-day"
        SelectedDayStyle-CssClass="selected-day" SelectorStyle-CssClass="selector" TitleStyle-CssClass="title" TodayDayStyle-CssClass="today-day" WeekendDayStyle-CssClass="weekend-day" OnSelectionChanged="calendar1_SelectionChanged" OnDayRender="calendar1_DayRender"/>
	    <h4>
		    <%= Lang.Trans("Group events for date") %>&nbsp;<asp:Label ID="lblDate" runat="server"/>
        </h4>
        <hr />
        <asp:Label ID="lblError" CssClass="alert text-danger" runat="server" EnableViewState="false"/>
        <asp:Repeater ID="rptEvents" runat="server">
            <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
	   		<ItemTemplate>
	   		    <li class="list-group-item">
                    <div class="media">
                        <a class="pull-left" href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'>
                            <img class="media-object img-thumbnail" src='GroupEventImage.ashx?id=<%# Eval("ID") %>&width=50&height=50&diskCache=1' alt=""/>
                        </a>
                        <div class="media-body">
                            <h5 class="media-heading"><a href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'><%# Eval("Title") %></a></h5>
                            <ul class="info-header info-header-sm">
                                <li><a class="tooltip-link" title="<%= Lang.Trans("Event date") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("Date") %></a></li>
                                <li><a class="tooltip-link" title="<%= Lang.Trans("Created by") %>"><i class="fa fa-user"></i></a>&nbsp;<a href='ShowUser.aspx?uid=<%# Eval("Username") %>'><%# Eval("Username") %></a></li>
                            </ul>
                            <asp:Label ID="lblAttenders" runat="server" Text='<%# "<b>" + Lang.Trans("Attending: ") + "</b>" + Eval("Attenders") %>' Visible='<%# (string) Eval("Attenders") != "0" %>'/>
                        </div>
                    </div>
		    	</li>
    		</ItemTemplate>
    		<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>