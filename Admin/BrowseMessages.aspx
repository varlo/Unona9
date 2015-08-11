<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BrowseMessages.aspx.cs" Inherits="AspNetDating.Admin.BrowseMessages" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
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
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Sender") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtFromUsername" onkeydown="javascript: DoSearch()" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Recipient") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtToUsername" onkeydown="javascript: DoSearch()" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Keyword") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtKeyword" onkeydown="javascript: DoSearch()" runat="server"/>
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
    <small class="text-muted"><%= Lang.TransA("Messages per page") %>:</small>
    <asp:DropDownList ID="dropMessagesPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropMessagesPerPage_SelectedIndexChanged"/>
</p>
<div class="table-responsive">
    <asp:DataGrid CssClass="table table-striped" ID="dgMessages" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" GridLines="None" OnItemCreated="dgMessages_ItemCreated" OnItemCommand="dgMessages_ItemCommand" OnSortCommand="dgMessages_SortCommand">
         <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
	     <Columns>
            <asp:TemplateColumn>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <a target="_blank" href="<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("FromUsername") %>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("FromUsername")%></a>
                    <span class="text-muted"><%# Eval("FromGender") %></span>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <a target="_blank" href="<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("ToUsername") %>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("ToUsername") %></a>
                    <span class="text-muted"><%# Eval("ToGender") %></span>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemStyle Wrap="True"></ItemStyle>
                <ItemTemplate>
                    <%# Eval("Body") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <%# Eval("Timestamp") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>'><i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("Delete") %></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <PagerStyle Mode="NumericPages"></PagerStyle>
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
