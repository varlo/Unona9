<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Groups.aspx.cs" Inherits="AspNetDating.Groups" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="~/Components/Groups/SearchResults.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server" />
	        <ul class="nav">
		        <li><asp:linkbutton id="lnkBrowseGroups" Runat="server" OnClick="lnkBrowseGroups_Click" /></li>
		        <li id="pnlMyGroups" runat="server"><asp:linkbutton id="lnkMyGroups" Runat="server" OnClick="lnkMyGroups_Click" /></li>
		        <li><asp:linkbutton id="lnkNewGroups" Runat="server" OnClick="lnkNewGroups_Click" /></li>
		        <li id="pnlPendingInvitations" runat="server"><asp:linkbutton id="lnkPendingInvitations" Runat="server" OnClick="lnkPendingInvitations_Click" /></li>
		        <li id="pnlCreateGroup" runat="server"><asp:linkbutton id="lnkCreateGroup" Runat="server" OnClick="lnkCreateGroup_Click" /></li>
		    </ul>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server" />
	</aside>
	<article>
        <asp:UpdatePanel ID="UpdatePanelGroups" runat="server">
        <ContentTemplate>
		<uc1:largeboxstart id="LargeBoxStart1" runat="server" />
		<components:BannerView id="bvGroupsRightTop" runat="server" Key="GroupsRightTop" />
		    <asp:Label ID="lblError" CssClass="alert text-danger" runat="server" EnableViewState="False" />
			<asp:MultiView ID="mvGroups" runat="server">
			    <asp:View ID="viewCategories" runat="server">
			        <asp:Panel ID="pnlFilterGroup" class="input-group input-group-sm filter" DefaultButton="btnSearchGroup" runat="server">
                        <span class="input-group-addon"><%= Lang.Trans("keywords") %>:</span>
                        <asp:TextBox ID="txtGroupToSearch" CssClass="form-control" runat="server" />
                        <span class="input-group-addon">
                            <div class="checkbox"><label><asp:CheckBox ID="cbSearchInDescription" runat="server" /><%= Lang.Trans("include description") %></label></div>
                        </span>
                        <div class="input-group-btn"><asp:Button ID="btnSearchGroup" CssClass="btn btn-default" runat="server" OnClick="btnSearchGroup_Click" /></div>
			        </asp:Panel>
			        <asp:DataList ID="dlCategories" SkinID="Categories" GridLines="None" CssClass="table table-striped" runat="server" OnItemCommand="dlCategories_ItemCommand">
			            <ItemStyle Width="50%" VerticalAlign="Top" HorizontalAlign="Left"></ItemStyle>
			            <ItemTemplate>
			                <asp:LinkButton id="lnkViewGroups" runat="server" CommandName="ViewGroups" CommandArgument='<%# Eval("CategoryID") %>'><%# Eval("Name") %></asp:LinkButton>
			                <a title='<%= Lang.Trans("Number of groups") %>' href="#"><span class="badge"><%# Eval("GroupsNumber")%></span></a>
					    </ItemTemplate>
			        </asp:DataList>
			    </asp:View>
			    <asp:View ID="viewGroups" runat="server">
			        <div id="pnlSearchParameters" runat="server">
			            <asp:Panel ID="pnlFilterGroupByCategory" class="input-group input-group-sm filter" DefaultButton="btnSearchGroupByCategory" runat="server">
			                <span class="input-group-addon"><%= Lang.Trans("keywords") %>:</span>
			                <asp:TextBox ID="txtSearchGroupByCategory" CssClass="form-control" runat="server" />
			                <span class="input-group-addon">
                                <div class="checkbox"><asp:CheckBox ID="cbSearchInDescriptionByCategory" runat="server" /><%= Lang.Trans("include description") %></div>
                            </span>
			                <div class="input-group-btn"><asp:Button ID="btnSearchGroupByCategory" CssClass="btn btn-default" runat="server" OnClick="btnSearchGroupByCategory_Click" /></div>
			            </asp:Panel>
          			</div> 
			        <asp:DataList ID="dlGroups" CssClass="table table-striped" runat="server" OnItemCreated="dlGroups_ItemCreated" OnItemCommand="dlGroups_ItemCommand">
			            <ItemTemplate>
			            	<div class="media">
                                <a class="pull-left" href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'>
                                    <img class="media-object img-thumbnail" src='GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=120&height=120' border="0"/>
                                </a>
                                <div class="media-body">
                                    <h4 class="media-heading">
                                        <a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'><%# Eval("Name") %></a>
                                        <div id="pnlPendingActions" class="btn-group btn-group-xs pull-right" runat="server" visible='<%# ShowPnlPendingActions %>'>
                                            <asp:LinkButton CssClass="btn btn-default" ID="lnkAccept" runat="server" CommandArgument='<%# Eval("GroupID") %>' CommandName="Accept"/>
                                            <asp:LinkButton CssClass="btn btn-default" ID="lnkReject" runat="server" CommandArgument='<%# Eval("GroupID") %>' CommandName="Reject"/>
                                        </div>
                                    </h4>
                                    <ul class="info-header info-header-sm">
                                        <li><a class="tooltip-link" title="<%= Lang.Trans("Date Created") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("DateCreated")%></a></li>
                                        <li><a class="tooltip-link" title="<%= Lang.Trans("Access Level") %>"><i class="fa fa-globe"></i>&nbsp;<%# Eval("AccessLevel")%></a></li>
                                        <li><a class="tooltip-link" title="<%= Lang.Trans("Members") %>"><i class="fa fa-users"></i>&nbsp;<%# Eval("MembersCount")%></a></li>
                                        <li <%# Eval("MemberType").ToString() == "" ? "style=\"display: none\"" : "" %>><a class="tooltip-link" title="<%= Lang.Trans("Member Type") %>"><i class="fa fa-user"></i>&nbsp;<%# Eval("MemberType")%></a></li>
                                        <li class="pull-right" <%# Eval("Pending").ToString() == "" ? "style=\"display: none\"" : "" %>><a class="tooltip-link" title="<%= Lang.Trans("Group Status") %>"><i class="fa fa-spinner"></i>&nbsp;<%# Eval("Pending") %></a></li>
                                    </ul>
                                    <%# Eval("Description") %>
                                    <a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>' id="lnkDescriptionMore" runat="server" visible='<%# Eval("IsVisibleLinkMore") %>'>
                                        <%# Lang.Trans("more") %>
                                    </a>
                                </div>
			            	</div>
			            </ItemTemplate>
			        </asp:DataList>
			        <asp:Panel ID="pnlPaginator" runat="server">
		            <ul class="pager">
                        <li><asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click" /></li>
                        <li><asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click" /></li>
                        <li class="text-muted"><asp:Label ID="lblPager" runat="server" /></li>
                        <li><asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click" /></li>
                        <li><asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click" /></li>
                    </ul>
                    </asp:Panel>
			    </asp:View>
			    <asp:View ID="viewGroupSearchResults" runat="server">
			        <uc1:SearchResults id="SearchResults1" runat="server" />
			    </asp:View>
			</asp:MultiView>
			<components:BannerView id="bvGroupsRightBottom" runat="server" Key="GroupsRightBottom" />
			<uc1:largeboxend id="LargeBoxEnd1" runat="server" />
			</ContentTemplate>
			</asp:UpdatePanel>
	</article>
</asp:Content>
