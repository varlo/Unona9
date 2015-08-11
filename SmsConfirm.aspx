<%@ Page Language="c#" MasterPageFile="~/Site.Master" Codebehind="SmsConfirm.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating.SmsConfirm" %>
<%@ Register TagPrefix="cv" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
        <cv:ContentView ID="ContentView1" Key="SmsConfirm" runat="server"/>
        <div class="input-group">
            <span class="input-group-addon"><%= Lang.Trans("Confirmation code") %></span>
            <asp:TextBox CssClass="form-control" ID="txtConfirmationCode" runat="server"/>
            <span class="input-group-btn"><asp:Button CssClass="btn btn-default" ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" /></span>
        </div>
    <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
</asp:Content>
