<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewGroup.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewGroup" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewMembers" Src="~/Components/Groups/NewMembers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewTopics" Src="~/Components/Groups/NewTopics.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewPhotos" Src="~/Components/Groups/NewPhotos.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewEvents" Src="~/Components/Groups/NewEvents.ascx" %>
<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc2" %>
<asp:MultiView ID="mvViewGroup" ActiveViewIndex="0" runat="server">
<asp:View ID="vGroupInfo" runat="server">
    <uc1:largeboxstart id="LargeBoxStart1" runat="server" />
    <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False" />
    <div class="media">
        <div class="pull-left"><img class="media-object img-thumbnail" src='GroupIcon.ashx?groupID=<%= GroupID %>&width=120&height=120&diskCache=1' /></div>
        <div id="pnlGroupInformation" class="media-body">
            <h4 class="media-heading">
                <uc2:HeaderLine ID="hlGroupName" runat="server" />
            </h4>
            <ul class="info-header info-header-sm">
                <li><a class="tooltip-link" title="<%= Lang.Trans("Date Created") %>"><i class="fa fa-clock-o"></i>&nbsp;<asp:Label ID="lblCreated" runat="server" /></a></li>
                <li><a class="tooltip-link" title="<%= Lang.Trans("Categories") %>"><i class="fa fa-sitemap"></i>&nbsp;<asp:Label ID="lblCategories" runat="server" /></a></li>
                <li><a class="tooltip-link" title="<%= Lang.Trans("Group Type") %>"><i class="fa fa-globe"></i>&nbsp;<asp:Label ID="lblType" runat="server" /></a></li>
                <li><a class="tooltip-link" title="<%= Lang.Trans("Owner") %>"><i class="fa fa-user"></i></a>&nbsp;<a id="lnkOwner" runat="server"></a></li>
                <li><a class="tooltip-link" title="<%= Lang.Trans("Members") %>"><i class="fa fa-users"></i>&nbsp;<asp:Label ID="lblMembers" runat="server" Visible="false" /></a><asp:LinkButton ID="lnkMembers" runat="server" OnClick="lnkMembers_Click" /></li>
            </ul>
            <asp:Label ID="lblGroupDescription" runat="server" />
        </div>
    </div>
    <uc1:largeboxend id="LargeBoxEnd1" runat="server" />
    <uc1:NewEvents id="NewEvents1" runat="server" />
    <uc1:NewTopics ID="NewTopics1" runat="server" />
    <uc1:NewMembers id="NewMembers1" runat="server" />
    <uc1:NewPhotos ID="NewPhotos1" runat="server" />
    <div class="actions">
	    <asp:Button ID="btnJoinGroup" CssClass="btn btn-primary" runat="server" OnClick="btnJoinGroup_Click"/>
    </div>
</asp:View>
<asp:View ID="vJoinGroup" runat="server">
<uc1:largeboxstart id="LargeBoxStart2" runat="server" />
    <asp:Label CssClass="alert text-danger" ID="lblError2" runat="server" EnableViewState="False" />
    <div id="pnlTerms" runat="server">
        <asp:Literal ID="ltrTerms" runat="server" />
        <div class="checkbox">
            <label><asp:CheckBox ID="cbAgree" runat="server" /></label>
        </div>
    </div>
    <div id="pnlQuestion" runat="server">
        <label><asp:Label ID="lblQuestion" runat="server" /></label>
        <asp:TextBox ID="txtAnswer" Columns="50" CssClass="form-control" runat="server" />
    </div>
    <div class="actions">
	    <asp:Button ID="btnJoinGroup2" CssClass="btn btn-default" runat="server" OnClick="btnJoinGroup2_Click"/>
    </div>
<uc1:largeboxend id="LargeBoxEnd2" runat="server" />
</asp:View>
</asp:MultiView>
