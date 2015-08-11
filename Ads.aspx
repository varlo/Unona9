<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ads.aspx.cs" Inherits="AspNetDating.Ads" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<asp:UpdatePanel ID="UpdatePanelCategories" runat="server">
	    <ContentTemplate>
            <aside>
                <uc1:smallboxstart id="SmallBoxStart1" runat="server" />
                    <ul class="nav">
                        <li id="pnlAllAds" runat="server"><asp:linkbutton ID="lnkAllAds" runat="server" onclick="lnkAllClassifieds_Click"/></li>
                        <li id="pnlMyAds" runat="server"><asp:linkbutton ID="lnkMyAds" runat="server" onclick="lnkMyClassifieds_Click"/></li>
                        <li id="pnlPostAd" runat="server"><asp:linkbutton ID="lnkPostAd" runat="server" onclick="lnkPostAd_Click"/></li>
                    </ul>
                <uc1:smallboxend id="SmallBoxEnd1" runat="server" />
            </aside>
            <article>
                <uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
                    <div class="input-group input-group-sm filter">
                        <span class="input-group-addon"><%= "Keyword".Translate() %>:</span>
                        <asp:TextBox ID="txtKeyword" CssClass="form-control" runat="server" />
                        <span class="input-group-btn"><uc1:FlexButton ID="fbSearch" CssClass="btn btn-default" runat="server" RenderAs="Button" OnClick="lnkSearch_Click"/></span>
                    </div>
                    <asp:Label ID="lblError" CssClass="alert text-danger" runat="server" EnableViewState="False"/>
                    <asp:MultiView ID="mvCategories" runat="server">
                        <asp:View ID="viewCategories" runat="server">
                            <asp:DataList ID="dlCategories" CssClass="table" RepeatColumns="2" runat="server" onitemdatabound="dlCategories_ItemDataBound">
                                <ItemTemplate>
                                    <h4><%# Eval("Title") %></h4>
                                    <asp:Repeater ID="rptSubcategories" runat="server">
                                        <HeaderTemplate>
                                            <ul class="list-group">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li class="list-group-item"><a href='Ads.aspx?cid=<%# Eval("ID")%>'><%# Eval("Title") %></a></li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:DataList>
                        </asp:View>
                        <asp:View ID="viewAds" runat="server">
                            <asp:DataList CssClass="table table-striped" ID="dlAds" runat="server">
                                <ItemTemplate>
                                    <div class="media">
                                        <a class="pull-left" href='ShowAd.aspx?id=<%# Eval("ID") %>'>
                                            <img class="img-thumbnail" src='AdPhoto.ashx?id=<%# Eval("AdPhotoID") %>&width=50&height=50&diskCache=1' />
                                        </a>
                                        <div class="media-body">
                                            <h4 class="media-heading"><a href='ShowAd.aspx?id=<%# Eval("ID") %>'><%# Eval("Subject") %></a>&nbsp;<small class="text-muted"><%# Eval("Pending") %></small></h4>
                                            <ul class="info-header info-header-sm">
                                                <li><a class="tooltip-link" title="<%= Lang.Trans("Posted On") %>"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("Date") %></a></li>
                                                <li><a class="tooltip-link" title="<%= Lang.Trans("Posted By") %>"><i class="fa fa-user"></i></a>&nbsp;<a href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("PostedBy"))%>'><%# Eval("PostedBy") %></a></li>
                                            </ul>
                                        </div>
                                     </div>
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:Panel ID="pnlPaginator" runat="server">
                                <ul class="pager">
                                    <li><asp:LinkButton ID="lnkFirst" runat="server" onclick="lnkFirst_Click"/></li>
                                    <li><asp:LinkButton ID="lnkPrev" runat="server" onclick="lnkPrev_Click"/></li>
                                    <li class="text-muted"><asp:Label ID="lblPager" runat="server"/></li>
                                    <li><asp:LinkButton ID="lnkNext" runat="server" onclick="lnkNext_Click"/></li>
                                    <li><asp:LinkButton ID="lnkLast" runat="server" onclick="lnkLast_Click"/></li>
                                </ul>
                            </asp:Panel>
                        </asp:View>
                    </asp:MultiView>
                <uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
            </article>
	    </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
