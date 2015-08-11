<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ViewMultiChoiceBig.ascx.cs" Inherits="AspNetDating.Components.Profile.ViewMultiChoiceBig" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="form-group">
    <label><asp:Label id="lblName" runat="server" /></label>
    <asp:Panel ID="pnlMessage" Runat="server">
        <asp:Label id="lblMessage" Runat="server"/>
    </asp:Panel>
    <asp:Panel ID="pnlValues" Runat="server">
        <asp:Repeater id="rptValues" Runat="server">
            <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
            <ItemTemplate>
                <li class="list-group-item">
                    <%# Container.DataItem %>
                </li>
            </ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
</div>
