<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ManagePolls.aspx.cs" Inherits="AspNetDating.Admin.ManagePolls" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="form-horizontal medium-width">
    <div class="form-group">
        <label class="control-label col-sm-3"><%= Lang.TransA("Polls") %></label>
        <div class="col-sm-7"><asp:DropDownList CssClass="form-control" ID="ddPolls" runat="server" AutoPostBack="true" onselectedindexchanged="ddPolls_SelectedIndexChanged"/></div>
        <div class="col-sm-2"><asp:LinkButton CssClass="btn btn-primary" ID="btnDelete" runat="server" onclick="btnDelete_Click" /></div>
    </div>
</div>
<hr />
<div id="divPoll" runat="server">
    <div class="form-horizontal medium-width">
        <div class="form-group">
            <label class="control-label col-sm-3"><%= "Poll Question:".TranslateA() %></label>
            <div class="col-sm-9"><asp:TextBox CssClass="form-control" ID="txtQuestion" runat="server"/></div>
        </div>
        <div class="form-group">
            <div class="col-sm-9 col-sm-offset-3">
                <asp:DataList ID="dlChoices" runat="server">
                    <ItemTemplate>
                        <div class="form-group">
                            <asp:HiddenField ID="hidID" Value='<%# Eval("ID") %>' runat="server" />
                            <label class="control-label col-sm-4"><%# "Choice".TranslateA() + " " + Eval("Number") %></label>
                            <div class="col-sm-8"><asp:TextBox CssClass="form-control" ID="txtChoiceValue" Text='<%# Eval("ChoiceValue") %>' runat="server"/></div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-3"><%="Start Date:".TranslateA() %></label>
            <div class="col-sm-9"><uc2:DatePicker ID="dpFromDate" MinYear="2009" MaxYear="2019"  runat="server" /></div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-3"><%="End Date:".TranslateA() %></label>
            <div class="col-sm-9"><uc2:DatePicker ID="dpToDate" runat="server" MinYear="2009" MaxYear="2019" /></div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-3"><%="Show results until:".TranslateA() %></label>
            <div class="col-sm-9"><uc2:DatePicker ID="dpShowResultsUntil" runat="server" MinYear="2009" MaxYear="2019" /></div>
        </div>
        <div class="actions">
            <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" onclick="btnSave_Click" />
        </div>
    </div>
</div>
</asp:Content>
