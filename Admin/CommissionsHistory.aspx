<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CommissionsHistory.aspx.cs" Inherits="AspNetDating.Admin.CommissionsHistory" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc1:MessageBox ID="MessageBox" runat="server"/>
    <div class="form-horizontal small-width">
       <div class="form-group">
            <label class="col-sm-4 control-label"><%= Lang.TransA("Affiliate") %>:</label>
            <div class="col-sm-8"><asp:DropDownList CssClass="form-control" ID="ddAffiliates" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddAffiliates_SelectedIndexChanged"/></div>
        </div>
    </div>

    <asp:Panel ID="pnlAffiliateCommissionsPerPage" runat="server">
    <p class="text-right">
        <small class="text-muted"><asp:Label ID="lblAffiliateCommissionsPerPage"  runat="server"/>:</small>
        <asp:DropDownList ID="ddAffiliateCommissionsPerPage" CssClass="form-control form-control-inline input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddAffiliateCommissionsPerPage_SelectedIndexChanged"/>
    </p>
    </asp:Panel>
    <div class="table-responsive">
    <asp:GridView ID="gvCommissions" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="1" CssClass="table table-striped" PagerSettings-Mode="Numeric" OnPageIndexChanging="gvCommissions_PageIndexChanging" GridLines="None">
        <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("Username")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("Amount") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("TimeStamp")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("Notes") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
