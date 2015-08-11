<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BrowseAffiliates.aspx.cs" Inherits="AspNetDating.Admin.BrowseAffiliates" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
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
                    <div class="col-sm-8">
                         <asp:TextBox CssClass="form-control" ID="txtUsername" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Name") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtName" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Email") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtEmail" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Site URL") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtSiteURL" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Deleted") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddDeleted" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Active") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddActive" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Request payment") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddRequestPayment" runat="server"/>
                    </div>
                </div>
                <div class="actions">
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
<uc1:messagebox id="MessageBox" runat="server"/>
<p class="text-right">
    <small class="text-muted"><%= Lang.TransA("Affiliates per page") %>:</small>
    <asp:DropDownList ID="ddAffiliatesPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddAffiliatesPerPage_SelectedIndexChanged"/>
</p>
<div class="table-responsive">
<asp:GridView CssClass="table table-striped" ID="gvAffiliates" GridLines="None" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" OnRowCommand="gvAffiliates_RowCommand" OnRowCreated="gvAffiliates_RowCreated" OnSorting="gvAffiliates_Sorting">
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
    <Columns>
        <asp:TemplateField SortExpression="Username">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <%# Eval("Username") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Name">
            <ItemTemplate>
                <%# Eval("Name") %>
            </ItemTemplate>
        </asp:TemplateField>                            
        <asp:TemplateField>
            <ItemTemplate>
                    <%# Eval("SiteURL")%></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Balance">
            <ItemTemplate>
                    <%# Eval("Balance")%>
            </ItemTemplate>
        </asp:TemplateField>                            
        <asp:TemplateField>
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("RequestPayment")) ? Lang.TransA("Yes") : Lang.TransA("No")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("Commission")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDeleteGroup" runat="server" CommandName="DeleteAffiliate" CommandArgument='<%# Eval("AffiliateID") %>'><i class="fa fa-trash-o"></i>&nbsp;<%= Lang.TransA("Delete")%></asp:LinkButton>
                <a class="btn btn-primary btn-xs" href="EditAffiliate.aspx?id=<%# Eval("AffiliateID") %>"><i class="fa fa-edit"></i>&nbsp;<%= Lang.TransA("Edit")%></a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerSettings Position="Bottom" Mode="Numeric"></PagerSettings>
</asp:GridView>
</div>
<div class="text-right"><asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnGetCSV" runat="server" Visible="false" onclick="btnGetCSV_Click" /></div>
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
