<%@ Control Language="c#" AutoEventWireup="True" Codebehind="DeleteButton.ascx.cs" Inherits="AspNetDating.Admin.DeleteButton" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0" width="150" height="30" id="deleteButton" align="middle" VIEWASTEXT>
<param name="allowScriptAccess" value="sameDomain" />
<param name="movie" value="<%= RootPath %>Admin/DeleteVideoButton/deleteButton.swf?sessID=<%= strSessID %>&myName=<%= strUserID %>&username=<%= strTargetUsername %>&" />
<param name="quality" value="high" />
<param name="bgcolor" value="#cccccc" />
<embed src="<%= RootPath %>Admin/DeleteVideoButton/deleteButton.swf?sessID=<%= strSessID %>&myName=<%= strUserID %>&username=<%= strTargetUsername %>&"
		quality="high" bgcolor="#cccccc" width="150" height="30" name="deleteButton" align="middle"
		allowScriptAccess="sameDomain" type="application/x-shockwave-flash"
		pluginspage="http://www.macromedia.com/go/getflashplayer" />
</object>