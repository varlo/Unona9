<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditAdsCategories.aspx.cs" Inherits="AspNetDating.Admin.EditAdsCategories" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="table-responsive">
<asp:DataGrid ID="dgAdsCategories" AutoGenerateColumns="False" AllowPaging="False" PageSize="10" CssClass="table table-striped" runat="server" GridLines="None" onitemcommand="dgAdsCategories_ItemCommand">
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
    <Columns>
        <asp:TemplateColumn>
            <ItemStyle Wrap="False" Width="2%"></ItemStyle>
            <ItemTemplate>
                <input type="checkbox" id="cbSelect" value='<%# Eval("ID") %>' runat="server" />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <%# Eval("Title")%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton ID="lnkEdit" CssClass="btn btn-primary btn-sm" CommandName="EditCategory" runat="server"><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
</div>
<div class="actions">
	<asp:LinkButton CssClass="btn btn-secondary" ID="btnAddNewCategory" runat="server" onclick="btnAddNewCategory_Click"/>
    <asp:LinkButton CssClass="btn btn-primary" ID="btnDeleteSelectedCategories" runat="server" onclick="btnDeleteSelectedCategories_Click"/>
</div>
</asp:Content>
