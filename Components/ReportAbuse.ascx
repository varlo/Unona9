<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportAbuse.ascx.cs" Inherits="AspNetDating.Components.ReportAbuse" %>
<%@ Register TagPrefix="cv" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<asp:Panel ID="pnlReportAbusePopup" runat="server">
	<asp:Label CssClass="alert text-danger" ID="lblError" EnableViewState="false" runat="server"/>
    <asp:MultiView ID="mvContentViews" runat="server">
        <asp:View ID="vProfile" runat="server">
            <cv:ContentView ID="cvProfile" runat="server">
                <!-- Place your content for reporting profile abuse here -->
            </cv:ContentView>
        </asp:View>
        <asp:View ID="vMessage" runat="server">
            <cv:ContentView ID="cvMessage" runat="server">
                <!-- Place your content for reporting message abuse here -->
            </cv:ContentView>        
        </asp:View>
        <asp:View ID="vPhoto" runat="server">
            <cv:ContentView ID="cvPhoto" runat="server">
                <!-- Place your content for reporting photo abuse here -->
            </cv:ContentView>        
        </asp:View>     
    </asp:MultiView>
    <label><asp:Literal ID="litText" runat="server"/></label>
    <asp:TextBox ID="txtReport" CssClass="form-control" TextMode="MultiLine" runat="server"/>
    <div class="actions">
        <asp:Button CssClass="btn btn-default" ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
        <asp:Button CssClass="btn btn-default" ID="btnSend" runat="server" OnClick="btnSend_Click"/>
    </div>  
</asp:Panel>

