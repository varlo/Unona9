<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="News.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.NewsPage" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewsBox" Src="Components/NewsBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
		<uc1:NewsBox id="NewsBox1" runat="server" />
	</aside>
	<article>
		<uc1:largeboxstart id="LargeBoxStart1" runat="server" />
		<div id="divNewsContent" runat="server"></div>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server" />
	</article>
</asp:Content>
