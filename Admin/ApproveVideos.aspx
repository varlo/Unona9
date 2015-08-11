<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveVideos.aspx.cs" Inherits="AspNetDating.Admin.ApproveVideos" %>
<%@ Register TagPrefix="uc1" TagName="VideoRecorder" Src="../VR/VideoRecorder.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DeleteButton" Src="DeleteVideoButton/DeleteButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
<table width="100%" cellpadding=0 cellspacing=0>
	<tr>
		<td align="right">
			<asp:Panel ID="pnlVideosPerPage" runat="server" CssClass="perpage">
				<%= Lang.TransA("Videos per page") %>:
				<asp:DropDownList id="dropVideosPerPage" CssClass="pages" Font-Name="Verdana" Font-Size="9px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropVideosPerPage_SelectedIndexChanged"></asp:DropDownList>
			</asp:Panel>
	        <div class="separator06"></div>
		</td>
	</tr>
</table>					
<asp:DataGrid id="dgPendingVideos" Runat="server" Width="100%" PageSize="2" AllowPaging="True" 
AutoGenerateColumns="False"	CssClass="filter_results" cellpadding="0" cellspacing="0"  BorderWidth="0" GridLines="None"
OnItemDataBound="gridPendingVideos_ItemDataBound" OnItemCommand="dgPendingVideos_ItemCommand" OnPageIndexChanged="dgPendingVideos_PageIndexChanged">
	<AlternatingItemStyle CssClass="alternative_item"></AlternatingItemStyle>
	<HeaderStyle></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<HeaderStyle CssClass="filter_results_header"></HeaderStyle>
			<ItemStyle   Wrap="False" Width="10%"></ItemStyle>
			<ItemTemplate>
				<a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>"><%# Eval("Username")%></a>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<HeaderStyle CssClass="filter_results_header" Wrap="False"></HeaderStyle>
			<ItemStyle  Width="90%"></ItemStyle>
			<ItemTemplate>
				<uc1:VideoRecorder id="VideoRecorder1" runat="server"></uc1:VideoRecorder>
				<div class="separator06"></div>
				<uc1:DeleteButton id="DeleteButton" runat="server"></uc1:DeleteButton>
				<div class="separator06"></div>
				<asp:Button ID="btnApprove" Runat="server"></asp:Button>
			</ItemTemplate>
		</asp:TemplateColumn>																					
	</Columns>
	<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>	
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
