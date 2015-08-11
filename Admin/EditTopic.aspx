<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditTopic.aspx.cs" Inherits="AspNetDating.Admin.EditTopic" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
    <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Topic Details") %></h4></div>
    <div class="panel-body">
        <div class="form-horizontal medium-width">
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Name") %></label>
                <div class="col-sm-8"><asp:TextBox CssClass="form-control" id="txtTopicTitle" Runat="server"/></div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Edit Columns") %></label>
                <div class="col-sm-8"><asp:dropdownlist CssClass="form-control" id="dropEditColumns" Runat="server"/></div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("View Columns") %></label>
                <div class="col-sm-8"><asp:dropdownlist CssClass="form-control" id="dropViewColumns" Runat="server"/></div>
            </div>
        </div>
	</div>
</div>
<div class="label"><%= Lang.TransA("Topic Questions") %></div>
<div class="table-responsive">
<asp:DataGrid CssClass="table table-striped" id="dgQuestions" Runat="server" PageSize="10" AllowPaging="False" AutoGenerateColumns="False" GridLines="None" OnItemCreated="dgQuestions_ItemCreated" OnItemCommand="dgQuestions_ItemCommand" OnItemDataBound="dgQuestions_ItemDataBound">
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
            <ItemStyle Width="20"></ItemStyle>
			<ItemTemplate>
				<input type=checkbox id="cbSelect" value='<%# Eval("QuestionID") %>' runat=server NAME="cbSelect">
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<%# Eval("Name")%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<%# (((string)Eval("Description")).Length > 30)?((string)Eval("Description")).Substring(0,30)+"...":Eval("Description")%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<%# ((bool)Eval("Required"))?Lang.TransA("Yes"):Lang.TransA("No")%>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
                <asp:LinkButton ID="lnkUp" CommandName="ChangeOrder" CommandArgument="Up" runat="server"><i class="fa fa-caret-up"></i></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton ID="lnkDown" CommandName="ChangeOrder" CommandArgument="Down" runat="server"><i class="fa fa-caret-down"></i></asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
			<ItemTemplate>
				<asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkEdit" CommandName="EditQuestion" Runat="server"><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%></asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
</div>
<div class="text-right">
	<asp:LinkButton CssClass="btn btn-default btn-sm pull-left" id="btnDeleteSelectedQuestions" runat="server" onclick="btnDeleteSelectedQuestions_Click"/>
	<asp:LinkButton CssClass="btn btn-secondary btn-sm" id="btnAddNewQuestion" runat="server" onclick="btnAddNewQuestion_Click"/>
</div>
<div class="actions">
    <hr />
	<asp:Button CssClass="btn btn-default" id="btnCancel" runat="server" onclick="btnCancel_Click"/>
	<asp:Button CssClass="btn btn-primary" id="btnSave" runat="server" onclick="btnSave_Click"/>
</div>
</asp:Content>
