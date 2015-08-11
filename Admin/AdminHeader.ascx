<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AdminHeader.ascx.cs" Inherits="AspNetDating.Admin.AdminHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:ScriptManager ID="ScriptManagerMaster" runat="server" />
<asp:UpdateProgress ID="UpdateProgressMaster" runat="server" DisplayAfter="0">
    <ProgressTemplate>
        <div style="position:absolute; top: 0; right: 0">
            <asp:Image ID="imgLoadingProgress" ImageUrl="~/Images/loading2.gif" runat="server" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<header>
    <div id="masthead" class="clearfix">
        <div class="pull-left">
            <a href="Home.aspx"><span id="logo"></span></a>
        </div>
        <ul class="nav navbar-nav navbar-right">
            <li><p class="navbar-text"><asp:Label id="lblWelcome" runat="server"/></p></li>
            <li><a href="http://support.aspnetdating.com" target="_blank"><i class="fa fa-life-ring"></i>&nbsp;Support</a></li>
            <li><asp:LinkButton id="lnkLogout" runat="server"/></li>
        </ul>
    </div>
    <asp:Panel class="glance" ID="pnlLogout" Runat="server">
        <a class="tooltip-link" title="<%= Lang.TransA("At a glance")%>"><i class="fa fa-info-circle fa-lg"></i><span>&nbsp;<%= Lang.TransA("At a glance")%></span></a>
        <ul>
            <li><b><%=NewUsersForTheLast24hours%></b>&nbsp;<%= Lang.TransA("new members for the last twenty-four hours") %></li>
            <li><b><%=PendingPhotos%></b>&nbsp;<%= Lang.TransA("photos pending") %></li>
            <li><b><%= PendingAnswers%></b>&nbsp;<%= Lang.TransA("answers pending") %></li>
        </ul>
    </asp:Panel>
</header>
