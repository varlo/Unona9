<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AffiliateHeader.ascx.cs" Inherits="AspNetDating.Affiliates.AffiliateHeader" %>
<asp:ScriptManager ID="ScriptManagerMaster" runat="server" />
<asp:UpdateProgress ID="UpdateProgressMaster" runat="server">
    <ProgressTemplate>
        <asp:Image ID="imgLoadingProgress" ImageUrl="~/Images/loading2.gif" runat="server" />
    </ProgressTemplate>
</asp:UpdateProgress>
<header>
    <div id="masthead" class="clearfix">
        <div class="pull-left">
            <a href="Home.aspx"><span id="logo"></span></a>
        </div>
        <ul class="nav navbar-nav navbar-right">
            <li><p class="navbar-text"><asp:Label id="lblWelcome" runat="server"/></p></li>
            <li><asp:LinkButton id="lnkLogout" runat="server" OnClick="lnkLogout_Click"/></li>
        </ul>
    </div>
    <asp:Panel class="glance" ID="pnlLogout" Runat="server">
    <ul>
        <li><%= Lang.Trans("Your balance is") %>:&nbsp;<b><%= Affiliate.Balance.ToString("C") %></b></li>
    </ul>
    </asp:Panel>

</header>