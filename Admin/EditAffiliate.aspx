<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditAffiliate.aspx.cs" Inherits="AspNetDating.Admin.EditAffiliate" %>
<%@ Register TagPrefix="uc1" TagName="EditAffiliateCtrl" Src="~/Admin/EditAffiliateCtrl.ascx"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <uc1:EditAffiliateCtrl ID="EditAffiliateCtrl1" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
