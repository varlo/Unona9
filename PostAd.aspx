<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PostAd.aspx.cs" Inherits="AspNetDating.PostAd" %>
<%@ Import Namespace="AspNetDating.Classes"%>

<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<aside>
    <uc1:smallboxstart id="SmallBoxStart1" runat="server"/>
        <ul class="nav"><li><a href='Ads.aspx'><%= "All Classifieds".Translate() %></a></li></ul>
    <uc1:smallboxend id="SmallBoxEnd1" runat="server"/>
</aside>
<article>
    <uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
        <asp:Label ID="lblError" CssClass="alert text-danger" runat="server" EnableViewState="False"/>
        <asp:Panel ID="pnlEditAd" class="form-horizontal" runat="server">
                <div class="form-group">
                    <label class="control-label col-sm-2"><%= "Category".Translate() %>:</label>
                    <div class="col-sm-5">
                        <asp:DropDownList ID="ddCategories" CssClass="form-control" runat="server" AutoPostBack="true" onselectedindexchanged="ddCategories_SelectedIndexChanged"/>
                    </div>
                    <div class="col-sm-5">
                        <asp:DropDownList ID="ddSubcategories" runat="server" CssClass="form-control" Visible="false"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2"><%= "Subject".Translate() %>:</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtSubject" CssClass="form-control" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2"><%= "Location".Translate() %>:</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtLocation" CssClass="form-control" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2"><%= "Life Time".Translate() %>:</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddExpiration" CssClass="form-control" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2"><%= "Message".Translate() %>:</label>
                    <div class="col-sm-10">
                        <asp:PlaceHolder ID="phEditor" runat="server"/>
                    </div>
                </div>
        </asp:Panel>
        <asp:Panel ID="pnlUploadAdPhotos" runat="server">
            <ul class="repeater-horizontal">
            <asp:Repeater ID="rptAdPhotos" runat="server" onitemcommand="rptAdPhotos_ItemCommand" onitemcreated="rptAdPhotos_ItemCreated">
                <ItemTemplate>
                    <li>
                        <%# AspNetDating.AdPhoto.RenderImageTag((int)Eval("AdPhotoID"), 100, 100, null, true) %><br />
                        <asp:LinkButton CssClass="btn btn-default btn-block" ID="lnkDelete" runat="server" CommandArgument='<%# Eval("AdPhotoID") %>' CommandName="DeletePhoto"/>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
            </ul>
            <asp:Repeater ID="rptAddPhoto" runat="server">
                <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
                <ItemTemplate>
                    <li class="list-group-item">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-3 control-label"><%= "Image file".Translate() %></label>
                                <div class="col-sm-9"><p class="form-control-static"><asp:FileUpload ID="fuAdPhoto" runat="server" /></p></div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label"><%= "Image description".Translate() %></label>
                                <div class="col-sm-9"><asp:TextBox ID="txtAdPhotoDescription" CssClass="form-control" runat="server"/></div>
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
                <Footertemplate></ul></Footertemplate>
            </asp:Repeater>
        </asp:Panel>
        <div class="actions">
            <asp:Button ID="btnSave" CssClass="btn btn-default" runat="server" onclick="btnSave_Click" />
        </div>
    <uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
</article>
</asp:Content>
