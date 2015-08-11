<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditGroupCtrl.ascx.cs" Inherits="AspNetDating.Admin.EditGroupCtrl" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<uc1:MessageBox id="MessageBox" runat="server"/>
<div class="panel clear-panel">
    <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Group Information") %></h4></div>
    <div class="panel-body">
        <div class="form-horizontal medium-width">
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Group Icon") %></label>
                <div class="col-sm-8">
                    <img class="img-thumbnail" src='<%= Config.Urls.Home%>/GroupIcon.ashx?groupID=<%= GroupID %>&width=150&height=150&diskCache=1' />
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Name") %></label>
                <div class="col-sm-8">
                    <asp:TextBox CssClass="form-control" ID="txtName" runat="server"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Categories") %></label>
                <div class="col-sm-8">
                    <asp:ListBox CssClass="form-control" ID="lbCategories" runat="server" SelectionMode="Multiple"/>
                    <div class="text-muted small"><i class="fa fa-lightbulb-o"></i>&nbsp;<%= Lang.TransA("Hold Ctrl for multiple selection") %></div>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Description") %></label>
                <div class="col-sm-8">
                    <asp:TextBox CssClass="form-control" ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="50"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Group Image") %></label>
                <div class="col-sm-8">
                    <asp:FileUpload ID="fuGroupImage" runat="server"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Owner") %></label>
                <div class="col-sm-8">
                    <asp:DropDownList CssClass="form-control" ID="ddOwner" runat="server"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Access Level") %></label>
                <div class="col-sm-8">
                    <asp:DropDownList CssClass="form-control" ID="ddAccessLevel" runat="server"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Approved") %></label>
                <div class="col-sm-8">
                    <asp:DropDownList CssClass="form-control" ID="ddApproved" runat="server"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Minimum age restriction") %></label>
                <div class="col-sm-8">
                    <asp:DropDownList CssClass="form-control" ID="ddAgeRestriction" runat="server"/>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-4 col-sm-8">
                    <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" OnClick="btnCancel_Click"/>
                    <asp:Button CssClass="btn btn-primary pull-right" ID="btnSave" runat="server" OnClick="btnSave_Click"/>
                </div>
            </div>
        </div>
    </div>
</div>