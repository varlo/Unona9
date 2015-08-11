<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="LoginBox.ascx.cs" Inherits="AspNetDating.Components.LoginBox" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>

<asp:Panel ID="pnlLogin" runat="server" DefaultButton="fbLogin">
    <div class="panel panel-login">
        <div class="panel-body">
            <div class="login-row">
                <div class="col" id="pnlLoginButtons" runat="server">
                    <span id="divFacebook" runat="server">
                        <asp:LinkButton CssClass="btn btn-sm" ID="btnUseFacebook" runat="server" OnClick="btnUseFacebook_Click"><i class="fa fa-facebook fa-lg"></i></asp:LinkButton>
                    </span>
                </div>
                <div class="col">
                    <label class="sr-only"><%= Lang.Trans("Username") %></label>
                    <asp:TextBox ID="txtUsername" TabIndex="1" class="form-control input-sm" runat="server" Placeholder="Username" />
                    <div class="help-block">
                        <label class="checkbox-inline"><small><asp:CheckBox TabIndex="4" ID="cbRememberMe" runat="server" /><%= Lang.Trans("remember me")%></small></label>
                        <label class="checkbox-inline"><small><asp:CheckBox  TabIndex="5" ID="cbStealthMode" runat="server" /></small></label>
                    </div>
                </div>
                <div class="col">
                    <label class="sr-only"><%= Lang.Trans("Password") %></label>
                    <asp:TextBox ID="txtPassword" TabIndex="2" class="form-control input-sm" runat="server" TextMode="Password" Placeholder="Password" />
                    <div class="help-block">
                        <small><asp:LinkButton TabIndex="6" ID="fbForgotPassword" runat="server" SkinID="ForgotPassword" OnClick="fbForgotPassword_Click" /></small>
                    </div>
                </div>
                <div class="col">
                    <asp:LinkButton TabIndex="3" CssClass="btn btn-primary btn-sm" ID="fbLogin" runat="server" OnClick="fbLogin_Click" SkinID="Login" />
                </div>
            </div>
        </div><!-- /.panel-body -->
    </div><!-- /.panel-login -->  
</asp:Panel>
