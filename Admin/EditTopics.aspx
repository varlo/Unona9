<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditTopics.aspx.cs" Inherits="AspNetDating.Admin.EditTopics" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="table-responsive">
<asp:DataGrid CssClass="table table-striped" ID="dgTopics" AutoGenerateColumns="False" AllowPaging="False" PageSize="10" runat="server" GridLines="None" OnItemCreated="dgTopics_ItemCreated" OnItemCommand="dgTopics_ItemCommand" OnItemDataBound="dgTopics_ItemDataBound">
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
    <Columns>
        <asp:TemplateColumn>
            <ItemStyle Width="20"></ItemStyle>
            <ItemTemplate>
                <input type="checkbox" id="cbSelect" value='<%# Eval("TopicID") %>' runat="server" />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <%# Eval("Title")%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <%# Eval("EditColumns")%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <%# Eval("ViewColumns")%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkUp" CommandName="ChangeOrder" CommandArgument="Up" runat="server"><i class="fa fa-caret-up"></i></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton ID="lnkDown" CommandName="ChangeOrder" CommandArgument="Down" runat="server"><i class="fa fa-caret-down"></i></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkEdit" CommandName="EditTopic" runat="server"><i class="fa fa-edit"></i>&nbsp;<%# Lang.TransA("Edit")%></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
</div>
<div class="text-right">
    <asp:LinkButton CssClass="btn btn-default btn-sm pull-left" ID="btnDeleteSelectedTopics" runat="server" OnClick="btnDeleteSelectedTopics_Click"/>
	<asp:LinkButton CssClass="btn btn-secondary btn-sm" ID="btnAddNewTopic" runat="server" OnClick="btnAddNewTopic_Click"/>
</div>
</asp:Content>
