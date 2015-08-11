<%@ Import Namespace="AspNetDating" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewGroupPhotos.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewGroupPhotos" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>



<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
<asp:Label ID="lblError" runat="server" CssClass="alert text-danger" EnableViewState="False"/>
    <div id="divSlideshowLink" class="text-right" runat="server">
        <asp:LinkButton ID="lnkRunSlideshow" runat="server" OnClick="lnkRunSlideshow_Click"/>
        <hr />
    </div>
    <div id="divSlideshow" class="text-center" runat="server" visible="false">
        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0" width="<%= Config.Photos.PhotoMaxWidth %>" height="<%= Config.Photos.PhotoMaxHeight %>" id="flvplayer">
            <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/ImageRotator/imagerotator.swf">
            <param name="quality" value="high">
            <param name="bgcolor" value="#FFFFFF">
            <param name="wmode" value="transparent">
            <param name="allowfullscreen" value="true">
            <param name="menu" value="false">
            <param name="flashvars" value="width=<%= Config.Photos.PhotoMaxWidth %>&height=<%= Config.Photos.PhotoMaxHeight %>&file=Components/Groups/<%= "GroupPhotoData.ashx?gid=" + GroupID %>&shuffle=false" />
            <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/ImageRotator/imagerotator.swf"
                quality="high" wmode="transparent" bgcolor="#FFFFFF" width="<%= Config.Photos.PhotoMaxWidth %>"
                height="<%= Config.Photos.PhotoMaxHeight %>" name="flvplayer" align="" type="application/x-shockwave-flash"
                allowfullscreen="true" menu="false" pluginspage="http://www.macromedia.com/go/getflashplayer"
                flashvars="width=<%= Config.Photos.PhotoMaxWidth %>&height=<%= Config.Photos.PhotoMaxHeight %>&file=Components/Groups/<%= "GroupPhotoData.ashx?gid=" + GroupID %>&shuffle=false"></embed></object>
    </div>
    <asp:MultiView ID="mvGroupPhotos" ActiveViewIndex="0" runat="server">
        <asp:View ID="viewGroupPhotosView" runat="server">
            <div class="global-gallery">
                <asp:DataList ID="dlGroupPhotos" CssClass="repeater-horizontal clearfix" runat="server" RepeatLayout="Flow" GridLines="None" OnItemCommand="dlGroupPhotos_ItemCommand" OnItemDataBound="dlGroupPhotos_ItemDataBound">
                    <ItemTemplate>
                        <div class="thumbnail">
                            <a href='GroupImage.ashx?gpid=<%# Eval("GroupPhotoID") %>&diskCache=1' title='<%# Eval("Name") %>' data-type="image" data-toggle="lightbox" data-gallery="global-gallery"
                            data-parent=".global-gallery" data-title="<%# Eval("Name") %>" data-footer='<%# Eval("Description") %>
                                                                                                        <ul class="info-header info-header-sm">
                                                                                                            <li><i class="fa fa-user"></i>&nbsp;<a href="<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username"))%>"><%# Eval("Username") %></a></li>
                                                                                                            <li><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("Date") %></li>
                                                                                                        </ul>'>
                                <img src='GroupImage.ashx?gpid=<%# Eval("GroupPhotoID") %>&width=200&height=200&diskCache=1' class="img-responsive" />
                            </a>
                            <div class="thumb-icons">
                                <small class="text-muted"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("Date") %></small>
                                <div id="ulEditControls" class="pull-right" runat="server">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("GroupPhotoID") %>'><i class="fa fa-pencil"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("GroupPhotoID") %>'><i class="fa fa-trash-o"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <asp:Panel ID="pnlPaginator" runat="server">
                <ul class="pager">
                    <li><asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"/></li>
                    <li><asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"/></li>
                    <li class="text-muted"><asp:Label ID="lblPager" runat="server"/></li>
                    <li><asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"/></li>
                    <li><asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"/></li>
                </ul>
            </asp:Panel>
            <div class="actions">
                <asp:Button ID="btnUploadPhoto" CssClass="btn btn-default" runat="server" OnClick="btnUploadPhoto_Click" />
            </div>
        </asp:View>
        <asp:View ID="viewDeleteOptions" runat="server">
            <label><%= Lang.Trans("What do you want to do?") %></label>
            <div class="input-group">
                <span class="input-group-addon"><asp:RadioButtonList ID="rbList" runat="server" /></span>
                <span class="input-group-addon"><asp:CheckBox ID="cbKickMember" runat="server" /></span>
                <span class="input-group-addon"><asp:CheckBox ID="cbBanMember" runat="server" /></span>
                <asp:DropDownList CssClass="form-control" ID="ddBanPeriod" runat="server"/>
            </div>
            <div class="actions">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
        <asp:View ID="viewEditGroupPhoto" runat="server">
            <img class="img-thumbnail center-block" src='GroupImage.ashx?gpid=<%= GroupPhotoID %>&width=450&height=450&diskCache=1' />
            <hr />
            <div class="form-group">
              <label><%= Lang.Trans("Name") %></label>
              <asp:TextBox ID="txtName" CssClass="form-control" runat="server"/>
            </div>
            <div class="form-group">
                <label><%= Lang.Trans("Description") %></label>
                <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="5" Columns="50"/>
            </div>
            <div class="actions">
                <asp:Button ID="btnUpdate" CssClass="btn btn-default" runat="server" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-link" runat="server" OnClick="btnCancelUpdate_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
