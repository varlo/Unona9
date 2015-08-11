<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="ReviewNewPhotos.aspx.cs" Inherits="AspNetDating.ReviewNewPhotos"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="~/Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="~/Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server" />
		<div class="text-center">
		    <%= Lang.Trans("Moderation points") %>&nbsp;<b><%= CurrentUserSession.ModerationScores %></b>
        </div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server" />
	    <uc1:smallboxstart id="SmallBoxStart2" runat="server" />
	        <components:ContentView ID="cvPhotoModerationRules" Key="PhotoModerationRules" runat="server"/>
		<uc1:smallboxend id="SmallBoxEnd2" runat="server" />
    </aside>
    <article>
        <asp:UpdatePanel ID="UpdatePanelPhotoDetails" runat="server">
        	<ContentTemplate>
	            <uc1:largeboxstart id="LargeBoxStart1" runat="server" />
	            <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False" />
	            <div class="media">
		            <asp:Image ID="imgPhoto" CssClass="pull-left img-thumbnail media-object" runat="server" />
		            <div class="media-body">
		                <div class="form-horizontal">
                            <div class="form-group" id="pnlPhotoName" runat="server">
                                <label class="control-label col-sm-3"><%= Lang.Trans("Name") %></label>
                                <div class="col-sm-9"><p class="form-control-static"><asp:Label ID="lblPhotoName" runat="server" /></p></div>
                            </div>
                            <div class="form-group" id="pnlPhotoDescription" runat="server">
                                <label class="control-label col-sm-3"><%= Lang.Trans("Description") %></label>
                                <div class="col-sm-9"><p class="form-control-static"><asp:Label ID="lblPhotoDescription" runat="server" /></p></div>
                            </div>
                            <div class="form-group" id="pnlUsername" runat="server">
                                <label class="control-label col-sm-3"><%= Lang.Trans("Username") %></label>
                                <div class="col-sm-9"><p class="form-control-static"><a id="lnkUsername" runat="server"></a></p></div>
                            </div>
                        </div>
                        <div class="actions">
                            <asp:Button CssClass="btn btn-default" ID="btnApprove" runat="server" onclick="btnApprove_Click" />
                            <asp:Button CssClass="btn btn-default" ID="btnReject" runat="server" onclick="btnReject_Click" />
                            <asp:Button CssClass="btn btn-default" ID="btnPass" runat="server" onclick="btnPass_Click" />
                        </div>
		            </div>
	            </div>
            </ContentTemplate>
        </asp:UpdatePanel>        
    	<uc1:largeboxend id="LargeBoxEnd1" runat="server" />
	</article>
</asp:Content>
