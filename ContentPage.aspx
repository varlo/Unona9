<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="ContentPage.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.ContentPage" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <article class="no-sidebar">
        <div class="panel">
            <div class="panel-body" id="divContent" runat="server"></div>
        </div>
    </article>
</asp:Content>