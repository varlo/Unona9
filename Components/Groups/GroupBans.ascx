<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="GroupBans.ascx.cs" Inherits="AspNetDating.Components.Groups.GroupBans" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
<asp:DataGrid ID="dgGroupBans" runat="server" OnItemCommand="dgGroupBans_ItemCommand" AutoGenerateColumns="false" CssClass="table table-striped" GridLines="None">
    <Columns>
        <asp:TemplateColumn>
        <HeaderStyle Font-Bold="True"></HeaderStyle>        
            <ItemTemplate>
        			<a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'><%# Eval("Username") %></a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
        <HeaderStyle Font-Bold="True"></HeaderStyle>
            <ItemTemplate>
                <%= Lang.Trans("Expires on:") %>&nbsp;<%# Eval("ExpirationDate") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkUnban" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Unban" Text='<%# Lang.Trans("Unban") %>'/>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>