<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="EnterPhotoContest.aspx.cs"
    Inherits="AspNetDating.EnterPhotoContestPage" %>

<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="grb" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"/>
            <ul class="nav">
		        <li><asp:linkbutton id="lnkBackToContest" Runat="server" OnClick="lnkBackToContest_Click"/></li>
		    </ul>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
		<asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
        <h4><asp:Label ID="lblContestName" runat="server" /></h4>
        <ul class="info-header">
            <li><b><%= Lang.Trans("Contest starts on") %></b>&nbsp;<asp:Label ID="lblStartDate" CssClass="date" runat="server" /></li>
            <li class="pull-right"><label><%= Lang.Trans("Contest ends on") %></label>&nbsp;<asp:Label ID="lblEndDate" CssClass="date" runat="server" /></li>
        </ul>
        <label><%= Lang.Trans("Contest terms") %></label>
        <p><asp:Label ID="lblContestTerms" runat="server" /></p>
        <label><%= Lang.Trans("Which photo do you want to submit to this contest?") %></label>
        <asp:DataList ID="dlPhotos" runat="server" CssClass="repeater-horizontal" RepeatLayout="Flow" ShowFooter="False" ShowHeader="False">
            <ItemTemplate>
                <div class="thumbnail">
                    <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 100, 100, "", true, true, false) %>
                    <div class="caption text-center">
                        <asp:HiddenField ID="hidPhotoId" Value='<%# Eval("PhotoId") %>' runat="server" />
                        <grb:GroupRadioButton ID="rbPhoto" GroupName="photos" runat="server" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:DataList>
        <div class="actions">
            <asp:Button CssClass="btn btn-default" ID="btnEnterContest" runat="server" OnClick="btnEnterContest_Click" />
        </div>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
    </article>
</asp:Content>
