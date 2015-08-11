<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="ReviewNewUsers.aspx.cs" Inherits="AspNetDating.ReviewNewUsers"%>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server"/>
		<div class="SideMenuItem">
		<%= Lang.Trans("Moderation points") %>:<b>
		<%= CurrentUserSession.ModerationScores %></b></div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <asp:UpdatePanel ID="UpdatePanelPhotoDetails" runat="server">
        	<ContentTemplate>
	            <uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
	            <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
	            <div id="pnlNewUser" runat="server">
	                <div class="text-center">
                        <span class="img-thumbnail"><asp:Literal ID="ltrPhoto" runat="server"/></span>
                        <br />
                        <label><asp:Label ID="lblPhotoName" runat="server" /></label>
                        <div><asp:Label ID="lblPhotoDescription" runat="server" /></div>
                    </div>
                    <hr />
                    <asp:DataList ID="dlPhotos" runat="server" SkinID="UserPhotos" RepeatLayout="Flow" CssClass="repeater-horizontal" ShowFooter="False"  ShowHeader="False" onitemcommand="dlPhotos_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkShowPhoto" CssClass="thumbnail" CommandName="ShowPhoto" CommandArgument='<%# Eval("PhotoId") %>' runat="server">
                                <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, null, false, true, false) %>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList>
                    <ul class="info-header info-header-sm">
                        <%= CurrentNewUser != null ? CurrentNewUser.Username : String.Empty%>
                        <% if (!Config.Users.DisableAgeInformation) { %>
                            <li>
                                <label><%= Lang.Trans("Age") %></label>&nbsp;<%= CurrentNewUser != null ? CurrentNewUser.Age : 0 %>
                            </li>
                        <% } %>
                        <li>
                            <label><%= Lang.Trans("Gender") %></label>&nbsp;<%= CurrentNewUser.Gender %>
                        </li>
                    </ul>
                    <div id="pnlLocation" runat="server">
                        <div class="form-group"><label><%= Lang.Trans("Location") %></label>&nbsp;<%= CurrentNewUser.LocationString %></div>
                        <div class="form-group"><uc1:HeaderLine ID="hlSlogan" runat="server" /></div>
                        <asp:PlaceHolder ID="plhProfile" runat="server"/>
                        <div class="actions">
                            <asp:Button ID="btnApprove" runat="server" onclick="btnApprove_Click" />
                            <asp:Button ID="btnReject" runat="server" onclick="btnReject_Click" />
                            <asp:Button ID="btnPass" runat="server" onclick="btnPass_Click" />
                        </div>
                    </div>
	            </div>
            </ContentTemplate>
        </asp:UpdatePanel>        
    	<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
	</article>
</asp:Content>
