<%@ Import namespace="System.Globalization"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditAffiliateCtrl.ascx.cs" Inherits="AspNetDating.Admin.EditAffiliateCtrl" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>

<uc1:MessageBox id="MessageBox" runat="server"/>
<div class="panel clear-panel">
    <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Affilaite Information") %></h4></div>
    <div class="panel-body">
        <div class="form-horizontal">
            <div class="medium-width">
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Username") %></label>
                    <div class="col-sm-8"><p class="form-control-static"><%= Affiliate.Username %></p></div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Password") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtPassword" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Name") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtName" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Email")%></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtEmail" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Site URL") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtSiteURL" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Payment details") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtPaymentDetails" runat="server" TextMode="MultiLine" Rows="5" Columns="50"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Deleted") %></label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddDeleted" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Active") %></label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddActive" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Percentage") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control form-control-inline" size="6" ID="txtPercentage" runat="server"/>&nbsp;%
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Fixed amount") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control form-control-inline" size="6" ID="txtFixedAmount" runat="server"/>&nbsp;<%= CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol %>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Recurrent") %></label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddRecurrent" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-4"><%= Lang.TransA("Balance") %></label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control form-control-inline" size="6" ID="txtBalance" runat="server"/>&nbsp;<%= CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol %>
                    </div>
                </div>
            </div>
            <hr />
            <div class="medium-width">
                <div class="form-group">
                    <div class="col-sm-8 col-sm-offset-4">
                        <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" OnClick="btnCancel_Click"/>
                        <asp:Button CssClass="btn btn-primary pull-right" ID="btnSave" runat="server" OnClick="btnSave_Click"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
