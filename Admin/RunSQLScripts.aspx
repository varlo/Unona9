<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RunSQLScripts.aspx.cs" Inherits="AspNetDating.Admin.RunSQLScripts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
        <div id="divInstallation" runat="server">
            E-mail : <asp:TextBox id="txtEmail" runat="server"></asp:TextBox>
            Sql File: <asp:DropDownList ID="ddSqlFiles" runat="server"></asp:DropDownList>
            <asp:Button ID="btnRun" runat="server" Text="Run" onclick="btnRun_Click" />
            <br />
            <asp:ListBox ID="lbTables" runat="server" Height="400" Width="300"></asp:ListBox>
            <asp:ListBox ID="lbSPs" runat="server" Height="400" Width="300"></asp:ListBox>
            <br />
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
