<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BrowseGroups.aspx.cs" Inherits="AspNetDating.Admin.BrowseGroups" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<uc1:messagebox id="MessageBox" runat="server"/>
<div class="panel panel-filter">
    <div class="panel-heading">
        <h4 class="panel-title">
            <i class="fa fa-filter"></i>&nbsp;<%= Lang.TransA("Filter") %>
            <span class="pull-right" id="tblHideSearch" runat="server" visible="false">
               <!-- <a onclick="document.getElementById('tblSearch').style.display = 'block'; document.getElementById('<%= tblHideSearch.ClientID %>').style.display = 'none';" href="javascript: void(0);" title="<%= Lang.TransA("Expand filter") %>">
                    <i class="fa fa-expand"></i>
                </a>-->
                <a data-toggle="collapse" data-parent=".panel-filter" href="#collapseFilter" title="<%= Lang.TransA("Expand/Collapse Filter") %>"><i class="fa fa-expand"></i></a>
            </span>
        </h4>
    </div>
    <div id="collapseFilter" class="panel-collapse collapse in">
        <div class="panel-body">
            <div class="form-horizontal form-sm">
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Category") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddCategory" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Owner") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtOwner" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Name") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtName" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Access level") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddAccessLevel" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Approved") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddApproved" runat="server"/>
                    </div>
                </div>
                <div class="actions">
                    <asp:Button CssClass="btn btn-primary" ID="btnSearch" OnClick="btnSearch_Click" runat="server"/>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    if (document.getElementById('<%= tblHideSearch.ClientID %>'))
        document.getElementById('tblSearch').style.display = 'none';
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<p class="text-right">
    <small class="text-muted"><%= Lang.TransA("Groups per page") %>:</small>
    <asp:DropDownList ID="ddGroupsPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddGroupsPerPage_SelectedIndexChanged"/>
</p>
<div class="table-responsive">
<asp:DataGrid CssClass="table table-striped" ID="dgGroups" GridLines="None" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" OnItemCommand="dgGroups_ItemCommand" OnItemCreated="dgGroups_ItemCreated" OnSortCommand="dgGroups_SortCommand">
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
    <Columns>
        <asp:TemplateColumn SortExpression="Name">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <a target="_blank" href="<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>">
                    <img class="img-thumbnail" src='<%= Config.Urls.Home%>/GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=20&height=20&diskCache=1' />&nbsp;<%# Eval("Name") %>
                </a>
            </ItemTemplate>
        </asp:TemplateColumn>                            
        <asp:TemplateColumn SortExpression="Owner">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Owner")%>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("Owner")%></a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemStyle Wrap="True"></ItemStyle>
            <ItemTemplate>
                    <%# Eval("Category")%>
            </ItemTemplate>
        </asp:TemplateColumn>                            
        <asp:TemplateColumn SortExpression="DateCreated">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <%# Eval("DateCreated")%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn SortExpression="AccessLevel">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <%# Eval("AccessLevel")%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <ItemTemplate>
                <a class="btn btn-primary btn-xs" href="EditGroup.aspx?id=<%# Eval("GroupID") %>"><i class="fa fa-edit"></i>&nbsp;<%= Lang.TransA("Edit")%></a>
                <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDeleteGroup" runat="server" CommandName="DeleteGroup" CommandArgument='<%# Eval("GroupID") %>'><i class="fa fa-trash-o"></i>&nbsp;<%= Lang.TransA("Delete")%></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
</div>
<div class="text-right"><asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnGetCSV" runat="server" Visible="false" onclick="btnGetCSV_Click"/></div>
<asp:Panel ID="pnlPaginator" runat="server">
    <ul class="pager">
        <li><asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"/></li>
        <li><asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"/></li>
        <li class="text-muted"><asp:Label ID="lblPager" runat="server"/></li>
        <li><asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"/></li>
        <li><asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"/></li>
	</ul>
</asp:Panel>  
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
