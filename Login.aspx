<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AspNetDating.Login" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <article class="no-sidebar">
        <uc1:WideBoxStart id="WideBoxStart1" runat="server" />
            <asp:Panel ID="pnlLogin" runat="server">
                <components:ContentView ID="cvLoginBoxNotes" Key="LoginNotes" runat="server">
                Registration is 100% free and easy. Once registered - you have the opportunity
                to browse thousands of profiles with photos and to answer those you find interesting.
                </components:ContentView>
                <div class="actions">
                    <uc1:FlexButton ID="fbRegister" runat="server" RenderAs="Button" CssClass="btn-register" OnClick="lnkRegister_Click" SkinID="InnerRegister"/>
                </div>
            </asp:Panel>
        <uc1:WideBoxEnd id="WideBoxEnd1" runat="server" />
    </article>
</asp:Content>
