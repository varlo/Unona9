<%@ Page language="c#" MasterPageFile="Site.Master" Codebehind="InviteFriend.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.InviteFriend" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<uc1:WideBoxStart id="WideBoxStart1" runat="server"/>
		<div class="form-horizontal">
		    <div class="form-group">
			    <label class="control-label col-sm-3"><%= Lang.Trans("Friend's Name") %></label>
			    <div class="col-sm-9"><input class="form-control" type=text ID="txtFriendsName" Runat="server"></div>
			</div>
			<div class="form-group">
			    <label class="control-label col-sm-3"><%= Lang.Trans("Friend's Email Address") %></label>
			    <div class="col-sm-9"><input class="form-control" type=text id="txtFriendsEmail" runat="server"></div>
			</div>
            <div class="form-group">
                <label class="control-label col-sm-3"><%= Lang.Trans("Personal note") %></label>
                <div class="col-sm-9"><div class="fckeditor">
                    <asp:PlaceHolder ID="phEditor" runat="server"/>
                </div>
                </div>
            </div>
            <div class="form-group">
                 <div class="col-sm-9 col-sm-offset-3">
                    <div class="actions">
                        <asp:button CssClass="btn btn-default" id="btnSubmit" tabIndex="1" runat="server"/>
                    </div>
                 </div>
            </div>
		</div>
        <asp:label CssClass="alert text-danger" id="lblError" runat="server"/>
	<uc1:WideBoxEnd id="WideBoxEnd1" runat="server"/>
</asp:Content>
