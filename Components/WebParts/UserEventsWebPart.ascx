<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserEventsWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.UserEventsWebPart" %>

<asp:MultiView ID="mvUserEvents" runat="server" ActiveViewIndex="0">
    <asp:View ID="viewEvents" runat="server">
        <div class="list-group list-group-striped"><asp:PlaceHolder ID="plhEvents" runat="server" /></div>
        <asp:Literal ID="ltrEvents" runat="server"/>
    </asp:View>
    <asp:View ID="viewNoEvents" runat="server">
	    <div class="text-center"><%= Lang.Trans("There are no user events!") %></div>
    </asp:View>
</asp:MultiView>
