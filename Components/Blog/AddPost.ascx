<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AddPost.ascx.cs" Inherits="AspNetDating.Components.Blog.AddPostCtrl"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<uc1:largeboxstart id="LargeBoxStart" runat="server"/>
<h4><uc1:headerline id="hlBlogSettings" runat="server"/></h4>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server"/>
<div class="form-group">
    <label><%= Lang.Trans("Title") %></label>
    <asp:TextBox ID="txtName" CssClass="form-control" runat="server"/>
</div>
<div class="form-group">
    <label><%= Lang.Trans("Description") %></label>
    <div class="fckeditor">
        <asp:PlaceHolder ID="phEditor" runat="server"/>
    </div>
</div>
<div class="actions">
    <asp:Button CssClass="btn btn-default" runat="server" ID="btnSaveChanges" onclick="btnSaveChanges_Click"/>
    <asp:Button CssClass="btn btn-link" runat="server" ID="btnCancel" Visible="False"/>
</div>
<uc1:largeboxend runat="server" id="Largeboxend1"/>