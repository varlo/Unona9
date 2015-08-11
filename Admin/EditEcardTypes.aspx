<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditEcardTypes.aspx.cs" Inherits="AspNetDating.Admin.EditEcardTypes" %>
<%@ Import Namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
	<div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("E-cards") %></h4></div>
	<div class="panel-body">
        <div class="form-horizontal">
            <div class="medium-width">
                <div class="form-group" id="trLanguage" runat="server">
                    <label class="col-sm-3 control-label"><%= Lang.TransA("Select e-card:") %></label>
                    <div class="col-sm-9"><asp:dropdownlist CssClass="form-control" id="ddName" AutoPostBack="True" Runat="server" onselectedindexchanged="ddName_SelectedIndexChanged"/></div>
                </div>
                <div id="pnlEditEcardType" runat="server" Visible="False">
                    <hr />
                    <h4><%= Lang.TransA("Edit Page") %></h4>
                    <hr />
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Name") %></label>
                        <div class="col-sm-9"><asp:textbox CssClass="form-control" id="txtName" Runat="server"/></div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-9 col-sm-offset-3" id="tdImage" runat="server">
                            <img class="img-thumbnail" src='..\<%= "EcardContent.ashx?ect=" + ddName.SelectedValue %>' />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-9 col-sm-offset-3" id="tdFlash" runat="server">
                            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                                id="flvplayer">
                                <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/<%= "EcardContent.ashx?ect=" + ddName.SelectedValue + "&seed=" + new Random().NextDouble() %>">
                                <param name="quality" value="high">
                                <param name="bgcolor" value="#FFFFFF">
                                <param name="wmode" value="transparent">
                                <param name="allowfullscreen" value="true">
                                <param name="flashvars" value="file=..\<%= "EcardContent.ashx?ect=" + ddName.SelectedValue + "&seed=" + new Random().NextDouble() %>&shuffle=false" />
                                <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/<%= "EcardContent.ashx?ect=" + ddName.SelectedValue + "&seed=" + new Random().NextDouble() %>"
                                    quality="high" wmode="transparent" bgcolor="#FFFFFF"
                                    name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                    pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="file=..\<%= "EcardContent.ashx?ect=" + ddName.SelectedValue + "&seed=" + new Random().NextDouble() %>&shuffle=false"></embed></object>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("File") %></label>
                        <div class="col-sm-9"><p class="form-control-static"><asp:FileUpload ID="fuContent" runat="server" /></p></div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-9 col-sm-offset-3"><div class="checkbox"><label><%= Lang.TransA("Active")%><asp:CheckBox ID="cbActive" runat="server" /></label></div></div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label"><%= Lang.TransA("Credits required")%></label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtCreditsRequired" runat="server"/>
                            <small class="text-muted"><i class="fa fa-lightbulb-o"></i>&nbsp;<%= Lang.TransA("Credits required to send this e-card. Leave empty to use billing plan settings.")%></small>
                        </div>
                    </div>
                    <div class="actions">
                        <asp:Button CssClass="btn btn-primary" id="btnSave" Runat="server" onclick="btnSave_Click"/>
                        <asp:Button CssClass="btn btn-primary" ID="btnDelete" Runat="server" onclick="btnDelete_Click"/>
                    </div>
                </div>
            </div>
         </div>
    </div>
</div>



</asp:Content>
