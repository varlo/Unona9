<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditMultiChoiceCheck.ascx.cs" Inherits="AspNetDating.Components.Profile.EditMultiChoiceCheck" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc1" %>
<div id="pnlID" runat="server">
    <h4 id="tdUser" runat="server"><uc1:HeaderLine ID="hlName" runat="server" /></h4>
    <h4 id="tdAdmin" runat="server"><%= hlName.Title %></h4>
    <div id="pnlDescription" Runat="server">
	<asp:Label ID="lblDescription" Runat="server" />
    </div>
    <p><asp:checkboxlist CssClass="checkboxlist" id="cbValues" SkinID="Interests" RepeatColumns="4" Width="100%" runat="server"/></p>
    <p class="text-muted" id="pnlHint" Runat="server">
	    <small><i class="fa fa-lightbulb-o"></i>&nbsp;<asp:Label ID="lblHint" Runat="server" /></small>
    </p>
    <input id="hidQuestionId" type="hidden" name="hidQuestionId" runat="server">
</div>

