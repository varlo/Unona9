<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Settings.ascx.cs" Inherits="AspNetDating.Components.Blog.SettingsCtrl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>
<uc1:largeboxstart id="LargeBoxStart" runat="server"/>
<h4><uc1:headerline id="hlBlogSettings" runat="server"/></h4>
<asp:Label CssClass="alert text-danger" ID="lblError" Runat="server"/>
<div class="form-group">
	<label><%= Lang.Trans("Title") %></label>
	<asp:textbox id="txtName" CssClass="form-control" Runat="server"/>
</div>
<div class="form-group">
	<label><%= Lang.Trans("Content") %></label>
	<asp:textbox id="txtDescription" Rows="5" CssClass="form-control" TextMode="MultiLine" Runat="server"/>
</div>
<br/>
<div class="actions">
	<asp:Button CssClass="btn btn-default" Runat="server" id="btnSaveChanges" onclick="btnSaveChanges_Click"/>
</div>
<uc1:largeboxend runat="server" id="Largeboxend1"/>
