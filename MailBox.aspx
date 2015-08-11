<%@ Page Language="c#" MasterPageFile="Site.Master" Codebehind="MailBox.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating.MailBox" %>

<%@ Import Namespace="AspNetDating" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<aside>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"/>
        <ul class="nav">
            <li><asp:LinkButton ID="lnkInbox" runat="server" /></li>
            <li><asp:LinkButton ID="lnkOutbox" runat="server" /></li>
            <li><asp:LinkButton ID="lnkTrash" runat="server" /></li>
            <li id="divReceivedEcards" runat="server"><asp:LinkButton ID="lnkReceivedEcards" runat="server" /></li>
            <li id="divSentEcards" runat="server"><asp:LinkButton ID="lnkSentEcards" runat="server" /></li>
        </ul>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"/>
    </aside>
    <article>
        <asp:UpdatePanel ID="UpdatePanelGridMessages" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlMailBox" runat="server">
                    <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"/>
                    <components:BannerView id="bvMailboxRightTop" runat="server" Key="MailboxRightTop"/>
                    <asp:Panel DefaultButton="btnFilter" class="input-group input-group-sm filter" runat="server">
                        <span class="input-group-addon"><%= Lang.Trans("Username") %>:</span>
                        <asp:TextBox ID="txtSearchMail" CssClass="form-control" runat="server"/>
                        <div class="input-group-btn"><asp:LinkButton ID="btnFilter" CssClass="btn btn-default" runat="server" OnClick="btnFilter_Click" /></div>
                        <div class="input-group-btn"><asp:LinkButton ID="btnClearFilter" CssClass="btn btn-default" runat="server" OnClick="btnClearFilter_Click" /></div>
                    </asp:Panel>
                    <asp:DataGrid ID="gridMessages" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="table table-striped" ShowHeader="true" GridLines="None" onitemdatabound="gridMessages_ItemDataBound">
                        <Columns>
                            <asp:TemplateColumn HeaderText="&lt;input type=checkbox onClick=&quot;a=0;for(i=0; i&lt;this.form.elements.length;i++){if(this.form.elements[i]==this) {a=3}; if ((this.form.elements[i].type=='checkbox') &amp;&amp; (a!=0) &amp;&amp; (i&gt;a)) {this.form.elements[i].checked=this.checked}}&quot;&gt;">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="cbSelect" value='<%# Eval("Id") %>' runat="server" name="cbSelect" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                <ItemTemplate>
                                    <span class="text-muted small">
                                    <%-- <%# Convert.ToBoolean(Eval("IsRead")) ? String.Format("<i class=\"fa fa-envelope-o\"></i>", "Read".Translate()) : String.Format("<i class=\"fa fa-envelope\"></i>", "Unread".Translate())%>&nbsp; --%>
                                    <%# Convert.ToBoolean(Eval("IsDeleted")) ? String.Format("<i class=\"fa fa-trash-o\"></i>", "Deleted".Translate()) : ""%>
                                    <%# Convert.ToBoolean(Eval("IsRepliedTo")) ? String.Format("<i class=\"fa fa-reply\"></i>", "Replied".Translate()) : ""%>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemStyle></ItemStyle>
                                <ItemTemplate>
                                    <a href="<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>" target="_new"><%# ImageHandler.RenderImageTag((int)Eval("PhotoID"), 50, 50, null, false, true, true) %></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" ></HeaderStyle>
                                <ItemTemplate>
                                    <a class='<%# Convert.ToBoolean(Eval("IsRead")) ? "read" : "unread" %>' href="<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>" target="_new"><%# Eval("Username") %></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                <ItemTemplate>
                                    <a id="lnkReadMessage" class='<%# Convert.ToBoolean(Eval("IsRead")) ? "read" : "unread" %>' runat="server" href='<%# "ShowMessage.aspx?mid=" + Eval("Id") %>'><%# Eval("Message") %></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Right"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" Wrap="False" Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <span class="text-muted"><%# (DateTime.Now.Add(Config.Misc.TimeOffset).ToString("d") == Convert.ToDateTime(Eval("Date")).ToString("d"))?Convert.ToDateTime(Eval("Date")).ToString("t"):Convert.ToDateTime(Eval("Date")).ToString("d") %></span>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <asp:Label ID="lblError" CssClass="alert text-danger" runat="server"/>
                    <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnDelete" runat="server"/>
                    <asp:Panel ID="pnlPaginator" Visible="True" runat="server">
                        <ul class="pager">
                            <li><asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"/></li>
                            <li><asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click" /></li>
                            <li class="text-muted"><asp:Label ID="lblPager" runat="server"/></li>
                            <li><asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"/></li>
                            <li><asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"/></li>
                        </ul>
                    </asp:Panel>
                    <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"/>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkInbox" />
                <asp:AsyncPostBackTrigger ControlID="lnkOutbox" />
                <asp:AsyncPostBackTrigger ControlID="lnkSentEcards" />
                <asp:AsyncPostBackTrigger ControlID="lnkTrash" />
                <asp:AsyncPostBackTrigger ControlID="lnkReceivedEcards" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanelGridEcards" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlReceivedEcards" runat="server">
                    <uc1:LargeBoxStart ID="LargeBoxStart2" runat="server"/>
                    <asp:DataGrid ID="dgEcards" runat="server" PageSize="20" AllowPaging="True" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="table table-striped" GridLines="None">
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="40px" ></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="cbSelect2" value='<%# Eval("Id") %>' runat="server" name="Checkbox2" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                <itemstyle HorizontalAlign="Left"></itemstyle>
                                <ItemTemplate>
                                    <a class='<%# Convert.ToBoolean(Eval("IsOpened")) ? "read" : "unread" %>' href="<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>" target="_new"><%# Eval("Username") %></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                <itemstyle HorizontalAlign="Left"></itemstyle>
                                <ItemTemplate>
                                    <a class='<%# Convert.ToBoolean(Eval("IsOpened")) ? "read" : "unread" %>' href='ShowEcard.aspx?ect=<%# Eval("EcardTypeID") %>&ecid=<%# Eval("Id") %>'><%# Eval("EcardTypeName")%></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Right"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <span class="text-muted">
                                        <%# (DateTime.Now.Add(Config.Misc.TimeOffset).ToString("d") == Convert.ToDateTime(Eval("DatePosted")).ToString("d"))?Convert.ToDateTime(Eval("DatePosted")).ToString("t"):Convert.ToDateTime(Eval("DatePosted")).ToString("d") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnDeleteEcard" runat="server"/>
                    <asp:Label CssClass="alert text-danger" ID="lblMessage2" runat="server"/>
                    <uc1:LargeBoxEnd ID="Largeboxend2" runat="server"/>
                </asp:Panel>
                <components:BannerView id="bvMailboxRightBottom" runat="server" Key="MailboxRightBottom"/>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkInbox" />
                <asp:AsyncPostBackTrigger ControlID="lnkOutbox" />
                <asp:AsyncPostBackTrigger ControlID="lnkSentEcards" />
                <asp:AsyncPostBackTrigger ControlID="lnkTrash" />
                <asp:AsyncPostBackTrigger ControlID="lnkReceivedEcards" />
            </Triggers>
        </asp:UpdatePanel>
    </article>
</asp:Content>
