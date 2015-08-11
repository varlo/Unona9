<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ManageBadWords.aspx.cs" Inherits="AspNetDating.Admin.ManageBadWords" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="small-width">
    <div class="form-group">
		<div class="checkbox"><label><asp:CheckBox ID="cbUserRegularExpressions" runat="server" /><%= Lang.TransA("Use regular expressions") %></label></div>
    </div>
    <div class="form-group">
        <asp:TextBox CssClass="form-control" ID="txtBadWords" Rows="20" runat="server" TextMode="multiLine"/>
        <small class="text-muted"><i class="fa fa-lightbulb-o"></i>&nbsp;<%= Lang.TransA("Every word have to be on new line") %></small>
    </div>
    <div class="actions">
        <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" OnClick="btnSave_Click" />
    </div>
</div>
</asp:Content>
