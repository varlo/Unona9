<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Footer.ascx.cs" Inherits="AspNetDating.Components.Footer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<footer>
    <nav class="navbar" role="navigation">
        <div class="container-fluid">
            <ul class="nav navbar-nav">
                <asp:Repeater ID="rptPages" Runat="server">
                    <ItemTemplate>
                        <li id="liContentPage" data-id='<%# Eval("ID") %>' runat="server">
                            <a href='<%# Eval("URL") == null ? UrlRewrite.CreateContentPageUrl((int)Eval("ID")) : (string) Eval("URL")%>'><%# Eval("LinkText")%></a></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div><!-- /.container-fluid -->
    </nav>
	<components:BannerView id="bvDefaultFooter" runat="server" Key="DefaultFooter" />
	<div class="clearfix copyright">
	    <%= Lang.Trans("Copyright © 2003-2014") %> Powered by <a href="http://www.aspnetdating.com" target="_blank">ASPnetDating</a>
	</div>
</footer>