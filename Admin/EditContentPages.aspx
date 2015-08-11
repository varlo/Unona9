<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditContentPages.aspx.cs" Inherits="AspNetDating.Admin.EditContentPages" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
	<div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Languages/Pages") %></h4></div>
	<div class="panel-body">
        <div class="form-horizontal">
            <div class="medium-width">
                <div class="form-group" id="trLanguage" runat="server">
                    <label class="col-sm-3 control-label"><%= Lang.TransA("Select Language:") %></label>
                    <div class="col-sm-9"><asp:dropdownlist CssClass="form-control" id="ddLanguage" AutoPostBack="True" Runat="server" OnSelectedIndexChanged="ddLanguage_SelectedIndexChanged"/></div>
                </div>
                <div class="form-group" id="trPageName" runat="server">
                    <label class="col-sm-3 control-label"><%= Lang.TransA("Select Page:") %></label>
                    <div class="col-sm-9"><asp:dropdownlist CssClass="form-control" id="ddPageName" AutoPostBack="True" Runat="server" OnSelectedIndexChanged="ddPageName_SelectedIndexChanged"/></div>
                </div>
            </div>
            <div id="divEditPage" runat="server" Visible="False">
                <hr />
                    <h4><%= Lang.TransA("Edit Page") %></h4>
                <hr />
                <div class="medium-width">
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Page Title") %></label>
                        <div class="col-sm-9"><asp:textbox CssClass="form-control" id="txtPageTitle" Runat="server"/></div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Link Text") %></label>
                        <div class="col-sm-9"><asp:textbox CssClass="form-control" id="txtLinkText" Runat="server"/></div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-9 col-sm-offset-3">
                            <div class="checkbox"><label><asp:CheckBox ID="cbURL" runat="server" AutoPostBack="true" /><%= Lang.TransA("Redirect to Url")%></label></div>
                        </div>
                    </div>
                    <div class="form-group" id="trURL" runat="server" visible="false">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("URL") %></label>
                        <div class="col-sm-9"><asp:TextBox CssClass="form-control" ID="txtURL" runat="server"/></div>
                    </div>
                    <div class="form-group" id="trUrlRewriteCheckbox" runat="server">
                        <div class="col-sm-9 col-sm-offset-3">
                            <div class="checkbox"><label><asp:CheckBox ID="cbRewriteUrl" runat="server" AutoPostBack="true" /><%= Lang.TransA("Rewrite Url")%></label></div>
                        </div>
                    </div>
                    <div class="form-group" id="trUrlRewriteTextbox" runat="server" visible="false">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("URL") %></label>
                        <div class="col-sm-9"><asp:TextBox CssClass="form-control" ID="txtUrlRewrite" runat="server"/> <%= Lang.TransA("(must end with .aspx)") %></div>
                    </div>
                    <div class="form-group" id="trPageContent" runat="server">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Page Content") %></label>
                        <div class="col-sm-9">
                            <div class="fckeditor">
                                <asp:PlaceHolder ID="phEditor" runat="server"/>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Header position") %></label>
                        <div class="col-sm-9"><asp:DropDownList CssClass="form-control" ID="ddHeaderPosition" Runat="server"/></div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Footer position") %></label>
                        <div class="col-sm-9"><asp:DropDownList CssClass="form-control" ID="ddFooterPosition" Runat="server"/></div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Visible for") %></label>
                        <div class="col-sm-9"><asp:DropDownList CssClass="form-control" ID="ddVisibleFor" Runat="server"/></div>
                    </div>
                    <div class="form-group" id="trMetaDescription" runat="server">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Meta description") %></label>
                        <div class="col-sm-9"><asp:TextBox CssClass="form-control" ID="txtMetaDescription" runat="server"/></div>
                    </div>
                    <div class="form-group" id="trMetaKeyword" runat="server">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Meta keyword") %></label>
                        <div class="col-sm-9"><asp:TextBox CssClass="form-control" ID="txtMetaKeyword" runat="server"/></div>
                    </div>

                    <div class="actions">
                        <asp:Button CssClass="btn btn-primary" ID="btnDelete" Runat="server" OnClick="btnDelete_Click"/>
                        <asp:Button CssClass="btn btn-primary" id="btnSave" Runat="server" OnClick="btnSave_Click"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
