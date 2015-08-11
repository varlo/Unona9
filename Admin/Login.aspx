<%@ Page language="c#" Codebehind="Login.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.Login" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- Bootstrap -->
    <link href="../Images/bootstrap.css" rel="stylesheet">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <link href="../Images/font-awesome.css" rel="stylesheet">
    <link href="../Images/ekko-lightbox.css" rel="Stylesheet" type="text/css" />
    <link href="../Images/common.css" rel="Stylesheet" type="text/css" />
    <link href="../Images/common.less" rel="stylesheet/less" />
    <script src="../Images/less.js" type="text/javascript"></script>
    <link href="images/style.css" rel="Stylesheet" type="text/css" />
</head>
	<body class="login">
	<form id="Form1" method="post" runat="server">
		<uc1:AdminHeader id="AdminHeader1" runat="server"/>
		    <uc1:MessageBox id="MessageBox" runat="server"/>
		    <div class="container">
                <div class="panel login-panel default-panel">
                    <div class="panel-heading">
                        <h3 class="panel-title"><%= Lang.TransA("Authorize") %></h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-3"><%= Lang.TransA("Username") %></label>
                                <div class="col-sm-9"><asp:TextBox CssClass="form-control" id="txtUsername" runat="server"/></div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3"><%= Lang.TransA("Password") %></label>
                                <div class="col-sm-9"><asp:TextBox CssClass="form-control" id="txtPassword" runat="server" TextMode="Password"/></div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3"><%= Lang.TransA("Language") %></label>
                                <div class="col-sm-9"><asp:DropDownList CssClass="form-control" ID="ddLanguage" runat="server"/></div>
                            </div>
                        </div>
                        <div class="actions">
                            <asp:Button CssClass="btn btn-default btn-lg" id="btnLogin" runat="server" Text="Button" onclick="btnLogin_Click"/>
                        </div>
                    </div>
                </div>
		    </div>
		</form>
	</body>
</HTML>
