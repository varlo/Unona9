<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="UploadLogo.aspx.cs" Inherits="AspNetDating.Admin.UploadLogo" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
	<div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Upload Logo") %></h4></div>
	<div class="panel-body">
        <asp:Image ID="imgLogo" Runat="server"/>
        <hr />
        <div class="input-group medium-width">
            <span class="input-group-addon"><%= Lang.TransA("Image file") %></span>
            <p class="form-control-static"><input id="ufLogo" type="file" runat="server" name="ufLogo"></p>
            <div class="input-group-btn">
                <asp:Button CssClass="btn btn-primary" id="btnUpload" OnClick="btnUpload_Click" Runat="server"/>
            </div>
        </div>
    </div>
</div>
</asp:Content>
