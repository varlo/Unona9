<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApprovePhotos2.aspx.cs" Inherits="AspNetDating.Admin.ApprovePhotos2" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
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
<asp:UpdatePanel ID="UpdatePanelPhotoDetails" runat="server">
	<ContentTemplate>
	    <uc1:MessageBox ID="MessageBox" runat="server"/>
	    <div id="stable" runat="server">
	        <div class="panel default-panel medium-width center-block">
                <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Photo Approval") %></h4></div>
                <div class="panel-body">
                    <div class="form-horizontal form-striped clearfix">
                        <div class="form-group">
                            <label class="control-label col-sm-4"><%= Lang.TransA("Username") %>:</label>
                            <div class="col-sm-8"><p class="form-control-static"><asp:Label ID="lblUsername" runat="server"/></p></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4"><%= Lang.TransA("Photo Name") %>:</label>
                            <div class="col-sm-8"><p class="form-control-static"><asp:Label ID="lblPhotoName" runat="server"/></p></div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4"><%= Lang.TransA("Photo Description") %>:</label>
                            <div class="col-sm-8"><p class="form-control-static"><asp:Label ID="lblPhotoDescription" runat="server"/></p></div>
                        </div>
                        <div class="form-group" id="trExplicitPhoto" runat="server">
                            <div class="col-sm-offset-4 col-sm-8">
                                <div class="checkbox"><label><asp:CheckBox ID="chkExplicitPhoto" runat="server"/></label></div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="actions">
                                <div class="btn-group">
                                    <asp:LinkButton CssClass="btn btn-secondary" ID="btnApprove" runat="server" OnClick="btnApprove_Click"/>
                                    <asp:LinkButton CssClass="btn btn-default" ID="btnReject" runat="server" OnClick="btnReject_Click"/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="input-group">
                        <span class="input-group-addon"><div class="checkbox"><asp:CheckBox ID="cbDeleteAccount" runat="server" /></div></span>
                        <asp:TextBox CssClass="form-control" ID="txtReason" onkeydown="javascript: RejectOnEnter()" runat="server"/>
                    </div>
                    <hr />
                    <asp:Image ID="imgBigPhoto" CssClass="img-thumbnail center-block" runat="server"/>
                </div>
            </div>
	    </div><!-- /#stable -->
	</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanelPhoto" runat="server">
    <ContentTemplate>
        <div class="table-responsive">
            <asp:DataList CssClass="table table-striped" RepeatColumns="5" ID="listPendingApproval" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal" OnItemCommand="listPendingApproval_ItemCommand">
                <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkApprovePhoto" CommandArgument='<%# Eval("PhotoID")%>' runat="server">
                        <img class="img-thumbnail" src="<%= Config.Urls.Home%>/Image.ashx?id=<%# Eval("PhotoID")%>&width=100&height=100&cache=1" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="actions">
            <asp:LinkButton CssClass="btn btn-primary" ID="btnApproveAll" runat="server" OnClick="btnApproveAll_Click" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
