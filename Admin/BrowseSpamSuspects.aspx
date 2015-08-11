<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BrowseSpamSuspects.aspx.cs" Inherits="AspNetDating.Admin.BrowseSpamSuspects" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc1:messagebox id="MessageBox" runat="server" EnableViewState="false"/>
<asp:MultiView ID="mvSpamSuspects" runat="server" ActiveViewIndex="0">
<asp:View ID="viewSpamSuspects" runat="server">
<p class="text-right">
    <small class="text-muted"><%= Lang.TransA("Users per page") %>:</small>
	<asp:DropDownList ID="ddUsersPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" onselectedindexchanged="ddUsersPerPage_SelectedIndexChanged" />
</p>
<div class="table-responsive">
<asp:GridView CssClass="table table-striped" ID="gvUsers" runat="server" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" onrowcommand="gvUsers_RowCommand" onrowcreated="gvUsers_RowCreated" onrowdatabound="gvUsers_RowDataBound" onsorting="gvUsers_Sorting" Width="100%">
    <Columns>
        <asp:TemplateField SortExpression="Username">
            <HeaderStyle Font-Bold="True"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <a href='<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("Username") %>' target="_blank" title="<%= Lang.TransA("View User Profile")%>"><i class="fa fa-eye"></i>&nbsp;<%# Eval("Username") %></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle Font-Bold="True"></HeaderStyle>
            <ItemStyle Wrap="True"></ItemStyle>
            <ItemTemplate>
                <%# Eval("Message") %>
                <div class="text-right"><a href='BrowseMessages.aspx?uid=<%# Eval("Username") %>'><i class="fa fa-comments-o"></i>&nbsp;<%= Lang.TransA("View messages") %></a></div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle Font-Bold="True" HorizontalAlign="Right"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <div class="btn-group btn-group-xs">
                    <asp:LinkButton CssClass="btn btn-primary" ID="lnkDeleteUser" runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="DeleteUser"><%# Lang.TransA("Delete User")%></asp:LinkButton>
                    <asp:LinkButton CssClass="btn btn-primary" ID="lnkDeleteUserAndMessages" runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="DeleteUserAndMessages"><%# Lang.TransA("Delete User and Messages")%></asp:LinkButton>
                    <asp:LinkButton CssClass="btn btn-primary" ID="lnkMarkAsReviewed" runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="MarkAsReviewed"><%# Lang.TransA("Mark as Reviewed")%></asp:LinkButton>
                    <asp:LinkButton CssClass="btn btn-primary" ID="lnkSendMessage" runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="SendMessage"><%# Lang.TransA("Send message")%></asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerSettings Mode="Numeric" Position="Bottom" />
</asp:GridView>
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
</asp:View>
<asp:View ID="viewSendMessage" runat="server">
    <div class="panel small-width">
        <label><%= Lang.TransA("Message") %>:</label>
        <asp:TextBox CssClass="form-control" ID="txtMessage" runat="server"/>
        <div class="actions">
            <div class="btn-group">
                <asp:Button CssClass="btn btn-secondary" ID="btnSendMessage" runat="server" onclick="btnSendMessage_Click" />
                <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" onclick="btnCancel_Click"/>
            </div>
        </div>
    </div>
</asp:View>
<asp:View ID="viewDeleteReason" runat="server">
    <div class="panel small-width">
    <label><%= Lang.TransA("Reason") %>:</label>
    <asp:TextBox CssClass="form-control" ID="txtDeleteReason" runat="server"/>
    <div class="actions">
        <asp:Button CssClass="btn btn-default" ID="btnDelete" runat="server" onclick="btnDelete_Click" />
    </div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
