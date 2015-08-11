<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditPhotosCtrl.ascx.cs" Inherits="AspNetDating.Admin.EditPhotosCtrl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:MessageBox id="MessageBox" runat="server"/>
<asp:DataList id="dlPhotos" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" CssClass="table table-striped">
		<ItemTemplate>
			<div class="media">
                <a class="pull-left img-thumbnail text-center" style="min-width: 180px" href="<%= Config.Urls.Home%>/Image.ashx?id=<%# Eval("PhotoID") %>&seed=<%= new Random().NextDouble().ToString() %>" target="_new">
                    <img src="<%= Config.Urls.Home%>/Image.ashx?id=<%# Eval("PhotoID") %>&width=160&height=160&seed=<%= new Random().NextDouble().ToString() %>">
                </a>
				<div class="media-body">
					<!--<asp:Label ID="lblError" ForeColor="Red" EnableViewState="False" Font-Bold="True" Runat="server" />-->
                    <div class="form-horizontal medium-width form-sm">
                        <div class="form-group">
                            <label class="control-label col-sm-2"><asp:Label id="lblName" Runat="server"><%# Lang.TransA("Name") %></asp:Label>:</label>
                            <div class="col-sm-10">
                                <asp:TextBox CssClass="form-control" id="txtName" Runat="server" Text='<%# Eval("Name") %>'/>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2"><asp:Label id="lblDescription" Runat="server"><%# Lang.TransA("Description")%></asp:Label>:</label>
                            <div class="col-sm-10">
                                <asp:TextBox CssClass="form-control" id="txtDescription" Runat="server" TextMode="MultiLine" Text='<%# Eval("Description") %>'/>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2"><asp:Label ID="lblPhoto" Runat="server"><%# Lang.TransA("Photo")%></asp:Label>:</label>
                            <div class="col-sm-10">
                                <p class="form-control-static"><input id="ufPhoto" class="pull-left" type="file" runat="server" NAME="ufPhoto" />&nbsp;
                                <asp:LinkButton CssClass="btn btn-primary btn-xs" id="btnUpload" CommandName="UploadPhoto" CommandArgument='<%# ((string)(Eval("PhotoId")) == "0")?NewTempID:Eval("PhotoId") %>' Runat="server">
                                    <i class="fa fa-upload"></i>&nbsp;<%# AspNetDating.Classes.Lang.TransA("Upload")%>
                                </asp:LinkButton>
                                <input id="hidPictureID" type="hidden" value='<%# ((string)(Eval("PhotoId")) == "0")?CurrentTempID:Eval("PhotoId") %>' runat="server" NAME="hidPictureID" />
                                </p>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-10 col-sm-offset-2">
                                <div class="checkbox-inline">
                                    <label><asp:CheckBox ID="chkExplicitPhoto" Runat="server" Text='<%# AspNetDating.Classes.Lang.TransA("Explicit Photo")%>' Checked='<%# Eval("ExplicitPhoto") %>' Visible='<%# (AspNetDating.Classes.Config.Photos.EnableExplicitPhotos && ((string)Eval("PhotoId") != "0"))%>'/></label>
                                </div>
                                <%#  (AspNetDating.Classes.Config.Photos.EnableExplicitPhotos && ((string)Eval("PhotoId") != "0"))?"&nbsp;&nbsp;":""%>
                                <asp:LinkButton CssClass="btn btn-primary btn-xs"  id="lnkSetPrimary" CommandName="SetPrimary" Runat="server" Visible='<%# !Convert.ToBoolean(Eval("Primary"))&&((string)Eval("PhotoId")) != "0" %>' CommandArgument='<%# Eval("PhotoId")%>'>
                                    <i class="fa fa-star-o"></i>&nbsp;<%# Lang.TransA("Set Primary")%>
                                </asp:LinkButton>
                                <%# (!Convert.ToBoolean(Eval("Primary"))&&((string)Eval("PhotoId")) != "0" )?"&nbsp;&nbsp;":""%>
                                <asp:LinkButton CssClass="btn btn-default btn-xs" id="lnkDelete" CommandName="Delete" Runat="server" Visible='<%# ((string)Eval("PhotoId")) != "0" %>' CommandArgument='<%# Eval("PhotoId")%>'>
                                    <i class="fa fa-times"></i>&nbsp;<%# Lang.TransA("Delete")%>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
				</div>
			</div>
	  </ItemTemplate>
</asp:DataList>
<div class="actions">
    <div class="btn-group">
        <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server"/>
        <asp:Button CssClass="btn btn-secondary" ID="btnSave" runat="server"/>
    </div>
</div>
<input id="hidUsername" type="hidden" name="hidUsername" runat="server" />