<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditMetaTags.aspx.cs" Inherits="AspNetDating.Admin.EditMetaTags" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
    <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Available tags") %></h4></div>
    <div class="panel-body">
        <ul class="list-group list-group-striped">
            <li class="list-group-item"><b>%%USERNAME%%</b> - Username</li>
            <li class="list-group-item"><b>%%AGE%%</b> - User age</li>
            <li class="list-group-item"><b>%%GENDER%%</b> - User gender</li>
            <li class="list-group-item"><b>%%COUNTRY%%</b> - User country</li>
            <li class="list-group-item"><b>%%STATE%%</b> - User state</li>
            <li class="list-group-item"><b>%%ZIP%%</b> - User ZIP code</li>
            <li class="list-group-item"><b>%%CITY%%</b> - User city</li>
            <asp:Literal ID="ltrTags" runat="server"/>
            <li class="list-group-item"><b>%%CATEGORIES%%</b> - Group categories</li>
            <li class="list-group-item"><b>%%NAME%%</b> - Group name</li>
            <li class="list-group-item"><b>%%DATECREATED%%</b> - Group creation date</li>
            <li class="list-group-item"><b>%%OWNER%%</b> - Group owner</li>
            <li class="list-group-item"><b>%%TYPE%%</b> - Group type</li>
            <li class="list-group-item"><b>%%MEMBERS%%</b> - Number of group members</li>
            <li class="list-group-item"><b>%%NAME%%</b> - Topic name</li>
            <li class="list-group-item"><b>%%GROUP%%</b> - Topic group</li>
            <li class="list-group-item"><b>%%SUBJECT%%</b> - Classified subject</li>
            <li class="list-group-item"><b>%%CATEGORY%%</b> - Classified category</li>
            <li class="list-group-item"><b>%%DATE%%</b> - Classified creation date</li>
            <li class="list-group-item"><b>%%EXPIRATIONDATE%%</b> - Classified expiration date</li>
            <li class="list-group-item"><b>%%LOCATION%%</b> - Classified location</li>
            <li class="list-group-item"><b>%%POSTEDBY%%</b> - Classified poster</li>
        </ul>
    </div>
</div>
<div class="panel clear-panel">
    <div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Meta tags") %></h4></div>
    <div class="panel-body">
        <table class="table table-striped">
    <tr>
        <td>
            <%= Lang.TransA("Default Title") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtDefaultTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Default Meta Description") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtDefaultMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Default Meta Keywords") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtDefaultMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowUser.aspx, ShowUserBlog.aspx, ShowUserEvents.aspx, ShowUserPhotos.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowUserTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowUser.aspx, ShowUserBlog.aspx, ShowUserEvents.aspx, ShowUserPhotos.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowUserMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowUser.aspx, ShowUserBlog.aspx, ShowUserEvents.aspx, ShowUserPhotos.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowUserMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Ads.aspx Title") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtAdsTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Ads.aspx Meta Description") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtAdsMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Ads.aspx Meta Keywords") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtAdsMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ChangeLostPassword.aspx Title") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtChangeLostPasswordTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ChangeLostPassword.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtChangeLostPasswordMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ChangeLostPassword.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtChangeLostPasswordMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Default.aspx Title") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtDefaultPageTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Default.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtDefaultPageMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Default.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtDefaultPageMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Groups.aspx Title") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtGroupsTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Groups.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtGroupsMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Groups.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtGroupsMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Login.aspx Title") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtLoginTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Login.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtLoginMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Login.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtLoginMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("LostPassword.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtLostPasswordTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("LostPassword.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtLostPasswordMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("LostPassword.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtLostPasswordMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("News.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtNewsTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("News.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtNewsMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("News.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtNewsMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Register.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtRegister" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Register.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtRegisterMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Register.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtRegisterMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Search.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtSearchTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Search.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtSearchMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("Search.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtSearchMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("SendProfile.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtSendProfileTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("SendProfile.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtSendProfileMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("SendProfile.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtSendProfileMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowAd.aspx Title") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowAdTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowAd.aspx Meta Description") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowAdMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowAd.aspx Meta Keywords") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowAdMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroup.aspx Title") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroup.aspx Meta Description") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroup.aspx Meta Keywords") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroupEvents.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupEventsTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroupEvents.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupEventsMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroupEvents.aspx Meta Keywords") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupEventsMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroupPhotos.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupPhotosTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroupPhotos.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupPhotosMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroupPhotos.aspx Meta Keywords") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupPhotosMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroupTopics.aspx Title") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupTopicTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroupTopics.aspx Meta Description") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupTopicMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("ShowGroupTopics.aspx Meta Keywords") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtShowGroupTopicMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("SmsConfirm.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtSmsConfirmTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("SmsConfirm.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtSmsConfirmMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("SmsConfirm.aspx Meta Keywords")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtSmsConfirmMetaKeywords" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("TopCharts.aspx Title")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtTopChartsTitle" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("TopCharts.aspx Meta Description")%>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtTopChartsMetaDescription" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= Lang.TransA("TopCharts.aspx Meta Keywords") %>
        </td>
        <td>
            <asp:TextBox CssClass="form-control" ID="txtTopChartsMetaKeywords" runat="server"/>
        </td>
    </tr>
</table>
    </div>
</div>
<div class="actions">
    <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" OnClick="btnSave_Click" />
</div>
</asp:Content>
