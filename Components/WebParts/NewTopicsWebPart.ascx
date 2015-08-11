<%@ Import namespace="AspNetDating"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewTopicsWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.NewTopicsWebPart" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:MultiView ID="mvNewTopics" ActiveViewIndex="0" runat="server">
<asp:View ID="vNewTopics" runat="server">
    <ul class="list-group list-group-striped">
    <asp:Repeater ID="rptNewGroupTopics" runat="server" Visible="false">
        <ItemTemplate>
            <li class="list-group-item">
                <!--<a href='ShowUser.aspx?uid=<%# Eval("Username") %>'><%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 50, 50, "img-thumbnail media-object", true, true, true) %></a>-->
                <a class="tooltip-link" title="<%= Lang.Trans("Group Name") %>"><b><%# Eval("GroupName") %></b></a>&nbsp;<i class="fa fa-angle-right text-muted"></i>&nbsp;<a href='<%# UrlRewrite.CreateShowGroupTopicsUrl((string) Eval("GroupID"), (string) Eval("GroupTopicID")) %>' title="<%= Lang.Trans("Topic") %>"><%# Eval("GroupTopicName") %></a>
                <ul class="info-header info-header-sm">
                    <li><a class="tooltip-link" title="<%= Lang.Trans("Date created") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("GroupTopicDateCreated")%></a></li>
                    <li><a class="tooltip-link" title="<%= Lang.Trans("Posted by") %>"><i class="fa fa-user"></i></a>&nbsp;<a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a></li>
                </ul>
            </li>
        </ItemTemplate>
    </asp:Repeater>
    </ul>
</asp:View>
<asp:View ID="vNoNewTopics" runat="server">
    <div class="text-center">
	    <%= Lang.Trans("There are no new group topics!") %>
    </div>
</asp:View>
</asp:MultiView>