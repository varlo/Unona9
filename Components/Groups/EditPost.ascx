<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPost.ascx.cs" Inherits="AspNetDating.Components.Groups.EditPost" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagName="DatePicker" TagPrefix="uc1" Src="~/Components/DatePicker.ascx" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
<asp:Label CssClass="alert text-danger" ID="lblError" runat="server" EnableViewState="False"/>
    <div id="pnlTopic" class="form-group" runat="server">
        <label><%= Lang.Trans("Topic") %></label>
        <asp:TextBox ID="txtTopicName" CssClass="form-control" runat="server" />
    </div>
    <div class="form-group" id="pnlPost" runat="server">
        <label><%= Lang.Trans("Post content") %></label>
        <div id="pnlSmilies" runat="server">
            <script language="JavaScript" type="text/javascript">
							<!--
							function insertSmiley(text) {
								var area = document.forms[0].<%= MessageBodyClientId %>;
								area.focus();
								if (document.selection)
									document.selection.createRange().text = text;
								else
									area.value += text;
								return false;
							}
							//-->
            </script>
            <table class="table">
                <tr>
                    <td class="prev-next">
                        <asp:ImageButton CssClass="prev" ID="ibtnPrevSmilies" ImageUrl="" OnClick="ibtnPrevSmilies_Click" runat="server" />
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatePanelSmilies" runat="server">
                            <ContentTemplate>
                                <asp:DataList ID="dlSmilies" SkinID="Smilies" RepeatLayout="Flow" RepeatDirection="Horizontal" EnableViewState="false" runat="server">
                                    <ItemTemplate>
                                        <a href="#" onclick="return insertSmiley('<%# Eval("Key") %>')"><img src="<%# Eval("Image") %>" title="<%# Eval("Description") %>" /></a>
                                    </ItemTemplate>
                                </asp:DataList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ibtnPrevSmilies" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ibtnNextSmilies" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="prev-next">
                        <asp:ImageButton CssClass="next" ID="ibtnNextSmilies" ImageUrl="" OnClick="ibtnNextSmilies_Click" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:TextBox ID="txtPost" runat="server" CssClass="form-control" Rows="7" TextMode="MultiLine"/>
    </div>
    <div class="form-group" id="pnlEditReason" runat="server">
        <label><%= Lang.Trans("Edit reason") %></label>
        <asp:TextBox ID="txtEditReason" CssClass="form-control" runat="server"/>
    </div>
    <div class="form-group">
        <span id="pnlLocked" runat="server">
            <label class="checkbox-inline">
                <asp:CheckBox ID="cbLocked" runat="server" /><%= Lang.Trans("Locked") %>
            </label>
        </span>
    </div>
    <div class="form-group">
        <span id="pnlSticky" runat="server">
            <label class="checkbox-inline">
                <asp:CheckBox ID="cbSticky" runat="server" /><%= Lang.Trans("Sticky Until") %>
            </label>
        </span>
        <uc1:DatePicker ID="DatePicker1" CssClass="form-control-inline" runat="server"/>
    </div>
    <div class="form-group" id="pnlMoveTopic" visible="false" runat="server">
        <label><%= Lang.Trans("Move topic to") %></label>
        <asp:DropDownList CssClass="form-control" ID="ddMoveToGroups" runat="server" />
    </div>
    <asp:UpdatePanel class="form-group" ID="upPoll" runat="server">
        <ContentTemplate>
            <div id="pnlPoll" runat="server">
                <asp:CheckBox ID="cbCreatePoll" runat="server" AutoPostBack="true" OnCheckedChanged="cbCreatePoll_CheckedChanged" />
                <%= Lang.Trans("Create a poll") %>
                <div id="pnlChoices" runat="server" class="col-sm-offset-2" visible="false">
                    <asp:Repeater ID="rptChoices" runat="server">
                        <ItemTemplate>
                            <p>
                                <%# String.Format(Lang.Trans("Choice {0}"), Eval("NumberOfChoice"))%>&nbsp;<asp:TextBox ID="txtChoice" CssClass="form-control input-sm form-control-inline" runat="server"/>
                            </p>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
<div class="actions">
    <asp:Button CssClass="btn btn-default" ID="btnStartNewTopic" runat="server" OnClick="btnStartNewTopic_Click" />
    <asp:Button CssClass="btn btn-default" ID="btnCancelStartNewTopic" runat="server" OnClick="btnCancelStartNewTopic_Click" />
    <asp:Button CssClass="btn btn-default" ID="btnNewPost" runat="server" OnClick="btnNewPost_Click" />
    <asp:Button CssClass="btn btn-default" ID="btnCancelNewPost" runat="server" OnClick="btnCancelNewPost_Click" />
    <asp:Button CssClass="btn btn-default" ID="btnUpdatePost" runat="server" OnClick="btnUpdatePost_Click" />
    <asp:Button CssClass="btn btn-default" ID="btnCancelUpdatePost" runat="server" OnClick="btnCancelUpdatePost_Click" />
    <asp:Button CssClass="btn btn-default" ID="btnUpdateTopic" runat="server" OnClick="btnUpdateTopic_Click" />
    <asp:Button CssClass="btn btn-default" ID="btnCancelUpdateTopic" runat="server" OnClick="btnCancelUpdateTopic_Click" />
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
