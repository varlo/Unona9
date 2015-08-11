<%@ Page Language="c#" CodeBehind="BroadcastVideoWindow.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.BroadcastVideoWindow" %>
<%@ Import namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE html>
<html class="broadcastvideowindow">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <!-- Bootstrap -->
    <link href="Images/bootstrap.css" rel="stylesheet">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <link href="Images/font-awesome.css" rel="stylesheet">
    <link href="Images/common.css" rel="Stylesheet" type="text/css" />
</head>
<body id="ctl00_body" runat="server">
    <form id="form1" runat="server">
        <input id="hidVideoGuid" type="hidden" value='<%= VideoGuid %>' />
        <input id="hidCurrentUser" type="hidden" value='<%= ((PageBase)Page).CurrentUserSession.Username %>' />
        <div id="divUserPreview" style="display: none; position: absolute; z-index: 99999">
            <div id="divUserPreviewImage"></div>
            <div id="divUserPreviewDetails"></div>
        </div>
	    <asp:ScriptManager ID="ScriptManager" runat="server" />
	    <script language="javascript" type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (sender, args) { 
                newEventsTimer = setInterval("FetchNewEventsForVideoBroadcast()", 2000);
                FetchNewEventsForVideoBroadcast();
            });
	    </script>

            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="320" height="240">
                <param name="movie" value="BroadcastVideoResource.ashx?res=broadcast&br_width=320&br_height=240&br_guid=<%= VideoGuid %>&br_mode=audiovideo&br_add=<%= FlashServer %>">
                <param name="quality" value="high">
                <embed src="BroadcastVideoResource.ashx?res=broadcast&br_width=320&br_height=240&br_guid=<%= VideoGuid %>&br_mode=audiovideo&br_add=<%= FlashServer %>"
                    quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="320" height="240"></embed>
            </object>
            <asp:UpdatePanel ID="UpdatePanelUsers" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Timer ID="TimerMessages" Interval="30000" runat="server" ontick="TimerMessages_Tick" />
                    <h4><%= Lang.Trans("Broadcast Watchers")%> <span class="badge" id="UsersWatching"></span></h4>
                    <div class="BroadcastWatchers">
                        <asp:DataList ID="dlUsers" SkinID="BroadcastWatchers" CssClass="table table-striped" RepeatColumns="1" runat="server" onitemcommand="dlUsers_ItemCommand">
                            <ItemTemplate>
                                <a href='<%# UrlRewrite.CreateShowUserUrl((string)DataBinder.GetDataItem(Container))%>' target="_blank" class="user-name" onmouseover="showUserPreview('<%# DataBinder.GetDataItem(Container) %>')" onmouseout="hideUserPreview()">
                                    <%# DataBinder.GetDataItem(Container)%>
                                </a>
                                <asp:LinkButton CssClass="btn btn-default btn-sm pull-right" ID="lnkBlock" Text='<%# Lang.Trans("Block") %>' runat="server" CommandName="Block" CommandArgument='<%# DataBinder.GetDataItem(Container) %>'/>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        <div id="VideoBroadcastMessages"></div>
        <div id="VideoBroadcastSendMessage">
            <div class="input-group input-group-lg">
                <input id="txtSendMessage" class="form-control" type="text" value="" onkeydown="return KeyHandler(event)" />
                <span class="input-group-btn"><input id="btnSendMessage" class="btn btn-default" type="button" value='<%= Lang.Trans("Send") %>' onclick="SendMessage()" /></span>
            </div>
        </div>
    </form>
</body>
</html>
