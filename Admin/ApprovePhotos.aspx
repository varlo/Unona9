<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApprovePhotos.aspx.cs" Inherits="AspNetDating.Admin.ApprovePhotos" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
    <asp:CheckBox ID="cbShowManualApprovalPhotos" runat="server" Visible="false" AutoPostBack="true" 
        oncheckedchanged="cbShowManualApprovalPhotos_CheckedChanged"/>
    <table width="100%" border="0" cellpadding=0 cellspacing=0>
	<tr>
		<td align="right" class="perpage">
			<asp:Panel ID="pnlPhotosPerPage"  runat="server">
				<asp:Label id="lblPhotosPerPage"  runat="server"></asp:Label>:
				<asp:DropDownList id="dropPhotosPerPage" CssClass="pages" Font-Name="Verdana" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropPhotosPerPage_SelectedIndexChanged"></asp:DropDownList>
			</asp:Panel>
			<div class="separator06"></div>
		</td>
	</tr>
	<tr>
		<td>
			<asp:DataGrid id="gridPendingApproval" Runat="server" PageSize="20" AllowPaging="True" AutoGenerateColumns="False"
			Width="100%" CssClass="filter_results" cellpadding="0" cellspacing="0"  BorderWidth="0" GridLines="None"
            OnPageIndexChanged="gridPendingApproval_PageIndexChanged">
				<AlternatingItemStyle CssClass="alternative_item"></AlternatingItemStyle>
				<HeaderStyle></HeaderStyle>
				<Columns>
					<asp:TemplateColumn>
							<HeaderStyle CssClass="filter_results_header"></HeaderStyle>
							<ItemStyle   Wrap="False"></ItemStyle>
							<ItemTemplate>
							<a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>"><%# Eval("Username")%></a>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn>
							<HeaderStyle CssClass="filter_results_header"></HeaderStyle>
							<ItemStyle   Wrap="false"></ItemStyle>
							<ItemTemplate>
							<%# Eval("Name")%>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn>
							<HeaderStyle CssClass="filter_results_header"></HeaderStyle>
							<ItemStyle   Wrap="false"></ItemStyle>
							<ItemTemplate>
							<%# Eval("Description")%>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn>
							<HeaderStyle CssClass="filter_results_header"></HeaderStyle>
							<ItemStyle  VerticalAlign="Middle" Wrap=False></ItemStyle>
							<ItemTemplate>
							<a href="ApprovePhoto.aspx?pid=<%# Eval("PhotoID")%>">
								<img border=0 src="<%= Config.Urls.Home%>/Image.ashx?id=<%# Eval("PhotoID")%>&width=100&height=100">
							</a></ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
					<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
				</asp:DataGrid>								
			</td>
		</tr>
	</table>
	</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
