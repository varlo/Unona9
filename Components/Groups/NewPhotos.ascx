<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewPhotos.ascx.cs" Inherits="AspNetDating.Components.Groups.NewPhotos" %>

<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import namespace="AspNetDating"%>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"/>
<asp:Label ID="lblError" CssClass="alert text-danger" runat="server"/>
<asp:DataList ID="dlGroupPhotos" CssClass="repeater-horizontal" RepeatLayout="Flow" Runat="server" SkinId="GroupNewPhotos">
	<ItemTemplate>
	    <div class="thumbnail">
            <img src='GroupImage.ashx?gpid=<%# Eval("GroupPhotoID") %>&width=90&height=90&diskCache=1' title="<%# Eval("Name") %>" />
        </div>
	</ItemTemplate>
</asp:DataList>
<div id="pnlMore" runat="server" visible="false" class="text-right">
	<asp:LinkButton ID="lnkMore" runat="server" OnClick="lnkMore_Click"/>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>