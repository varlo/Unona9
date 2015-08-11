<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AffiliatePayments.aspx.cs" Inherits="AspNetDating.Admin.AffiliatePayments" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Import namespace="System.Globalization"%>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <uc1:messagebox id="MessageBox" runat="server"/>
    <asp:MultiView ID="mvAffiliateRequestPayments" runat="server">
        <asp:View ID="viewPaymentRequests" runat="server">
            <div class="panel clear-panel">
                <asp:Repeater ID="rptAffiliateRequestPayments" runat="server" OnItemCommand="rptAffiliateRequestPayments_ItemCommand" OnItemDataBound="rptAffiliateRequestPayments_ItemDataBound">
                    <ItemTemplate>
                        <h4><%= Lang.TransA("Affiliate Request Payment") %></h4>
                        <div class="form-horizontal medium-width">
                            <div class="form-group">
                                <label class="col-sm-4 control-label"><%= Lang.TransA("Username") %>:</label>
                                <div class="col-sm-8"><p class="form-control-static"><%# Eval("Username") %></p></div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label"><%= Lang.TransA("Balance") %>:</label>
                                <div class="col-sm-8"><p class="form-control-static"><%# Eval("Balance") %></p></div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label"><%= Lang.TransA("Payment Details") %>:</label>
                                <div class="col-sm-8"><p class="form-control-static"><%# Eval("PaymentDetails") %></p></div>
                            </div>
                            <div class="actions">
                                <asp:Button CssClass="btn btn-primary" ID="btnMarkPaid" runat="server" CommandName="Pay" CommandArgument='<%# Eval("AffiliateID") %>'/>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
			</div>
        </asp:View>
        <asp:View ID="viewPay" runat="server">
            <div class="panel clear-panel">
                <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Affiliate Payment Notes") %></h4></div>
                <div class="panel-body">
                    <div class="form-horizontal medium-width">
                        <div class="form-group">
                            <label class="col-sm-4 control-label"><%= Lang.TransA("Amount") %>:</label>
                            <div class="col-sm-8"><asp:TextBox CssClass="form-control" ID="txtAmount" runat="server"/><%= CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol %></div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label"><%= Lang.TransA("Notes") %>:</label>
                            <div class="col-sm-8"><asp:TextBox CssClass="form-control" ID="txtNotes" runat="server" TextMode="multiline" Rows="5" Columns="50"/></div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label"><%= Lang.TransA("Private Notes") %></label>
                            <div class="col-sm-8"><asp:TextBox CssClass="form-control" ID="txtPrivateNotes" runat="server" TextMode="multiline" Rows="5" Columns="50"/></div>
                        </div>
                        <div class="actions"><asp:Button CssClass="btn btn-primary" ID="btnPay" runat="server" OnClick="btnPay_Click"/></div>
                    </div>
                </div>
            </div>
            </asp:View>
    </asp:MultiView>
	</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
