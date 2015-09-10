<%@ Import Namespace="AspNetDating.Admin" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AdminMenu.ascx.cs" Inherits="AspNetDating.Admin.AdminMenu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div class="panel-group" id="accordion">
	<div class="panel">
	    <div class="panel-heading">
            <h4 class="panel-title">
                <a href="Home.aspx" class="collapsed"><i class="fa home"></i>&nbsp;<%= Lang.TransA("Dashboard") %></a>
            </h4>
        </div>
    </div>
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "user-management" ? "expanded" : "" %>">
    	<div class="panel-heading">
    	    <h4 class="panel-title">
    	        <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseOne"><i class="fa user-management"></i>&nbsp;<%= Lang.TransA("User Management") %><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
    	    </h4>
        </div>
        <div id="collapseOne" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "user-management" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li><a href="BrowseUsers.aspx"><%= Lang.TransA("Browse Members") %></a></li>
                    <li><a href="BrowsePhotos.aspx"><%= Lang.TransA("Browse Photos") %></a></li>
                    <li><a href="BrowseMessages.aspx"><%= Lang.TransA("Browse Messages") %></a></li>
                    <li><a href="BrowseSpamSuspects.aspx"><%= Lang.TransA("Browse Spam Suspects") %></a></li>
                    <li><a href="BrowseVideoUploads.aspx"><%= Lang.TransA("Browse Video Uploads") %></a></li>
                    <li><a href="ApprovePhotos2.aspx"><%= Lang.TransA("Approve Photos") %></a></li>
                    <li id="pnlApproveSalutePhotos" runat="server"><a href="ApproveSalutePhotos.aspx"><%= Lang.TransA("Approve Salute Photos") %></a></li>
                    <li id="pnlApproveVideoUploads" runat="server"><a href="ApproveVideoUploads.aspx"><%= Lang.TransA("Approve Video Uploads") %></a></li>
                    <li id="pnlApproveAudioUploads" runat="server"><a href="ApproveAudioUploads.aspx"><%= Lang.TransA("Approve Audio Uploads") %></a></li>
                    <li><a href="ApproveAnswers.aspx"><%= Lang.TransA("Approve Answers") %></a></li>
                    <li><a href="ApproveBlogPosts.aspx"><%= Lang.TransA("Approve Blog Posts") %></a></li>
                    <li><a href="EditScheduledAnnouncements.aspx"><%= Lang.TransA("Send Announcement") %></a></li>
                    <li id="pnlSpamCheck" runat="server"><a href="SpamCheck.aspx"><%= Lang.TransA("Spam Check") %></a></li>
                    <li id="pnlAbuseReports" runat="server"><a href="AbuseReports.aspx"><%= Lang.TransA("Abuse Reports") %></a></li>
                    <li><a href="ManageUserLevels.aspx"><%= Lang.TransA("User Levels") %></a></li>
                    <li><a href="CreditsHistory.aspx"><%= Lang.TransA("Credits History") %></a></li>
                </ul>
            </div>
        </div>
    </div>
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "contests" ? "expanded" : "" %>">
	    <div class="panel-heading">
            <h4 class="panel-title">
                <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseTwo"><i class="fa contests"></i>&nbsp;<%= Lang.TransA("Contests") %><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
            </h4>
        </div>
        <div id="collapseTwo" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "contests" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li id="pnlManageContests" runat="server"><a href="ManageContests.aspx"><%= Lang.TransA("Manage Contests") %></a></li>
                    <li id="pnlContestEntries" runat="server"><a href="ContestEntries.aspx"><%= Lang.TransA("Contest Entries") %></a></li>
                </ul>
            </div>
        </div>
    </div> 
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "classifieds" ? "expanded" : "" %>">
	    <div class="panel-heading">
	        <h4 class="panel-title">
	            <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseThree"><i class="fa classifieds"></i>&nbsp;<%= Lang.TransA("Classifieds") %><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
	        </h4>
	    </div>
        <div id="collapseThree" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "classifieds" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li><a href="EditAdsCategories.aspx"><%= Lang.TransA("Manage Categories")%></a></li>
                    <li><a href="ApproveAds.aspx"><%= Lang.TransA("Approve Classifieds") %></a></li>
                </ul>
            </div>
        </div>
    </div> 
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "group-management" ? "expanded" : "" %>">
	    <div class="panel-heading">
	        <h4 class="panel-title">
	            <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseFour"><i class="fa group-management"></i>&nbsp;<%= Lang.TransA("Group Management") %><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
	        </h4>
	    </div>
        <div id="collapseFour" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "group-management" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li><a href="ManageGroupCategories.aspx"><%= Lang.TransA("Manage Categories") %></a></li>
                    <li><a href="BrowseGroups.aspx"><%= Lang.TransA("Browse Groups") %></a></li>
                    <li><a href="ApproveGroups.aspx"><%= Lang.TransA("Approve Groups") %></a></li>
                </ul>
            </div>
        </div>
    </div>
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "site-management" ? "expanded" : "" %>">
	    <div class="panel-heading">
	        <h4 class="panel-title">
	            <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseFive"><i class="fa site-management"></i>&nbsp;<%= Lang.TransA("Site Management") %><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
	        </h4>
	    </div>
        <div id="collapseFive" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "site-management" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li><a href="EditLanguages.aspx"><%= Lang.TransA("Edit Languages") %></a></li>
                    <li><a href="EditTopics.aspx"><%= Lang.TransA("Edit Topics") %>&amp;<%= Lang.TransA("Questions") %></a></li>
                    <li><a href="EditNews.aspx"><%= Lang.TransA("Edit News") %></a></li>
                    <li><a href="ManageWebParts.aspx"><%= Lang.TransA("Manage Web Parts") %></a></li>
                    <li><a href="ManagePolls.aspx"><%= Lang.TransA("Manage Polls") %></a></li>
                    <li><a href="ManageBadWords.aspx"><%= Lang.TransA("Edit Bad Words") %></a></li>
                    <li><a href="EditTemplates.aspx"><%= Lang.TransA("Edit Templates") %></a></li>
                    <li><a href="EditGoogleAnalytics.aspx"><%= Lang.TransA("Edit Google Analytics") %></a></li>
                    <li><a href="EditBanners.aspx"><%= Lang.TransA("Edit Banners") %></a></li>
                    <li><a href="UploadLogo.aspx"><%= Lang.TransA("Upload Site Logo") %></a></li>
                    <li><a href="EditContentPages.aspx"><%= Lang.TransA("Edit Content Pages") %></a></li>
                    <li><a href="EditContentViews.aspx"><%= Lang.TransA("Edit Content Views") %></a></li>
                    <li><a href="EditEcardTypes.aspx"><%= Lang.TransA("Edit e-card types") %></a></li>
                    <li><a href="EditStrings.aspx"><%= Lang.TransA("Text Management") %></a></li>
                    <li><a href="ThemeManager.aspx"><%= Lang.TransA("Themes Manager") %></a></li>
                    <li><a href="Settings.aspx"><%= Lang.TransA("Settings") %></a></li>
                </ul>
            </div>
        </div>
    </div>
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "admin-management" ? "expanded" : "" %>">
	    <div class="panel-heading">
	        <h4 class="panel-title">
	            <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseSix"><i class="fa admin-management"></i>&nbsp;<%= Lang.TransA("Admin Management") %><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
	        </h4>
	    </div>
        <div id="collapseSix" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "admin-management" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li><a href="BrowseAdmins.aspx"><%= Lang.TransA("Browse Admins") %></a></li>
                </ul>
            </div>
        </div>
    </div>
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "payment-management" ? "expanded" : "" %>">
	    <div class="panel-heading">
	        <h4 class="panel-title">
	            <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseSeven"><i class="fa payment-management"></i>&nbsp;<%= Lang.TransA("Payment Management") %><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
	        </h4>
	    </div>
        <div id="collapseSeven" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "payment-management" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li><a href="BillingSettings.aspx"><%= Lang.TransA("Billing Settings") %></a></li>
                    <li><a href="CreditsPackages.aspx"><%= Lang.TransA("Credits Packages") %></a></li>
                </ul>
            </div>
        </div>
    </div>
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "seo-management" ? "expanded" : "" %>">
	    <div class="panel-heading">
	        <h4 class="panel-title">
	            <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseEight"><i class="fa seo-management"></i>&nbsp;<%= Lang.TransA("SEO Management") %><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
	        </h4>
	    </div>
        <div id="collapseEight" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "seo-management" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li><a href="EditMetaTags.aspx"><%= Lang.TransA("Edit Meta Tags") %></a></li>
                </ul>
            </div>
        </div>
    </div>
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "affiliate-management" ? "expanded" : "" %>">
	    <div class="panel-heading">
	        <h4 class="panel-title">
	            <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseNine"><i class="fa affiliate-management"></i>&nbsp;<%= Lang.TransA("Affiliate Management") %><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
	        </h4>
	    </div>
        <div id="collapseNine" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "affiliate-management" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li><a href="BrowseAffiliates.aspx"><%= Lang.TransA("Browse Affiliates") %></a></li>
                    <li><a href="AffiliatePayments.aspx"><%= Lang.TransA("Payment Requests") %></a></li>
                    <li><a href="PaymentHistory.aspx"><%= Lang.TransA("Payment History") %></a></li>
                    <li><a href="CommissionsHistory.aspx"><%= Lang.TransA("Affiliate Commissions") %></a></li>
                    <li><a href="AffiliateBanners.aspx"><%= Lang.TransA("Affiliate Banners") %></a></li>
                </ul>
            </div>
        </div>
    </div>
	<div class="panel <%= ((SiteAdmin)Page.Master).Section == "statistics" ? "expanded" : "" %>">
	    <div class="panel-heading">
	        <h4 class="panel-title">
	            <a data-toggle="collapse" class="collapsed" data-parent="#accordion" href="#collapseTen"><i class="fa statistics"></i>&nbsp;<%= Lang.TransA("Statistics")%><i class="fa fa-angle-down"></i><i class="fa fa-angle-left"></i></a>
	        </h4>
	    </div>
        <div id="collapseTen" class="panel-collapse collapse <%= ((SiteAdmin)Page.Master).Section == "statistics" ? "in" : "" %>">
            <div class="panel-body">
                <ul class="nav">
                    <li><a href="NewUsersStats.aspx"><%= Lang.TransA("New Users Stats")%></a></li>
                    <li><a href="OnlineUsersStats.aspx"><%= Lang.TransA("Online Users Stats")%></a></li>
                </ul>
            </div>
        </div>
    </div>
</div>