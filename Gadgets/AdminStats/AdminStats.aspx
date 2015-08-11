<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />

    <script src="AdminStats.js" type="text/javascript"></script>

</head>
<body onload="StartTimer()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptManager" EnablePartialRendering="false" EnablePageMethods="false"
            runat="server">
            <Services>
                <asp:ServiceReference Path="~/Services/Gadgets.asmx" />
            </Services>
        </asp:ScriptManager>
        <div id="divTitle">
            <%= Config.Misc.SiteTitle %>
        </div>
        <table id="tblStats" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td class="left">
                    <%= Lang.Trans("new users:") %>
                </td>
                <td id="newusers" class="right">
                </td>
            </tr>
            <tr>
                <td class="left">
                    <%= Lang.Trans("pending photos:") %>
                </td>
                <td id="pendingphotos" class="right">
                </td>
            </tr>
            <tr>
                <td class="left">
                    <%= Lang.Trans("pending answers:") %>
                </td>
                <td id="pendinganswers" class="right">
                </td>
            </tr>
        </table>
        <div id="divButton">
            <a href="<%= Config.Urls.Home + "/admin/home.aspx" %>" target="_blank">
                <img src="button.png" alt="Open admin tool" /></a>
        </div>
    </form>
</body>
</html>
