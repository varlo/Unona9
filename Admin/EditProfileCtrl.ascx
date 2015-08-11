<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditProfileCtrl.ascx.cs" Inherits="AspNetDating.Admin.EditProfileCtrl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<uc1:MessageBox id="MessageBox" runat="server"/>
<div class="panel clear-panel">
    <div class="panel-heading">
        <h4 class="panel-title"><%= Lang.TransA("Personal Settings") %></h4>
    </div>
    <div class="panel-body">
        <div class="form-horizontal medium-width">
            <div class="form-group" id="trCountry" runat="server">
                <label class="control-label col-sm-5"><%= Lang.TransA("Country") %></label>
                <div class="col-sm-7"><select class="form-control" ID="dropCountry" enableviewstate="false" runat="server"></select></div>
            </div>
            <div class="form-group" id="trState" runat="server">
                <label class="control-label col-sm-5"><%= Lang.TransA("State") %></label>
                <div class="col-sm-7"><select class="form-control" ID="dropRegion" runat="server"></select></div>
            </div>
            <div class="form-group" id="trCity" runat="server">
                <label class="control-label col-sm-5"><%= Lang.TransA("City") %></label>
                <div class="col-sm-7"><select class="form-control" ID="dropCity" runat="server"></select></div>
            </div>
            <div class="form-group"  id="trZipCode" runat="server">
                <label class="control-label col-sm-5"><%= Lang.TransA("Zip/Postal Code") %></label>
                <div class="col-sm-7"><asp:TextBox CssClass="form-control" ID="txtZipCode" runat="server"/></div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-5"><%= Lang.TransA("Name") %></label>
                <div class="col-sm-7"><asp:TextBox CssClass="form-control" ID="txtName" runat="server"/></div>
            </div>
            <div class="form-group" id="pnlGender" runat="server">
                <label class="control-label col-sm-5"><%= Lang.TransA("Gender") %></label>
                <div class="col-sm-7">
                    <asp:DropDownList CssClass="form-control" ID="dropGender" runat="server">
                        <asp:ListItem Value=""/>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group" id="trInterestedIn" runat="server">
                <label class="control-label col-sm-5"><%= Lang.TransA("Interested in") %></label>
                <div class="col-sm-7">
                    <asp:DropDownList CssClass="form-control" ID="dropInterestedIn" runat="server">
                        <asp:ListItem Value=""/>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group" id="pnlBirthdate" runat="server">
                <label class="control-label col-sm-5"><%= Lang.TransA("Birthdate") %></label>
                <div class="col-sm-7"><uc2:DatePicker id="datePicker1" runat="server"/></div>
            </div>
            <div class="form-group" id="trBirthdate2" runat="server">
                <label class="control-label col-sm-5"><asp:Label ID="lblBirthdate2" runat="server"/></label>
                <div class="col-sm-7"><uc2:DatePicker id="datePicker2" runat="server"/></div>
            </div>
            <div class="form-group" >
                <label class="control-label col-sm-5"><%= Lang.TransA("E-Mail") %></label>
                <div class="col-sm-7"><asp:TextBox CssClass="form-control" ID="txtEmail" runat="server"/></div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-5"><%= Lang.TransA("New password") %></label>
                <div class="col-sm-7"><asp:TextBox CssClass="form-control" ID="txtPassword" runat="server" TextMode="Password"/></div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-5"><%= Lang.TransA("Confirm new password") %></label>
                <div class="col-sm-7"><asp:TextBox CssClass="form-control" ID="txtPassword2" runat="server" TextMode="Password"/></div>
            </div>
            <div class="form-group">
                <div class="col-sm-7 col-sm-offset-5">
                    <div class="checkbox">
                        <label><asp:CheckBox ID="cbReceiveEmails" runat="server"/></label>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-7 col-sm-offset-5">
                    <div class="checkbox">
                        <label><asp:CheckBox ID="cbProfileVisible" runat="server"/></label>
                    </div>
                </div>
            </div>
            <div class="form-group" id="trUserVerified" runat="server">
                <div class="col-sm-7 col-sm-offset-5">
                    <div class="checkbox">
                        <label><asp:CheckBox ID="cbUserVerified" runat="server"/></label>
                    </div>
                </div>
            </div>
            <div id="trFeaturedMember" runat="server">
                <div class="col-sm-7 col-sm-offset-5">
                    <div class="checkbox">
                        <label><asp:CheckBox ID="cbFeaturedMember" runat="server"/></label>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hidUsername" runat="server" name="hidUsername" />
    </div>
</div>
<asp:PlaceHolder ID="plhProfile" runat="server"/>
<div class="actions">
    <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" onclick="btnCancel_Click"/>
    <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" onclick="btnSave_Click"/>
</div>

