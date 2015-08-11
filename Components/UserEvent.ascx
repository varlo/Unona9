<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Import Namespace="AspNetDating" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserEvent.ascx.cs" Inherits="AspNetDating.Components.UserEvent" %>
<div class="list-group">
	<div class="media">
        <!--<a class="pull-left" id="lnkLeftImage" runat="server" target="_blank">
            <img id="imgLeft" runat="server" class="media-object img-thumbnail" />
        </a>-->
		<div class="media-body">
		    <div id="divEventText" runat="server">
		        <asp:Literal ID="ltrEventText" runat="server" />
                <ul class="info-header">
                    <li id="spanEventTime" visible="false" runat="server">
                        <a class="tooltip-link" title="<%= Lang.Trans("Event Time") %>"><i class="fa fa-clock-o"></i>&nbsp;<asp:Literal ID="ltrEventTime" runat="server" /></a>
                    </li>
                    <li class="pull-right">
                        <span id="spanLeaveComment" runat="server">
                            <asp:LinkButton CssClass="btn btn-default btn-xs" ID="lnkLeaveComment" runat="server" onclick="lnkLeaveComment_Click" />
                        </span>
                        <span id="spanDelete" runat="server">
                            <asp:LinkButton ID="lnkDelete" CssClass="btn btn-default" runat="server" onclick="lnkDelete_Click" />
                        </span>
                    </li>
		        </ul>
		    </div>
		    <div class="repeater-horizontal">
		    <asp:Repeater ID="rptEventImages" EnableViewState="false" runat="server" onitemdatabound="rptEventImages_ItemDataBound">
		        <ItemTemplate>
                    <a class="thumbnail" href='<%# Eval("BigImageUrl") %>' target="_blank">
                        <img id="imgUrl" runat="server" src='<%# Eval("SmallImageUrl") %>'>
                    </a>
		        </ItemTemplate>
		    </asp:Repeater>
		    </div>
		    <div id="pnlEventComments" runat="server">
		        <div class="clearfix">
                    <span id="spanAddComment" runat="server" visible="false">
                        <asp:TextBox CssClass="form-control" ID="txtComment" runat="server" TextMode="MultiLine" Rows="2"/>
                        <span id="spanCancel" runat="server" visible="false">
                            <asp:Button CssClass="btn btn-default btn-sm" ID="btnCancel" runat="server" onclick="btnCancel_Click" />
                        </span>
                        <asp:LinkButton CssClass="btn btn-default btn-sm pull-right" ID="btnAddComment" runat="server" onclick="btnAddComment_Click" />
                    </span>
                </div>
                <ul class="list-group list-group-striped">
			    <asp:Repeater ID="rptEventComments" runat="server" onitemcommand="rptEventComments_ItemCommand" onitemcreated="rptEventComments_ItemCreated" onitemdatabound="rptEventComments_ItemDataBound">
			        <ItemTemplate>
			            <li class="list-group-item">
                        <div class="small"><a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a>&nbsp;<%# Eval("Comment") %></div>
                        <div class="clearfix">
                            <small class="text-muted"><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("Date") %></small>
                            <span class="pull-right"><asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-default btn-xs" CommandArgument='<%# Eval("ID") %>' CommandName="Delete"/></span>
                        </div>
                        </li>
			        </ItemTemplate>
			    </asp:Repeater>
			    </ul>
			    <div id="pnlShowMore" runat="server" visible="false" class="EventComment">
				    <asp:LinkButton ID="lnkShowMore" CssClass="btn btn-default btn-sm" runat="server" onclick="lnkShowMore_Click"/>
			    </div>
			</div>
		</div>
	</div>
</div>