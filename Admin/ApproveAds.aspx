<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveAds.aspx.cs" Inherits="AspNetDating.Admin.ApproveAds" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanelPendingApproval" runat="server">
    <ContentTemplate>
		<uc1:MessageBox ID="MessageBox" runat="server" />
		<table width="100%" cellpadding="0" cellspacing="0">
			<tr>
				<td align="right" class="perpage">
				<asp:Label ID="lblGroupsPerPage"  runat="server">
				</asp:Label>
				<asp:DropDownList ID="ddAdsPerPage" OnSelectedIndexChanged="ddAdsPerPage_SelectedIndexChanged" CssClass="pages" runat="server" AutoPostBack="True">
				</asp:DropDownList>
				<div class="separator06">
				</div>
				</td>
			</tr>
		</table>
		<div class="table-responsive">
		<asp:DataGrid ID="dgPendingApproval" runat="server" CssClass="table table-striped" PageSize="2" AllowPaging="True" AutoGenerateColumns="False" GridLines="None" onitemcommand="dgPendingApproval_ItemCommand" onitemcreated="dgPendingApproval_ItemCreated" onitemdatabound="dgPendingApproval_ItemDataBound" onpageindexchanged="dgPendingApproval_PageIndexChanged">
            <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
			<Columns>
				<asp:templatecolumn>
					<ItemTemplate>
						<img class="img-thumbnail" src='../AdPhoto.ashx?id=<%# Eval("AdPhotoID") %>&amp;width=120&amp;height=120&amp;diskCache=1' />
					</ItemTemplate>
				</asp:templatecolumn>
				<asp:templatecolumn>
					<ItemTemplate>
						<div class="form-group"><b><%= Lang.TransA("Category") %>:</b>&nbsp;<%# Eval("CategoryTitle")%></div>
						<div class="form-group"><b><%= Lang.TransA("Posted by") %>:</b>&nbsp;<%# Eval("PostedBy")%></div>
						<div class="form-group"><b><%= Lang.TransA("Subject") %>:</b>&nbsp;<%# Eval("Subject")%></div>
						<div class="form-group"><b><%= Lang.TransA("Description") %>:</b>&nbsp;<%# Eval("Description")%></div>
						<div class="form-group"><b><%= Lang.TransA("Approved") %>:</b>&nbsp;<%# Eval("Approved")%></div>
						<div class="form-group"><b><%= Lang.TransA("Created on") %>:</b>&nbsp;<%# Eval("DateCreated")%></div>
					</ItemTemplate>
				</asp:templatecolumn>
				<asp:templatecolumn>
                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
					<ItemTemplate>
                        <asp:LinkButton CssClass="btn btn-secondary btn-xs" ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("ID")%>' />
                        <asp:LinkButton CssClass="btn btn-default btn-xs" ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("ID")%>' />
						<a class="btn btn-primary btn-xs" href='EditAd.aspx?id=<%# Eval("ID")%>&amp;cid=<%# Eval("CategoryID") %>'><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%></a>
						<hr />
						<div class="input-group">
						    <span class="input-group-addon"><%= Lang.TransA("Reason") %>:</span>
						    <asp:TextBox CssClass="form-control" ID="txtReason" runat="server"/>
						</div>
					</ItemTemplate>
				</asp:templatecolumn>
			</Columns>
			<PagerStyle HorizontalAlign="Center" Mode="NumericPages" />
		</asp:DataGrid>
	</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
