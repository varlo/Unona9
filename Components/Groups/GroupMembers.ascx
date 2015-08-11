<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupMembers.ascx.cs" Inherits="AspNetDating.Components.Groups.GroupMembers" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="~/Components/Search/SearchResults.ascx" %>

<uc1:largeboxstart id="LargeBoxStart1" runat="server"/>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
<asp:MultiView ID="mvViewGroupMembers" runat="server">
    <asp:View ID="viewGroupMembers" runat="server">
        <div id="pnlGroupMembersSearch" runat="server" class="filter-group">
            <div class="col" id="pnlGender" runat="server" >
                <div class="input-group input-group-sm filter">
                    <span class="input-group-addon"><%= Lang.Trans("Gender") %></span>
                    <asp:DropDownList CssClass="form-control" ID="ddFilterByGender" runat="server"/>
                </div>
            </div>
            <div id="pnlAge" runat="server" class="col">
                <div class="input-group input-group-sm filter">
                    <span class="input-group-addon"><%= Lang.Trans("Age") %></span>
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" Size="2" MaxLength="2"/>
                    <span class="input-group-addon"><%= Lang.Trans("to") %></span>
                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" Size="2" MaxLength="2"/>
                </div>
            </div>
            <div class="col">
                <div class="input-group input-group-sm filter">
                    <span class="input-group-addon"><%= Lang.Trans("Type") %></span>
                    <asp:DropDownList CssClass="form-control" ID="ddGroupMemberType" runat="server"/>
                    <span class="input-group-btn"><asp:LinkButton ID="btnFilter" CssClass="btn btn-default" runat="server" OnClick="btnFilter_Click"/></span>
                </div>
            </div>
        </div>
        <uc1:SearchResults ID="SearchResults1" ShowSlogan="false" ShowCity="false" ShowIcons="false" ShowDistance="false" runat="server" />    
    </asp:View>
    <asp:View ID="viewDeleteOpitions" runat="server">
        <label><%= Lang.Trans("What do you want to do?") %></label>
        <div class="radio"><asp:RadioButtonList ID="rbList" runat="server" /></div>
        <div class="checkbox"><label><asp:CheckBox ID="cbKickMember" runat="server" /></label></div>
        <label class="checkbox-inline"><asp:CheckBox ID="cbBanMember" runat="server" /></label>
        <asp:DropDownList ID="ddBanPeriod" CssClass="form-control form-control-inline input-sm" runat="server"/>
        <div class="actions">
	        <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
	        <asp:Button CssClass="btn btn-primary" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
        </div>
    </asp:View>
</asp:MultiView>
<uc1:largeboxend id="LargeBoxEnd1" runat="server"/>

