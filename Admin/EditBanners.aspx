<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditBanners.aspx.cs" Inherits="AspNetDating.Admin.EditBanners" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
	<div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Edit Banner") %></h4></div>
	<div class="panel-body medium-width">
        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-sm-2"><%= "Position".TranslateA() %></label>
                <div class="col-sm-10"><asp:DropDownList CssClass="form-control" ID="ddPosition" runat="server" AutoPostBack="true" onselectedindexchanged="ddPosition_SelectedIndexChanged"/></div>
            </div>
            <div id="pnlBannerCode" runat="server" visible="false">
                <div class="form-group">
                    <div class="col-sm-10 col-sm-offset-2">
                        <asp:Label ID="lblDescription" runat="server"/>
                        <img class="img-thumbnail" id="imgBannerCode" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2"><%= "Target".TranslateA() %></label>
                    <div class="col-sm-10"><asp:DropDownList CssClass="form-control" ID="ddTarget" runat="server" AutoPostBack="true" onselectedindexchanged="ddTarget_SelectedIndexChanged"/></div>
                </div>
                <div class="form-group" id="pnlTarget" runat="server" visible="false">
                    <div class="col-sm-10 col-sm-offset-2">
                        <hr />
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= "Visible for".TranslateA() %></label>
                            <div class="col-sm-9"><asp:DropDownList CssClass="form-control" ID="ddVisibleFor" runat="server"/></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= "Name".TranslateA() %></label>
                            <div class="col-sm-9"><asp:TextBox CssClass="form-control" ID="txtName" runat="server"/></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= "Country".TranslateA() %></label>
                            <div class="col-sm-9"><select class="form-control" ID="ddCountry" enableviewstate="false" runat="server"></select></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= "Region".TranslateA() %></label>
                            <div class="col-sm-9"><select class="form-control" ID="ddRegion" runat="server"></select></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= "City".TranslateA() %></label>
                            <div class="col-sm-9"><select class="form-control" ID="ddCity" runat="server"></select></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= "Gender".TranslateA() %></label>
                            <div class="col-sm-9"><asp:DropDownList CssClass="form-control" ID="ddGender" runat="server"/></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= "Paid".TranslateA() %></label>
                            <div class="col-sm-9"><asp:DropDownList CssClass="form-control" ID="ddPaid" runat="server"/></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= "Age range" %></label>
                            <div class="col-sm-9"><asp:DropDownList CssClass="form-control form-control-inline" ID="ddFromAge" runat="server"/>
                            <%= "to".TranslateA() %>
                            <asp:DropDownList CssClass="form-control form-control-inline" ID="ddToAge" runat="server"/></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3"><%= "Priority".TranslateA() %></label>
                            <div class="col-sm-9"><asp:DropDownList CssClass="form-control" ID="ddPriority" runat="server"/></div>
                        </div>
                        <hr />
                    </div>
                </div><!-- /#pnlBannerCode -->
                <div class="form-group">
                    <div class="col-sm-10 col-sm-offset-2">
                        <%= Lang.TransA("Paste advertisment code below.")%>
                        <asp:TextBox CssClass="form-control" ID="txtBannerCode" runat="server" TextMode="MultiLine" Rows="10" Columns="50"/>
                    </div>
                </div>
                <div class="actions">
                    <asp:Button CssClass="btn btn-primary" ID="btnDelete" runat="server" Visible="false" onclick="btnDelete_Click"/>
                    <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" onclick="btnSave_Click" />
                </div>

            </div>
        </div>
    </div>
</div>
</asp:Content>
