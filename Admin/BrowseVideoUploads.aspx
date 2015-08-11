<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BrowseVideoUploads.aspx.cs" Inherits="AspNetDating.Admin.BrowseVideoUploads" %>
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
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Approved") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddApproved" runat="server">
                            <asp:ListItem/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Private") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddIsPrivate" runat="server">
                            <asp:ListItem/>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="actions">
                    <asp:Button CssClass="btn btn-primary" ID="btnSearch" runat="server" onclick="btnSearch_Click"/>
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
    <asp:DropDownList ID="ddVideoUploadsPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" onselectedindexchanged="ddVideoUploadsPerPage_SelectedIndexChanged"/>
</p>
<div class="table-responsive">
    <asp:DataGrid CssClass="table table-striped" ID="dgVideoUploads" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" GridLines="None" onitemcommand="dgVideoUploads_ItemCommand" onsortcommand="dgVideoUploads_SortCommand" onitemcreated="dgVideoUploads_ItemCreated">
    <HeaderStyle Font-Bold="True"></HeaderStyle>
	    <Columns>

            <asp:TemplateColumn SortExpression="2">

                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <a target="_blank" href="<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("Username") %>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("Username")%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                <div id="pnlVideoUpload" runat="server" visible="false">
                    <div class="center">
                        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                            width="325" height="262">
                            <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf">
                            <param name="quality" value="high">
                            <param name="bgcolor" value="#FFFFFF">
                            <param name="wmode" value="transparent">
                            <param name="allowfullscreen" value="true">
                            <param name="flashvars" value="width=325&height=262&file=<%# Eval("VideoUrl") %>&image=<%# ((string) Eval("VideoUrl")).Replace(".flv", ".png") %>" />
                            <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf"
                                quality="high" wmode="transparent" bgcolor="#FFFFFF" width="325" height="262"
                                name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="width=325&height=262&file=<%# Eval("VideoUrl")%>&image=<%# ((string) Eval("VideoUrl")).Replace(".flv", ".png") %>"></embed></object>
                    </div>       
                </div>
                <asp:ImageButton ID="imgbtnViewVideo" runat="server" ImageUrl='<%# Eval("ThumbnailUrl") %>' CommandName="ViewVideo" CommandArgument='<%# Eval("ID") + "|" + Eval("VideoUrl")%>' class="img-thumbnail" style="behavior: none" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# Boolean.Parse((string)Eval("Approved")) ? Lang.TransA("Yes") : Lang.TransA("No") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# Boolean.Parse((string)Eval("IsPrivate")) ? Lang.TransA("Yes") : Lang.TransA("No") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-times"></i>&nbsp;<%# Lang.TransA("Delete") %></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <PagerStyle Mode="NumericPages"></PagerStyle>
    </asp:DataGrid>
</div>
<asp:Panel ID="pnlPaginator" runat="server">
    <ul class="pager">
        <li><asp:LinkButton ID="lnkFirst" runat="server" onclick="lnkFirst_Click"/></li>
        <li><asp:LinkButton ID="lnkPrev" runat="server" onclick="lnkPrev_Click"/></li>
        <li class="text-muted"><asp:Label ID="lblPager" runat="server"/></li>
        <li><asp:LinkButton ID="lnkNext" runat="server" onclick="lnkNext_Click"/></li>
        <li><asp:LinkButton ID="lnkLast" runat="server" onclick="lnkLast_Click"/></li>
	</ul>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
