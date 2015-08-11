<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditAdmin.aspx.cs" Inherits="AspNetDating.Admin.EditAdmin" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div id="tblAll" runat="server">
    <div class="panel clear-panel">
        <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Account Details") %></h4></div>
        <div class="panel-body">
            <div class="form-horizontal medium-width">
                <div class="form-group" id="trUsername" runat="server">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Username") %></label>
                    <div class="col-sm-8"><asp:textbox CssClass="form-control" id="txtUsername" Runat="server"/></div>
                </div>
                <div class="form-group" id="trCurrentPassword" runat="server">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Current password") %></label>
                    <div class="col-sm-8"><asp:textbox CssClass="form-control" id="txtCurrentPassword" Runat="server" TextMode="Password"/></div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><asp:label id="lblNewPassword" Runat="server"/></label>
                    <div class="col-sm-8"><asp:textbox CssClass="form-control" id="txtNewPassword" Runat="server" TextMode="Password"/></div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><asp:label id="lblConfirmNewPassword" Runat="server"/></label>
                    <div class="col-sm-8"><asp:textbox CssClass="form-control" id="txtConfirmNewPassword" Runat="server" TextMode="Password"/></div>
                </div>
            </div>
        </div>
    </div>
    <div id="trPermissions" runat="server">
        <asp:label id="lblAccountPermissions" runat="server"/>
        <asp:datagrid id="gridPermissions" CssClass="table table-striped" Runat="server" AutoGenerateColumns="False" AllowPaging="False" PageSize="10" GridLines="None">
            <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal Text='<%# Eval("Section") %>' ID="litSection" Runat=server/>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbReadPermission" Runat=server Checked='<%# (bool)Eval("Read")%>'/>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbWritePermission" Runat=server Checked='<%# (bool)Eval("Write")%>'/>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:datagrid>
    </div>
    <div class="actions">
        <asp:button CssClass="btn btn-primary" id="btnSave" runat="server" onclick="btnSave_Click"/>
    </div>
</div>
</asp:Content>
