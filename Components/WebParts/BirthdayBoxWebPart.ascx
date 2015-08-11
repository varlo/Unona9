<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BirthdayBoxWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.BirthdayBoxWebPart" %>

<asp:MultiView ID="mvBirthdays" ActiveViewIndex="0" runat="server">
<asp:View ID="vBirthdays" runat="server">
    <ul class="list-group list-group-striped">
    <asp:Repeater ID="rptBirthdays" Runat="server">
        <ItemTemplate>
            <li class="list-group-item">
                <%# ((DateTime)Eval("Date")).ToShortDateString() %>&nbsp;<a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'><%# Eval("Username") %></a>
            </li>
        </ItemTemplate>
    </asp:Repeater>
    </ul>
</asp:View>
<asp:View ID="vNoBirthdays" runat="server">
    <div class="text-center"><%= Lang.Trans("There are no birthdays today!") %></div>
</asp:View>
</asp:MultiView>