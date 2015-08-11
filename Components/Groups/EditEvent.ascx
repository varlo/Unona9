<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditEvent.ascx.cs" Inherits="AspNetDating.Components.Groups.EditEvent" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="~/Components/DatePicker.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TimeInput" Src="~/Components/TimeInput.ascx" %>
<%@ Register Src="~/Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
    <asp:Label ID="lblError" CssClass="alert text-danger" runat="server"/>
    <div id="pnlEventImage" runat="server" class="text-center"><img class="img-thumbnail" src='GroupEventImage.ashx?id=<%= EventID %>&width=120&height=120&diskCache=1' alt=""/>&nbsp;</div>
    <div class="form-inline">
        <div class="form-group">
            <label><%= Lang.Trans("Image") %></label>
        </div>
        <div class="form-group"><asp:FileUpload ID="fuImage" runat="server" /></div>
    </div>
    <div class="form-group">
        <label><%= Lang.Trans("Title") %></label>
        <asp:TextBox ID="txtTitle" CssClass="form-control" runat="server"/>
    </div>
    <div class="form-group">
        <label><%= Lang.Trans("Description") %></label>
        <asp:TextBox ID="txtDescription" runat="server"  CssClass="form-control" TextMode="MultiLine" Rows="5" Columns="50"/>
    </div>
    <div class="form-group">
        <label><%= Lang.Trans("Location") %></label>
        <asp:TextBox ID="txtLocation" CssClass="form-control" runat="server"/>
    </div>
    <div class="row">
        <div class="col-sm-7">
            <div class="form-group">
                <label><%= Lang.Trans("Date") %></label><br />
                <uc2:DatePicker ID="datePicker1" runat="server"/>
            </div>
        </div>
        <div class="col-sm-5">
            <div class="form-group">
                <label><%= Lang.Trans("Time") %></label><br />
                <uc1:TimeInput id="tiHoursMin" runat="server"/>
            </div>
        </div>
    </div>
<div class="actions">
	<asp:Button CssClass="btn btn-default" ID="btnAdd" runat="server" OnClick="btnAdd_Click" />
	<asp:Button CssClass="btn btn-default" ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" />
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>