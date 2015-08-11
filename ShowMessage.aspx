<%@ Page Language="c#" MasterPageFile="Site.Master" CodeBehind="ShowMessage.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.ShowMessage" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Site" %>
<%@ Register Src="Components/ReportAbuse.ascx" TagName="ReportAbuse" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <aside>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"/>
        <ul class="nav">
            <li><asp:LinkButton ID="lnkReply" runat="server" OnClick="lnkReply_Click" /></li>
            <li id="divTranslate" visible="false" runat="server"><a href="#" id="btnTranslate"><%= "Translate Message".Translate() %></a></li>
            <li id="pnlBlockUser" runat="server"><asp:LinkButton ID="lnkBlockUser" runat="server" OnClick="lnkBlockUser_Click" /></li>
            <li id="pnlUnblockUser" runat="server"><asp:LinkButton ID="lnkUnblockUser" runat="server" OnClick="lnkUnblockUser_Click" /></li>
            <li id="pnlGrantAccessToPrivatePhotos" runat="server"><asp:LinkButton ID="lnkGrantAccess" runat="server" OnClick="lnkGrantAccess_Click" /></li>
            <li id="pnlDenyAccessToPrivatePhotos" runat="server"><asp:LinkButton ID="lnkDenyAccess" runat="server" OnClick="lnkDenyAccess_Click" /></li>
            <li id="pnlReportAbuse" runat="server"><asp:LinkButton ID="lnkReportAbuse" runat="server" OnClick="lnkReportAbuse_Click" /></li>
            <li id="pnlDelete" runat="server"><asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" /></li>
            <li><asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" /></li>
        </ul>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
        <uc2:ReportAbuse ID="ReportAbuse1" runat="server" />
        <ul class="info-header">
           <!-- <li><a target="_new" id="lnkShowUser" runat="server"><asp:Literal ID="ltrPhoto" runat="server"/></a></li>-->
            <li>
                <a class="tooltip-link" title="<%= Lang.Trans("From") %>"><b><asp:Label ID="lblFromUsername" runat="server"/></b></a>
                &nbsp;<i class="fa fa-comments-o"></i>&nbsp;
                <a class="tooltip-link" title="<%= Lang.Trans("To") %>"><b><asp:Label ID="lblToUsername" runat="server"/></b></a>
            </li>
            <li class="pull-right"><a class="tooltip-link" title="<%= Lang.Trans("Date Created") %>"><i class="fa fa-clock-o"></i>&nbsp;<asp:Label ID="lblMessageTime" runat="server"/></a></li>
        </ul>
        <div class="list-group-item"><asp:Label ID="lblMessage" runat="server"/></div>
        <div id="pnlPreviousMessages" visible="false" runat="server" class="text">
            <hr />
            <asp:Repeater ID="rptPreviousMessages" runat="server">
                <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
                <ItemTemplate>
                    <li class="list-group-item">
                        <b><%# Eval("Username") %></b> <%# Eval("Message") %>
                    </li>
                </ItemTemplate>
                <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater>
        </div>
        <components:BannerView ID="bvShowMessageRightBottom" runat="server" Key="ShowMessageRightBottom"/>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
    </article>
</asp:Content>
