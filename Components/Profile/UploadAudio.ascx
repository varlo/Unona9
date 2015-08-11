<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadAudio.ascx.cs" Inherits="AspNetDating.Components.Profile.UploadAudio" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
<uc1:HeaderLine ID="HeaderLine1" runat="server" />
<components:ContentView ID="cvNote" Key="UploadAudioNotes" runat="server">
    Your audio uploads are not going to be visible to the other members until they are approved by our team. This process can take up to 24 hours. Any audio uploads that doesn't conform to our specifications will be deleted.
</components:ContentView>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="false"/>
<div id="divUploadAudio" runat="server">
    <div class="form-group">
        <label><%= Lang.Trans("Title") %></label>
        <asp:TextBox ID="txtTitle" CssClass="form-control" runat="server"/>
    </div>
    <div class="form-group">
        <div class="form-inline">
            <label><%= Lang.Trans("Audio file") %></label>&nbsp;<p class="form-control-static form-control-inline"><asp:FileUpload ID="fileAudio" runat="Server" /></p>
        </div>
    </div>
    <div class="form-group">
        <div class="checkbox"><label><asp:CheckBox ID="cbPrivateAudio" runat="server" />
        <%= Lang.Trans("Set this audio as private")%></label></div>
    </div>
    <div class="actions"><asp:Button CssClass="btn btn-default" ID="btnUpload" runat="server" onclick="btnUpload_Click"/></div>
</div>
<hr />
<asp:Repeater ID="rptAudioUploads" runat="server" onitemcommand="rptAudioUploads_ItemCommand" onitemcreated="rptAudioUploads_ItemCreated">
    <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
    <ItemTemplate>
        <li class="list-group-item">
        <div class="media">
            <span class="pull-left"><i class="fa fa-file-audio-o fa-3x"></i></span>
            <div class="media-body">
                <div class="row">
                    <div class="col-sm-10">
                        <label><asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'/></label>
                        <div class="small text-muted"><asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'/></div>
                    </div>
                    <div class="col-sm-2 text-right">
                        <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnDeleteAudioUpload" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Delete"><i class="fa fa-trash-o"></i>&nbsp;<%= Lang.Trans("Delete") %></asp:LinkButton>
                    </div>
                </div>
            </div>
		</div>
		</li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
<br>
<div class="clear"></div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>