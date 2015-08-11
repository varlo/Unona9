<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveAnswers.aspx.cs" Inherits="AspNetDating.Admin.ApproveAnswers" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <uc1:messagebox id="MessageBox" runat="server"/>
    <p class="text-right">
        <small class="text-muted"><asp:Label id="lblAnswersPerPage" runat="server"/></small>
        <asp:DropDownList id="dropAnswersPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropAnswersPerPage_SelectedIndexChanged"/>
	</p>
	<div class="table-responsive">
	<asp:DataGrid CssClass="table table-striped" id="dgPendingApproval" Runat="server" PageSize="2" AllowPaging="True" AutoGenerateColumns="False" GridLines="None" OnItemCommand="dgPendingApproval_ItemCommand" OnItemCreated="dgPendingApproval_ItemCreated" OnPageIndexChanged="dgPendingApproval_PageIndexChanged" OnItemDataBound="dgPendingApproval_ItemDataBound">
        <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
		<Columns>
			<asp:TemplateColumn>
				<ItemTemplate>
				    <a target="_blank" href="<%= AspNetDating.Classes.Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("Username")%></a>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemTemplate>
				<%# Eval("Question")%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemTemplate>
					<%# Eval("Answer")%>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
				<ItemTemplate>
					<asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkApprove" CommandName="Approve" CommandArgument='<%# Eval("Username") + ":" + Eval("QuestionID")%>' Runat="server"><i class="fa fa-check"></i>&nbsp;<%# Lang.TransA("Approve")%></asp:LinkButton>
					<asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkReject" CommandName="Reject" CommandArgument='<%# Eval("Username") + ":" + Eval("QuestionID")%>'  Runat="server"><i class="fa fa-times"></i>&nbsp;<%# Lang.TransA("Reject")%></asp:LinkButton>
					<a class="btn btn-primary btn-xs" href="ApproveAnswer.aspx?uid=<%# Eval("Username")%>&qid=<%# Eval("QuestionID")%>"><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%></a>
				</ItemTemplate>
			</asp:TemplateColumn>																						
		</Columns>
		<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
	</asp:DataGrid>
	</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
