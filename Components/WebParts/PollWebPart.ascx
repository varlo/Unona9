<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PollWebPart.ascx.cs" Inherits="AspNetDating.Components.WebParts.PollWebPart" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>

<asp:MultiView ID="mvPolls" ActiveViewIndex="0" runat="server">
<asp:View ID="vPoll" runat="server">
    <label><asp:Label ID="lblQuestion" runat="server"/></label>
    <asp:Repeater ID="rptPoll" Runat="server">
        <ItemTemplate>
            <div class="radio">
                <label>
                    <asp:HiddenField ID="hidID" Value='<%# Eval("ID") %>' runat="server" />
                    <components:GroupRadioButton ID="rbChoice" GroupName="PollChoices" runat="server" /> <%# Eval("ChoiceValue") %>
                </label>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="actions">
        <uc1:FlexButton CssClass="btn btn-default btn-sm" ID="fbVote" runat="server" RenderAs="Button" OnClick="fbVote_Click" />
        <uc1:FlexButton CssClass="btn btn-default btn-sm" ID="fbResults" runat="server" RenderAs="Button" OnClick="fbResults_Click" />
    </div>
</asp:View>
<asp:View id="vPollResults" runat="server">
    <label><asp:Label ID="lblQuestion2" runat="server"/></label>
    <ul class="list-group">
    <asp:Repeater ID="rptResults" Runat="server">
        <ItemTemplate>
            <li class="list-group-item">
                <div class="row">
                    <div class="col-sm-6"><%# Eval("ChoiceValue") %></div>
                    <div class="col-sm-6 small">
                        <span class="poll-line" style='width:<%# Convert.ToInt32((double)Eval("Percentage")*100) %>px'>&nbsp;</span>&nbsp;<%# String.Format("{0:P}", Eval("Percentage")) %>
                    </div>
                </div>
            </li>
        </ItemTemplate>
    </asp:Repeater>
    </ul>
    <div class="info-header info-header-sm text-right">
        <%= Lang.Trans("Total Votes") %>&nbsp;<b><asp:Label ID="lblTotalVotes" runat="server"/></b>
    </div>
    <uc1:FlexButton CssClass="btn btn-default btn-sm" ID="fbBack" runat="server" RenderAs="Button" OnClick="fbBack_Click" />
</asp:View>
<asp:View ID="vNoPolls" runat="server">
    <div class="text-center">
        <%= Lang.Trans("There are no polls available!") %></div>
</asp:View>
</asp:MultiView>
