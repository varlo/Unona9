<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadGroupPhoto.ascx.cs" Inherits="AspNetDating.Components.Groups.UploadGroupPhoto" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="~/Components/Search/SearchResults.ascx" %>

<uc1:largeboxstart id="LargeBoxStart1" runat="server" />
	<asp:Label ID="lblError" runat="server" CssClass="alert text-danger" EnableViewState="False"/>
    <div class="form-group">
        <label><%= Lang.Trans("Name") %></label>
        <asp:TextBox ID="txtName" CssClass="form-control" runat="server"/>
	</div>
	<div class="form-group">
        <label><%= Lang.Trans("Description") %></label>
        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control"/>
	</div>
	<div class="form-group"><asp:FileUpload ID="fuGroupPhoto" runat="server" /></div>
	<div class="actions"><asp:Button ID="btnUpload" CssClass="btn btn-default" runat="server" OnClick="btnUpload_Click" /></div>
<uc1:largeboxend id="LargeBoxEnd1" runat="server" />