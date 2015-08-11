<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditSingleChoiceSelect.ascx.cs" Inherits="AspNetDating.Components.Profile.EditSingleChoiceSelect" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" runat="server" class="form-horizontal">
    <div class="form-group">
		<div class="col-sm-5"><p class="form-control-static"><asp:Label id="lblName" runat="server" />:</p></div>
		<div class="col-sm-7"><asp:DropDownList id="dropValue" CssClass="form-control" runat="server" /></div>
    </div>
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId">
</div>
