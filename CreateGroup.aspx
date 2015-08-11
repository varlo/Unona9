<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="CreateGroup.aspx.cs" Inherits="AspNetDating.CreateGroup" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server"/>
	        <ul class="nav">
		        <li><asp:linkbutton id="lnkBrowseGroups" Runat="server" OnClick="lnkBrowseGroups_Click"/></li>
		    </ul>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
        <p><components:ContentView ID="cvCreateGroupRequirement" Key="CreateGroupRequirements" runat="server">
            Don't forget to invite your friends once you create the group.
            As a group owner, it's your responsibility to make your group a fun and safe place.
        </components:ContentView></p>
        <asp:Label ID="lblError" CssClass="alert text-danger" runat="server" EnableViewState="False"/>
        <div class="form-group">
            <label><%= Lang.Trans("Name") %></label>
            <asp:TextBox ID="txtName" CssClass="form-control" runat="server"/>
        </div>
        <div class="form-group">
            <label><%= Lang.Trans("Categories") %></label>
            <asp:ListBox ID="lbCategories" runat="server" CssClass="form-control" SelectionMode="Multiple"/>
            <small class="text-muted"><i class="fa fa-lightbulb-o"></i> <%= Lang.Trans("Hold Ctrl for multiple selection") %></small>
        </div>
        <div class="form-group">
            <label><%= Lang.Trans("Description") %></label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" Columns="50"/>
        </div>
        <div class="form-group">
            <label><%= Lang.Trans("Terms & Conditions") %></label>
            <asp:TextBox ID="txtTerms" CssClass="form-control" TextMode="MultiLine" Rows="7" runat="server"/>
        </div>
        <div class="form-horizontal">
        	<div class="form-group">
        		<label class="control-label col-sm-4"><%= Lang.Trans("Group Image") %></label>
 	       		<div class="col-sm-8">
 	       			<p class="form-control-static"><asp:FileUpload ID="fuGroupImage" runat="server"/></p>
        		</div>
        	</div>
        	<div class="form-group">
        		<label class="control-label col-sm-4"><%= Lang.Trans("Access Level") %></label>
 	       		<div class="col-sm-8">
 	       			<asp:DropDownList ID="ddAccessLevel" AutoPostBack="true" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddAccessLevel_SelectedIndexChanged"/>
        		</div>
        	</div>
        	<div class="form-group">
 	       		<label class="control-label col-sm-4"><%= Lang.Trans("Minimum age restriction")%></label>
   			    <div class="col-sm-8">
   			        <asp:DropDownList ID="ddAgeRestriction" CssClass="form-control" runat="server"/>
        		</div>
        	</div>
        	<div id="pnlAutomaticallyJoin" class="form-group" runat="server" Visible="false">
 	       		<label class="control-label col-sm-4"><%= Lang.Trans("Users automatically join group")%></label>
   			    <div class="col-sm-8">
   			        <asp:CheckBox ID="cbAutomaticallyJoin" runat="server" AutoPostBack="true" oncheckedchanged="cbAutomaticallyJoin_CheckedChanged" />
        		</div>
        	</div>
        	<div id="trCountry" class="form-group" runat="server" visible="false">
				<label class="control-label col-sm-4"><%= Lang.Trans("Country") %></label>
				<div class="col-sm-8">
					<select ID="dropCountry" enableviewstate="false" class="form-control" runat="server"></select>
				</div>
			</div>
			<div id="trState" class="form-group" runat="server" visible="false">
				<label class="control-label col-sm-4"><%= Lang.Trans("Region/State") %></label>
				<div class="col-sm-8">
    				<select ID="dropRegion" class="form-control" runat="server"></select>
				</div>
			</div>
			<div id="trCity" class="form-group" runat="server" visible="false">
				<label class="control-label col-sm-4"><%= Lang.Trans("City") %></label>
				<div class="col-sm-8">
               		<select ID="dropCity" CssClass="form-control" runat="server"></select>
				</div>
			</div>
        </div><!-- /.form-horizontal -->
        <asp:UpdatePanel Class="row" ID="UpdatePanelPhotoDetails" runat="server">
            <ContentTemplate>
                <div id="pnlQuestion" class="col-sm-8 col-sm-offset-4" visible="false" runat="server">
                    <%= Lang.Trans("Enter a question to ask the user when joining the group") %>
                    <asp:TextBox ID="txtQuestion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                    <small class="text-muted"><i class="fa fa-lightbulb-o"></i> <%= Lang.Trans("You may leave this field blank") %></small>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddAccessLevel" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <hr />
        <div class="text-center">
            <asp:Button ID="btnCreateGroup" CssClass="btn btn-default" runat="server" OnClick="btnCreateGroup_Click" />
            <asp:Button ID="btnCancel" CssClass="btn btn-link" runat="server" OnClick="btnCancel_Click" Text="Button" />
		</div>
    	<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>
	</article>
</asp:Content>
