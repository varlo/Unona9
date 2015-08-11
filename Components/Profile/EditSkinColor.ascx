<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditSkinColor.ascx.cs"
    Inherits="AspNetDating.Components.Profile.EditSkinColor" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<table>
    <tr>
        <td>
            <%= "Color:".Translate() %>
        </td>
        <td>
            <asp:TextBox ID="txtColor" runat="server"/>
            <script type="text/javascript">
                $('#<%= txtColor.ClientID %>').jPicker({ window: { position: { y: 'center'} }, images: { clientPath: '<%= Config.Urls.Home + "/scripts/jpicker/images/" %>'} });
            </script>
        </td>
    </tr>
</table>
