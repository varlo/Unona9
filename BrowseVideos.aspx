<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BrowseVideos.aspx.cs" Inherits="AspNetDating.BrowseVideos" %>
<%@ Import Namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <aside>
        <uc1:smallboxstart id="SmallBoxStart2" runat="server" />
        <div class="form-horizontal form-sm">
            <div class="form-group">
				<label class="control-label col-sm-4"><%= Lang.Trans("Video") %></label>
    			<div class="col-sm-8"><asp:DropDownList CssClass="form-control" ID="ddVideoType" runat="server" AutoPostBack="true" onselectedindexchanged="ddVideoType_SelectedIndexChanged" /></div>
            </div>
            <div class="form-group" id="pnlGenderFilterOnline" runat="server">
            	<label class="control-label col-sm-4"><%= Lang.Trans("Gender") %></label>
				<div class="col-sm-8"><asp:DropDownList CssClass="form-control" ID="ddGender" runat="server" /></div>
            </div>
            <div class="form-group" id="pnlAgeFilterOnline" runat="server">
                <label class="control-label col-sm-4"><%= Lang.Trans("Age") %></label>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtFromAge" runat="server" CssClass="form-control form-control-inline" size="2" MaxLength="2" />
                    <%= Lang.Trans("to") %>
                    <asp:TextBox ID="txtToAge" runat="server" CssClass="form-control form-control-inline" size="2" MaxLength="2" />
                </div>
            </div>
            <div class="form-group" id="pnlKeyword" runat="server" visible="false">
                <label class="control-label col-sm-4"><%= "Keyword".Translate() %></label>
                <div class="col-sm-8"><asp:TextBox ID="txtKeyword" CssClass="form-control" runat="server" /></div>
            </div>
	        <div class="actions"><asp:Button CssClass="btn btn-default" ID="btnSearch" runat="server" onclick="btnSearch_Click" /></div>
        </div>
        <uc1:smallboxend id="SmallBoxEnd2" runat="server" />
    </aside>
    <article>
        <uc1:largeboxstart id="LargeBoxStart" runat="server" />
        <asp:UpdatePanel ID="UpdatePanelVideos" runat="server">
            <ContentTemplate>
                <div class="table-responsive clearfix">
                <asp:DataList id="dlVideos" CssClass="repeater-horizontal" RepeatLayout="flow" runat="server" EnableViewState="false">
	                <ItemTemplate>
                        <a class="thumbnail" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                            <img src='<%# Eval("ThumbnailUrl") %>' />
                            <div class="caption">
                                <div><b><%# Eval("Title") %></b></div>
                                <%# Eval("Username") %>
                                <div class="text-muted">
                                    <span id="pnlGenderAge" runat="server" ><%# Lang.Trans(Eval("Gender").ToString()) %><span id="pnlDelimiter" runat="server">/</span><%# Eval("Age") %></span>
                                </div>
                            </div>
                        </a>
	                </ItemTemplate>
	            </asp:DataList>
	            </div>
	            <asp:Panel ID="pnlPaginator" Visible="True" Runat="server">
                    <ul class="pager">
	                    <li><asp:LinkButton id="lnkFirst" runat="server" OnClick="lnkFirst_Click" /></li>
	                    <li><asp:LinkButton id="lnkPrev" runat="server" OnClick="lnkPrev_Click" /></li>
	                    <li class="text-muted"><asp:Label id="lblPager" runat="server" /></li>
	                    <li><asp:LinkButton id="lnkNext" runat="server" OnClick="lnkNext_Click" /></li>
	                    <li><asp:LinkButton id="lnkLast" runat="server" OnClick="lnkLast_Click" /></li>
                    </ul>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc1:largeboxend id="LargeBoxEnd" runat="server" />
    </article>
</asp:Content>
