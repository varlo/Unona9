<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AffiliateBanners.aspx.cs" Inherits="AspNetDating.Affiliates.AffiliateBanners" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AffilaiteHeader" Src="AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateMenu" Src="AffiliateMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html>

<html>
    <head runat="server">
        <title><%= Lang.Trans("Affiliate Banners") %></title>
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
	    <form id="form1" runat="server">
			<uc1:AffilaiteHeader ID="AdminHeader1" runat="server"/>
            <div class="container">
                <aside>
                    <uc1:AffiliateMenu ID="AdminMenu1" runat="server"/>
                </aside>
                <section>
                    <article>
                        <div class="content">
                            <div class="panel clear-panel">
                                <div id="content-head">
                                    <div class="panel-heading"><h4 class="panel-title"><%= Lang.Trans("Affiliate Banners")%></h4></div>
                                </div>
                                <div class="panel-body">
                                    <uc1:messagebox id="MessageBox" runat="server"/>
                                    <ul class="list-group list-group-striped">
                                    <asp:Repeater ID="rptAffiliateBanners" runat="server">
                                        <ItemTemplate>
                                            <li class="list-group-item">
                                                <div class="media">
                                                    <img class="img-thumbnail media-object pull-left" src='<%= Config.Urls.Home%>/AffiliateBannerImage.ashx?id=<%# Eval("ID") %>' />
                                                    <div class="media-body medium-width">
                                                        <h3 class="media-heading"><%# Eval("Name") %></h3>
                                                        <br />
                                                        <div class="form-group">
                                                            <label><%= Lang.Trans("HTML Code") %></label>
                                                            <asp:TextBox TextMode="MultiLine" Rows="5" CssClass="form-control" ID="txtHTMLCode" runat="server" ReadOnly="true" Text='<%# Eval("HTMLCode") %>'/>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </article>
                </section>
            </div>
	    </form>
	</body>
</html>
