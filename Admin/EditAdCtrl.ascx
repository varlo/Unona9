<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditAdCtrl.ascx.cs" Inherits="AspNetDating.Admin.EditAdCtrl" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<uc1:MessageBox id="MessageBox" runat="server"/>
<asp:Panel ID="pnlEditAd" runat="server">
<table border="0" cellpadding="0" cellspacing="0" id="stable">
    <tr>
        <th colspan="2">
            <%= Lang.TransA("Classified Information") %></th>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Category") %></td>
        <td>
            <asp:DropDownList ID="ddCategories" runat="server" AutoPostBack="true" onselectedindexchanged="ddCategories_SelectedIndexChanged"/>
            <asp:DropDownList ID="ddSubcategories" runat="server" Visible="false"/></td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Expiration date") %></td>
        <td>
            <asp:TextBox ID="txtExpiratoinDate" runat="server"/></td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Location")%></td>
        <td>
            <asp:TextBox ID="txtLocation" runat="server"/></td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Subject")%></td>
        <td>
            <asp:TextBox ID="txtSubject" runat="server"/></td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Message")%></td>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="textbox" Rows="5" Columns="50"/></td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Approved") %></td>
        <td>
            <asp:DropDownList ID="ddApproved" runat="server"/></td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlUploadAdPhotos" runat="server" Visible="false">
                <%= Lang.TransA("Classified Photos") %>
            <asp:Repeater ID="rptAdPhotos" runat="server" onitemcommand="rptAdPhotos_ItemCommand" onitemcreated="rptAdPhotos_ItemCreated">
                    <ItemTemplate>
                        <%# AspNetDating.AdPhoto.RenderImageTag((int)Eval("AdPhotoID"), 90, 90, null, true) %>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("AdPhotoID") %>' CommandName="DeletePhoto"/>
                        <br />
                    </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="rptAddPhoto" runat="server">
                <ItemTemplate>
                    <asp:FileUpload ID="fuAdPhoto" runat="server" />
                    <br />
                    <asp:TextBox ID="txtAdPhotoDescription" runat="server"/>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
</asp:Panel>
<div class="actions">
    <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" onclick="btnCancel_Click"/>
    <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" onclick="btnSave_Click"/>
</div>