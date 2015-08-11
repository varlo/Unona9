<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ExportPhotos.aspx.cs" Inherits="AspNetDating.Admin.ExportPhotos" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Timer ID="Timer1" Interval="5000" runat="server" ontick="Timer1_Tick">
            </asp:Timer>
            <uc1:MessageBox ID="MessageBox1" runat="server"></uc1:MessageBox>
            <asp:Button ID="btnStartExport" runat="server" Text="Start exporting" OnClick="btnStartExport_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
