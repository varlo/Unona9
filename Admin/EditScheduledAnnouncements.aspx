<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditScheduledAnnouncements.aspx.cs" Inherits="AspNetDating.Admin.EditScheduledAnnouncements" %>
<%@ Import Namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register Src="~/Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc1" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div id="pnlAnnouncements" runat="server">
    <div class="table-responsive">
	<asp:DataGrid CssClass="table table-striped" ID="dgAnnouncements" GridLines="None" runat="server" AllowPaging="False" onitemcommand="dgAnnouncements_ItemCommand" OnItemCreated="dgAnnouncements_ItemCreated">
        <HeaderStyle Font-Bold="True"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <%# Eval("Name") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemStyle Wrap="True"></ItemStyle>
                <ItemTemplate>
                        <%# Eval("Type")%>
                </ItemTemplate>
            </asp:TemplateColumn>                            
            <asp:TemplateColumn>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                    <div class="btn-group btn-group-xs">
                        <asp:LinkButton CssClass="btn btn-primary" id="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-edit"></i>&nbsp;<%= Lang.TransA("Edit")%></asp:LinkButton>
                        <asp:LinkButton CssClass="btn btn-primary" ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-times"></i>&nbsp;<%= Lang.TransA("Delete")%></asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    </div>
    <div class="add-buttons">
        <asp:Button ID="btnAddNew" runat="server" onclick="btnAddNew_Click" />
    </div>
</div>
<div id="pnlAnnouncement" runat="server" visible="false">
    <div class="panel panel-filter">
        <div class="panel-heading">
            <h4 class="panel-title">
                <i class="fa fa-filter"></i>&nbsp;<%= Lang.TransA("Filter") %>
                <span class="pull-right"><a data-toggle="collapse" data-parent=".panel-filter" href="#collapseFilter" title="<%= Lang.TransA("Expand/Collapse Filter") %>"><i class="fa fa-expand"></i></a></span>
            </h4>
        </div>
        <div id="collapseFilter" class="panel-collapse collapse in">
            <div class="panel-body">
                <div class="form-horizontal form-sm">
                    <div class="form-group" id="pnlGender" runat="server">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Gender") %>:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList CssClass="form-control" ID="ddGender" Runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Paid Member") %>:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList CssClass="form-control" ID="ddPaid" Runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Has photos") %>:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList CssClass="form-control" ID="ddHasPhotos" Runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Has profile") %>:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList CssClass="form-control" ID="ddHasProfile" Runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Language") %>:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList CssClass="form-control" ID="ddLanguage" Runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Country") %>:</label>
                        <div class="col-sm-8">
                            <select ID="ddCountry" class="form-control" enableviewstate="false" Runat="server"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Region") %>:</label>
                        <div class="col-sm-8">
                            <select ID="ddRegion" class="form-control" Runat="server"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Type") %>:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList CssClass="form-control" ID="ddType" runat="server" AutoPostBack="true" onselectedindexchanged="ddType_SelectedIndexChanged"/>
                        </div>
                    </div>
                    <div class="form-group" id="pnlName" runat="server" visible="false">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Name") %>:</label>
                        <div class="col-sm-8">
                            <asp:TextBox CssClass="form-control" ID="txtName" runat="server"/>
                        </div>
                    </div>
                    <div class="form-group" id="pnlDate" runat="server" visible="false">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Date") %>:</label>
                        <div class="col-sm-8">
                            <uc1:DatePicker ID="datePicker1" CssClass="datepicker" runat="server"/>
                        </div>
                    </div>
                    <div class="form-group" id="pnlInactivity" runat="server" visible="false">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Inactivity") %>:</label>
                        <div class="col-sm-8">
                            <asp:TextBox CssClass="form-control" ID="txtInactivity" runat="server" MaxLength="4" Columns="4"/>&nbsp;<%= Lang.TransA("days") %>
                        </div>
                    </div>
                    <div class="form-group" id="pnlAfterRegistration" runat="server" visible="false">
                        <label class="control-label col-sm-4"><%= Lang.TransA("After Registration") %>:</label>
                        <div class="col-sm-8">
                            <asp:TextBox CssClass="form-control" ID="txtAfterRegistration" runat="server" MaxLength="4" Columns="4"/>&nbsp;<%= Lang.TransA("days") %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
	</div>
	<div class="panel clear-panel">
		<div class="panel-heading">
			<h4 class="panel-title"><%= Lang.TransA("Submit Announcement") %></h4>
		</div>
		<div class="panel-body medium-width">
            <div class="form-group">
                <label><%= Lang.TransA("Subject") %></label>
                <asp:textbox CssClass="form-control" id="txtSubject" Runat="server"/>
            </div>
            <div class="form-group">
                <asp:PlaceHolder ID="phEditor" runat="server"/>
                <p class="help-block"><i class="fa fa-lightbulb-o"></i>&nbsp;<%= Lang.TransA("Use %%NAME%% or %%USER%% to specify the user's name or username") %></p>
            </div>
            <div class="input-group">
                <span class="input-group-addon"><%= "Send test e-mail to username".TranslateA() %></span>
                <asp:TextBox CssClass="form-control" ID="txtTestUsername" runat="server"/>
                <div class="input-group-btn"><asp:Button CssClass="btn btn-primary" ID="btnSendTestEmail" runat="server" onclick="btnSendTestEmail_Click" /></div>
            </div>
		</div>
		<div class="panel-footer">
            <div class="text-center medium-width">
                <asp:button CssClass="btn btn-secondary" id="btnSave" runat="server" onclick="btnSave_Click" Visible="false"/>
                <asp:button CssClass="btn btn-primary" id="btnSend" runat="server" onclick="btnSend_Click"/>
            </div>
        </div>
	</div>
</div>
</asp:Content>
