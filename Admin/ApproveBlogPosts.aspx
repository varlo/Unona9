<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveBlogPosts.aspx.cs" Inherits="AspNetDating.Admin.ApproveBlogPosts" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <uc1:messagebox id="MessageBox" runat="server"/>
    <p class="text-right">
        <small class="text-muted"><asp:Label id="lblBlogPostsPerPage"  runat="server"/></small>
		<asp:DropDownList id="dropBlogPostsPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" onselectedindexchanged="dropBlogPostsPerPage_SelectedIndexChanged"/>
    </p>
    <div class="table-responsive">
    <asp:DataGrid id="dgPendingApproval" CssClass="table table-striped" Runat="server" PageSize="2" AllowPaging="True" AutoGenerateColumns="False" GridLines="None" onitemcommand="dgPendingApproval_ItemCommand" onitemcreated="dgPendingApproval_ItemCreated" onitemdatabound="dgPendingApproval_ItemDataBound" onpageindexchanged="dgPendingApproval_PageIndexChanged">
	<HeaderStyle Font-Bold="True"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
			    <a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("Username")%></a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
		    <ItemStyle Wrap="True"></ItemStyle>
			<ItemTemplate>
			<%# Eval("Title")%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
		    <ItemStyle Wrap="True"></ItemStyle>
			<ItemTemplate>
				<%# Eval("Content")%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemStyle HorizontalAlign="Right" Wrap="False" Width="300px"></ItemStyle>
			<ItemTemplate>
                <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkReject" CommandName="Reject" CommandArgument='<%# Eval("Username") + ":" + Eval("ID") %>'  Runat="server"><i class="fa fa-times"></i>&nbsp;<%= Lang.TransA("Reject")%></asp:LinkButton>
                <asp:LinkButton CssClass="btn btn-secondary btn-sm" ID="lnkApprove" CommandName="Approve" CommandArgument='<%# Eval("Username") + ":" + Eval("ID") %>' Runat="server"><i class="fa fa-check"></i>&nbsp;<%= Lang.TransA("Approve")%></asp:LinkButton>
                <a class="btn btn-primary btn-sm" href="ApproveBlogPost.aspx?uid=<%# Eval("Username") %>&bpid=<%# Eval("ID")%>"><i class="fa fa-pencil-square-o"></i>&nbsp;<%# Lang.TransA("Edit")%></a>
			</ItemTemplate>
		</asp:TemplateColumn>																						
	</Columns>
	<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
