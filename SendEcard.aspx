<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="SendEcard.aspx.cs" Inherits="AspNetDating.SendEcard" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<uc1:WideBoxStart id="WideBoxStart1" runat="server"/>
		<asp:label CssClass="alert text-danger" id="lblError" runat="server"/>
		<div class="row">
            <div class="col-sm-offset-3 col-sm-6">
                    <div class="form-group">
                        <label><%= Lang.Trans("E-card's Name") %></label>
                        <asp:DropDownList CssClass="form-control form-control-inline" ID="ddEcards" runat="server" onselectedindexchanged="ddEcards_SelectedIndexChanged" AutoPostBack="true"/>
                    </div>
                    <div class="form-group text-center">
                        <div id="pnlImage" runat="server"><img class="img-thumbnail" src='<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue %>' /></div>
                        <div id="pnlFlash" runat="server">
                            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                                                    id="flvplayer">
                                                    <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue + "&seed=" + new Random().NextDouble() %>">
                                                    <param name="quality" value="high">
                                                    <param name="bgcolor" value="#FFFFFF">
                                                    <param name="wmode" value="transparent">
                                                    <param name="allowfullscreen" value="true">
                                                    <param name="flashvars" value="file=<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue + "&seed=" + new Random().NextDouble() %>&shuffle=false" />
                                                    <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue + "&seed=" + new Random().NextDouble() %>"
                                                        quality="high" wmode="transparent" bgcolor="#FFFFFF"
                                                        name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                                        pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="file=<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue + "&seed=" + new Random().NextDouble() %>&shuffle=false"></embed>
                            </object>
                        </div>
                    </div>
                    <div id="pnlMessage" class="form-group" runat="server">
                        <label><%= Lang.Trans("Add a personal message") %></label>
                        <div class="fckeditor" ><asp:PlaceHolder ID="phEditor" runat="server"/></div>
                    </div>
            </div>
		</div>
		<div class="actions">
		    <asp:button class="btn btn-default" id="btnSend" tabIndex="1" runat="server" onclick="btnSend_Click"/>
		</div>
	<uc1:WideBoxEnd id="WideBoxEnd1" runat="server"/>
</asp:Content>