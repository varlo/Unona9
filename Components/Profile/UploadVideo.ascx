<%@ Control Language="c#" AutoEventWireup="True" Codebehind="UploadVideo.ascx.cs"
    Inherits="AspNetDating.Components.Profile.UploadVideo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="VideoRecorder" Src="../../VR/VideoRecorder.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register src="RecordVideo.ascx" tagname="RecordVideo" tagprefix="uc2" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
<input type="hidden" id="hidUsername" runat="server" name="hidUsername" />
<div id="pnlButtons" class="actions" runat="server">
    <asp:LinkButton CssClass="btn btn-default" ID="btnShowRecordVideo" runat="server" onclick="btnShowRecordVideo_Click"><i class="fa fa-video-camera"></i>&nbsp;<%= Lang.Trans("Record Video") %></asp:LinkButton>
    <asp:LinkButton CssClass="btn btn-default" ID="btnShowUploadVideo" runat="server" onclick="btnShowUploadVideo_Click"><i class="fa fa-upload"></i>&nbsp;<%= Lang.Trans("Upload Video") %></asp:LinkButton>
    <asp:LinkButton CssClass="btn btn-default" ID="btnShowEmbedVideo" runat="server" onclick="btnShowEmbedVideo_Click"><i class="fa fa-youtube"></i>&nbsp;<%= Lang.Trans("Embed Video") %></asp:LinkButton>
</div>
<br />
<asp:MultiView ID="mvVideo" ActiveViewIndex="0" runat="server">
<asp:View ID="vRecordVideo" runat="server">
    <uc2:RecordVideo ID="RecordVideo1" runat="server" />
</asp:View>
<asp:View ID="vUploadVideo" runat="server">
    <h4><uc1:HeaderLine ID="HeaderLine1" runat="server" /></h4>
    <p class="help-block">
    <components:ContentView ID="cvNote" Key="UploadVideosNotes" runat="server">
        Your videos are not going to be visible to the other members until they are approved by our team. This process can take up to 24 hours. Any video that doesn't conform to our specifications will be deleted.
    </components:ContentView>
    </p>
    <hr />
    <asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="false"/>
    <div class="row" id="divUploadVideo" runat="server">
        <div class="col-sm-9">
            <div class="form-group">
                <label><%= Lang.Trans("Video file") %></label>&nbsp;<p class="form-control-static form-control-inline"><asp:FileUpload ID="fileVideo" runat="Server" /></p>
            </div>
            <div class="form-group">
                <div class="checkbox">
                    <label><asp:CheckBox ID="cbPrivateVideo" runat="server" /><%= Lang.Trans("Set this video as private")%></label>
                </div>
            </div>
        </div>
        <div class="col-sm-3 text-right">
            <asp:LinkButton CssClass="btn btn-primary" ID="btnUpload" runat="server" OnClick="btnUpload_Click"><i class="fa fa-upload"></i>&nbsp;<%= Lang.Trans("Upload")%></asp:LinkButton>
        </div>
    </div>
    <hr />
    <div class="actions">
        <span id="divUploadMultipleVideosFlash" runat="server">
            <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkUploadMultipleFlash" runat="server" onclick="lnkUploadMultipleFlash_Click"/>
        </span>
        <span id="divUploadMultipleVideosSilverlight" runat="server">
            <asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkUploadMultipleSilverlight" runat="server" onclick="lnkUploadMultipleSilverlight_Click"/>
        </span>
    </div>
<asp:Repeater ID="rptVideoUploads" runat="server" onitemcommand="rptVideoUploads_ItemCommand"  onitemcreated="rptVideoUploads_ItemCreated">
    <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
    <ItemTemplate>
        <li class="list-group-item">
        <div class="media">
            <span class="pull-left"><i class="fa fa-file-video-o fa-3x"></i></span>
            <div class="media-body">
                <div class="row">
                    <div class="col-sm-10">
                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'/>
                    </div>
                    <div class="col-sm-2">
                        <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnDeleteVideoUpload" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Delete"><i class="fa fa-trash-o"></i>&nbsp;<%= Lang.Trans("Delete") %></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        </li>
    </ItemTemplate>
</asp:Repeater>
<asp:Panel id="pnlUploadMultipleVideosFlash" runat="server" Visible="false">
    <components:FlashUpload ID="flashUpload" runat="server" OnUploadComplete="flashUploadIsCompleted()" UploadPage="../../FlashUpload.ashx" FileTypeDescription="Videos" FileTypes="*.mpeg; *.mov; *.mpg; *.avi; *.wmv" UploadFileSizeLimit="52428800" TotalUploadSizeLimit="209715200" />
</asp:Panel>
<asp:Panel ID="pnlUploadMultipleVideosSilverlight" runat="server" Visible="false">
    <div class="text-center"><asp:Silverlight ID="Silverlight1" runat="server" Source="~/ClientBin/MultiFileUploader.xap" MinimumVersion="2.0.31005.0" Width="400" Height="300"/></div>
</asp:Panel>
<asp:LinkButton CssClass="btn btn-default btn-sm" ID="lnkBackToVideoUploads" runat="server" onclick="lnkBackToVideoUploads_Click"><i class="fa fa-reply"></i>&nbsp;<%= Lang.Trans("Back to Video Uploads")%></asp:LinkButton>
</asp:View>
<asp:View ID="vEmbedVideo" runat="server">
    <h4><uc1:HeaderLine ID="HeaderLine2" runat="server" /></h4>
    <p class="help-block">
    <components:ContentView ID="cvNote2" Key="EmbedVideosNotes" runat="server">
        You can add videos from YouTube to your profile!
    </components:ContentView>
    </p>
    <asp:Label CssClass="alert text-danger" ID="lblError2" runat="server" EnableViewState="false"/>
<%--    <asp:UpdatePanel ID="UpdatePanelVideoEmbeds" runat="server">
        <ContentTemplate>--%>
            <asp:Panel ID="divVideoKeywords" DefaultButton="btnSearchVideoKeywords" runat="server">
                <div class="input-group input-group-sm filter">
                    <span class="input-group-addon"><%= Lang.Trans("Keywords") %></span>
                    <asp:TextBox CssClass="form-control" ID="txtVideoKeywords" runat="server"/>
                    <span class="input-group-btn"><asp:Button CssClass="btn btn-default" ID="btnSearchVideoKeywords" runat="server" OnClick="btnSearchVideoKeywords_Click" /></span>
                </div>
            </asp:Panel>
            <div id="divVideoPreview" class="text-center" visible="false" runat="server">
                <asp:Literal ID="ltrVideoPreview" runat="server"/>
                <div class="actions">
                    <asp:Button CssClass="btn btn-primary" ID="btnEmbedVideo" runat="server" OnClick="btnEmbedVideo_Click" />
                </div>
            </div>
            <div id="divVideoThumbnails" runat="server">
                <asp:DataList ID="dlVideos" CssClass="table table-striped" runat="server" OnItemCommand="dlVideos_ItemCommand">
                    <ItemTemplate>
                        <div class="media">
                            <asp:ImageButton CssClass="pull-left media-object" ImageUrl='<%# Eval("ThumbnailUrl") %>' CommandName="SelectVideo" CommandArgument='<%# Eval("ThumbnailUrl") + "|" + Eval("VideoUrl") + "|" + Eval("Title") %>'  Height="100" runat="server" />
                            <div class="media-body">
                                <asp:LinkButton Text='<%# Eval("Title") %>' CommandName="SelectVideo" CommandArgument='<%# Eval("ThumbnailUrl") + "|" + Eval("VideoUrl") + "|" + Eval("Title") %>' runat="server" />
                                <hr />
                                <asp:LinkButton CssClass="btn btn-default btn-sm" CommandName="RemoveVideo" CommandArgument='<%# ((int?)Eval("ID")) %>' Visible='<%# ((int)Eval("ID")) != 0 %>' runat="server">
                                    <i class="fa fa-trash-o"></i>&nbsp;<%= Lang.Trans("Remove Video")%>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div class="actions" id="pnlAddYouTubeVideoButton" runat="server">
                <asp:Button CssClass="btn btn-default" id="btnAddYouTubeVideo" runat="server" onclick="btnAddYouTubeVideo_Click" />
            </div>
            <div class="actions" id="pnlBackButton" runat="server">
                <asp:Button CssClass="btn btn-default" ID="btnBack" runat="server" onclick="btnBack_Click" />
            </div>
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:View>
</asp:MultiView>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
