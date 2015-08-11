<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="BlockedUsers.aspx.cs" Inherits="AspNetDating.BlockedUsers" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="~/Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="~/Components/SmallBoxEnd.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<article class="no-sidebar">
	    <asp:UpdatePanel ID="UpdatePanelBlockedUsers" runat="server">
	    <ContentTemplate>
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
		<asp:Label ID="lblMessage" Runat="server"/>
		<asp:DataList HorizontalAlign="Center" CssClass="table table-striped" ID="dlBlockedUsers" Runat="server" Width="100%" onitemcommand="dlBlockedUsers_ItemCommand" onitemcreated="dlBlockedUsers_ItemCreated">
			<ItemTemplate>
			    <div class="media">
			        <a class="pull-left" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server" id="A1"><%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, "img-thumbnail media-object", false, true, true) %></a>
                    <div class="media-body">
                        <div class="clearfix">
                            <h4 class="media-heading pull-left"><%# Eval("Username") %></h4>
                            <div class="pull-right">
                                <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkUnblock" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Unblock"/>
                            </div>
                        </div>
                        <ul class="info-header info-header-sm">
                            <% if (!Config.Users.DisableAgeInformation) { %>
                            <li>
                                <label><%= Lang.Trans("Age") %></label>&nbsp;<%# Eval("Age") %>
                            </li>
                            <% } %>
                            <% if (showLastOnline) { %>
                            <li>
                                <label><%= Lang.Trans("Last Online") %></label>&nbsp;<%# Eval("LastOnlineString") %>
                            </li>
                            <% } %>
                            <% if (showRating) { %>
                                <li>
                                    <label><%= Lang.Trans("Rating") %></label>&nbsp;<%# Eval("Rating") %>
                                </li>
                            <% } %>
                            <li class="pull-right">
                                <label><%= Lang.Trans("Blocked on") %></label>&nbsp;<%# Eval("BlockedOn") %>
                            </li>
                        </ul>
                        <% if (showSlogan) { %>
                            <%# ((string) Eval("Slogan")).Length > 80 ? ((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan") %>
                        <% } %>
					</div>
			    </div>
			</ItemTemplate>
			</asp:DataList>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
		</ContentTemplate>
		</asp:UpdatePanel>
	</article>
</asp:Content>
