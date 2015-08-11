<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" Codebehind="SendProfile.aspx.cs" Inherits="AspNetDating.SendProfile" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Components/Search/SearchResults.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
        <uc1:smallboxstart id="SmallBoxStart2" runat="server"/>
            <ul class="nav">
                <li><asp:LinkButton ID="lnkViewProfile" runat="server" OnClick="lnkViewProfile_Click" /></li>
            </ul>
        <uc1:smallboxend id="SmallBoxEnd2" runat="server"/>
    </aside>
    <article>
        <uc1:largeboxstart id="LargeBoxStart" runat="server"/>
		    <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
	        <div id="pnlSendProfile" class="form-horizontal" runat="server">
                <div class="form-group">
                    <label class="col-sm-3 control-label">
                        <%= Lang.Trans("Your Name") %>
                    </label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtSender" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-3 control-label">
                        <%= Lang.Trans("Friend's Name") %>
                    </label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtRecipientName" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-3 control-label">
                        <%= Lang.Trans("Friend's Email") %>
                    </label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtRecipientMail" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-3 control-label"><%= Lang.Trans("Message") %></label>
                    <div class="col-sm-9">
                        <div class="fckeditor">
                            <asp:PlaceHolder ID="phEditor" runat="server"/>
                        </div>
                    </div>
                </div>
	        </div>
            <div class="actions">
            	<asp:Button CssClass="btn btn-default" ID="btnSend" runat="server" OnClick="btnSend_Click" />
            </div>
	    <uc1:largeboxend id="LargeBoxEnd" runat="server"/>
	</article>
</asp:Content>
