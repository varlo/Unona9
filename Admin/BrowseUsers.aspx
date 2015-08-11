<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BrowseUsers.aspx.cs" Inherits="AspNetDating.Admin.BrowseUsers" %>
<%@ Import Namespace="AspNetDating.Classes" %>
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
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Country") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtCountry" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Region/State") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtRegion" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("City") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtCity" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Username") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtUsername" CssClass="form-control" onkeydown="javascript: DoSearch()" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Name") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtName" CssClass="form-control" onkeydown="javascript: DoSearch()" runat="server"/></div>
                </div>
                <div class="form-group" id="pnlGender" runat="server">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Gender") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddGender" CssClass="form-control" runat="server">
                            <asp:ListItem Value=""/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Age Range") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2"/>
                        <%= Lang.TransA("to") %>
                        <asp:TextBox ID="txtAgeTo" runat="server" CssClass="form-control form-control-inline" Size="2" MaxLength="2"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("E-mail") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtEmail" CssClass="form-control" onkeydown="javascript: DoSearch()" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Signup IP") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtIP" CssClass="form-control" onkeydown="javascript: DoSearch()" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Has Photo") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddPhoto" runat="server">
                            <asp:ListItem Value=""/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Activated") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddActive" runat="server">
                            <asp:ListItem Value=""/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Deleted") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddDeleted" runat="server">
                            <asp:ListItem Value=""/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Paid Member") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddPaid" runat="server">
                            <asp:ListItem Value=""/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="text-center"><asp:Button CssClass="btn btn-primary" ID="btnSearch" runat="server" OnClick="btnSearch_Click"/></div>
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
    <small class="text-muted"><%= Lang.TransA("Users per page") %>:</small>
    <asp:DropDownList ID="dropUsersPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropUsersPerPage_SelectedIndexChanged"/>
</p>
<div class="table-responsive">
    <asp:DataGrid CssClass="table table-striped" ID="dgUsers" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" GridLines="None" OnItemCreated="dgUsers_ItemCreated" OnItemCommand="dgUsers_ItemCommand" OnSortCommand="dgUsers_SortCommand">
        <HeaderStyle Wrap="False"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn SortExpression="Username">
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("Username")%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Name">
                <ItemTemplate>
                    <%# Eval("Name") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Gender">
                <ItemTemplate>
                    <%# Eval("Gender") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="False" SortExpression="Birthdate">
                <ItemTemplate>
                    <%# Eval("Birthdate") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="SignupDate">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemTemplate>
                    <%# Eval("SignupDate") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="SignupIP">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemTemplate>
                    <%# Eval("CountryCode").ToString().Length > 0 ? String.Format("<img src=\"images/countryicons/{0}.png\" title=\"{1}\">", Eval("CountryCode"), Eval("CountryName")) : "" %>
                    <%# Eval("SignupIP") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Email">
                <ItemTemplate>
                    <a href='<%# "mailto:" + Eval("Email") %>'><%# Eval("Email") %></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"  Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <a class="btn btn-primary btn-xs" href="EditProfile.aspx?username=<%# Eval("Username") %>"><i class="fa fa-user"></i>&nbsp;<%= Lang.TransA("Profile")%></a>
                    <a class="btn btn-primary btn-xs" href="EditPhotos.aspx?username=<%# Eval("Username") %>"><i class="fa fa-file-image-o"></i>&nbsp;<%= Lang.TransA("Photos")%></a>
                    <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkManage" runat="server" CommandName="Manage" CommandArgument='<%# Eval("Username")%>'><i class="fa fa-cog"></i>&nbsp;<%# Lang.TransA("Manage")%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <PagerStyle Mode="NumericPages"></PagerStyle>
    </asp:DataGrid>
</div>
<div class="text-right"><asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnGetCSV" runat="server" onclick="btnGetCSV_Click" Visible="false"/></div>
<asp:Panel ID="pnlPaginator" runat="server">
    <ul class="pager">
        <li><asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"/></li>
        <li><asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"/></li>
        <li class="text-muted"><asp:Label ID="lblPager" runat="server" /></li>
        <li><asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"/></li>
        <li><asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"/></li>
    </ul>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>