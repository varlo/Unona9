<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="ShowEcard.aspx.cs" Inherits="AspNetDating.ShowEcard" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<article class="no-sidebar">
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
		<asp:Label ID="lblMessage" Runat="server" EnableViewState="false"/>
		<label><%= Lang.Trans("E-card's Name") %></label>:&nbsp;<asp:Label ID="lblEcardTypeName" runat="server"/>
		<div id="pnlImage" class="text-center" runat="server">
		    <img class="img-thumbnail" src='<%= "EcardContent.ashx?ect=" + Request.Params["ect"] %>' />
		</div>
		<div id="pnlFlash" class="text-center" runat="server">
		    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                                        id="flvplayer">
                                        <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/<%= "EcardContent.ashx?ect=" + Request.Params["ect"] + "&seed=" + new Random().NextDouble() %>">
                                        <param name="quality" value="high">
                                        <param name="bgcolor" value="#FFFFFF">
                                        <param name="wmode" value="transparent">
                                        <param name="allowfullscreen" value="true">
                                        <param name="flashvars" value="file=<%= "EcardContent.ashx?ect=" + Request.Params["ect"] + "&seed=" + new Random().NextDouble() %>&shuffle=false" />
                                        <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/<%= "EcardContent.ashx?ect=" + Request.Params["ect"] + "&seed=" + new Random().NextDouble() %>"
                                            quality="high" wmode="transparent" bgcolor="#FFFFFF" 
                                            name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                            pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="file=<%= "EcardContent.ashx?ect=" + Request.Params["ect"] + "&seed=" + new Random().NextDouble() %>&shuffle=false"></embed></object>
		</div>
		<asp:Label ID="lblEcardMessage" runat="server"/>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
	</article>
</asp:Content>
