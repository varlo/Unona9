<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditMultiLine.ascx.cs" Inherits="AspNetDating.Components.Profile.EditMultiLine" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc1" %>
<div id="pnlID" runat="server">
    <h4 ID="pnlUser" Runat="server"><uc1:HeaderLine ID="hlName" runat="server" /></h4>
    <h4 id="tdAdmin" runat="server"><%= hlName.Title %></h4>
	<asp:Label ID="lblDescription" Runat="server" />
	<asp:TextBox ID="txtValue" TextMode="MultiLine" Rows="5" CssClass="form-control" Runat="server" />
    <p class="text-muted">
        <asp:TextBox ID="txtCharCount" ReadOnly="true" CssClass="form-control char-count" Size="2" MaxLength="2" runat="server"/>
	    <small><i class="fa fa-lightbulb-o"></i>&nbsp;<asp:Label ID="lblHint" Runat="server" /></small>
    </p>
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId">
</div>