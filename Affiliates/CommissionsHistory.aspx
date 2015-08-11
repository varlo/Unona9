<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CommissionsHistory.aspx.cs"
    Inherits="AspNetDating.Affiliates.CommissionsHistory" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AffilaiteHeader" Src="AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateMenu" Src="AffiliateMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html>

<html>
    <head runat="server">
        <title><%= Lang.Trans("Commissions") %></title>
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
                                <div class="panel-heading"><h4 class="panel-title"><%= Lang.Trans("Commissions")%></h4></div>
                            </div>
                            <div class="panel-body">


                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                <uc1:MessageBox ID="MessageBox" runat="server"/>

                                    <asp:Panel ID="pnlAffiliateCommissionsPerPage" runat="server">
                                        <p class="text-right">
                                            <small class="text-muted"><asp:Label ID="lblAffiliateCommissionsPerPage" runat="server"/>:</small>
                                            <asp:DropDownList CssClass="form-control form-control-inline input-sm" ID="ddAffiliateCommissionsPerPage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddAffiliateCommissionsPerPage_SelectedIndexChanged"/>
                                        </p>
                                    </asp:Panel>
                                    <div class="table-responsive">
                                    <asp:GridView CssClass="table table-striped" ID="gvCommissions" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="1" PagerSettings-Mode="Numeric" GridLines="None" OnPageIndexChanging="gvCommissions_PageIndexChanging">
                                        <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%# Eval("Username") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%# Eval("Amount") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>

                                                <ItemTemplate>
                                                    <%# Eval("TimeStamp") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>

                                                <ItemTemplate>
                                                    <%# Eval("Notes") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </div>
                                </ContentTemplate>
                               </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </article>
            </section>
        </div>
    </form>
</body>
</html>
