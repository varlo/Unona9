<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewTopics.ascx.cs" Inherits="AspNetDating.Components.Groups.NewTopics" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import namespace="AspNetDating"%>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server" />
<asp:Label ID="lblError" CssClass="alert text-danger" runat="server" />
<div id="pnlSearchTopic" runat="server">
    <asp:Panel class="input-group input-group-sm filter" DefaultButton="btnSearch" runat="server">
        <span class="input-group-addon"><%= Lang.Trans("Find Topics") %>:</span>
        <asp:TextBox ID="txtTopicToSearch" CssClass="form-control" runat="server" />
        <span class="input-group-addon">
            <div class="checkbox"><asp:CheckBox ID="cbSearchInPosts" runat="server" /><%= Lang.Trans("include posts") %></div>
        </span>
        <div class="input-group-btn"><asp:Button ID="btnSearch" CssClass="btn btn-default" runat="server" OnClick="btnSearch_Click"/></div>
    </asp:Panel>
</div>
<asp:DataGrid ID="dgGroupTopics" Runat="server" GridLines="None" AutoGenerateColumns="false" HorizontalAlign="Center" CssClass="table table-striped">
	<Columns>
	    <asp:TemplateColumn>
            <ItemTemplate>
	            <%# Eval("Icon") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <a href='ShowUser.aspx?uid=<%# Eval("Username") %>' title="<%# Eval("Username") %>">
                    <%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 40, 40, "img-thumbnail", false, true, true) %>
                </a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemStyle Wrap="True"></ItemStyle>
            <ItemTemplate>
                <a href='<%# UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString(), (string) Eval("GroupTopicID")) %>'>
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
<div id="pnlViewAllTopics" runat="server" visible="false" class="text-right">
	<asp:LinkButton ID="lnkViewAllTopics" runat="server" OnClick="lnkViewAllTopics_Click"/>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>