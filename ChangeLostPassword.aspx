<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="ChangeLostPassword.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.ChangeLostPassword" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<asp:Content id="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<uc1:WideBoxStart id="WideBoxStart1" runat="server"/>
		<asp:label CssClass="alert text-danger" id="lblError" runat="server"/>
		<h4><%= AspNetDating.Classes.Lang.Trans("New password") %></h4>
		<div class="form-horizontal">
			<div class="form-group">
                <label class="control-label col-sm-5"><%= AspNetDating.Classes.Lang.Trans("Please enter a new password") %></label>
                <div class="col-sm-7"><input id="txtNewPassword" type="password" runat="server" name="txtNewPassword" /></div>
			</div>
			<div class="form-group">
                <label class="control-label col-sm-5"><%= AspNetDating.Classes.Lang.Trans("Confirm your new password") %></label>
                <div class="col-sm-7"><input id="txtConfirmedPassword" type="password" runat="server" name="txtConfirmedPassword" /></div>
			</div>
		</div>
		<div class="actions"><asp:button id="btnSubmit" class="btn btn-default" tabIndex="1" runat="server" onclick="btnSubmit_Click"/></div>
	<uc1:WideBoxEnd id="WideBoxEnd1" runat="server"/>
</asp:Content>
