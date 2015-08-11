<%@ Page Language="c#" MasterPageFile="~/Site.Master" CodeBehind="SendMessage.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating.SendMessage" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ MasterType TypeName="AspNetDating.Site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
    <aside>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"/>
        <div class="SideMenuItem">
            <asp:LinkButton ID="lnkBack" runat="server" />
        </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
        <!--<asp:Literal ID="ltrPhoto" runat="server"/>-->
        <ul class="info-header">
            <li>
                <a class="tooltip-link" title="<%= Lang.Trans("From") %>"><b><asp:Label ID="lblFromUsername" runat="server"/></b></a>
                &nbsp;<i class="fa fa-comments-o"></i>&nbsp;
                <a class="tooltip-link" title="<%= Lang.Trans("To") %>"><b><asp:Label ID="lblToUsername" runat="server"/></b></a>
            </li>
        </ul>
        <components:BannerView id="bvSendMessageRight" runat="server" Key="SendMessageRight"/>
        <div id="pnlUserResponse" runat="server">
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
                            <asp:ImageButton CssClass="prev" ID="ibtnPrevSmilies" OnClick="ibtnPrevSmilies_Click" runat="server" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updatePanelSmilies" runat="server">
                                <ContentTemplate>
                                    <asp:DataList ID="dlSmilies" SkinID="Smilies" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
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
                            <asp:ImageButton CssClass="next" ID="ibtnNextSmilies" OnClick="ibtnNextSmilies_Click" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:TextBox ID="txtMessageBody" CssClass="form-control" Rows="7" runat="server" TextMode="MultiLine"/>
        </div>
        <div class="actions">
            <asp:Button CssClass="btn btn-default" ID="btnSend" runat="server"/>
        </div>
        <div id="pnlPreviousMessages" visible="false" runat="server">
            <hr />
            <asp:Repeater ID="rptPreviousMessages" runat="server">
                <HeaderTemplate><ul class="list-group list-group-striped"></HeaderTemplate>
                <ItemTemplate>
                    <li class="list-group-item">
                        <b><%# Eval("Username") %></b>
                        <%# Eval("Message") %>
                    </li>
                </ItemTemplate>
                <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater>
        </div>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
    </article>
</asp:Content>