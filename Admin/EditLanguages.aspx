<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditLanguages.aspx.cs" Inherits="AspNetDating.Admin.EditLanguages" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanelSearchLanguages" runat="server">
	<ContentTemplate>
        <uc1:messagebox id="MessageBox" runat="server"/>
	    <div id="pnlLanguage" runat="server" visible="false">
            <div class="panel clear-panel">
                <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Language details") %></h4></div>
                <div class="panel-body">
                    <div class="form-horizontal medium-width">
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= Lang.TransA("Language name") %></label>
                            <div class="col-sm-9"><asp:TextBox ID="txtLanguageName" CssClass="form-control" runat="server"/></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= Lang.TransA("Active") %></label>
                            <div class="col-sm-9"><div class="checkbox"><label><asp:CheckBox ID="cbActive" runat="server" /></label></div></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= Lang.TransA("Browser Languages") %></label>
                            <div class="col-sm-9"><asp:TextBox ID="txtBrowserLanguages" CssClass="form-control" runat="server"/></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= Lang.TransA("IP Countries") %></label>
                            <div class="col-sm-9"><asp:TextBox ID="txtIpCountries" CssClass="form-control" runat="server"/></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= Lang.TransA("Theme") %></label>
                            <div class="col-sm-9"><asp:TextBox ID="txtTheme" CssClass="form-control" runat="server"/></div>
                        </div>
                        <div class="actions">
                            <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
                            <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" OnClick="btnSave_Click" />
                        </div>
	                </div>
	            </div>
	        </div>
	    </div>

	    <div id="pnlLanguages" runat="server">
	        <div class="table-responsive">
	        <asp:DataGrid CssClass="table table-striped" ID="dgLanguages" runat="server" AutoGenerateColumns="false" OnItemCommand="dgLanguages_ItemCommand" OnItemCreated="dgLanguages_ItemCreated" OnItemDataBound="dgLanguages_ItemDataBound" GridLines="None">
                <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
	            <Columns>
	                <asp:TemplateColumn>
	                    <ItemTemplate>
	                        <%# Eval("LanguageName") %>
	                    </ItemTemplate>
	                </asp:TemplateColumn>
	                <asp:TemplateColumn>
	                    <ItemTemplate>
	                        <%# (bool) Eval("IsActive") ? Lang.TransA("Yes") : Lang.TransA("No") %>
	                    </ItemTemplate>
	                </asp:TemplateColumn>
	                <asp:TemplateColumn>
	                    <ItemTemplate>
	                        <asp:LinkButton ID="lnkUp" CommandName="MoveUp" CommandArgument='<%# Eval("LanguageID") %>' runat="server"><i class="fa fa-caret-up"></i></asp:LinkButton>&nbsp;&nbsp;
	                        <asp:LinkButton ID="lnkDown" CommandName="MoveDown" CommandArgument='<%# Eval("LanguageID") %>' runat="server"><i class="fa fa-caret-down"></i></asp:LinkButton>
	                    </ItemTemplate>
	                </asp:TemplateColumn>
	                <asp:TemplateColumn>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
	                    <ItemTemplate>
	                        <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkEdit" runat="server" CommandArgument='<%# Eval("LanguageID") %>' CommandName="EditLanguage">
								<i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%>
	                        </asp:LinkButton>
	                        <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDelete" runat="server" CommandArgument='<%# Eval("LanguageID") %>' CommandName="DeleteLanguage" Visible='<%# ! (bool) Eval("Predefined") %>'>
								<i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("Delete")%>
	                        </asp:LinkButton>
	                    </ItemTemplate>
	                </asp:TemplateColumn>
	            </Columns>
	        </asp:DataGrid>
            </div>
	        <div class="actions"><asp:LinkButton CssClass="btn btn-secondary" ID="btnAdd" runat="server" OnClick="btnAdd_Click" /></div>
	    </div>
	</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
