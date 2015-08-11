<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="LostPassword.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.LostPassword" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<article class="no-sidebar">
	    <uc1:WideBoxStart id="WideBoxStart1" runat="server" />
            <p><%= Lang.Trans("Please enter the email address given during the"+" registration in order to change your password") %></p>
            <div class="input-group input-group-lg">
                <span class="input-group-addon"><%= Lang.Trans("email") %></span>
                <asp:TextBox id="txtEmail" CssClass="form-control" runat="server" />
                <div class="input-group-btn"><asp:Button id="btnSubmit" CssClass="btn btn-default" runat="server" onclick="btnSubmit_Click" /></div>
            </div>
            <asp:Label id="lblError" CssClass="alert text-danger" runat="server" />
	    <uc1:WideBoxEnd id="WideBoxEnd1" runat="server" />
	</article>
</asp:Content>