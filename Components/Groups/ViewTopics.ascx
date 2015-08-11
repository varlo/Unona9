<%@ Import namespace="AspNetDating"%>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ViewTopics.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewTopics" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="~/Components/Groups/SearchResults.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server" />
<asp:Label ID="lblError" CssClass="alert text-danger" runat="server" />
<asp:Panel ID="pnlFilterTopics" class="input-group input-group-sm filter" DefaultButton="btnSearch" runat="server">
    <span class="input-group-addon"><%= Lang.Trans("keywords") %>:</span>
    <asp:TextBox CssClass="form-control" ID="txtTopicToSearch" runat="server" />
    <span class="input-group-addon">
        <div class="checkbox"><asp:CheckBox ID="cbSearchInPosts" runat="server" /><%= Lang.Trans("include posts") %></div>
    </span>
    <div class="input-group-btn"><asp:Button CssClass="btn btn-default" ID="btnSearch" runat="server" OnClick="btnSearch_Click" /></div>
</asp:Panel>
<asp:DataGrid ID="dgGroupTopics" runat="server" AllowPaging="False" AutoGenerateColumns="False" GridLines="None" CssClass="table table-striped" EnableViewState="false">
    <Columns>
        <asp:TemplateColumn>
            <ItemTemplate>
	            <%# Eval("Icon") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <a href='ShowUser.aspx?uid=<%# Eval("Username") %>' title="<%# Eval("Username") %>">
                    <%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 40, 40, "img-thumbnail", true, true, true) %>
                </a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemStyle Wrap="True"></ItemStyle>
            <ItemTemplate>
                <a  href='<%# UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString(), (string) Eval("GroupTopicID")) %>'>
                    <%# Eval("GroupTopicName") %>
                </a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <ItemTemplate>
                <%# Eval("Posts") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <ItemTemplate>
                <%# Eval("Views") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <%# Eval("DateCreated") %>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
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
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
