<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveBlogPost.aspx.cs" Inherits="AspNetDating.Admin.ApproveBlogPost" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel default-panel medium-width center-block">
    <div class="panel-heading">
        <h4 class="panel-title"><%= Lang.TransA("Approve Blog Post")%></h4>
    </div>
    <div class="panel-body">
        <div class="form-group">
            <div class="form-inline"><label><%= Lang.TransA("Username") %>:</label>&nbsp;<asp:Label id="lblUsername" runat="server"/></div>
        </div>
        <div class="form-group">
            <label><%= Lang.TransA("Blog post title") %></label>
            <asp:TextBox CssClass="form-control" id="txtTitle" runat="server" TextMode="SingleLine"/>
        </div>
        <div class="form-group">
            <label><%= Lang.TransA("Blog post content") %></label>
            <asp:TextBox CssClass="form-control" Rows="7" id="txtContent" runat="server" TextMode="MultiLine"/>
        </div>
	</div>
	<div class="panel-footer">
        <asp:LinkButton CssClass="btn btn-default" id="btnCancel" runat="server" onclick="btnCancel_Click"/>
        <div class="btn-group pull-right">
            <asp:LinkButton CssClass="btn btn-default" id="btnReject" runat="server" onclick="btnReject_Click"/>
            <asp:LinkButton CssClass="btn btn-secondary" id="btnSaveAndApprove" runat="server" onclick="btnSaveAndApprove_Click"/>
        </div>
	</div>
</div>
</asp:Content>
