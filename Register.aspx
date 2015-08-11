<%@ Page Language="c#" MasterPageFile="Site.Master" Codebehind="Register.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Register" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LoginBox" Src="Components/LoginBox.ascx" %>
<%@ Register Src="Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">
        $(function() {
            var disableAgeInformation = <%= Config.Users.DisableAgeInformation.ToString().ToLower() %>;
            
            if ($('#<%= dropGender.ClientID %>').val() == 3 && !disableAgeInformation)
                $('#<%= trBirthday2.ClientID %>').show();
            else
                $('#<%= trBirthday2.ClientID %>').hide();            
            
            $('#<%= dropGender.ClientID %>').change(function() {
                if ($(this).val() == 3 && !disableAgeInformation) {
                    $('#<%= trBirthday2.ClientID %>').show();
                }
                else
                    $('#<%= trBirthday2.ClientID %>').hide();
            });
        });

    </script>
    <article class="no-sidebar">
        <uc1:WideBoxStart id="WideBoxStart1" runat="server" />
              <!--  <div id="divLogin" runat="server" class="text-center" visible="false">
                    <p><%= "If you already have an account for this site use the Login button below to link it with your Facebook account".Translate() %></p>
                    <asp:Button ID="btnLogin" PostBackUrl="~/Login.aspx?facebook=1" runat="server" />
                </div>
                <div class="clearfix text-center">
                    <span id="divFacebook" runat="server">
                        <asp:LinkButton CssClass="btn btn-sm" ID="btnUseFacebook" runat="server" OnClick="btnUseFacebook_Click"><i class="fa fa-facebook"></i>&nbsp;<%= Lang.Trans("Log In")%></asp:LinkButton>
                    </span>
                </div>-->
    <%--        <asp:UpdatePanel ID="UpdatePanelRegister" runat="server">
                <ContentTemplate>--%>
                <components:ContentView ID="cvRegistrationInfo" Key="RegistrationInfo1" runat="server">
                   <p>All you have to do is fill in the information.</p>
                   <p>After the information is received we will send you a confirmation e-mail on the
                    e-mail address you specify. You must click on the link in that e-mail to activate
                    your account. After you activate your account you'll be redirected to a page where
                    you can create your profile and upload photos.
                    </p>
                </components:ContentView>
                <div class="row">
                    <div class="form-horizontal col-sm-6 col-sm-offset-3">
                        <asp:Label ID="lblError" CssClass="alert text-danger" runat="server" EnableViewState="False" />
                        <div class="form-group" id="trCountry" runat="server">
                            <label class="control-label col-sm-4"><%= Lang.Trans("Country") %></label>
                            <div class="col-sm-8">
                                <select ID="dropCountry" enableviewstate="false" class="form-control" runat="server"></select>
                            </div>
                        </div>
                        <div class="form-group" id="trState" runat="server">
                            <label class="control-label col-sm-4"><%= Lang.Trans("Region/State") %></label>
                            <div class="col-sm-8">
                                <select ID="dropRegion" class="form-control" runat="server">
                                </select>
                            </div>
                        </div>
                        <div class="form-group" id="trCity" runat="server">
                            <label class="control-label col-sm-4"><%= Lang.Trans("City") %></label>
                            <div class="col-sm-8">
                                <select ID="dropCity" class="form-control" runat="server"></select>
                            </div>
                        </div>
                        <div class="form-group" id="trZipCode" runat="server">
                            <label class="control-label col-sm-4"><%= Lang.Trans("Zip/Postal Code") %></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtZipCode" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4"><asp:Label ID="lblName" runat="server" /></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtName" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                        <div class="form-group" id="pnlGender" runat="server">
                            <label class="control-label col-sm-4"><asp:Label ID="lblGender" runat="server" /></label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="dropGender" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="trInterestedIn" runat="server">
                            <label class="control-label col-sm-4"><asp:Label ID="lblInterestedIn" runat="server" /></label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="dropInterestedIn" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="pnlBirthdate" runat="server">
                            <label class="control-label col-sm-4"><asp:Label ID="lblBirthdate" runat="server" /></label>
                            <div class="col-sm-8">
                                <uc2:DatePicker ID="datePicker1" runat="server" />
                            </div>
                        </div>
                        <div class="form-group" id="trBirthday2" runat="server">
                            <label class="control-label col-sm-4"><asp:Label ID="lblBirthdate2" runat="server" /></label>
                            <div class="col-sm-8">
                                <uc2:DatePicker ID="datePicker2" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4"><asp:Label ID="lblUsername" runat="server" /></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4"><asp:Label ID="lblEmail" runat="server" /></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4"><asp:Label ID="lblPassword" runat="server" /></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server" TextMode="Password" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4"><asp:Label ID="lblPassword2" runat="server" /></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtPassword2" CssClass="form-control" runat="server" TextMode="Password" EnableViewState="true" />
                            </div>
                        </div>

                        <div class="form-group" id="trInvitationCode" runat="server">
                            <label class="control-label col-sm-4"><asp:Label ID="lblInvitationCode" runat="server" /></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtInvitationCode" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                        <div id="trCaptcha" visible="false" class="form-group" runat="server">
                            <div class="col-sm-8 col-sm-offset-4">
                                <div class="input-group captcha-group">
                                    <span class="input-group-addon"><img src="Captcha.ashx" alt="Captcha" /></span>
                                    <asp:TextBox CssClass="form-control" ID="txtCaptcha" runat="server" />
                                </div>
                                <span class="text-muted"><asp:Label ID="lblCaptcha" runat="server" /></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-8 col-sm-offset-4">
                                <div class="checkbox">
                                    <label><asp:CheckBox ID="cbAgreement" runat="server" /><a href="Agreement.aspx" target="_new"><%= Lang.Trans("I ACCEPT the terms and conditions") %></a></label>
                                </div>
                            </div>
                        </div>
                    </div><!-- /.form-horizontal -->
                </div><!-- /.row -->
                <hr />
                <div class="actions">
                    <asp:Button ID="btnRegister" CssClass="btn btn-default" runat="server" OnClick="btnRegister_Click" />
                </div>
    <%--            </ContentTemplate>
            </asp:UpdatePanel>--%>
        <uc1:WideBoxEnd id="WideBoxEnd1" runat="server" />
    </article>
</asp:Content>
