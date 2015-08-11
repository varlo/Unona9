<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchMultiChoiceCheck2.ascx.cs" Inherits="AspNetDating.Components.Search.SearchMultiChoiceCheck2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" enableviewstate="false" runat="server">
    <a href="#" onclick="return false;" class="expander">
        <asp:Literal ID="ltrName" Runat="server" />
    </a>
    <div id="divExpandee" class="expandee checkbox" style="display: none" runat="server">
		<asp:CheckBoxList id="cbValues" runat="server"/>
    </div>
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId">
</div>