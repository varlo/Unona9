<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditContentViews.aspx.cs" Inherits="AspNetDating.Admin.EditContentViews" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<uc1:messagebox id="Messagebox1" runat="server"/>
<div class="panel clear-panel">
	<div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Languages/Content") %></h4></div>
	<div class="panel-body">
        <div class="form-horizontal">
            <div class="medium-width">
                <div class="form-group" id="trLanguage" runat="server">
                    <label class="col-sm-3 control-label"><%= Lang.TransA("Select Language:") %></label>
                    <div class="col-sm-9"><asp:dropdownlist CssClass="form-control" id="ddLanguage" AutoPostBack="True" Runat="server" OnSelectedIndexChanged="ddLanguage_SelectedIndexChanged"/></div>
                </div>
                <div class="form-group" id="trPageName" runat="server">
                    <label class="col-sm-3 control-label"><%= Lang.TransA("Select Content:") %></label>
                    <div class="col-sm-9"><asp:dropdownlist CssClass="form-control" id="ddContentKey" AutoPostBack="True" Runat="server" OnSelectedIndexChanged="ddContentKey_SelectedIndexChanged"/></div>
                </div>

                <div id="divEditContentView" runat="server" Visible="False">
                    <hr />
                    <h4><%= Lang.TransA("Edit Content") %></h4>
                    <hr />
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Content") %></label>
                        <div class="col-sm-9">
                            <div class="fckeditor">
                                <asp:PlaceHolder ID="phEditor" runat="server"/>
                            </div>
                        </div>
                    </div>
                    <div class="actions">
                        <asp:Button CssClass="btn btn-primary" id="btnSave" Runat="server" OnClick="btnSave_Click"/>
                    </div>
                </div>
            </div>
         </div>
    </div>
</div>
</asp:Content>
