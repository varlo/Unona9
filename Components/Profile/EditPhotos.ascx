<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="EditPhotos.ascx.cs" Inherits="AspNetDating.Components.Profile.EditPhotos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
<input type="hidden" id="hidUsername" runat="server" />
<h4><uc1:HeaderLine ID="PostingPhotosHeaderLine" runat="server" /></h4>
<p>
    <components:ContentView ID="cvNote" Key="UploadPhotosNotes" runat="server">Your photos
        are not going to be visible to the other members until they are approved by our
        team. This process can take up to 24 hours. Any photo that doesn't conform to our
        specifications will be deleted. For more information please read "Photo guidelines".
    </components:ContentView>
</p>
<asp:MultiView ID="mvPhotoAlbum" runat="server">
    <asp:View ID="viewPhotoAlbums" runat="server">
    <div class="input-group input-group-sm">
        <span class="input-group-addon"><%= "Upload to".Translate() %></span>
        <asp:DropDownList CssClass="form-control form-control-inline" ID="ddPhotoAlbums" runat="server" OnSelectedIndexChanged="ddPhotoAlbums_SelectedIndexChanged" AutoPostBack="true"/>
        <div class="input-group-btn"><asp:LinkButton CssClass="btn btn-default" ID="lnkCreateNewAlbum" runat="server" OnClick="lnkCreateNewAlbum_Click"/></div>
        <div class="input-group-btn"><asp:LinkButton CssClass="btn btn-default" ID="lnkEditAlbum" runat="server" onclick="lnkEditAlbum_Click"/></div>
        <div class="input-group-btn"><asp:LinkButton CssClass="btn btn-default" ID="lnkDeleteAlbum" runat="server" onclick="lnkDeleteAlbum_Click"/></div>
    </div>
    </asp:View>
    <asp:View ID="viewCreatePhotoAlbum" runat="server">
        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-sm-4"><%= "Album name".Translate() %></label>
                <div class="col-sm-8"><asp:TextBox CssClass="form-control" ID="txtPhotoAlbumName" runat="server"/></div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-4"><%= "Who can view this album?" %></label>
                <div class="col-sm-8"><asp:DropDownList CssClass="form-control" ID="ddPhotoAlbumAccess" runat="server"/></div>
            </div>
            <div class="form-group">
                <div class="col-sm-8 col-sm-offset-4 text-right">
                    <asp:Button CssClass="btn btn-default pull-left" ID="btnCancelCreatePhotoAlbum" runat="server" OnClick="btnCancelCreatePhotoAlbum_Click" />
                    <asp:Button CssClass="btn btn-primary" ID="btnCreateEditPhotoAlbum" runat="server" OnClick="btnCreateEditPhotoAlbum_Click" />
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView>
<asp:Panel ID="pnlManagePhotos" runat="server">
    <hr />
    <div class="clearfix">
    <asp:DataList ID="dlPhotos" runat="server" CssClass="repeater-horizontal" RepeatLayout="Flow">
        <ItemTemplate>
            <div class="thumbnail">
                <div class="caption">
                    <b><%# Eval("Name") %>&nbsp;</b>
                </div>
                <div class="over-top watermark">
                    <a class="tooltip-link tooltip-go" data-toggle="tooltip" data-placement="bottom" title='<%= Lang.Trans("Primary Photo") %>'><%# Convert.ToBoolean(Eval("Primary")) ? "<i class=\"fa fa-check-square-o fa-lg\"></i>" : "" %></a>&nbsp;
                    <a class="tooltip-link tooltip-go" data-toggle="tooltip" data-placement="bottom" title='<%= Lang.Trans("Private Photo") %>'><%# Convert.ToBoolean(Eval("Private")) ? "&nbsp;<i class=\"fa fa-eye fa-lg\"></i>" : "" %></a>
                </div>
                <a data-toggle="tooltip" data-placement="top" class="tooltip-go" title='<%# Eval("Description") %>'>
                    <img src="Image.ashx?id=<%# Eval("PhotoId") %>&width=220&height=220" />
                </a>
                <div class="thumb-icons">
                    <a class="tooltip-link tooltip-go" data-toggle="tooltip" data-placement="bottom" title='<%= Lang.Trans("Pending for Approval") %>'>
                        <%# Convert.ToBoolean(Eval("Approved")) ? "" : "<i class=\"fa fa-spinner fa-spin\"></i>" %>
                    </a>
                    <div class="pull-right">
                    <asp:LinkButton ID="lnkUploadPhoto" CommandName="UploadPhoto" CommandArgument='<%# Eval("PhotoId") %>' runat="server" data-toggle="tooltip" data-placement="bottom" class="tooltip-go" title='<%# Lang.Trans("Edit Photo") %>'>
                        <i class="fa fa-pencil"></i>
                    </asp:LinkButton>
                    <asp:LinkButton  ID="lnkDeletePhoto" CommandName="DeletePhoto" CommandArgument='<%# Eval("PhotoId") %>' Visible='<%# Convert.ToInt32(Eval("PhotoId")) > 0 %>' runat="server" data-toggle="tooltip" data-placement="bottom" class="tooltip-go" title='<%# Lang.Trans("Delete Photo") %>'>
                        <i class="fa fa-trash-o"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="lnkPrimaryPhoto" CommandName="PrimaryPhoto" CommandArgument='<%# Eval("PhotoId") %>' Visible='<%# !Convert.ToBoolean(Eval("Primary")) && Convert.ToInt32(Eval("PhotoId")) > 0 %>' runat="server" data-toggle="tooltip" data-placement="bottom" class="tooltip-go" title='<%# Lang.Trans("Make Primary Photo") %>'>
                       <i class="fa fa-check-square-o"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="lnkAddNotes" CommandName="AddNotes" CommandArgument='<%# Eval("PhotoId") %>' Visible='<%# Config.Photos.EnablePhotoNotes && Convert.ToInt32(Eval("PhotoId")) > 0 && !Request.IsIE6() %>' runat="server" data-toggle="tooltip" data-placement="bottom" class="tooltip-go" title='<%# Lang.Trans("Add Notes") %>'>
                        <i class="fa fa-file-text-o"></i>
                    </asp:LinkButton>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:DataList>
    </div>
<div class="text-center">
    <span id="divUploadMultiplePhotosFlash" runat="server">
        <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkUploadMultipleFlash" runat="server" OnClick="lnkUploadMultipleFlash_Click"/>
    </span>
    <span id="divUploadMultiplePhotosSilverlight" runat="server">
        <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkUploadMultipleSilverlight" runat="server" OnClick="lnkUploadMultipleSilverlight_Click"/>
    </span>
    <span id="divUploadViaWebCam" runat="server">
        <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkUploadViaWebCam" runat="server" onclick="lnkUploadViaWebCam_Click"/>
    </span>
</div>
</asp:Panel>
<div class="text-center">

	<asp:Panel ID="pnlUploadMultiplePhotosFlash" runat="server" Visible="false">
	    <components:FlashUpload ID="flashUpload" runat="server" OnUploadComplete="flashUploadIsCompleted()" UploadPage="../../FlashUpload.ashx" FileTypeDescription="Images" FileTypes="*.gif; *.png; *.jpg; *.jpeg" UploadFileSizeLimit="5242880" TotalUploadSizeLimit="20971520" />
	</asp:Panel>
	<div ID="pnlUploadMultiplePhotosSilverlight" runat="server" Visible="false">
	    <asp:Silverlight ID="Silverlight1" runat="server" Source="~/ClientBin/MultiFileUploader.xap" MinimumVersion="2.0.31005.0" Width="400" Height="300"/>
	</div>
	<div id="pnlUploadViaWebCam" runat="server" visible="false">
	    <components:WebcamUpload ID="webcamUpload" runat="server" OnUploadComplete="webcamUploadIsCompleted()" Width="400" Height="400" FileTypeDescription="Images" FileTypes="*.gif; *.png; *.jpg; *.jpeg" UploadFileSizeLimit="5242880" TotalUploadSizeLimit="20971520" />
	</div>
	<asp:LinkButton CssClass="btn btn-default" ID="lnkBackToPhotos" runat="server" Visible="false" OnClick="lnkBackToPhotos_Click"/>

</div>
<asp:Panel ID="pnlEditImage" Visible="False" runat="server">
    <asp:Label CssClass="alert text-danger" ID="lblError" EnableViewState="False" runat="server" />
    <div class="media">
        <div class="pull-left thumbnail">
            <img class="media-object" id="tempPhoto" src="Image.ashx?id=session&width=180&height=180&seed=<%= new Random().NextDouble().ToString() %>"/>
            <div class="caption"><div class="checkbox text-muted"><label><asp:CheckBox ID="chkPrivatePhoto" runat="server"/></label></div></div>
        </div>
        <div class="media-body">
            <asp:Panel ID="pnlPhotoAlbum" runat="server">
                <div class="form-group">
                    <label><%= Lang.Trans("Album") %></label>
                    <p class="form-control-static form-control-inline"><asp:Label ID="lblPhotoAlbumName" runat="server"/></p>
                </div>
            </asp:Panel>
            <div class="form-group">
                <label><%= Lang.Trans("Name") %>&nbsp;</label>
                <asp:TextBox ID="txtName" CssClass="form-control form-control-inline" runat="server" Size="60" MaxLength="80"/>
            </div>
            <div class="form-group">
                <label><%= Lang.Trans("Description")%></label>
                <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" Rows="4" MaxLength="200" TextMode="MultiLine"/>
            </div>
        </div>
    </div><!-- /.media -->
    <div class="form-group" id="divFileUploadControls" runat="server">
        <div class="input-group">
            <span class="input-group-addon"><asp:Label ID="lblPhoto" runat="server" /></span>
            <p class="form-control-static"><asp:FileUpload ID="ufPhoto" runat="server" /></p>
            <span class="input-group-btn"><asp:LinkButton CssClass="btn btn-primary" ID="btnUpload" runat="server" OnClick="btnUpload_Click"><i class="fa fa-upload"></i>&nbsp;<%= Lang.Trans("Upload") %></asp:LinkButton></span>
        </div>
    </div>
    <div class="form-group" id="divImageRotateControls" visible="false" runat="server">
        <div class="btn-group btn-group-sm">
            <asp:LinkButton CssClass="btn btn-default" ID="btnRotateLeft" runat="server" OnClick="btnRotateLeft_Click"><i class="fa fa-undo"></i>&nbsp;<%= Lang.Trans("Rotate Left") %></asp:LinkButton>
            <asp:LinkButton CssClass="btn btn-default" ID="btnRotateRight" runat="server" OnClick="btnRotateRight_Click"><i class="fa fa-repeat"></i>&nbsp;<%= Lang.Trans("Rotate Right") %></asp:LinkButton>
        </div>
    </div>
    <asp:PlaceHolder ID="plhPhotoFaces" Visible="false" runat="server">
        <label><%= Lang.Trans("Which photo crop shows your face?") %></label>
        <asp:DataList ID="dlPhotoFaces" CssClass="repeater-horizontal" runat="server" RepeatLayout="Flow">
            <ItemTemplate>
                <div class="thumbnail">
                    <img src="<%# "Image.ashx?id=session&width=100&height=100&findFace=1&faceX=" + Eval("X") + "&faceY=" + Eval("Y") + "&faceW=" + Eval("Width") + "&faceH=" + Eval("Height") %>" class="img-thumbnail" />
                    <div class="caption">
                        <div class="radio"><label><components:GroupRadioButton ID="rbFace" GroupName="faces" runat="server" /></label></div>
                        <asp:HiddenField ID="hidFace" Value='<%# Eval("X") + "|" + Eval("Y") + "|" + Eval("Width") + "|" + Eval("Height") %>' runat="server" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:DataList>
        <div class="radio"><label><components:GroupRadioButton ID="rbNoFace" GroupName="faces" runat="server"/><%= Lang.Trans("None of the above") %></label></div>
    </asp:PlaceHolder>
    <div class="actions">
        <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"/>
        <asp:LinkButton CssClass="btn btn-default" ID="lnkSave" runat="server" OnClick="lnkSave_Click"/>
    </div>
</asp:Panel>
<asp:Panel ID="pnlCropImage" Visible="false" runat="server">
    <p><%= Lang.Trans("To make a photo thumbnail, click a corner of the crop frame, and then drag it. When you're ready click \"Save Thumbnail\"")%></p>
    <div class="text-center">
        <div class="thumbnail form-control-inline">
            <img id="imgFaceCropPhoto" runat="server" />
            <div class="thumb-icons text-left">
                <asp:LinkButton ID="lnkSkipThumbnail" runat="server" OnClick="lnkCancelFaceCrop_Click" data-toggle="tooltip" data-placement="bottom" class="tooltip-go"><i class="fa fa-times"></i></asp:LinkButton>
                <div class="pull-right">
                    <asp:LinkButton ID="lnkSaveThumbnail" runat="server" OnClick="lnkSaveFaceCrop_Click" data-toggle="tooltip" data-placement="bottom" class="tooltip-go"><i class="fa fa-crop"></i></asp:LinkButton>
                </div>
            </div>
            <input type="hidden" id="hidCropPhotoId" runat="server" />
            <input type="hidden" id="hidCropX" runat="server" />
            <input type="hidden" id="hidCropY" runat="server" />
            <input type="hidden" id="hidCropW" runat="server" />
            <input type="hidden" id="hidCropH" runat="server" />
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlPhotoNote" Visible="false" runat="server">
    <p><%= Lang.Trans("To add a photo note, click a corner of the note frame, and then drag it. When you're ready click \"Add Note\"")%></p>
    <img id="imgAddPhotoNote" class="img-thumbnail center-block" runat="server" />

    <asp:UpdatePanel ID="updatePanelNotes" runat="server">
        <ContentTemplate>
            <div class="media" id="tblCropPreview" style="display: none;">
                <img class="media-object img-thumbnail" id="imgNoteCropPreview" runat="server" />
                <div class="media-body">
                    <div class="form-group">
                        <div class="form-inline">
                            <label><%= "Enter note".Translate() %></label>
                            <asp:TextBox CssClass="form-control" ID="txtNoteText" runat="server"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-inline">
                            <label><%= "Is this a friend registered here?".Translate() %></label>
                            <asp:DropDownList CssClass="form-control" ID="ddNoteFriend" runat="server"/>
                        </div>
                    </div>
                    <asp:Button CssClass="btn btn-default" ID="btnAddNote" runat="server" OnClick="btnAddNote_Click" />
                </div>
            </div>
            <asp:Repeater ID="rptNotes" OnItemCommand="rptNotes_ItemCommand" runat="server">
                <ItemTemplate>
                    <img src="<%# Eval("ImageCropUrl") %>" class="photoframe" alt="" />
                    <%# Eval("Username") != DBNull.Value ? ("Username: ".Translate() + Eval("Username") + "<br />") : "" %><%# Eval("Notes") != DBNull.Value ? ("Notes: ".Translate() + Eval("Notes") + "<br />") : "" %>
                    <asp:Button CssClass="btn btn-default btn-sm" ID="btnDeleteNote" runat="server" CommandName="DeleteNote" CommandArgument='<%# Eval("Id") %>' Text='<%# "Delete".Translate() %>' />
                </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:LinkButton CssClass="btn btn-default" ID="lnkDoneAddingPhotoNotes" runat="server" OnClick="lnkDoneAddingPhotoNotes_Click"/>
    <input type="hidden" id="hidNotePhotoId" runat="server" />
    <input type="hidden" id="hidNoteX" runat="server" />
    <input type="hidden" id="hidNoteY" runat="server" />
    <input type="hidden" id="hidNoteW" runat="server" />
    <input type="hidden" id="hidNoteH" runat="server" />
</asp:Panel>
<hr />
    <h4><uc1:HeaderLine ID="PhotoGuidelinesHeaderLine" runat="server" /></h4>
    <components:ContentView ID="cvGuideLines" Key="UploadPhotosGuideLines" runat="server">
        Please read the following guidelines before sending your photos:
        Group photos are only accepted if you have specified who you are and there are no more than 3 people on the photo.
        Avoid small photos. Your photo should be at least 150 x 150 pixels. The recommended resolution is 450 x 450.
        When scanning your photo please cut the empty areas around the photo - this way your photo will seem bigger and the file size will be smaller.
        Photos with erotic content are not allowed.
        Celebrity or copyrighted photos are not accepted.
    </components:ContentView>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
<script type="text/javascript">
    $(document).ready(function() {
        $('.tooltip-go').tooltip({
            animation: false
        });
    });
</script>