<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditGroup.aspx.cs" Inherits="AspNetDating.Admin.EditGroup" %>
<%@ Register TagPrefix="uc1" TagName="EditGroupCtrl" Src="~/Admin/EditGroupCtrl.ascx"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <uc1:EditGroupCtrl ID="EditGroupCtrl1" runat="server" />
</asp:Content>
