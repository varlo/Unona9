<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveAnswer.aspx.cs" Inherits="AspNetDating.Admin.ApproveAnswer" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="panel clear-panel">
        <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Approve Answer")%></h4></div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="medium-width">
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><%= Lang.TransA("Username") %>:</label>
                        <div class="col-sm-8"><asp:Label id="lblUsername" runat="server"/></div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><%= Lang.TransA("Question") %>:</label>
                        <div class="col-sm-8"><asp:Label id="lblQuestion" runat="server"/></div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-4 control-label"><%= Lang.TransA("Answer") %>:</label>
                        <div class="col-sm-8"><asp:TextBox CssClass="form-control" Width="100%" Rows="5" id="txtAnswer" runat="server" TextMode="MultiLine"/></div>
                    </div>
                </div>
                <hr />
                <div class="medium-width">
                    <div class="col-sm-8 col-sm-offset-4 text-right">
                        <asp:Button CssClass="btn btn-default pull-left" id="btnCancel" runat="server" onclick="btnCancel_Click"/>
                        <asp:Button CssClass="btn btn-primary" id="btnReject" runat="server" onclick="btnReject_Click"/>
                        <asp:Button CssClass="btn btn-secondary" id="btnSaveAndApprove" runat="server" onclick="btnSaveAndApprove_Click"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
