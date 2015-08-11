<%@ Page Title="Contests" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ContestEntries.aspx.cs" Inherits="AspNetDating.Admin.ContestEntries" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:Panel ID="pnlFilter" DefaultButton="btnSearch" runat="server">
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
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Contest Name") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddContests" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Username") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtUsername" runat="server"/>
                    </div>
                </div>
                <div class="actions">
                    <asp:Button CssClass="btn btn-primary" ID="btnSearch" OnClick="btnSearch_Click" runat="server"/>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Panel>
<script type="text/javascript">
	if (document.getElementById('<%= tblHideSearch.ClientID %>'))
	    document.getElementById('tblSearch').style.display = 'none';
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<p class="text-right">
    <small class="text-muted"><asp:Label ID="lblPhotosPerPage" runat="server"/>:</small>
    <asp:DropDownList ID="dropPhotosPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropPhotosPerPage_SelectedIndexChanged"/>
</p>
<div class="table-responsive">
<asp:DataGrid ID="dgContestEntries" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" CssClass="table table-striped" GridLines="None" OnItemDataBound="dgContestEntries_ItemDataBound" OnSortCommand="dgContestEntries_SortCommand" OnItemCommand="dgContestEntries_ItemCommand">
        <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn SortExpression="Username">
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <a target="_blank" href="<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("Username") %>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("Username")%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
			<asp:TemplateColumn>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
					<a href='<%# String.Format("../Image.ashx?id={0}", Eval("PhotoId")) %>' target="_blank">
						<img class="img-thumbnail" src='<%# String.Format("../Image.ashx?id={0}&width=150&height=80", Eval("PhotoId")) %>' />
					</a>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="ContestName">
                <ItemStyle Wrap="False"></ItemStyle>
            	<ItemTemplate>
                	<%# Eval("ContestName")%>
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
