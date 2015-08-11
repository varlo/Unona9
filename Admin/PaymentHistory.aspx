<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="PaymentHistory.aspx.cs" Inherits="AspNetDating.Admin.PaymentHistory" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc1:MessageBox ID="MessageBox" runat="server"/>

<asp:Panel ID="pnlPaymentHistoryPerPage" runat="server">
<p class="text-right">
    <small class="text-muted"><asp:Label ID="lblPaymentHistoryPerPage" runat="server"/>:</small>
    <asp:DropDownList ID="ddAffiliatesPaymentHistoryPerPage"  CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddAffiliatesPaymentHistoryPerPage_SelectedIndexChanged"/>
</p>
</asp:Panel>
<div class="table-responsive">
<asp:GridView ID="gvPaymentHistory" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="1" CssClass="table table-striped" HorizontalAlign="Center" PagerSettings-Mode="Numeric" OnPageIndexChanging="gvPaymentHistory_PageIndexChanging" GridLines="None">
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("AffiliateUsername") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("Amount") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("DatePaid") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("Notes") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("PrivateNotes") %>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
