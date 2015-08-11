<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditAffiliateCtrl.ascx.cs" Inherits="AspNetDating.Affiliates.EditAffiliateCtrl" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>

<uc1:MessageBox id="MessageBox" runat="server"/>
<h4><%= Lang.Trans("Account Information") %></h4>
<div class="form-horizontal">
    <div class="medium-width">
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Current password") %></label>
            <div class="col-sm-8">
                <asp:TextBox CssClass="form-control" ID="txtCurrentPassword" runat="server" TextMode="Password"/></div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("New password") %></label>
            <div class="col-sm-8">
                <asp:TextBox CssClass="form-control" ID="txtNewPassword" runat="server" TextMode="Password"/></div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Confirm new password") %></label>
            <div class="col-sm-8">
                <asp:TextBox CssClass="form-control" ID="txtConfirmNewPassword" runat="server" TextMode="password"/></div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Name") %></label>
            <div class="col-sm-8">
                <asp:TextBox CssClass="form-control" ID="txtName" runat="server"/></div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Email") %></label>
            <div class="col-sm-8">
                <asp:TextBox CssClass="form-control" ID="txtEmail" runat="server"/></div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Site URL") %></label>
            <div class="col-sm-8">
                <asp:TextBox CssClass="form-control" ID="txtSiteURL" runat="server"/></div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4">
                <%= Lang.Trans("Payment Details")%></label>
            <div class="col-sm-8">
                <asp:TextBox CssClass="form-control" ID="txtPaymentDetails" runat="server" TextMode="MultiLine" Rows="5" Columns="50"/>
                </div>
        </div>
    </div>
    <hr />
    <div class="medium-width">
        <div class="col-sm-8 col-sm-offset-4 text-right">
            <asp:Button CssCLass="btn btn-default pull-left" ID="btnCancel" runat="server" OnClick="btnCancel_Click"/>
            <asp:Button CssCLass="btn btn-primary" ID="btnSave" runat="server" OnClick="btnSave_Click"/>
        </div>
    </div>
</div>