﻿<%@ Import namespace="AspNetDating"%>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="ViewVideoBroadcast.ascx.cs"
    Inherits="AspNetDating.Components.Profile.ViewVideoBroadcast" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<input id="hidVideoGuid" type="hidden" value='<%= VideoGuid.ToString() %>' />
<input id="hidCurrentUser" type="hidden" value='<%= ((PageBase)Page).CurrentUserSession.Username %>' />
<input id="hidViewedUser" type="hidden" value='<%= User.Username %>' />
<script language="javascript" type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (sender, args) { 
        newEventsTimer = setInterval("FetchNewEventsForVideoBroadcast()", 2000);
        FetchNewEventsForVideoBroadcast();
    });	    
</script>
<div id="divBroadcastEndMessage" style="text-align:center; display:none">
    <%= Lang.Trans("The user is no longer broadcasting.") %>
</div>
<table id='tblVideoBroadcast' cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td valign="top" width="320">
			<div id="divReceiveFlash">
			    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="320" height="240">
			        <param name="movie" value="BroadcastVideoResource.ashx?res=receive&br_width=320&br_height=240&br_guid=<%= VideoGuid.ToString() %>&br_mode=audiovideo&br_add=<%= FlashServer %>">
			        <param name="quality" value="high">
			        <embed src="BroadcastVideoResource.ashx?res=receive&br_width=320&br_height=240&br_guid=<%= VideoGuid.ToString() %>&br_mode=audiovideo&br_add=<%= FlashServer %>"
			            quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="320" height="240"></embed>
			    </object>
			</div>
		</td>
		<td valign="top">
			<div class="videopanel">
			    <div id="UsersWatching"></div>
			    <div id="VideoBroadcastMessages"></div>
			    <div id="VideoBroadcastSendMessage">
			        <input id="txtSendMessage" maxlength="100" class="textline" type="text" value="" onkeydown="return KeyHandler(event)" />
			        <input id="btnSendMessage" type="button" value='<%= Lang.Trans("Send") %>' onclick="SendMessage()" />
			    </div>
		    </div>
		</td>
	</tr>
</table>
<div>
</div>