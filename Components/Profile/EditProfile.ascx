<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditProfile.ascx.cs" Inherits="AspNetDating.Components.Profile.EditProfile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<input type="hidden" id="hidUsername" runat="server">
<asp:PlaceHolder id="plhProfile" runat="server"/>
<hr />
<div class="actions">
    <asp:Button CssClass="btn btn-default" id="btnSave" runat="server" onclick="btnSave_Click"/>
</div>
<asp:Literal ID="litAlert" Runat="server"/>
<asp:Label CssClass="alert text-danger" id="lblError" runat="server"/>
