<%@ Import Namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FriendsOnlineBoxWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.FriendsOnlineBoxWebPart" %>

<asp:MultiView ID="mvFriends" runat="server" ActiveViewIndex="0">
<asp:View ID="vFriends" runat="server">
    <ul class="list-group">
    <asp:Repeater ID="rptFriends" Runat="server">
        <ItemTemplate>
        <li class="list-group-item">
            <a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>' onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()"><%# Eval("Username") %></a>
            <span id="pnlStatusText" runat="server" visible='<%# Config.Users.EnableUserStatusText && User.IsPaidMember(((PageBase) Page).CurrentUserSession.Username) && Eval("StatusText") != DBNull.Value %>'> - <%# Eval("StatusText") %></span>
        </li>
        </ItemTemplate>
    </asp:Repeater>
    </ul>
</asp:View>
<asp:View ID="vNoFriends" runat="server">
    <div class="text-center">
        <%= Lang.Trans("There are no friends online!") %></div>
</asp:View>
</asp:MultiView>