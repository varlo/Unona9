<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="SpamCheck.aspx.cs" Inherits="AspNetDating.Admin.SpamCheck" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc1:MessageBox id="MessageBox" runat="server"/>
<p class="text-right">
    <small class="text-muted"><asp:Label id="lblMessagesPerPage"  runat="server"/></small>
	<asp:DropDownList id="dropMessagesPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropMessagesPerPage_SelectedIndexChanged"/>
</p>
<div class="table-responsive">
<asp:DataGrid id="dgPendingMessages" CssClass="table table-striped" Runat="server" PageSize="2" AllowPaging="True" AutoGenerateColumns="False" GridLines="None" OnItemCommand="dgPendingMessages_ItemCommand" OnPageIndexChanged="dgPendingMessages_PageIndexChanged" OnItemDataBound="dgPendingMessages_ItemDataBound" OnItemCreated="dgPendingMessages_ItemCreated">
	<HeaderStyle Font-Bold="True"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
            <ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
			<a target="_blank" href="<%= AspNetDating.Classes.Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("FromUsername")%>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("FromUsername")%></a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
			<a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("ToUsername")%>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("ToUsername")%></a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemStyle Wrap="True"></ItemStyle>
			<ItemTemplate>
				<%# Eval("Message")%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
			<ItemTemplate>
				<asp:LinkButton CssClass="btn btn-secondary btn-sm" ID="lnkApprove" CommandName="Approve" CommandArgument='<%# Eval("MessageID")%>' Runat=server><i class="fa fa-check"></i>&nbsp;<%= Lang.TransA("Approve")%></asp:LinkButton>
				<asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkReject" CommandName="Reject" CommandArgument='<%# Eval("MessageID")%>'  Runat=server><i class="fa fa-times"></i>&nbsp;<%= Lang.TransA("Reject")%></asp:LinkButton>
				<asp:LinkButton CssClass="btn btn-primary btn-sm" ID="lnkDeleteUser" CommandName="DeleteUser" CommandArgument='<%# Eval("FromUsername")%>'  Runat=server><i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("Delete User")%></asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>																						
	</Columns>
	<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
