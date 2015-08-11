<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AbuseReports.aspx.cs" Inherits="AspNetDating.Admin.AbuseReports" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel panel-filter">
    <div class="panel-heading">
        <h4 class="panel-title">
            <i class="fa fa-filter"></i>&nbsp;<%= Lang.TransA("Filter") %>
            <span class="pull-right" id="tblHideSearch" runat="server" visible="false">
                <a data-toggle="collapse" data-parent=".panel-filter" href="#collapseFilter" title="<%= Lang.TransA("Expand/Collapse Filter") %>"><i class="fa fa-expand"></i></a>
            </span>
        </h4>
    </div>
    <div id="collapseFilter" class="panel-collapse collapse in">
        <div class="panel-body">
            <asp:Panel id="pnlSearchBox" DefaultButton="btnSearch" runat="server">
                <div class="form-horizontal form-sm">
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Reported By") %>:</label>
                        <div class="col-sm-8">
                            <asp:TextBox CssClass="form-control" ID="txtReportedBy" runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Reported User") %>:</label>
                        <div class="col-sm-8">
                            <asp:TextBox CssClass="form-control" ID="txtReportedUser" runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("ReportType") %>:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList CssClass="form-control" ID="ddReportType" runat="server">
                                <asp:ListItem Value=""/>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("From Date") %>:</label>
                        <div class="col-sm-8">
                            <uc2:DatePicker ID="dpFromDate" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("To Date") %>:</label>
                        <div class="col-sm-8">
                            <uc2:DatePicker ID="dpToDate" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4"><%= Lang.TransA("Reviewed") %>:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList CssClass="form-control" ID="ddReviewed" runat="server">
                                <asp:ListItem Value=""/>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="actions">
                        <asp:Button CssClass="btn btn-primary" ID="btnSearch" runat="server" OnClick="btnSearch_Click"/>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
    <script type="text/javascript">
        if (document.getElementById('<%= tblHideSearch.ClientID %>'))
            document.getElementById('tblSearch').style.display = 'none';
    </script>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
		<uc1:MessageBox id="MessageBox" runat="server"/>
        <div class="table-responsive">
        <asp:DataGrid CssClass="table table-striped" ID="dgAbuseReports" runat="server" GridLines="None"  AutoGenerateColumns="False" AllowSorting="True" AllowPaging="false" OnSortCommand="dgAbuseReports_SortCommand" OnItemCommand="dgAbuseReports_ItemCommand" OnItemCreated="dgAbuseReports_ItemCreated">
            <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn SortExpression="DateReported">
                    <ItemTemplate>
                            <asp:HiddenField ID="hidReportId" Value='<%# Eval("ReportId") %>' runat="server" />
                            <%# Eval("DateReported")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn SortExpression="ReportedBy">
                    <ItemStyle Wrap="False"></ItemStyle>
                    <ItemTemplate>
                        <a target="_blank" href='<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("ReportedBy") %>' title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("ReportedBy")%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                    <asp:TemplateColumn SortExpression="ReportedUser">
                    <ItemStyle Wrap="False"></ItemStyle>
                    <ItemTemplate>
                        <a target="_blank" href='<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("ReportedUser") %>' title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("ReportedUser")%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle Wrap="True"></ItemStyle>
                    <ItemTemplate>
                        <div class="text-danger"><%# Eval("Report") %></div>
                        <div class="text-center">
                            <a id="A1" Visible='<%# ((AbuseReport.ReportType)Eval("ReportType")) == AbuseReport.ReportType.Photo %>' HRef='<%# "~/Image.ashx?id=" + Eval("ReportedItemId") %>' runat="server">
                                <img class="img-thumbnail" src='<%# Config.Urls.Home + "/Image.ashx?id=" + Eval("ReportedItemId") + "&width=60&height=60&cache=1"%>' />
                            </a>
                        </div>
                        <div id="Div1" Visible='<%# ((AbuseReport.ReportType)Eval("ReportType")) == AbuseReport.ReportType.Message %>' runat="server">
                            <%# Lang.TransA("Message")+ ": " + Eval("ReportedMessage") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>                            
                <asp:TemplateColumn SortExpression="ReportedType">
                    <ItemStyle Wrap="True"></ItemStyle>
                    <ItemTemplate>
                        <%# ((AbuseReport.ReportType)Eval("ReportType")).ToString()%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle Wrap="False"></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton CssClass="btn btn-secondary btn-xs" id="lnkMarkReviewed" CommandName="MarkReviewed" runat="server">
                            <i class="fa fa-check-square"></i>&nbsp;<%# Lang.TransA("Reviewed") %>
                        </asp:LinkButton><hr  />
                        <span id="Li1" runat="server" Visible='<%# (AbuseReport.ReportType)Eval("ReportType") == AbuseReport.ReportType.Message %>' >
                            <asp:LinkButton CssClass="btn btn-primary btn-xs" id="lnkDeleteReportedMessage" CommandName="DeleteMessage" CommandArgument='<%# Eval("ReportedItemId") %>' runat="server">
                                <i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("Message") %>
                            </asp:LinkButton>
                        </span>
                        <span id="Li2" runat="server" Visible='<%# (AbuseReport.ReportType)Eval("ReportType") == AbuseReport.ReportType.Photo %>'>
                            <asp:LinkButton  CssClass="btn btn-primary btn-xs" id="lnkDeleteReportedPhoto" CommandName="DeletePhoto" CommandArgument='<%# Eval("ReportedItemId") %>' runat="server">
                                <i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("Photo") %>
                            </asp:LinkButton>
                        </span>
                        <a class="btn btn-primary btn-xs" onclick="javascript: document.getElementById('lnkDeleteUser_<%# Eval("RowIndex") %>_panel').style.display = 'block'; return false;" href="#" title="<%# Lang.TransA("Delete User") %>">
                            <i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("User") %>
                        </a>
                        <span id="Li3" runat="server" Visible='<%# (AbuseReport.ReportType)Eval("ReportType") == AbuseReport.ReportType.Message %>'>
                            <a class="btn btn-primary btn-xs" onclick="javascript: document.getElementById('lnkDeleteUserAndTheirMessages_<%# Eval("RowIndex") %>_panel').style.display = 'block';return false;" href="#">
                                <i class="fa fa-trash-o"></i>&nbsp;<%# Lang.TransA("User and Messages") %>
                            </a>
                        </span>
                        <div id='lnkDeleteUser_<%# Eval("RowIndex") %>_panel' style="display: none">
                            <hr />
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon"><%= Lang.TransA("Reason") %>:</span>
                                <asp:TextBox CssClass="form-control" id="txtReason1" runat="server"/>

                                <span class="input-group-btn">
                                    <input class="btn btn-default" type="button" value='<%# Lang.TransA("Cancel") %>' onclick="javascript: document.getElementById('lnkDeleteUser_<%# Eval("RowIndex") %>_panel').style.display = 'none';" />
                                </span>
                                <span class="input-group-addon addon-hide"></span>
                                <span class="input-group-btn">
                                    <asp:Button CssClass="btn btn-primary" ID="btnDeleteUser" CommandName="DeleteUser" CommandArgument='<%# Eval("ReportedUser") %>' Text='<%# Lang.TransA("Delete") %>' runat="server" />
                                </span>
                            </div>
                        </div>
                        <div id='lnkDeleteUserAndTheirMessages_<%# Eval("RowIndex") %>_panel' style="display: none">
                            <hr />
                            <div class="input-group input-group-sm">
                                <span class="input-group-addon"><%= Lang.TransA("Reason") %>:</span>
                                <asp:TextBox CssClass="form-control" id="txtReason2" runat="server"/>
                                <span class="input-group-btn">
                                    <input class="btn btn-default" type="button" value='<%# Lang.TransA("Cancel") %>' onclick="javascript: document.getElementById('lnkDeleteUserAndTheirMessages_<%# Eval("RowIndex") %>_panel').style.display = 'none';" />
                                </span>
                                <span class="input-group-addon addon-hide"></span>
                                <span class="input-group-btn">
                                    <asp:Button CssClass="btn btn-primary" ID="btnDeleteUserAndTheirMessages" CommandName="DeleteUserAndTheirMessages" CommandArgument='<%# Eval("ReportedUser") %>' Text='<%# Lang.TransA("Delete") %>' runat="server" />
                                </span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>                                                                                                                                              
            </Columns>
            <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
        </div>
        <asp:Panel ID="pnlPaginator" Visible="false" runat="server">
            <ul class="pager">
                <li><asp:LinkButton ID="lnkFirst" runat="server"/></li>
                <li><asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"/></li>
                <li class="text-muted"><asp:Label ID="lblPager" runat="server"/></li>
                <li><asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"/></li>
                <li><asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"/></li>
            </ul>
        </asp:Panel> 
    </ContentTemplate>
    </asp:UpdatePanel>	
</asp:Content>
