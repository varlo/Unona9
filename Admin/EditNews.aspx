<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditNews.aspx.cs" Inherits="AspNetDating.Admin.EditNews" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div id="pnlLanguage" runat="server">
    <table cellspacing="0" cellpadding="0" class="filter">
        <tr>
			<th colspan="2"></th>
		</tr>
		<tr>
            <td>

            </td>
            <td>
                </td>
        </tr>
    </table>
</div>
<uc1:messagebox id="MessageBox" runat="server"/>
<div class="panel panel-filter">
    <div class="panel-heading">
        <h4 class="panel-title">
            <i class="fa fa-filter"></i>&nbsp;<%= Lang.TransA("Languages") %>
        </h4>
    </div>
    <div class="panel-body">
        <div class="form-horizontal form-sm">
            <div class="form-group">
                <label class="col-sm-4 control-label"><%= Lang.TransA("Select Language:") %></label>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddLanguage" CssClass="form-control" AutoPostBack="True" runat="server" onselectedindexchanged="ddLanguage_SelectedIndexChanged"/>
                </div>
            </div>
        </div>
    </div>
</div>

<div id="divNews" runat="server" visible="false">
    <div class="table-responsive">
    <asp:DataGrid ID="dgNews" CssClass="table table-striped" PageSize="10" AllowPaging="False" AutoGenerateColumns="False" runat="server" GridLines="None" onitemcommand="dgNews_ItemCommand" onitemdatabound="dgNews_ItemDataBound">
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <span><%# Eval("Title")%></span>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <span><%# ((DateTime)Eval("Date")).ToShortDateString() %></span>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton CssClass="btn btn-primary btn-xs" id="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("NID") %>'><i class="fa fa-edit"></i>&nbsp;<%= Lang.TransA("Edit")%></asp:LinkButton>
                    <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("NID") %>'><i class="fa fa-trash-o"></i>&nbsp;<%= Lang.TransA("Delete")%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
    </asp:DataGrid>
    </div>
	<div class="actions">
	    <asp:LinkButton CssClass="btn btn-secondary" ID="btnAddNews" runat="server" onclick="btnAddNews_Click"/>
	</div>	

    <div id="pnlAddEditNews" visible="false" runat="server">
        <hr />
        <div class="form-group">
            <label><%= "Publish date".TranslateA() %></label>
            <uc2:DatePicker id="Calendar" DateRestriction="false" runat="server"/>
        </div>
        <div class="form-group">
            <label><%= "News title".TranslateA() %></label>
            <asp:TextBox CssClass="form-control" ID="txtNewsTitle" runat="server" Columns="61" />
        </div>
        <div class="form-group">
            <label><%= "News content".TranslateA() %></label>
            <asp:PlaceHolder ID="phEditor" runat="server"/>
        </div>
        <hr />
        <div class="actions">
            <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" onclick="btnCancel_Click" />
            <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" onclick="btnSave_Click"/>
        </div>
    </div>					
</div>
</asp:Content>
