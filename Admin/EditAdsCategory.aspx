<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditAdsCategory.aspx.cs" Inherits="AspNetDating.Admin.EditAdsCategory" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
    <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Category Details") %></h4></div>
    <div class="panel-body">
        <div class="form-inline">
            <label><%= Lang.TransA("Name") %></label>
            <asp:textbox CssClass="form-control" id="txtCategoryTitle" Runat="server"/>
        </div>
    </div>
</div>

<asp:Panel ID="pnlSubcategories" Runat="server">
<div class="panel clear-panel">
    <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Sub categories") %></h4></div>
	<div class="panel-body">
        <asp:DataGrid id="dgSubcategories" CssClass="table table-striped" Runat="server" AutoGenerateColumns="False" AllowPaging="False" PageSize="10" ShowHeader="False" GridLines="None">
            <Columns>
                <asp:TemplateColumn>
                    <ItemStyle Wrap="False" Width="1%"></ItemStyle>
                    <ItemTemplate>
                        <div class="checkbox"><label><input type="checkbox" id="cbSelect" value='<%# Eval("ID") %>' runat="server" name="cbSelect" /></label></div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:TextBox CssClass="form-control small-width" ID="txtTitle" Text='<%# Eval("Title")%>' Runat=server/>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
        <asp:LinkButton CssClass="btn btn-default btn-sm" id="btnDeleteSelectedSubcategories" runat="server" onclick="btnDeleteSelectedSubcategories_Click"/>
        <div class="input-group input-group-sm small-width pull-right">
            <span class="input-group-addon"><%= Lang.TransA("Add") %></span>
            <asp:DropdownList CssClass="form-control form-control-inline" id="dropNewSubcategoriesCount" runat="server" />
            <span class="input-group-addon"><%= Lang.TransA("new sub categories") %></span>
            <span class="input-group-btn"><asp:LinkButton CssClass="btn btn-secondary" id="btnAddNewSubcategories" runat="server" onclick="btnAddNewSubcategories_Click"/></span>
        </div>
	</div>
</div>
</asp:Panel>
<div class="actions">
    <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" onclick="btnCancel_Click" />
    <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" onclick="btnSave_Click" />
</div>
</asp:Content>
