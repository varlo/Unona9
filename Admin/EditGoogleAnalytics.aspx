<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditGoogleAnalytics.aspx.cs" Inherits="AspNetDating.Admin.EditGoogleAnalytics" %>
<%@ Import Namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
	<div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Google Analytics")%></h4></div>
	<div class="panel-body medium-width">
        <%= Lang.TransA("Paste your google analytics tracking code in the textbox below") %>
        <asp:textbox CssClass="form-control" TextMode="MultiLine" Rows="20" Columns="50" id="txtGoogleAnalyticsCode" Runat="server"/>
        <div class="actions">
            <asp:Button CssClass="btn btn-default" ID="btnClear" Runat="server" onclick="btnClear_Click"/>
            <asp:Button CssClass="btn btn-primary" id="btnSave" Runat="server" onclick="btnSave_Click"/>
        </div>
    </div>
</div>
</asp:Content>
