<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AffiliateBanners.aspx.cs" Inherits="AspNetDating.Admin.AffiliateBanners" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc1:MessageBox ID="MessageBox" runat="server"/>
        <asp:MultiView ID="mvAffiliateBanners" runat="server">
        <asp:View ID="viewAffiliateBanners" runat="server">
            <asp:DataGrid CssClass="table table-striped" ID="dgGroups" GridLines="None" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" OnItemCommand="dgGroups_ItemCommand" OnItemDataBound="dgGroups_ItemDataBound">
                <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
                <Columns>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <img class="img-thumbnail" src='<%= Config.Urls.Home%>/AffiliateBannerImage.ashx?id=<%# Eval("ID") %>' width="100px"/>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <%# Eval("Name")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-trash-o"></i>&nbsp;<%= Lang.TransA("Delete")%></asp:LinkButton>
                            <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-edit"></i>&nbsp;<%= Lang.TransA("Edit")%></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            <div class="actions">
                <asp:LinkButton CssClass="btn btn-secondary" ID="btnAddAffiliateBanner" runat="server" OnClick="btnAddAffiliateBanner_Click" />
            </div>
        </asp:View>
        <asp:View ID="viewEditAffiliateBanners" runat="server">
            <div class="panel clear-panel">
                <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Edit Banner")%></h4></div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="medium-width">
                            <div class="form-group">
                                <div class="col-sm-8 col-sm-offset-4">
                                    <img class="img-thumbnail" src='<%= Config.Urls.Home%>/AffiliateBannerImage.ashx?id=<%= EditedAffiliateBannerID %>'/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.TransA("Affiliate banner")%>:</label>
                                <div class="col-sm-8">
                                    <p class="form-control-static"><asp:FileUpload ID="fuNewBannerImage" runat="server" /></p>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.TransA("Name")%>:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox CssClass="form-control" ID="txtNewName" runat="server"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.TransA("Deleted")%>:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList CssClass="form-control" ID="ddDeleted" runat="server"/>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="medium-width">
                            <div class="col-sm-8 col-sm-offset-4">
                                <asp:Button CssClass="btn btn-default" ID="btnCancelUpdate" runat="server" OnClick="btnCancelUpdate_Click"/>
                                <asp:Button CssClass="btn btn-primary pull-right" ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="viewUploadAffiliateBanner" runat="server">
            <div class="panel clear-panel">
                <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Affiliate Banner")%></h4></div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="medium-width">
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.TransA("Affiliate banner")%>:</label>
                                <div class="col-sm-8">
                                    <p class="form-control-static"><asp:FileUpload ID="fuAffiliateBannerImage" runat="server" /></p>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4"><%= Lang.TransA("Name")%>:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox CssClass="form-control" ID="txtName" runat="server"/>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="medium-width">
                            <div class="col-sm-8 col-sm-offset-4">
                                <asp:Button CssClass="btn btn-default" ID="btnCancelUpload" runat="server" OnClick="btnCancelUpload_Click" />
                                <asp:Button CssClass="btn btn-primary pull-right" ID="btnUpload" runat="server" OnClick="btnUpload_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
		</asp:View>
    </asp:MultiView>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnUpload" />
        <asp:PostBackTrigger ControlID="btnUpdate" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
