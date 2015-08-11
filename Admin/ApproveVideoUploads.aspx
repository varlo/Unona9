<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveVideoUploads.aspx.cs" Inherits="AspNetDating.Admin.ApproveVideoUploads" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc1:MessageBox ID="MessageBox" runat="server"/>
        <asp:Panel ID="pnlVideosPerPage" runat="server">
            <p class="text-right">
                <small class="text-muted"><%= Lang.TransA("Video uploads per page") %>:</small>
                <asp:DropDownList ID="dropVideosPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropVideosPerPage_SelectedIndexChanged" />
            </p>
        </asp:Panel>
        <div class="table-responsive">
        <asp:DataGrid ID="dgPendingVideos" runat="server" PageSize="2" AllowPaging="True" AutoGenerateColumns="False" CssClass="table table-striped" GridLines="None" OnItemCommand="dgPendingVideos_ItemCommand" OnPageIndexChanged="dgPendingVideos_PageIndexChanged">
            <HeaderStyle Font-Bold="True"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn>
                    <ItemStyle Wrap="False"></ItemStyle>
                    <ItemTemplate>
                        <a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("Username")%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle Wrap="False"></ItemStyle>
                    <ItemTemplate>
                        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0" width="325" height="262" id="flvplayer">
                            <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf">
                            <param name="quality" value="high">
                            <param name="bgcolor" value="#FFFFFF">
                            <param name="wmode" value="transparent">
                            <param name="allowfullscreen" value="true">
                            <param name="flashvars" value="width=325&height=262&file=<%# Eval("VideoUrl") %>&image=<%# ((string)Eval("VideoUrl")).Replace(".flv", ".png") %>" />
                            <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf"
                                quality="high" wmode="transparent" bgcolor="#FFFFFF" width="325" height="262"
                                name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="width=325&height=262&file=<%# Eval("VideoUrl") %>&image=<%# ((string)Eval("VideoUrl")).Replace(".flv", ".png") %>"></embed>
                        </object>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                    <ItemTemplate>
                        <div class="btn-group">
                            <asp:LinkButton CssClass="btn btn-secondary" ID="btnApprove" CommandName="approve" CommandArgument='<%# Eval("Id") %>' Enabled='<%# HasWriteAccess %>' runat="server"><i class="fa fa-check"></i>&nbsp;<%# Lang.TransA("Approve") %></asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-default" ID="btnReject" CommandName="reject" CommandArgument='<%# Eval("Id") %>' Enabled='<%# HasWriteAccess %>' runat="server"><i class="fa fa-times"></i>&nbsp;<%# Lang.TransA("Reject") %> </asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
