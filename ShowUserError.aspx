<%@ Page Title="" Language="C#" MasterPageFile="ShowUser.Master" AutoEventWireup="true" CodeBehind="ShowUserError.aspx.cs" Inherits="AspNetDating.ShowUserError" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftContent" runat="server">
    <div id="pnlPhotoAlbums" runat="server" visible="false">
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"/>
        <div class="list-group list-group-striped">
            <asp:DataList ID="dlPhotoAlbums" runat="server" onitemcommand="dlPhotoAlbums_ItemCommand">
                <ItemTemplate>
                    <div class="list-group-item">
                        <asp:LinkButton ID="lnkPhotoAlbum" runat="server" Text='<%# Eval("PhotoAlbumName") %>' CommandArgument='<%# Eval("PhotoAlbumID") %>' CommandName="ViewPhotoAlbum"/>
                        (<%# Eval("NumberOfPhotos") %>)
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"/>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphInnerContent" runat="server">
		<uc1:LargeBoxStart id="LargeBoxStart1" runat="server"/>
			<div class="text-center">
                <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
			</div>
		<uc1:LargeBoxEnd id="LargeBoxEnd1" runat="server"/>
</asp:Content>
