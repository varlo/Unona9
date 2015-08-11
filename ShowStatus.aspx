<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="ShowStatus.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.ShowStatus" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<article class="no-sidebar">
    <uc1:WideBoxStart id="WideBoxStart1" runat="server"/>
        <asp:Label ID="lblMessage" runat="server" />
        <asp:PlaceHolder ID="plhLinkContainer" runat="server"/>
        <div class="actions">
            <uc1:FlexButton CssClass="btn btn-default" ID="fbStatus" RenderAs="Button" runat="server" />
        </div>
    <uc1:WideBoxEnd id="WideBoxEnd1" runat="server"/>
</article>
</asp:Content>	
