<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditQuestion.aspx.cs" Inherits="AspNetDating.Admin.EditQuestion" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <div class="panel clear-panel">
        <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Question Details") %></h4></div>
        <div class="panel-body">
        <div class="form-horizontal medium-width">
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Name") %></label>
                <div class="col-sm-8"><asp:textbox CssClass="form-control" id="txtName" runat="server"/></div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Alternative Name") %></label>
                <div class="col-sm-8"><asp:textbox CssClass="form-control" id="txtAltName" runat="server"/></div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Description") %></label>
                <div class="col-sm-8">
                    <asp:textbox id="txtDescription" CssClass="form-control" Rows="5" runat="server" TextMode="MultiLine"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Hint") %></label>
                <div class="col-sm-8">
                    <asp:textbox id="txtHint" CssClass="form-control" runat="server"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= Lang.TransA("Edit Style") %></label>
                <div class="col-sm-8"><asp:dropdownlist CssClass="form-control" id="dropEditStyle" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropEditStyle_SelectedIndexChanged"/></div>
            </div>
            <div class="form-group" id="pnlShowStyle" runat="server">
                <label class="control-label col-sm-4"><%= Lang.TransA("Show Style") %></label>
                <div class="col-sm-8"><asp:dropdownlist CssClass="form-control" id="dropShowStyle" runat="server"/></div>
            </div>
            <div class="form-group" id="pnlSearchStyle" runat="server">
                <label class="control-label col-sm-4"><%= Lang.TransA("Search Style") %></label>
                <div class="col-sm-8"><asp:dropdownlist CssClass="form-control" ID="dropSearchStyle" runat="server"/></div>
            </div>
            <div class="form-group" id="pnlMatchField" runat="server">
                <label class="control-label col-sm-4"><%= Lang.TransA("Match field") %></label>
                <div class="col-sm-8"><asp:dropdownlist CssClass="form-control" ID="ddMatchFieldQuestion" runat="server" onselectedindexchanged="ddMatchFieldQuestion_SelectedIndexChanged" AutoPostBack="true"/></div>
            </div>
            <div class="form-group" id="pnlShowCondition" runat="server">
                <label class="control-label col-sm-4"><%= Lang.TransA("Show condition") %></label>
                <div class="col-sm-8"><asp:dropdownlist CssClass="form-control" ID="ddCondition" runat="server" AutoPostBack="true" onselectedindexchanged="ddCondition_SelectedIndexChanged"/></div>
            </div>
            <div class="form-group" id="pnlRequired" runat="server">
                <div class="col-sm-8 col-sm-offset-4"><div class="checkbox"><label><asp:checkbox id="cbRequired" runat="server"/><%= Lang.TransA("Required") %></label></div></div>
            </div>
            <div class="form-group">
                <div class="col-sm-8 col-sm-offset-4"><div class="checkbox"><label><asp:checkbox id="cbRequiresApproval" runat="server"/><%= Lang.TransA("Requires Approval") %></label></div></div>
            </div>
            <div class="form-group">
                <div class="col-sm-8 col-sm-offset-4"><div class="checkbox"><label><asp:checkbox id="cbVisibleOnlyForPaidMembers" runat="server"/><%= Lang.TransA("Viewable only by paid members")%></label></div></div>
            </div>
            <div class="form-group">
                <div class="col-sm-8 col-sm-offset-4"><div class="checkbox"><label><asp:checkbox id="cbEditableOnlyByPaidMembers" runat="server"/><%= Lang.TransA("Editable only by paid members")%></label></div></div>
            </div>
            <div class="form-group" id="pnlAppliesTo" runat="server">
                <label class="control-label col-sm-4"><%= Lang.TransA("Applies To") %></label>
                <div class="col-sm-8">
                    <label class="checkbox-inline"><asp:CheckBox ID="cbVisibleToMale" Runat="server"/></label>
                    <label class="checkbox-inline"><asp:CheckBox ID="cbVisibleToFemale" Runat="server"/></label>
                    <label class="checkbox-inline"><asp:CheckBox ID="cbVisibleToCouple" Visible="False" Runat="server"/></label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-8 col-sm-offset-4"><div class="checkbox"><label><asp:CheckBox ID="cbVisibleInSearchBox" runat="server"/><%= Lang.TransA("Display in the search box on home page (dropdown)") %></label></div></div>
            </div>
        </div>
    </div>
</div>
<asp:Panel ID="pnlAnswers" Runat="server">
    <div class="panel clear-panel">
        <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Answers") %></h4></div>
        <div class="panel-body">
            <div class="table-responsive">
            <asp:datagrid id="dgChoices" CssClass="table table-striped" Runat="server" AutoGenerateColumns="False" AllowPaging="False" PageSize="10" ShowHeader="False" GridLines="None">
                <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
                <Columns>
                    <asp:TemplateColumn>
                        <ItemStyle Wrap="False" Width="20"></ItemStyle>
                        <ItemTemplate>
                            <input type="checkbox" id="cbSelect" value='<%# Eval("ChoiceID") %>' runat="server" NAME="cbSelect" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:TextBox ID="txtValue" Text='<%# Eval("Value")%>' Runat=server/>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
                <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
            </asp:datagrid>
            </div>
            <asp:LinkButton CssClass="btn btn-default btn-sm" id="btnDeleteSelectedChoices" runat="server" OnClick="btnDeleteSelectedChoices_Click"/>
            <div class="input-group input-group-sm small-width pull-right">
                <span class="input-group-addon"><%= Lang.TransA("Add") %></span>
                <asp:dropdownlist CssClass="form-control" id="dropNewChoicesCount" runat="server"/>
                <span class="input-group-addon"><%= Lang.TransA("new choices") %></span>
                <span class="input-group-btn"><asp:LinkButton CssClass="btn btn-secondary" id="btnAddNewChoices" runat="server" OnClick="btnAddNewChoices_Click"/></span>
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlConditionAnswers" runat="server">
    <div class="panel">
        <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Conditional Answers") %></h4></div>
        <div class="panel-body">
            <asp:CheckBoxList ID="cblConditionAnswers" runat="server"/>
        </div>
    </div>
</asp:Panel>
<div class="actions">
    <hr />
    <asp:button CssClass="btn btn-default" id="btnCancel" runat="server" OnClick="btnCancel_Click"/>
    <asp:button CssClass="btn btn-primary" id="btnSave" runat="server" OnClick="btnSave_Click"/>
</div>
</asp:Content>
