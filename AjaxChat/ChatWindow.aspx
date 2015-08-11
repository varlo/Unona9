<%@ Page Language="C#" AutoEventWireup="true" Inherits="AjaxChat.ChatWindowPage" %>
<%@ Import namespace="AspNetDating.Classes"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ajax Chat</title>
    <script type="text/javascript" src="AjaxChat.js.aspx"></script>
</head>
<body onload="InitializeChat(<%= Request.Params["id"] != null ? Request.Params["id"] : "0" %>); SetHeight();" onunload="CloseChat()" onresize="SetHeight();" scroll="no">
    <form id="form1" runat="server" onsubmit="return false;" style="height: 100%">
        <asp:ScriptManager ID="scriptManager" runat="server">
            <Services>
                <asp:ServiceReference Path="ChatEngine.asmx" InlineScript="true" />
            </Services>
        </asp:ScriptManager>
        <div id="wrap">
            <div id="header">
                <img alt="" src="Images/logo.jpg" width="162" height="43" />
                <div id="header_right">
                	    <div id="righthead" class="righthead">
                         	Ajax Chat
                        </div>

                </div>
            </div>
            <div id="content-wrap">
            <div id="content">
            				<div id="left">
                            <div id="messages">
                            </div>
                            </div>
                            <div id="right">
                                <div id="users">
                                    <div id="usersonline"></div>
                                    <div id="usersright"></div>
                                </div>
                                <!--
                                <div id="slect_channel">
                                    <select name="chanel">
                                        <option value="1">Select a channel</option>
                                        <option>Select a channel</option>
                                    </select>
                                </div>
                                -->
            </div>
            </div>
            <div id="footer">
                <div id="footer_fonts">
                    <!--Format Text Options--></div>
                <table width="100%" border="0" cellspacing="10" cellpadding="0">
                    <tr>
                        <td id="sendtxt_td">
                            <div id="sendmsg_txt">
                                <input id="txtSendMessage" maxlength="200" type="text" value="" onkeypress="return RedirectEnterKey(event)" />
                            </div>
                        </td>
                        <td width="90">
                            <div id="sendmsg_btn">
                                <div align="center">
                                    <input id="btnSendMessage" type="button" value="" onclick="SendMessage()" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>