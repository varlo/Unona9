<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditTemplates.aspx.cs" Inherits="AspNetDating.Admin.EditTemplates" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<uc1:MessageBox id="MessageBox" runat="server"/>
<div class="medium-width">
    <div id="pnlLanguage" runat="server">
        <div class="panel clear-panel">
            <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Languages") %></h4></div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group" id="trLanguage" runat="server">
                        <label class="control-label col-sm-3"><%= Lang.TransA("Language") %>:</label>
                        <div class="col-sm-9">
                            <asp:dropdownlist CssClass="form-control" id="ddLanguage" Runat="server" AutoPostBack="True" onselectedindexchanged="ddLanguage_SelectedIndexChanged">
                                <asp:ListItem/>
                            </asp:dropdownlist>
                        </div>
                    </div>
                    <div class="form-group" id="trTemplateName" runat="server">
                        <label class="control-label col-sm-3"><%= Lang.TransA("Select Template:") %></label>
                        <div class="col-sm-9"><asp:dropdownlist CssClass="form-control" id="ddTemplateName" AutoPostBack="True" Runat="server" onselectedindexchanged="ddTemplateName_SelectedIndexChanged"/></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="phTemplate" runat="server"/>
    <asp:PlaceHolder id="phTemplates" runat="server"/>
    <div class="actions">
        <asp:Button CssClass="btn btn-primary" id="btnSave" runat="server" Visible="false" onclick="btnSave_Click"/>
    </div>
</div>
</asp:Content>
