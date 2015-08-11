<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditPhotos.aspx.cs" Inherits="AspNetDating.Admin.EditPhotos" %>
<%@ Register TagPrefix="uc1" TagName="EditPhotosCtrl" Src="EditPhotosCtrl.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <uc1:EditPhotosCtrl id="EditPhotosCtrl1" runat="server"/>
</asp:Content>
