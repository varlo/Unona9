<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BrowseAdmins.aspx.cs" Inherits="AspNetDating.Admin.BrowseAdmins" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:DataGrid CssClass="table table-striped" id="gridAdmins" Runat="server" AllowPaging="False" AutoGenerateColumns="False" OnItemCommand="gridAdmins_ItemCommand" OnItemDataBound="gridAdmins_ItemDataBound" GridLines="None" >
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<ItemTemplate>
				<asp:Literal Text='<%# Eval("Username")%>' ID="litUsername" Runat="server"/>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<%# Eval("LastLogin") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
			<ItemTemplate>
                <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkEdit" CommandName="Edit" CommandArgument='<%# Eval("Username")%>' Runat="server"><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit") %></asp:LinkButton>
                <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDelete" CommandName="Delete" CommandArgument='<%# Eval("Username")%>' Runat="server"><i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("Delete") %></asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
<div class="actions">
	<asp:LinkButton CssClass="btn btn-secondary" ID="btnAddAdmin" Runat="server" onclick="AddAdmin_Click"/>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
