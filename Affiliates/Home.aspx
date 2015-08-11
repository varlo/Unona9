<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="AspNetDating.Affiliates.Home" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateHeader" Src="AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateMenu" Src="AffiliateMenu.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<!DOCTYPE html>

<html>
    <head runat="server">
        <title> <%= Lang.Trans("Home") %></title>
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
			<uc1:AffiliateHeader id="AdminHeader1" runat="server"/>
            <div class="container">
                <aside>
                    <uc1:AffiliateMenu id="Adminmenu1" runat="server"/>
                </aside>
                <section>
                    <article>
                        <uc1:messagebox id="MessageBox" runat="server"/>
                        <div class="content">
                            <%= Lang.Trans("Welcome!") %>
                        </div>
                    </article>
                </section>
            </div>
		</form>
	</body>
</html>