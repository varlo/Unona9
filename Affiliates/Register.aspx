<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Register.aspx.cs" Inherits="AspNetDating.Affiliates.Register" %>

<%@ Register TagPrefix="uc1" TagName="AffiliateHeader" Src="~/Affiliates/AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<!DOCTYPE html>

<html>
    <head runat="server">
        <title><%= Lang.Trans("Register") %></title>
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
        <link href="../Images/common.css" rel="Stylesheet" type="text/css" />
        <link href="../Images/common.less" rel="stylesheet/less" />
        <script src="../Images/less.js" type="text/javascript"></script>
        <link href="images/style.css" rel="Stylesheet" type="text/css" />
    </head>
	<body>
	<form id="Form1" method="post" runat="server">
	    <uc1:AffiliateHeader id="AffiliateHeader1" runat="server"/>
        <uc1:MessageBox id="MessageBox" runat="server"/>
		    <div class="container">
                <div class="panel register-panel default-panel">
                    <div class="panel-heading">
                        <h3 class="panel-title"><%= Lang.Trans("Register") %></h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.Trans("Username") %></label>
                                <div class="col-sm-8"><asp:TextBox CssClass="form-control" id="txtUsername" runat="server"/></div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.Trans("Password") %></label>
                                <div class="col-sm-8"><asp:TextBox CssClass="form-control" id="txtPassword" runat="server" TextMode="Password"/></div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.Trans("Confirm Password") %></label>
                                <div class="col-sm-8"><asp:TextBox CssClass="form-control" id="txtPasswordConfirm" runat="server" TextMode="Password"/></div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.Trans("Name") %></label>
                                <div class="col-sm-8"><asp:TextBox CssClass="form-control" id="txtName" runat="server"/></div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.Trans("Email") %></label>
                                <div class="col-sm-8"><asp:TextBox CssClass="form-control" id="txtEmail" runat="server"/></div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.Trans("Site URL")%></label>
                                <div class="col-sm-8"><asp:TextBox CssClass="form-control" id="txtSiteUrl" runat="server"/></div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.Trans("Payment Details")%></label>
                                <div class="col-sm-8"><asp:TextBox CssClass="form-control" id="txtPaymentDetails" runat="server" TextMode="MultiLine" Rows="10" Columns="50"/></div>
                            </div>
                            <div class="actions">
                                <asp:Button CssClass="btn btn-primary" id="btnRegister" runat="server" OnClick="btnRegister_Click"/>
                            </div>
                        </div>
                    </div>
                </div>
		    </div>
		</form>
	</body>
</html>
