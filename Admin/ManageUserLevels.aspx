<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ManageUserLevels.aspx.cs" Inherits="AspNetDating.Admin.ManageUserLevels" %>
<%@ Register Src="MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <div class="table-responsive">
    <asp:DataGrid id="dgUserLevels" CssClass="table table-striped" Runat="server" AutoGenerateColumns="False" GridLines="None" onitemcommand="dgUserLevels_ItemCommand" onitemdatabound="dgUserLevels_ItemDataBound">
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
    <Columns>
        <asp:TemplateColumn>
            <ItemTemplate>
                <%# Eval("Level") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
               <!-- <img src='<%# Eval("IconURL") %>' title='<%# String.Format(Lang.TransA("Level {0}"), Eval("Level")) %> - <%# Eval("Name") %>' border='0' />-->
                <a class="tooltip-link user-level" title='<%# String.Format(Lang.TransA("Level {0}"), Eval("Level")) %> - <%# Eval("Name") %>'>
                    <span class="fa-stack fa-lg fa-badge">
                      <i class="fa fa-certificate fa-stack-2x"></i>
                      <i class="fa fa-stack-1x fa-inverse"><%# Eval("Level") %></i>
                    </span>
                </a>
            </ItemTemplate>
        </asp:TemplateColumn>																	
        <asp:TemplateColumn>
            <ItemStyle></ItemStyle>
            <ItemTemplate>
                <%# Eval("Name") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <%# Eval("MinScore") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton id="lnkEdit" CssClass="btn btn-primary btn-xs" CommandName="EditLevel" CommandArgument='<%# Eval("ID") %>' Runat="server"><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%></asp:LinkButton>&nbsp;
                <asp:LinkButton id="lnkDelete" CssClass="btn btn-primary btn-xs" CommandName="DeleteLevel" CommandArgument='<%# Eval("ID") %>' Runat="server"><i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("Delete")%></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
</div>
<div class="actions">
    <asp:LinkButton CssClass="btn btn-secondary" id="btnAddNewLevel" Runat="server" onclick="btnAddNewLevel_Click" />
</div>
<asp:panel id="pnlUserLevelInfo" Runat="server">
	<div class="panel clear-panel">
	    <div class="panel-heading">
	        <h4 class="panel-title"><%= Lang.TransA("User Level Details") %></h4>
	    </div>
        <div class="panel-body small-width">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-5"><%= Lang.TransA("Name") %></label>
                    <div class="col-sm-7">
                        <asp:TextBox CssClass="form-control" id="txtName" Runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-5"><%= Lang.TransA("Minimum Score") %></label>
                    <div class="col-sm-7">
                        <asp:TextBox CssClass="form-control form-control-inline" id="txtMinScore" Runat="server"/>
                    </div>
				</div>
	        </div>
		</div>
	</div>
	<asp:PlaceHolder ID="phLevelRestrictions" runat="server"/>
    <div class="actions">
        <asp:Button  CssClass="btn btn-default" id="btnCancel" runat="server" onclick="btnCancel_Click"/>
        <asp:Button CssClass="btn btn-primary" id="btnCreateUpdate" Runat="server" onclick="btnCreateUpdate_Click"/>
    </div>				
    <uc2:MessageBox ID="MessageBox1" runat="server" />
	<input id="hidCurrentLevelID" type="hidden" runat="server" />
</asp:panel>	
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
