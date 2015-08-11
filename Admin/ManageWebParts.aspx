<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ManageWebParts.aspx.cs" Inherits="AspNetDating.Admin.ManageWebParts" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"/>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="medium-width">
    <div class="row">
        <div class="col-sm-5">
            <h4><%= "Sidebar Zone Components".TranslateA() %></h4>
            <div class="form-group">
                <asp:ListBox CssClass="form-control" Rows="5" ID="lbLeftZone" runat="server"/>
            </div>
            <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnLeftZoneRemove" runat="server" onclick="btnLeftZoneRemove_Click" />
            <div class="pull-right">
                <asp:LinkButton CssClass="btn btn-default btn-sm" ID="imgbLeftZoneUp" runat="server" onclick="imgbLeftZoneUp_Click"><i class="fa fa-caret-up"></i></asp:LinkButton>
                <asp:LinkButton CssClass="btn btn-default btn-sm" ID="imgbLeftZoneDown" runat="server" onclick="imgbLeftZoneDown_Click"><i class="fa fa-caret-down"></i></asp:LinkButton>
            </div>
        </div>
        <div class="col-sm-7">
            <h4><%= "Content Zone Components".TranslateA() %></h4>
            <div class="form-group">
                <asp:ListBox CssClass="form-control" Rows="5" ID="lbRightZone" runat="server"/>
            </div>
            <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnRightZoneRemove" runat="server" onclick="btnRightZoneRemove_Click" />
            <div class="pull-right">
                <asp:LinkButton CssClass="btn btn-default btn-sm" ID="imgbRightZoneUp" runat="server" onclick="imgbRightZoneUp_Click"><i class="fa fa-caret-up"></i></asp:LinkButton>
                <asp:LinkButton CssClass="btn btn-default btn-sm" ID="imgbRightZoneDown" runat="server" onclick="imgbRightZoneDown_Click"><i class="fa fa-caret-down"></i></asp:LinkButton>
            </div>
        </div>
    </div>
    <hr />
    <h4><%= "Available Components".TranslateA() %></h4>
    <div class="form-group"><asp:ListBox CssClass="form-control" Rows="13" ID="lbWebPartPool" runat="server"/></div>
    <asp:LinkButton CssClass="btn btn-primary btn-sm" ID="btnEnableWebPart" runat="server" onclick="btnEnableWebPart_Click" />
    <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnDisableWebPart" runat="server" onclick="btnDisableWebPart_Click" />
    <asp:LinkButton CssClass="btn btn-secondary pull-right btn-sm" ID="btnAddToZone" runat="server" onclick="btnAddToZone_Click" />
    <hr />
    <div class="text-center">
        <p><label class="checkbox-inline"><asp:CheckBox ID="cbResetUsersLayout" runat="server" /></label></p>
        <asp:Button CssClass="btn btn-primary" ID="btnStoreWebPartLayoutToDB" runat="server" onclick="btnStoreWebPartLayoutToDB_Click" />
    </div>
</div>
</asp:Content>
