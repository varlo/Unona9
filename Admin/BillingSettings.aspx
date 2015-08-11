<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BillingSettings.aspx.cs" Inherits="AspNetDating.Admin.BillingSettings" %>
<%@ Register Src="MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <uc2:MessageBox ID="MessageBox1" EnableViewState="false" runat="server" />
    <asp:PlaceHolder ID="phBillingPlans" runat="server"/>
    <div class="actions">
        <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" onclick="btnSave_Click" />
        <asp:LinkButton CssClass="btn btn-secondary" ID="btnAddNewPlan" runat="server" onclick="btnAddNewPlan_Click" />
    </div>
</ContentTemplate>
</asp:UpdatePanel>  
</asp:Content>
