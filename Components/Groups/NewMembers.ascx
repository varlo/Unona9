<%@ Import namespace="AspNetDating"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewMembers.ascx.cs" Inherits="AspNetDating.Components.Groups.NewMembers" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"/>
<asp:Label ID="lblError" CssClass="alert text-danger" runat="server"/>
<asp:DataList ID="dlGroupMembers" CssClass="repeater-horizontal" RepeatLayout="Flow" Runat="server" SkinId="GroupMembers">
	<ItemTemplate>
		<a class="thumbnail" href='ShowUser.aspx?uid=<%# Eval("Username") %>' title="<%# DataBinder.Eval(Container, "DataItem.Username") %>">
			<%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 90, 90, null, false, true, true) %>
            <div class="caption">
                <%# Eval("Username") %>
            </div>
	    </a>
	</ItemTemplate>
</asp:DataList>
<div id="pnlMore" runat="server" visible="false" class="text-right">
	<asp:LinkButton ID="lnkMore" runat="server" OnClick="lnkMore_Click"/>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>