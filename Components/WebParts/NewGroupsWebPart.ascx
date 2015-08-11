<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewGroupsWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.NewGroupsWebPart" %>
<asp:DataList ID="dlNewGroups" CssClass="repeater-horizontal" Runat="server" SkinID="NewGroups" RepeatLayout="Flow">
	<ItemStyle HorizontalAlign="Center"></ItemStyle>
    <ItemTemplate>
        <a class="thumbnail" href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'>
    		<img src='GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=100&height=100&diskCache=1'/>
    		<div class="caption">
                <%# Eval("Name") %>
                <div class="text-muted"><%# Eval("AccessLevel")%></div>
		    </div>
		</a>
    </ItemTemplate>
</asp:DataList>