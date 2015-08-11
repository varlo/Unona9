<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="AspNetDating.Admin.EditProfile" %>
<%@ Register TagPrefix="uc1" TagName="EditProfileCtrl" Src="EditProfileCtrl.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <uc1:editprofilectrl id="EditProfileCtrl1" runat="server"/>
</asp:Content>
