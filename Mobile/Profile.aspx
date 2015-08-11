<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="AspNetDating.Mobile.Profile" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script src="../scripts/jquery.min.js" type="text/javascript"></script>
    <h1 id="lblTitle" runat="server" /> 
    <a href="UploadPhotos.aspx"><%= "Upload Photos".Translate()%></a> 
    <asp:Label ID="lblError" runat="server" EnableViewState="False"></asp:Label>
    <asp:PlaceHolder id="plhProfile" runat="server"></asp:PlaceHolder>
    <asp:Button id="btnSave" runat="server" onclick="btnSave_Click" CssClass="btn btn-default"></asp:Button>
    <asp:Literal ID="litAlert" EnableViewState="false" Runat="server"></asp:Literal>
</asp:Content>
