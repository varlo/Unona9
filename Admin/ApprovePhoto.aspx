<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApprovePhoto.aspx.cs" Inherits="AspNetDating.Admin.ApprovePhoto" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
    function RejectOnEnter() {
        var btn = document.getElementById('<%= btnReject.ClientID %>');
		// process only the Enter key
		if (event.keyCode == 13) {
		    // cancel the default submit
		    event.returnValue = false;
		    event.cancel = true;
		    // submit the form by programmatically clicking the specified button
		    btn.click();
		}
	}
</script>
<table cellpadding="0" cellspacing="0" id="stable">
    <tr>
        <th colspan="2"><%= Lang.TransA("Photo Approval") %></th>
    </tr>
    <tr>
        <td colspan="2" valign="middle" align="center"  >
            <asp:Image CssClass="app-photo" ID="imgBigPhoto" runat="server"/></td>
    </tr>
    <tr>
            <td><%= Lang.TransA("Username") %>:</td>
            <td><a id="lnkUsername" runat="server"></a></td>
        </tr>
        <tr>
            <td><%= Lang.TransA("Photo Name") %>:</td>
            <td><asp:Label ID="lblPhotoName" runat="server"/>&nbsp;</td>
        </tr>
        <tr>
            <td><%= Lang.TransA("Photo Description") %>:</td>
            <td><asp:Label ID="lblPhotoDescription" runat="server"/>&nbsp;</td>
        </tr>
        <tr>
            <td>
            <asp:CheckBox ID="chkExplicitPhoto" runat="server"/>
            </td>
            <td>
            <asp:Button ID="btnApprove" runat="server" OnClick="btnApprove_Click"/>
            <asp:Button ID="btnReject" runat="server" OnClick="btnReject_Click"/>
            <asp:TextBox ID="txtReason" onkeydown="javascript: RejectOnEnter()" runat="server"/>
            </td>
        </tr>
    </table>

<div class="actions">
    <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" OnClick="btnCancel_Click"/>
</div>
</asp:Content>
