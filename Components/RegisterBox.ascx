<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="RegisterBox.ascx.cs" Inherits="AspNetDating.Components.RegisterBox" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>

<asp:Panel ID="pnlRegister" runat="server">
    <div class="panel panel-register">
        <div class="panel-body">
            <h2><%= Lang.Trans("Join us") %></h2>
            <div class="register-text">
                <components:ContentView ID="cvLoginBoxNotes" Key="LoginBoxNotes" runat="server">
                    Registration is 100% free and easy. Once registered - you have the opportunity
                    to browse thousands of profiles with photos and to answer those you find interesting.
                </components:ContentView>
            </div>
            <div class="actions">
                <uc1:FlexButton CssClass="btn-register" ID="fbRegister" runat="server" OnClick="fbRegister_Click" SkinID="Register" />
            </div>
        </div>
    </div>
</asp:Panel>
