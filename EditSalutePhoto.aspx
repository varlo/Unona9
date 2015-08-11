<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditSalutePhoto.aspx.cs" Inherits="AspNetDating.EditSalutePhoto" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<aside>
    <uc1:smallboxstart id="SmallBoxStart1" runat="server"/>
        <ul class="nav"><li><a href="Profile.aspx"><%= "Back to Profile".Translate() %></a></li></ul>
    <uc1:smallboxend id="SmallBoxEnd1" runat="server"/>
</aside>
<article>
    <uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
    <asp:Label ID="lblError" CssClass="alert text-danger" EnableViewState="False" runat="server" />
    <p>
        <components:contentview ID="cvNote" Key="UploadSalutePhotoNotes" runat="server">Your photos
            are not going to be visible to the other members until they are approved by our
            team. This process can take up to 24 hours. Any photo that doesn't conform to our
            specifications will be deleted. For more information please read "Photo guidelines".
        </components:contentview>
    </p>

        <div class="media">
            <div class="thumbnail pull-left">
                <img id="tempPhoto" runat="server" src="Image.ashx?id=session&width=180&height=180&seed=<%= new Random().NextDouble().ToString() %>" />
            	<div class="caption"><div class="checkbox text-muted"><label><asp:CheckBox ID="chkPrivatePhoto" runat="server"/></label></div></div>
            </div>
            <div class="media-body">
                <div class="form-group">
                    <label><%= Lang.Trans("Name") %></label>&nbsp;
                    <asp:TextBox ID="txtName" CssClass="form-control form-control-inline" runat="server" MaxLength="80"/>
                </div>
                <div class="form-group">
                    <label><%= Lang.Trans("Description")%></label>
                    <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" Rows="4" TextMode="MultiLine"/>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon"><asp:Label ID="lblPhoto" runat="server" /></span>
                        <p class="form-control-static"><asp:FileUpload ID="ufPhoto" runat="server" /></p>
                        <span class="input-group-btn"><asp:LinkButton CssClass="btn btn-primary" ID="btnUpload" runat="server" OnClick="btnUpload_Click"><i class="fa fa-upload"></i>&nbsp;<%= Lang.Trans("Upload") %></asp:LinkButton></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group" id="divImageRotateControls" visible="false" runat="server">
            <div class="btn-group btn-group-sm">
                <asp:LinkButton CssClass="btn btn-default" ID="btnRotateLeft" runat="server" OnClick="btnRotateLeft_Click"><i class="fa fa-undo"></i>&nbsp;<%= Lang.Trans("Rotate Left") %></asp:LinkButton>
                <asp:LinkButton CssClass="btn btn-default" ID="btnRotateRight" runat="server" OnClick="btnRotateRight_Click"><i class="fa fa-repeat"></i>&nbsp;<%= Lang.Trans("Rotate Right") %></asp:LinkButton>
            </div>
        </div>
        <div class="actions">
            <asp:LinkButton CssClass="btn btn-default" ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"/>
            <asp:LinkButton CssClass="btn btn-default" ID="lnkSave" runat="server" OnClick="lnkSave_Click"/>
        </div>

    <uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
</article>
</asp:Content>
