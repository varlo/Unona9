﻿<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Page language="c#" Codebehind="BillingConfig.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.BillingConfig" %>

<%@ Register Src="MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Billing Settings") %></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"/>
			<div id="layout">
				<div id="crupms">
		            <img src="images/i_billingsettings.jpg"> 
		            <div id="crumpstxt"><%= Lang.TransA("Billing Settings") %></div>
	            </div>
				<div id="left">
		            <uc1:AdminMenu id="AdminMenu1" runat="server"/>
                </div>
				<div id="right">
					<div class="content_header">
	                    <h2><%= Lang.TransA("Billing Settings") %></h2>
	                    <%= Lang.TransA("Use this section to choose and configure your billing plans...") %>
	                </div>
					<%--<table width="100%">
						<tr>
							<th colSpan="2"><asp:label id="lblBillingConfiguration" Runat="server"></asp:label></th>
						</tr>
						<tr>
							<td><asp:label id="lblPaymentProcessor" Runat="server"></asp:label></td>
							<td><asp:dropdownlist id="ddPaymentProcessors" Runat="server" AutoPostBack="True" onselectedindexchanged="System"></asp:dropdownlist></td>
						</tr>
						<tr>
							<td><asp:label id="lblProcessorUserID" Runat="server"></asp:label></td>
							<td><asp:textbox id="txtProcessorUserID" Runat="server"></asp:textbox></td>
						</tr>
						<tr>
							<td align="center" colSpan="2"><INPUT id="hidCurrentProcessorID" type="hidden" runat="server"><asp:button id="btnSaveConfiguration" Runat="server" onclick="btnSaveConfiguration_Click"></asp:button></td>
						</tr>
					</table> --%>
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
					<table cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr>
							<td>
								<div class="label">
									<%= Lang.TransA("Billing Plans") %>
								</div>
								<div class="separator06"></div>
							</td>
						</tr>
						<tr>
							<td><asp:datagrid id="dgBillingPlans" CssClass="filter_results" Runat="server" AutoGenerateColumns="False" Width="100%" cellpadding="0" cellspacing="0"  BorderWidth="0" GridLines="None">
									<AlternatingItemStyle CssClass="alternative_item"></AlternatingItemStyle>
									<HeaderStyle></HeaderStyle>
									<Columns>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="filter_results_header"  Width="10%" Wrap="False"></HeaderStyle>
											<ItemStyle></ItemStyle>
											<ItemTemplate>
												<%# Eval("Title") %>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="filter_results_header" Width="10%" Wrap="False"></HeaderStyle>
											<ItemStyle></ItemStyle>
											<ItemTemplate>
												<%# ((string)Eval("Amount")) == String.Empty?Lang.TransA("N/A"):(Convert.ToDecimal(Eval("Amount"))).ToString("c") %>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="filter_results_header" Width="30%" Wrap="False"></HeaderStyle>
											<ItemStyle></ItemStyle>
											<ItemTemplate>
												<%# ((string)Eval("Cycle"))== String.Empty?Lang.TransA("N/A"):Eval("Cycle") %>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="filter_results_header" Width="20%" Wrap="False"></HeaderStyle>
											<ItemStyle></ItemStyle>
											<ItemTemplate>
												<%# ((string)Eval("CycleUnit")) == String.Empty ? Lang.TransA("N/A") : Eval("CycleUnit")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="filter_results_header" Width="20%" Wrap="False"></HeaderStyle>
											<ItemStyle></ItemStyle>
											<ItemTemplate>
												<asp:LinkButton id="lnkDelete" CommandName="DeletePlan" CommandArgument='<%# Eval("PlanID") %>' Visible='<%# Convert.ToInt32(Eval("PlanID")) != -1 %>' Runat="server">
													<%# Lang.TransA("Delete")%></asp:LinkButton>&nbsp;
												<asp:LinkButton id="lnkEdit" CommandName="EditPlan" CommandArgument='<%# Eval("PlanID") %>' Runat="server">
													<%# Lang.TransA("Edit")%></asp:LinkButton>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:datagrid>
							</td>
						</tr>
						<tr>
							<td>
								<div class="add-buttons">
									<asp:button id="btnAddNewPlan" Runat="server" onclick="btnAddNewPlan_Click"></asp:button>
								</div>
							</td>
						</tr>
					</table>
					<div class="separator10"></div>
					<asp:panel id="pnlBillingPlanInfo" Runat="server">
						<TABLE id="tblBillingPlan" runat="server" cellpadding="0" cellspacing="0" class="small_box_wrap">
							<TR>
								<th colSpan="2">
									<asp:Label id="lblBillingPlanInfo" Runat="server"></asp:Label>
								</th>								
							</TR>
							<TR>
								<td>
									<asp:Label id="lblTitle" Runat="server">
									</asp:Label>
								</TD>
								
								<td>
									<asp:TextBox id="txtTitle" Runat="server">
									</asp:TextBox>
								</TD>
								
							</TR>
							<TR>
								<td>
									<asp:Label id="lblAmount" Runat="server">
									</asp:Label>
								</TD>
								
								<td>
									<asp:TextBox id="txtAmount" Runat="server">
									</asp:TextBox>
								</TD>
								
							</TR>
							<TR>
								<td>
									<asp:Label id="lblCycle" Runat="server">
									</asp:Label>
								</TD>
								<td>
									<asp:TextBox id="txtCycle" Runat="server">
									</asp:TextBox>
								</TD>
								
							</TR>
							<TR>
								<td>
									<asp:Label id="lblCycleUnit" Runat="server">
									</asp:Label>
								</TD>
								
								<td>
									<asp:DropDownList id="ddCycleUnit" Runat="server">
									</asp:DropDownList></TD>
							</TR>
						</TABLE>
						<TABLE cellpadding="0" cellspacing="0" class="small_box_wrap">
						<tr>
						    <td>
						        <asp:PlaceHolder ID="phBillingPlanOptions" runat="server"></asp:PlaceHolder>
						    </td>
						</tr>						
						<TR>
							<td>
								<div class="separator10"></div>
								<div class="add-buttons">
									<asp:Button id="btnCreateUpdate" Runat="server" onclick="btnCreateUpdate_Click"></asp:Button>
									<asp:Button id="btnCancel" runat="server" onclick="btnCancel_Click"></asp:Button>
								</div>
							</TD>
						</TR>
						</TABLE>						
                        <uc2:MessageBox ID="MessageBox1" runat="server" />
						<div class="separator10"></div>
						<input id="hidCurrentPlanID" type="hidden" runat="server" />
					</asp:panel>	
				</ContentTemplate>
				</asp:UpdatePanel>
            	</div>
			</div>
		</form>
	</body>
</HTML>
