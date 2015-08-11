<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ManageGroupCategories.aspx.cs" Inherits="AspNetDating.Admin.ManageGroupCategories" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:MultiView ID="mvCategories" runat="server">
    <asp:View ID="viewCategories" runat="server">
       <div class="table-responsive">
        <asp:DataGrid ID="dgCategories" AutoGenerateColumns="False" AllowPaging="False" PageSize="10" CssClass="table table-striped" GridLines="None" runat="server" OnItemCommand="dgCategories_ItemCommand" OnItemCreated="dgCategories_ItemCreated" OnItemDataBound="dgCategories_ItemDataBound">
            <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <input type="checkbox" id="cbSelect" value='<%# Eval("CategoryID") %>' runat="server" />
                    </ItemTemplate>
				</asp:TemplateColumn>
			<asp:TemplateColumn>
                    <ItemTemplate>
                        <%# Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle Wrap="False"></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkUp" CommandName="ChangeOrder" CommandArgument="Up" runat="server"><i class="fa fa-caret-up"></i></asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkDown" CommandName="ChangeOrder" CommandArgument="Down" runat="server"><i class="fa fa-caret-down"></i></asp:LinkButton>
                    </ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    <ItemTemplate>
						<asp:LinkButton CssClass="btn btn-default btn-xs" ID="lnkViewGroups" CommandArgument='<%# Eval("CategoryID") %>' CommandName="ViewGroups" runat="server"><i class="fa fa-eye"></i>&nbsp;<%# Lang.TransA("Groups")%></asp:LinkButton>
						<asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkEdit" CommandName="EditCategory" runat="server"><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
		</asp:DataGrid>
		</div>
        <div class="actions">
            <asp:LinkButton CssClass="btn btn-primary" ID="btnDeleteSelectedCategories" runat="server" OnClick="btnDeleteSelectedCategories_Click"/>
            <asp:LinkButton CssClass="btn btn-secondary" ID="btnAddNewCategory" runat="server" OnClick="btnAddNewCategory_Click"/>
        </div>
	</asp:View>
    <asp:View ID="viewCategory" runat="server">
        <div class="panel clear-panel">
            <div class="panel-heading"><h4 class="panel-title"><asp:Label ID="lblText" runat="server"/></h4></div>
            <div class="panel-body medium-width">
                <div class="form-group">
                    <label><%= Lang.TransA("Name") %></label>
                    <asp:TextBox ID="txtName" CssClass="form-control" runat="server"/>
                </div>
                <div class="form-group">
                    <div class="checkbox"><label><asp:CheckBox ID="cbUsersCanCreateGroups" runat="server" /><%= Lang.TransA("Users can create groups in this category") %></label></div>
                </div>
                <div class="actions">
                    <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
                    <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
	</asp:View>
</asp:MultiView>
</asp:Content>
