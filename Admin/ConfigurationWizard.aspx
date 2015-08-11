<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigurationWizard.aspx.cs" Inherits="AspNetDating.Admin.ConfigurationWizard" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE html>

<html>
<head>
    <title></title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- Bootstrap -->
    <link href="../Images/bootstrap.css" rel="stylesheet">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <link href="../Images/font-awesome.css" rel="stylesheet">
    <link href="../Images/ekko-lightbox.css" rel="Stylesheet" type="text/css" />
    <link href="../Images/common.css" rel="Stylesheet" type="text/css" />
    <link href="../Images/common.less" rel="stylesheet/less" />
    <script src="../Images/less.js" type="text/javascript"></script>
    <link href="images/style.css" rel="Stylesheet" type="text/css" />
</head>
<body onLoad="showHideAll()">
    <form id="form1" runat="server">
        <script src="../scripts/jquery.watermarkinput.js" type="text/javascript"></script>
        <script type="text/javascript">
            $(function () {
                $('#<%= txtOtherLanguage.ClientID %>').Watermark('<%= "Other language".TranslateA() %>');
            });
        </script>
        <uc1:AdminHeader ID="AdminHeader1" runat="server"/>
        <script language="javascript" type="text/javascript">
            function showHideDiv(checkBoxID, divID) {
                if (document.getElementById(checkBoxID) != null) {
                    if (document.getElementById(checkBoxID).checked)
                        document.getElementById(divID).style.display = '';
                    else
                        document.getElementById(divID).style.display = 'none';
                }
            }
            function showHideAll() {
                showHideDiv('<%= cbEnableClassifiedAds.ClientID %>', 'divClassifiedAdsOptions');
                showHideDiv('<%= cbEnableBlogs.ClientID %>', 'divBlogsOptions');
                showHideDiv('<%= cbEnableCommunityGroups.ClientID %>', 'divCommunityGroupOptions');
                showHideDiv('<%= cbEnableVideoFileUploads.ClientID %>', 'divVideoFileUploadsOptions');
                showHideDiv('<%= cbEnableYouTubeVideosEmbedding.ClientID %>', 'divYouTubeVideosEmbeddingOptions');
                showHideDiv('<%= cbEnableMP3FileUploads.ClientID %>', 'divMP3FileUploadsOptions');
                showHideDiv('<%= cbAllowExplicitPhotos.ClientID %>', 'divExplicitPhotosOptions');
                showHideDiv('<%= cbAllowUsersToRatePhotos.ClientID %>', 'divRatePhotosOptions');
                showHideDiv('<%= cbAllowUsersToUseSkins.ClientID %>', 'divSkinOptions');
                showHideDiv('<%= cbBlockSendingMultipleSimilarMessages.ClientID %>', 'divBlockSendingMultipleSimilarMessagesOptions');
                showHideDiv('<%= cbManuallyApproveInitialUserMessages.ClientID %>', 'divInitialMessagesApprovalOptions');
            }                                        
        </script>
        <uc1:messagebox id="MessageBox" runat="server"/>
        <div class="container center-block wizard">
            <h2><i class="fa fa-magic"></i>&nbsp;<%= Lang.TransA("Configuration Wizard") %></h2>
            <div class="panel default-panel">
            <asp:Wizard ID="wzWizard" DisplaySideBar="false" ActiveStepIndex="0" runat="server" onactivestepchanged="wzWizard_ActiveStepChanged" onnextbuttonclick="wzWizard_NextButtonClick" onfinishbuttonclick="wzWizard_FinishButtonClick">
                <WizardSteps>
                    <asp:WizardStep ID="wsSiteDetails" runat="server" Title="Site Details">
                        <div class="panel-heading"><h3 class="panel-title"><span class="badge">1</span><%= Lang.TransA("Site Details") %></h3></div>
                        <div class="panel-body">
                            <p class="help-block"><%= "Fill in some basic details for your site to get you started. Don't worry - you can always change them later.".TranslateA() %></p>
                            <ul class="list-group list-group-striped">
                                <li class="list-group-item">
                                    <b><%= "Site name".TranslateA() %></b> - <%= "the name of your site as it will appear in the browser title and e-mail notifications".TranslateA() %>
                                    <asp:TextBox CssClass="form-control" ID="txtSiteName" runat="server"/>
                                </li>
                                <li class="list-group-item">
                                    <b><%= "Site e-mail".TranslateA() %></b> - <%= "the e-mail address which your site will use to send e-mail notifications (e.g. noreply@yoursite.com)".TranslateA()%><div class="separator10"></div>
                                    <asp:TextBox CssClass="form-control" ID="txtSiteEmail" runat="server"/>
                                </li>
                                <li class="list-group-item">
                                    <b><%= "Site logo".TranslateA() %></b> - <%= "select your site logo (roughly 170x50)".TranslateA()%>
                                    <asp:FileUpload ID="fuLogo" runat="server" />
                                </li>
                                <li class="list-group-item">
                                    <b><%= "Site type".TranslateA() %></b> - <%= "select your site type and primary audience".TranslateA()%>
                                    <div class="radio"><label><asp:RadioButtonList ID="rblSiteType" runat="server"/></label></div>
                                </li>
                                <li class="list-group-item">
                                    <b><%= "Site languages".TranslateA() %></b> - <%= "AspNetDating comes with several language packs which you can use right away. If you need to use another language you can enter it in the textbox below. Additional languages can be added from the \"Edit Languages\" page.".TranslateA()%>
                                    <div class="checkbox"><label><asp:CheckBoxList ID="cblSiteLanguages" runat="server"/></label></div>
                                    <div class="checkbox"><label><asp:CheckBox ID="cbOtherLanguage" onclick="javascript:focusOtherLanguageTextBox();" runat="server" />
                                    <asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtOtherLanguage" onfocus="javascript:checkOtherLanguage();" runat="server"/>
                                    <script language="javascript" type="text/javascript">
                                        function focusOtherLanguageTextBox() {
                                            if (document.getElementById('<%= cbOtherLanguage.ClientID %>').checked)
                                                document.getElementById('<%= txtOtherLanguage.ClientID %>').focus();
                                        }
                                        function checkOtherLanguage() {
                                            document.getElementById('<%= cbOtherLanguage.ClientID %>').checked = true;
                                        }
                                    </script>
                                    </label></div>
                                </li>
                            </ul>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="wsSiteSettings" Title="Site Settings" runat="server">
                        <div class="panel-heading"><h3 class="panel-title"><span class="badge">2</span><%= Lang.TransA("Site Settings") %></h3></div>
                        <div class="panel-body">
                            <p class="help-block">
                                <%= "Select the features and add-ons that you would like to use. You can always enable or disable features from the \"Settings\" section. Additional minor settings and fine-tuning are also available from the \"Setting\" section".TranslateA() %>
                            </p>
                            <ul class="list-group list-group-striped">
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableAjaxChatRoom" runat="server" /></label></div>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableMessenger" runat="server" /></label></div>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableCoolIris" runat="server" /></label></div>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnablePhotoAlbums" runat="server" /></label></div>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableClassifiedAds" onclick="showHideDiv(this.id, 'divClassifiedAdsOptions')" runat="server" /></label></div>
                                    <div id="divClassifiedAdsOptions" class="col-sm-offset-1" style="display:none">
                                        <div class="checkbox"><label><asp:CheckBox ID="cbOnlyRegisteredUsersCanBrowseClassifiedAds" runat="server" /></label></div>
                                        <div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToLeaveCommentsOnTheClassifiedAds" runat="server" /></label></div>
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableBlogs" onclick="showHideDiv(this.id, 'divBlogsOptions')" runat="server" /></label></div>
                                    <div id="divBlogsOptions" class="col-sm-offset-1" style="display:none">
                                        <div class="checkbox"><label><asp:CheckBox ID="cbEnableBlogPostApproval" runat="server" /></label></div>
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableCommunityGroups" onclick="showHideDiv(this.id, 'divCommunityGroupOptions')" runat="server" /></label></div>
                                    <div id="divCommunityGroupOptions" class="col-sm-offset-1" style="display:none">
                                        <%= "Maximum number of groups a user can join ".TranslateA() %><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtMaximumGroupsToJoin" runat="server" />
                                        <div class="checkbox"><label><asp:CheckBox ID="cbEnableAjaxChatRoomsInGroups" runat="server" /></label></div>
                                        <div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToCreateGroups" runat="server" /></label></div>
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableVideoFileUploads" onclick="showHideDiv(this.id, 'divVideoFileUploadsOptions')" runat="server" /></label></div>
                                    <div id="divVideoFileUploadsOptions" class="col-sm-offset-1" style="display:none">
                                        <%= "Maximum number of uploaded videos per user ".TranslateA()%><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtMaximumUploadedVideosPerUser" runat="server" />
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableYouTubeVideosEmbedding" onclick="showHideDiv(this.id, 'divYouTubeVideosEmbeddingOptions')" runat="server" /></label></div>
                                    <div id="divYouTubeVideosEmbeddingOptions" class="col-sm-offset-1" style="display:none">
                                        <%= "Maximum number of YouTube videos per user ".TranslateA()%><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtMaximumYouTubeVideosPerUser" runat="server" />
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableMP3FileUploads" onclick="showHideDiv(this.id, 'divMP3FileUploadsOptions')" runat="server" /></label></div>
                                    <div id="divMP3FileUploadsOptions" class="col-sm-offset-1" style="display:none">
                                        <%= "Maximum number of MP3 files per user ".TranslateA()%><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtMaximumMP3FilesPerUser" runat="server" />
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableLiveWebcamVideoStreaming" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableGadgets" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableAdBlockerBlocker" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableSkypeIntegration" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableECards" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableFriends" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableFavorites" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableDistanceSearch" runat="server" /></label></div></li>
                            </ul>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="wsUserSettings" Title="User Settings" runat="server">
                        <div class="panel-heading"><h3 class="panel-title"><span class="badge">3</span><%= Lang.TransA("User Settings") %></h3></div>
                        <div class="panel-body">
                            <p class="help-block"><%= "Configure the user settings related to photos, videos, mp3 files, ratings, etc".TranslateA()%></p>
                            <ul class="list-group list-group-striped">
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbPhotoApprovalRequiredByAdministrator" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowExplicitPhotos" onclick="showHideDiv(this.id, 'divExplicitPhotosOptions')" runat="server" /></label></div>
                                    <div id="divExplicitPhotosOptions" class="col-sm-offset-1" style="display:none">
                                        <div class="checkbox"><label><asp:CheckBox ID="cbAlwaysMakeExplicitPhotosPrivate" runat="server" /></label></div>
                                        <%= "Minimum age to see explicit photos ".TranslateA()%><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtMinimumAgeToSeeExplicitPhotos" runat="server" />
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowPrivatePhotos" runat="server" /></label></div></li>
                                <li class="list-group-item"><%= "Maximum number of uploaded photos per user ".TranslateA()%><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtMaximumPhotosPerUser" runat="server" /></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToCommentOnProfiles" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToCommentOnPhotos" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToRateProfiles" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToRatePhotos" onclick="showHideDiv(this.id, 'divRatePhotosOptions')" runat="server" /></label></div>
                                    <div id="divRatePhotosOptions" class="col-sm-offset-1" style="display:none">
                                        <div class="checkbox"><label><asp:CheckBox ID="cbEnableHotOrNotStyle" runat="server" /></label></div>
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToSetStatus" runat="server" /></label></div>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToUseSkins" onclick="showHideDiv(this.id, 'divSkinOptions')" runat="server" /></label></div>
                                    <div id="divSkinOptions" class="col-sm-offset-1" style="display:none">
                                        <div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToCustomizeSkin" runat="server" /></label></div>
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToSpecifyRelationships" runat="server" /></label></div></li>
                            </ul>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="wsAntiSpamSettings" Title="Anti-spam Settings" runat="server">
                        <div class="panel-heading"><h3 class="panel-title"><span class="badge">4</span><%= Lang.TransA("Anti-spam Settings")%></h3></div>
                        <div class="panel-body">
                            <p class="help-block"><%= "Configure the anti-spam software features".TranslateA()%></p>
                            <ul class="list-group list-group-striped">
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableCaptchaValidation" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbRequireEmailConfirmation" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbEnableEmailAndPhoneNumberFiltering" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbBlockSendingMultipleSimilarMessages" onclick="showHideDiv(this.id, 'divBlockSendingMultipleSimilarMessagesOptions')" runat="server" /></label></div>
                                    <div id="divBlockSendingMultipleSimilarMessagesOptions" class="col-sm-offset-1" style="display:none">
                                        <%= "Number of similar messages to trigger the block ".TranslateA() %><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtNumberOfSimilarMessages" runat="server" />
                                    </div>
                                </li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbManuallyApproveInitialUserMessages" onclick="showHideDiv(this.id, 'divInitialMessagesApprovalOptions')" runat="server" /></label></div>
                                    <div id="divInitialMessagesApprovalOptions" class="col-sm-offset-1" style="display:none">
                                        <p><%= "Number of messages per user to be approved ".TranslateA()%><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtMessagesPerUserToBeApproved" runat="server" /></p>
                                    </div>
                                </li>
                                <li class="list-group-item"><%= "Maximum number of messages per day ".TranslateA()%><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtMaximumMessagesPerDay" runat="server" /></li>
                                <li class="list-group-item"><%= "Maximum users contacted per day ".TranslateA()%><asp:TextBox CssClass="form-control input-sm form-control-inline" ID="txtMaximumUsersContactedPerDay" runat="server" /></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToReportProfileAbuse" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToReportPhotoAbuse" runat="server" /></label></div></li>
                                <li class="list-group-item"><div class="checkbox"><label><asp:CheckBox ID="cbAllowUsersToReportMessageAbuse" runat="server" /></label></div></li>
                            </div>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="wsFinish" runat="server" StepType="Complete">
                        FINISH
                    </asp:WizardStep>
                </WizardSteps>
                <StartNextButtonStyle CssClass="btn btn-secondary btn-lg pull-right" />
                <StepPreviousButtonStyle CssClass="btn btn-default btn-lg pull-left" />
                <StepNextButtonStyle CssClass="btn btn-secondary btn-lg pull-right" />
                <FinishPreviousButtonStyle CssClass="btn btn-default btn-lg pull-left" />
                <FinishCompleteButtonStyle CssClass="btn btn-secondary btn-lg pull-right" />
            </asp:Wizard>
            </div>
        </div>
    </form>
</body>
</html>
