<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ManageUser.aspx.cs" Inherits="AspNetDating.Admin.ManageUser" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div id="pnlActivateUser" runat="server" class="panel medium-width">
    <h4><%= Lang.TransA("User Activation") %></h4>
	<p><asp:Button CssClass="btn btn-primary btn-sm" ID="btnActivateUser" Runat="server" OnClick="btnActivateUser_Click" /></p>
</div>					
<div id="pnlVerificationsLeft" runat="server" class="panel small-width">
    <h4><%= Lang.TransA("Message Verifications") %></h4>
    <div class="input-group input-group-sm">
        <span class="input-group-addon"><%= Lang.TransA("Message verifications left:") %></span>
        <asp:TextBox CssClass="form-control" ID="txtVerificationsLeft" Runat="server"/>
        <div class="input-group-btn">
            <asp:Button CssClass="btn btn-primary" ID="btnSave" Runat="server" onclick="btnSave_Click"/>
        </div>
    </div>
</div>
<div class="panel medium-width">
	<h4><%= Lang.TransA("Delete User") %></h4>
    <div class="input-group input-group-sm">
        <span class="input-group-addon">
            <div class="checkbox"><asp:CheckBox ID="cbAllRelatedData" Runat="server"/></div>
        </span>
        <span class="input-group-addon"><%= Lang.TransA("Reason") %>:</span>
        <asp:TextBox CssClass="form-control" ID="txtDeleteReason" runat="server" />
        <div class="input-group-btn">
            <asp:Button CssClass="btn btn-primary" ID="btnDelete" Runat="server" onclick="btnDelete_Click"/>
        </div>
    </div>
</div>
<div id="pnlManualSubscription" runat="server" class="panel small-width">
    <h4><%= Lang.TransA("Subscription") %></h4>
    <div class="form-group">
        <asp:DropDownList CssClass="form-control" id="ddPlans" runat="server"/>
        <div class="text-muted"><asp:Label ID="lblCurrentPlan" Runat="server"/></div>
    </div>
    <div class="form-group" id="pnlExpirationDate" runat="server">
        <%= Lang.TransA("Expires on:") %>
        <uc2:DatePicker ID="dpExpirationDate" runat="server" />
    </div>
    <div  class="form-group" id="pnlAffiliateCommission" runat="server">
        <div class="checkbox">
            <label>
                <asp:CheckBox ID="cbAffiliateCommission" runat="server"/>
                <%= Lang.TransA("Do not apply commission") %>
            </label>
        </div>
    </div>
    <div class="actions">
        <asp:Button CssClass="btn btn-primary" ID="btnSubscribeUpdate" runat="server" OnClick="btnSubscribeUpdate_Click" />
    </div>
</div>
<div id="pnlCredits" runat="server" class="panel small-width">
    <h4><%= Lang.TransA("Credits") %></h4>
    <div class="input-group input-group-sm">
	    <span class="input-group-addon"><%= Lang.TransA("Credits:") %></span>
        <asp:TextBox CssClass="form-control" ID="txtCredits" runat="server"/>
        <div class="input-group-btn">
            <asp:Button CssClass="btn btn-primary" ID="btnUpdateCredits" runat="server" onclick="btnUpdateCredits_Click"/>
        </div>
    </div>
</div>
</asp:Content>
