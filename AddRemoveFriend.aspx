<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddRemoveFriend.aspx.cs" Inherits="AspNetDating.AddRemoveFriend" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
		<uc1:smallboxstart id="SmallBoxStart1" runat="server"/>
		    <ul class="nav">
		        <li><asp:LinkButton ID="lnkBack" Runat="server" onclick="lnkBack_Click" /></li>
		        <li id="pnlGoToFriends" runat="server"><asp:LinkButton ID="lnkFriends" Runat="server"  onclick="lnkFriends_Click" /></li>
		    </ul>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"/>
	</aside>
	<article>
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
		    <asp:Label ID="lblMessage" Runat="server"/>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
	</article>
</asp:Content>
