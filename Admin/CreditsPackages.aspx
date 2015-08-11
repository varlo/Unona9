<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CreditsPackages.aspx.cs" Inherits="AspNetDating.Admin.CreditsPackages" %>
<%@ Register Src="MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc2:MessageBox ID="MessageBox1" runat="server" />
<asp:MultiView ID="mvCreditsPackages" runat="server">
    <asp:View ID="viewCreditsPackages" runat="server">
        <div class="panel clear-panel">
            <div class="panel-heading"><h4 class="panel-title"><asp:Label ID="lblCreditsPackages" runat="server"/></h4></div>
            <div class="panel-body">
                <asp:datagrid id="dgCreditsPackages" CssClass="table table-striped" Runat="server" AutoGenerateColumns="False" GridLines="None" onitemcommand="dgCreditsPackages_ItemCommand" onitemcreated="dgCreditsPackages_ItemCreated">
                    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderStyle CssClass="filter_results_header"  Width="10%" Wrap="False"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                            <ItemTemplate>
                                <%# Eval("Name") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle CssClass="filter_results_header" Width="30%" Wrap="False"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                            <ItemTemplate>
                                <%# Eval("Quantity") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle CssClass="filter_results_header" Width="20%" Wrap="False"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                            <ItemTemplate>
                                <%# Eval("Price") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton CssClass="btn btn-primary btn-xs" id="lnkDelete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' Runat="server"><i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("Delete")%></asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-primary btn-xs" id="lnkEdit" CommandName="Edit" CommandArgument='<%# Eval("ID") %>' Runat="server"><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:datagrid>

                <div class="actions">
                    <asp:LinkButton CssClass="btn btn-secondary" id="btnAddNewPackage" Runat="server" onclick="btnAddNewPackage_Click"/>
                </div>
            </div>
        </div>
    </asp:View>
    <asp:View ID="viewAddEditCreditsPackage" runat="server">
        <div class="panel clear-panel">
            <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Credit Package") %></h4></div>
            <div class="panel-body">
                <div class="form-horizontal medium-width">
                    <div class="form-group">
                        <label class="control-label col-sm-3">
                            <%= Lang.TransA("Name") %>:
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtName" runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3">
                            <%= Lang.TransA("Quantity") %>:
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtQuantity" runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3">
                            <%= Lang.TransA("Price") %>:
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtPrice" runat="server"/>
                        </div>
                    </div>
                    <div class="actions">
                        <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" onclick="btnCancel_Click" />
                        <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" onclick="btnSave_Click" />
                    </div>
			    </div>
            </div>
        </div>
    </asp:View>
    </asp:MultiView>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
