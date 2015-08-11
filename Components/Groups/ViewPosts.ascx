<%@ Import namespace="AspNetDating"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPosts.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewPosts" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="GroupPoll" Src="~/Components/Groups/GroupPoll.ascx" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
<asp:Label ID="lblError" runat="server" CssClass="alert text-danger" EnableViewState="False"/>
<b><%= Lang.Trans("Topic") %>:</b>&nbsp;<asp:Label ID="lblTopicName" runat="server"/>
<uc1:GroupPoll id="GroupPoll1" runat="server"/>
<asp:MultiView ID="mvPosts" runat="server">
<asp:View ID="viewPosts" runat="server">
    <asp:DataList ID="dlGroupPosts" runat="server" HorizontalAlign="Center" Width="100%" ShowHeader="false" CellPadding="0" OnItemCommand="dlGroupPosts_ItemCommand" OnItemDataBound="dlGroupPosts_ItemDataBound" EnableViewState="false">
        <ItemTemplate>
            <ul class="info-header info-header-sm">
                <li><a class="tooltip-link" title="<%= Lang.Trans("Posted on") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("DatePosted")%></a></li>
                <li><a class="tooltip-link" title="<%= Lang.Trans("Posted by") %>"><i class="fa fa-user"></i></a>&nbsp;
                    <a href='ShowUser.aspx?uid=<%# Eval("Username") %>' >
                       <%# (bool)Eval("IsWarned") ? String.Format("<a class=\"tooltip-link\" title=\"{0}\"><i class=\"fa fa-exclamation text-danger\"></i></a>&nbsp;", String.Format("Warned:{0}".Translate(), Eval("WarnReason"))) : String.Empty%>
                       <%# Eval("Username") %>
                    </a>
                <%# (!(bool) Eval("HideUserLevelIcon")) && Eval("UserLevel") is UserLevel ?
                    String.Format("<a class=\"tooltip-link\" title=\"{2}\"><span class=\"fa-stack fa-lg fa-badge\"><i class=\"fa fa-certificate fa-stack-2x\"></i><i class=\"fa fa-stack-1x fa-inverse\">{1}</i></span></a>",
                                                ((UserLevel)Eval("UserLevel")).GetIconUrl(), String.Format(Lang.Trans("{0}"), ((UserLevel)Eval("UserLevel")).LevelNumber), ((UserLevel)Eval("UserLevel")).Name) : ""%>
                </li>
                <li id="pnlDateEdited" runat="server"><a class="tooltip-link" title="<%= Lang.Trans("Edited on") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("DateEdited")%></a></li>
                <li id="pnlEditNotes" runat="server"><a class="tooltip-link" title="<%= Lang.Trans("Reason to edit") %>"><label><%= Lang.Trans("Reason to edit") %></label>&nbsp;<%# Eval("EditNotes")%></a></li>
            </ul>
            <p>
                <asp:Literal ID="ltrPost" runat="server" Text='<%# Eval("Post") %>'/>
            </p>
            <div class="btn-group btn-group-sm pull-right">
                <span id="liReply" runat="server">
                    <asp:LinkButton CssClass="btn btn-default" ID="lnkReply" CommandName="Reply" CommandArgument='<%# Eval("GroupPostID") %>' Runat="server"/>
                </span>
                <span id="liEdit" runat="server">
                    <asp:LinkButton CssClass="btn btn-default" ID="lnkEdit"  CommandName="Edit" CommandArgument='<%# Eval("GroupPostID") %>' Runat="server"/>
                </span>
                <span id="liDelete" runat="server">
                    <asp:LinkButton CssClass="btn btn-default" ID="lnkDelete" CommandName="Delete" CommandArgument='<%# Eval("GroupPostID") %>' Runat="server"/>
                </span>
            </div>
        </ItemTemplate>
    	<SeparatorTemplate>
			<div class="line"></div>
    	</SeparatorTemplate>
    </asp:DataList>
    <div class="line"></div>
    <asp:Panel ID="pnlPaginator" runat="server">
	    <ul class="pager">
            <li><asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"/></li>
            <li><asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"/></li>
            <li class="text-muted"><asp:Label ID="lblPager" runat="server"/></li>
            <li><asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"/></li>
            <li><asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"/></li>
        </ul>
    </asp:Panel>
</asp:View>
<asp:View ID="viewDeleteOptions" runat="server">
    <%= Lang.Trans("What do you want to do?") %><div class="separator6"></div>
    <asp:RadioButtonList ID="rbList" runat="server" />
    <div class="separator"></div>
    <asp:UpdatePanel ID="UpdatePanelWarn" runat="server">
        <ContentTemplate>
            <asp:CheckBox ID="cbKickMember" runat="server"
                AutoPostBack="true" oncheckedchanged="cbKickMember_CheckedChanged"/>
            <br />
            <asp:CheckBox ID="cbBanMember" runat="server" AutoPostBack="true" oncheckedchanged="cbBanMember_CheckedChanged"/>
            <asp:DropDownList ID="ddBanPeriod" CssClass="dropdownlist" runat="server"/>
            <div id="pnlWarn" runat="server">
                <asp:CheckBox ID="cbWarn" runat="server" AutoPostBack="true" oncheckedchanged="cbWarn_CheckedChanged"/>
                <asp:DropDownList ID="ddWarnExpirationDate"  CssClass="dropdownlist" runat="server"/>
                <asp:TextBox ID="txtWarnReason" runat="server" CssClass="textbox" Visible="false"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="separator"></div>
    <div class="buttons">
	    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
	    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
    </div>
    </asp:View>
</asp:MultiView>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>