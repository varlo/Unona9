<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Billing.ascx.cs" Inherits="AspNetDating.Components.Profile.Billing" %>
<uc1:LargeBoxStart ID="LargeBoxStart" runat="server"></uc1:LargeBoxStart>
<input id="hidUsername" type="hidden" name="hidUsername" runat="server" />
<div id="divPaymentType" runat="server">
    <uc1:HeaderLine ID="hlPaymentType" runat="server"></uc1:HeaderLine>
    <components:ContentView ID="cvPaymentTypeInfo" Key="PaymentTypeInfo" runat="server">
        <div class="hint">
            The subscription based membership allows you to use the site for a fixed subscription
            fee while with the credits based subscription you pay per-action (e.g. per message).
            If you are a regular site user then the subscription is a better choice. If you
            only log in occasionaly then you can use the credits based membership and pay only
            when you actually use it.
        </div>
    </components:ContentView>
    <div>
        <asp:RadioButton ID="radioSubscription" GroupName="PaymentType" AutoPostBack="true"
            OnCheckedChanged="radioPaymentType_CheckedChanged" runat="server" />
    </div>
    <div>
        <asp:RadioButton ID="radioCredits" GroupName="PaymentType" AutoPostBack="true" OnCheckedChanged="radioPaymentType_CheckedChanged"
            runat="server" />
    </div>
</div>
<div id="divPaymentMessage" visible="false" runat="server">
    <uc1:HeaderLine ID="hlPaymentMessage" runat="server"></uc1:HeaderLine>
</div>
<div class="separator">
</div>
<div id="divSubscriptionTypes" visible="false" runat="server">
    <uc1:HeaderLine ID="hlSubscriptionTypes" runat="server"></uc1:HeaderLine>
    <asp:Label ID="lblCurrentPlan" runat="server"></asp:Label>
    <div class="wrap-view">
        <asp:RadioButtonList ID="rlPlans" runat="server" />
    </div>
</DIV>
<div id="divCreditPackages" visible="false" runat="server">
    <uc1:HeaderLine ID="hlCreditPackages" runat="server"></uc1:HeaderLine>
    <div class="wrap-view">
        <asp:RadioButtonList ID="rlCreditPackages" runat="server" />
    </div>
</div>
<div class="separator">
</div>
<div id="divPaymentMethods" visible="false" runat="server">
    <uc1:HeaderLine ID="hlPaymentMethods" runat="server"></uc1:HeaderLine>
    <asp:DropDownList id="ddPaymentProcessors" CssClass="dropdown" runat="server" AutoPostBack="True" 
        OnSelectedIndexChanged="ddPaymentProcessors_SelectedIndexChanged"></asp:DropDownList>
</div>
<div class="separator">
</div>
<div id="divOutsidePayment" visible="false" runat="server">
    <uc1:HeaderLine ID="hlOutsidePayment" runat="server"></uc1:HeaderLine>
    <div class="buttons"><asp:Button ID="btnOutsidePayment" OnClick="btnOutsidePayment_Click" runat="server" /></div>
</div>
<div id="divCreditCardPayment" visible="false" runat="server">
    <uc1:HeaderLine ID="hlCreditCardPayment" runat="server"></uc1:HeaderLine>
    <div class="wrap-view">
        <table cellpadding="4" cellspacing="0" class="settings">
            <tr>
                <td>
                    <%= "First name".Translate() %>
                </td>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%= "Last name".Translate() %>
                </td>
                <td>
                    <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%= "Address".Translate() %>
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%= "City".Translate() %>
                </td>
                <td>
                    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%= "State".Translate() %>
                </td>
                <td>
                    <asp:TextBox ID="txtState" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%= "ZIP".Translate() %>
                </td>
                <td>
                    <asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%= "Country".Translate() %>
                </td>
                <td>
                    <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%= "Phone".Translate() %>
                    <div class="separator">
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                    <div class="separator">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <%= "Card Number".Translate() %>
                </td>
                <td>
                    <asp:TextBox ID="txtCardNumber" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%= "Card Expiration".Translate() %>
                </td>
                <td>
                    <asp:DropDownList CssClass="dropdownlist" ID="ddMonth" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="dropdownlist" ID="ddYear" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>2014</asp:ListItem>
                        <asp:ListItem>2015</asp:ListItem>
                        <asp:ListItem>2016</asp:ListItem>
                        <asp:ListItem>2017</asp:ListItem>
                        <asp:ListItem>2018</asp:ListItem>
                        <asp:ListItem>2019</asp:ListItem>
                        <asp:ListItem>2020</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div class="buttons"><asp:Button ID="btnCreditCardPayment" OnClick="btnCreditCardPayment_Click" runat="server" /></div>
</div>
<div id="divCheckPayment" visible="false" runat="server">
    <uc1:HeaderLine ID="hlCheckPayment" runat="server"></uc1:HeaderLine>
    <components:ContentView ID="cvCheckPayment" Key="CheckPaymentInfo" runat="server">
        Check payment details
    </components:ContentView>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd" runat="server"></uc1:LargeBoxEnd>
