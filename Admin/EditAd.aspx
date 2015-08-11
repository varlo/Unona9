<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditAd.aspx.cs" Inherits="AspNetDating.Admin.EditAd" %>
<%@ Register TagPrefix="uc1" TagName="EditAdCtrl" Src="~/Admin/EditAdCtrl.ascx"%>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <uc1:EditAdCtrl ID="EditAdCtrl1" runat="server" />
</asp:Content>
