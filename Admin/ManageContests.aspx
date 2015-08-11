<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ManageContests.aspx.cs" Inherits="AspNetDating.Admin.ManageContests" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div id="pnlSearchInfo" runat="server">
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
                        <label class="col-sm-4 control-label"><%= Lang.TransA("Active") %>:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList CssClass="form-control" ID="ddActive" runat="server">
                                <asp:ListItem Value=""/>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="actions">
                        <asp:Button CssClass="btn btn-primary" ID="btnSearch" OnClick="btnSearch_Click" runat="server"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    if (document.getElementById('<%= tblHideSearch.ClientID %>'))
        document.getElementById('tblSearch').style.display = 'none';
</script>

<div class="table-responsive">
<asp:DataGrid CssClass="table table-striped" ID="dgContests" GridLines="None" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" OnSortCommand="dgContests_SortCommand" OnItemCommand="dgContests_ItemCommand" OnItemDataBound="dgContests_ItemDataBound">
         <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
    <Columns>
        <asp:TemplateColumn SortExpression="Name">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <%# Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn SortExpression="Gender">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <%# Eval("Gender")%>
            </ItemTemplate>
        </asp:TemplateColumn>                            
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
            <ItemTemplate>
                <%# Eval("AgeRange")%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn SortExpression="DateCreated">
            <ItemStyle Wrap="True"></ItemStyle>
            <ItemTemplate>
                <%# Eval("DateCreated")%>
            </ItemTemplate>
        </asp:TemplateColumn>                            
        <asp:TemplateColumn SortExpression="DateEnds">
            <ItemStyle Wrap="True"></ItemStyle>
            <ItemTemplate>
                <%# Eval("DateEnds")%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton CssClass="btn btn-default btn-xs" ID="lnkViewEntries" CommandName="ViewEntries" CommandArgument='<%# Eval("Id") %>' runat="server"/>
                <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkEditContest" CommandName="EditContest" CommandArgument='<%# Eval("Id") %>' runat="server"/>
                <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDeleteContest" CommandName="DeleteContest" CommandArgument='<%# Eval("Id") %>' runat="server"/>
            </ItemTemplate>
        </asp:TemplateColumn>                            
    </Columns>
    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
</div>
<ul class="pager">
    <li><asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"/></li>
    <li><asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"/></li>
    <li class="text-muted"><asp:Label ID="lblPager" runat="server"/></li>
    <li><asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"/></li>
    <li><asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"/></li>
</ul>

<div class="actions">
    <asp:LinkButton CssClass="btn btn-secondary" ID="btnAddNewContest" runat="server" OnClick="btnAddNewContest_Click" />
</div>

<div id="pnlAddNewContest" visible="false" runat="server">
    <div class="panel clear-panel">
        <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Contest") %></h4></div>
        <div class="panel-body">
            <div class="form-horizontal medium-width">
                <div class="form-group">
                    <label class="control-label col-sm-4">
                        <%= Lang.TransA("Contest Name") %>:
                    </label>
                    <div class="col-sm-8">
                        <asp:TextBox id="txtName" CssClass="form-control" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4">
                        <%= Lang.TransA("Description") %>:
                    </label>
                    <div class="col-sm-8">
                        <asp:TextBox id="txtDescription" TextMode="MultiLine" CssClass="form-control" runat="server"/>
                    </div>
                </div>
                <div class="form-group" id="pnlGender" runat="server">
                    <label class="control-label col-sm-4">
                        <%= Lang.TransA("Restricted to Gender") %>:
                    </label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddGender" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4">
                        <%= Lang.TransA("Terms") %>:
                    </label>
                    <div class="col-sm-8">
                        <asp:TextBox id="txtTerms" TextMode="MultiLine" CssClass="form-control" runat="server"/>
                    </div>
                </div>
                <div class="form-group" id="pnlAge" runat="server">
                    <label class="control-label col-sm-4">
                        <%= Lang.TransA("Age Range") %>
                    </label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control form-control-inline" ID="ddFromAge" runat="server"/>
                        <%= Lang.TransA("to") %>
                        <asp:DropDownList CssClass="form-control form-control-inline" id="ddToAge" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4">
                        <%= Lang.TransA("Date Ends") %>:
                    </label>
                    <div class="col-sm-8">
                        <uc2:DatePicker ID="dpDateEnds" runat="server" />   
                    </div>
                </div>
                <div class="actions">
                    <hr />
                    <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" OnClick="btnCancel_Click"/>
                    <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div><!-- /.panel -->
</div>
</asp:Content>
