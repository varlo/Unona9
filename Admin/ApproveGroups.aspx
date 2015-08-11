<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveGroups.aspx.cs" Inherits="AspNetDating.Admin.ApproveGroups" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanelPendingApproval" runat="server">
    <ContentTemplate>
        <uc1:MessageBox ID="MessageBox" runat="server"/>
        <p class="text-right">
            <small class="text-muted"><asp:Label ID="lblGroupsPerPage" runat="server"/></small>
            <asp:DropDownList ID="ddGroupsPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddGroupsPerPage_SelectedIndexChanged"/>
        </p>
        <asp:DataGrid CssClass="table table-striped" ID="dgPendingApproval" runat="server" AutoGenerateColumns="False" GridLines="None" OnItemCommand="dgPendingApproval_ItemCommand" OnItemCreated="dgPendingApproval_ItemCreated" OnItemDataBound="dgPendingApproval_ItemDataBound">
            <HeaderStyle Font-Bold="True"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                    <div class="media">
                        <img class="img-thumbnail pull-left" src='../GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=160&height=160&diskCache=1' />
                        <div class="media-body">
                            <label><%# Eval("Name")%></label>
                            <ul class="info-header info-header-sm">
                                <li><a class="tooltip-link" title="<%= Lang.Trans("Date Created") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("DateCreated")%></a></li>
                                <li><a class="tooltip-link" title="<%= Lang.Trans("Categories") %>"><i class="fa fa-sitemap"></i>&nbsp;<%# Eval("Categories")%></a></li>
                                <li><a class="tooltip-link" title="<%= Lang.Trans("Access level") %>"><i class="fa fa-globe"></i>&nbsp;<%# Eval("AccessLevel")%></a></li>
                                <li><a class="tooltip-link" title="<%= Lang.Trans("Owner") %>"><i class="fa fa-user"></i>&nbsp;<%# Eval("Owner")%></a></li>
                                <li class="pull-right"><a class="tooltip-link" title="<%= Lang.Trans("Group Status") %>"><i class="fa fa-spinner"></i>&nbsp;<%# Eval("Approved")%></a></li>
                            </ul>
                            <%# Eval("Description")%>
                            <hr />
                            <div class="row">
                                <div class="col-sm-8">
                                    <div class="input-group input-group-sm">
                                        <span class="input-group-addon"><%= Lang.TransA("Reason to reject") %>:</span>
                                        <asp:TextBox CssClass="form-control" ID="txtReason" runat="server"/>
                                        <span class="input-group-btn"><asp:LinkButton CssClass="btn btn-default" ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("GroupID")%>' /></span>
                                    </div>
                                </div>
                                <div class="col-sm-4 text-right">
                                    <asp:LinkButton CssClass="btn btn-secondary btn-sm" ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("GroupID")%>' />
                                    <a class="btn btn-primary btn-sm" href="EditGroup.aspx?id=<%# Eval("GroupID")%>&src=ag"><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
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
