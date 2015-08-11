<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BrowsePhotos.aspx.cs" Inherits="AspNetDating.Admin.BrowsePhotos" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
    function DoSearch() {
        var btn = document.getElementById('<%= btnSearch.ClientID %>');
    	// process only the Enter key
    	if (event.keyCode == 13) {
    	    // cancel the default submit
    	    event.returnValue = false;
    	    event.cancel = true;
    	    // submit the form by programmatically clicking the specified button
    	    btn.click();
    	}
    }
</script>

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
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Username") %>:</label>
                    <div class="col-sm-8"><asp:TextBox CssClass="form-control" ID="txtUsername" onkeydown="javascript: DoSearch()" runat="server"/></div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Primary") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddPrimary" runat="server">
                            <asp:ListItem Value=""/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="trPrivatePhotosSearch" runat="server">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Private") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddPrivate" runat="server">
                            <asp:ListItem Value=""/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="trExplicitPhotosSearch" runat="server">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Explicit") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddExplicit" runat="server">
                            <asp:ListItem Value=""/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="text-center">
                    <asp:Button CssClass="btn btn-primary" ID="btnSearch" runat="server" OnClick="btnSearch_Click"/>
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
    <small class="text-muted"><%= Lang.TransA("Photos per page") %>:</small>
    <asp:DropDownList ID="dropPhotosPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropPhotosPerPage_SelectedIndexChanged"/>
</p>
<div class="table-responsive">
    <asp:DataGrid ID="dgPhotos" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" CssClass="table table-striped" GridLines="None" OnItemCreated="dgPhotos_ItemCreated" OnItemCommand="dgPhotos_ItemCommand" OnSortCommand="dgPhotos_SortCommand">
    <Columns>
        <asp:TemplateColumn>
            <HeaderStyle Font-Bold="True"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <a target="_blank" href="<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("Username") %>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("Username")%></a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
            <ItemTemplate>
                <a href='<%# String.Format("../Image.ashx?id={0}", Eval("Id")) %>' target="_blank">
                    <img class="img-thumbnail" src='<%# String.Format("../Image.ashx?id={0}&width=150&height=80", Eval("Id")) %>' />
                </a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle Font-Bold="True"></HeaderStyle>
            <ItemStyle Wrap="True"></ItemStyle>
            <ItemTemplate>
                <label><%# Eval("Name")%></label><br />
                <%# Eval("Description")%>
        </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <ItemTemplate>
                <i class="fa fa-check-square-o" runat="server" visible='<%# Eval("Primary") %>'></i>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <ItemTemplate>
                <i class="fa fa-check-square-o" runat="server" visible='<%# Eval("Private") %>' id="imgIconPrivate"></i>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <ItemTemplate>
                    <i class="fa fa-check-square-o" runat="server" visible='<%# Eval("Explicit") %>' id="imgIconExplicit"></i>&nbsp;
                    <asp:LinkButton CssClass="" ID="lnkRemoveExplicit" runat="server" visible='<%# Eval("Private") %>' CommandName="RemoveExplicit" CommandArgument='<%# Eval("Id") %>'>
                        <i class="fa fa-square-o"></i>&nbsp;<%# Lang.TransA("Unmark") %>
                    </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>'><i class="fa fa-times"></i>&nbsp;<%# Lang.TransA("Delete") %></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
    </asp:DataGrid>
</div>
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
