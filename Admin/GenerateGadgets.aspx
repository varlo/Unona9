<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="GenerateGadgets.aspx.cs" Inherits="AspNetDating.Admin.GenerateGadgets" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<table cellSpacing="0" cellPadding="0" class="small_box_wrap">
	<tr>
		<th colSpan="2"><%= Lang.TransA("Admin Stats Gadget") %></th>
	</tr>
	<tr>
		<td align="center" colSpan="2"><asp:Button id="btnGenerateAdminStatsGadget" Runat="server" OnClick="btnGenerateAdminStatsGadget_Click"></asp:Button></td>
	</tr>
</table>
<div class="separator10"></div>
</asp:Content>
