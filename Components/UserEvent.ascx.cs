using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components
{
    public partial class UserEvent : UserControl
    {
        public string Text { get; set; }

        public DateTime? Time { get; set; }

        public string TimeString
        {
            get
            {
                string dateString = "a long time ago".Translate();
                if (Time > DateTime.Now.AddMinutes(-5))
                {
                    dateString = "just now".Translate();
                }
                else if (Time > DateTime.Now.AddHours(-1))
                {
                    dateString = String.Format("{0} minutes ago".Translate(),
                                               DateTime.Now.Subtract(Time.Value).TotalMinutes.ToString("0"));
                }
                else if (Time.Value.Date == DateTime.Now.Date)
                {
                    dateString = String.Format("{0} hours ago".Translate(),
                                               DateTime.Now.Subtract(Time.Value).TotalHours.ToString("0"));
                }
                else if (Time.Value.Date == DateTime.Now.AddDays(-1).Date)
                {
                    dateString = "yesterday".Translate();
                }
                else if (Time >= DateTime.Now.AddDays(-6).Date)
                {
                    dateString = String.Format("{0} days ago".Translate(),
                                               DateTime.Now.Subtract(Time.Value).TotalDays.ToString("0"));
                }
                else if (Time >= DateTime.Now.AddDays(-27).Date)
                {
                    int weeks = ((int)DateTime.Now.Subtract(Time.Value).TotalDays) / 7;
                    dateString = weeks > 1 ? String.Format("{0} weeks ago".Translate(), weeks.ToString("0")) :
                        "a week ago".Translate();
                }
                else if (Time > DateTime.Now.AddMonths(-2).Date)
                {
                    dateString = "a month ago".Translate();
                }
                return dateString;
            }
        }

        public List<int> UserImageIDs { get; set; }
        public List<int> GroupImageIDs { get; set; }
        public List<int> GroupPhotoIDs { get; set; }
        public List<int> GroupEventImageIDs { get; set; }
        public List<string> VideoThumbnailsUrls { get; set; }
        public string FromUsername { get; set; }
        public Event.eType Type { get; set; }

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        private bool ShowMoreLink
        {
            get
            {
                if (ViewState["ShowMoreLink"] == null) return true;
                return (bool) ViewState["ShowMoreLink"];
            }
            set { ViewState["ShowMoreLink"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lnkLeaveComment.Text = "<i class=\"fa fa-comment-o\"></i>&nbsp;" + "Comment".Translate();
            lnkDelete.Text = "<i class=\"fa fa-trash-o\"></i>&nbsp;" + "Delete".Translate();
            btnAddComment.Text = "<i class=\"fa fa-comment\"></i>&nbsp;" + "Comment".Translate();
            btnCancel.Text = "Cancel".Translate();

            spanLeaveComment.Visible = CurrentUserSession != null && Config.Users.EnableEventComments
                                       && (Type != Event.eType.NewPhotoComment
                                           && Type != Event.eType.NewGroupPhoto
                                           && Type != Event.eType.NewFriendPhoto
                                           && Type != Event.eType.NewGroupTopic
                                           && Type != Event.eType.NewGroupEvent
                                           && Type != Event.eType.NewProfileComment);
            spanDelete.Visible = CurrentUserSession != null && CurrentUserSession.Username == FromUsername;
                                 
            pnlEventComments.Visible = Config.Users.EnableEventComments;

            Page.RegisterJQuery();
            Page.RegisterJQueryLightbox();
            Page.Header.Controls.Add(new LiteralControl(
                    "<link href=\"images/jquery.lightbox.css\" rel=\"stylesheet\" type=\"text/css\" />"));


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            loadImages();
            if (Config.Users.EnableEventComments
                && Type != Event.eType.NewPhotoComment && Type != Event.eType.NewGroupPhoto
                && Type != Event.eType.NewFriendPhoto && Type != Event.eType.NewGroupTopic
                && Type != Event.eType.NewGroupEvent && Type != Event.eType.NewProfileComment)
                    loadComments(Convert.ToInt32(ID));
        }

        protected void lnkLeaveComment_Click(object sender, EventArgs e)
        {
            spanAddComment.Visible = true;
            spanCancel.Visible = true;
            txtComment.Text = String.Empty;
        }

        protected void btnAddComment_Click(object sender, EventArgs e)
        {
            string comment = txtComment.Text.Trim();
            if (comment.Length == 0) return;

            EventComment eventComment = new EventComment(Convert.ToInt32(ID), CurrentUserSession.Username);
            eventComment.Comment = txtComment.Text.Trim();
            eventComment.Save();

            spanAddComment.Visible = false;
            spanCancel.Visible = false;
            ShowMoreLink = true;
        }

        private void loadImages()
        {
            DataTable dtImages = new DataTable();
            dtImages.Columns.Add("SmallImageUrl");
            dtImages.Columns.Add("BigImageUrl");
            dtImages.Columns.Add("IsVideoThumbnail", typeof (bool));

            if (UserImageIDs != null && UserImageIDs.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(UserEvent), "lightbox",
                    "$(function() {$('div.eventimg a').lightBox(); $('td.EventLeftImg a').lightBox(); });", true);

                lnkLeftImage.HRef = ImageHandler.CreateImageUrl(UserImageIDs[0], Config.Photos.PhotoMaxWidth,
                                                                Config.Photos.PhotoMaxHeight, false, true, false);
                imgLeft.Src = ImageHandler.CreateImageUrl(UserImageIDs[0], 60, 60, false, true, true);
                UserImageIDs.RemoveAt(0);

                foreach (var id in UserImageIDs)
                {
                    dtImages.Rows.Add(new object[]
                                          {
                                              ImageHandler.CreateImageUrl(id, 60, 60, false, true, true),
                                              ImageHandler.CreateImageUrl(id, Config.Photos.PhotoMaxWidth,
                                                                          Config.Photos.PhotoMaxHeight, false, true,
                                                                          false),
                                              false
                                          });
                }
            }

            if (GroupImageIDs != null && GroupImageIDs.Count > 0)
            {
                if (String.IsNullOrEmpty(imgLeft.Src))
                {
                    lnkLeftImage.HRef = ResolveUrl("~") + String.Format("GroupIcon.ashx?groupID={0}&width={1}&height={2}&diskCache=1", GroupImageIDs[0],
                                                      Config.Groups.IconMaxWidth, Config.Groups.IconMaxHeight);
                    imgLeft.Src = ResolveUrl("~") + String.Format("GroupIcon.ashx?groupID={0}&width=60&height=60&diskCache=1", GroupImageIDs[0]);
                    GroupImageIDs.RemoveAt(0);
                }

                foreach (var id in GroupImageIDs)
                {
                    dtImages.Rows.Add(new object[]
                                          {
                                              ResolveUrl("~") + String.Format(
                                                  "GroupIcon.ashx?groupID={0}&width=60&height=60&diskCache=1", id),
                                              ResolveUrl("~") + String.Format(
                                                  "GroupIcon.ashx?groupID={0}&width={1}&height={2}&diskCache=1", id,
                                                  Config.Groups.IconMaxWidth, Config.Groups.IconMaxHeight),
                                              false
                                          });
                }
            }

            if (GroupPhotoIDs != null && GroupPhotoIDs.Count > 0)
            {
                if (String.IsNullOrEmpty(imgLeft.Src))
                {
                    lnkLeftImage.HRef = ResolveUrl("~") + String.Format("GroupImage.ashx?gpid={0}&width={1}&height={2}&diskCache=1", GroupPhotoIDs[0],
                                                      Config.Groups.GroupPhotoMaxWidth,
                                                      Config.Groups.GroupPhotoMaxHeight);
                    imgLeft.Src = ResolveUrl("~") + String.Format("GroupImage.ashx?gpid={0}&width=60&height=60&diskCache=1", GroupPhotoIDs[0]);
                    GroupPhotoIDs.RemoveAt(0);
                }

                foreach (var id in GroupPhotoIDs)
                {
                    dtImages.Rows.Add(new object[]
                                          {
                                              ResolveUrl("~") + String.Format("GroupImage.ashx?gpid={0}&width=60&height=60&diskCache=1",
                                                            id),
                                              ResolveUrl("~") + String.Format(
                                                  "GroupImage.ashx?gpid={0}&width={1}&height={2}&diskCache=1", id,
                                                  Config.Groups.GroupPhotoMaxWidth, Config.Groups.GroupPhotoMaxHeight),
                                              false
                                          });
                }
            }

            if (GroupEventImageIDs != null && GroupEventImageIDs.Count > 0)
            {
                if (String.IsNullOrEmpty(imgLeft.Src))
                {
                    lnkLeftImage.HRef = ResolveUrl("~") + String.Format("GroupEventImage.ashx?id={0}&width={1}&height={2}&diskCache=1",
                                                      GroupEventImageIDs[0],
                                                      Config.Groups.GroupEventImageMaxWidth,
                                                      Config.Groups.GroupEventImageMaxHeight);
                    imgLeft.Src = ResolveUrl("~") + String.Format("GroupEventImage.ashx?id={0}&width=60&height=60&diskCache=1", GroupEventImageIDs[0]);
                    GroupEventImageIDs.RemoveAt(0);
                }

                foreach (var id in GroupEventImageIDs)
                {
                    dtImages.Rows.Add(new object[]
                                          {
                                              ResolveUrl("~") + String.Format(
                                                  "GroupEventImage.ashx?id={0}&width=60&height=60&diskCache=1", id),
                                              ResolveUrl("~") + String.Format(
                                                  "GroupEventImage.ashx?id={0}&width={1}&height={2}&diskCache=1", id,
                                                  Config.Groups.GroupEventImageMaxWidth,
                                                  Config.Groups.GroupEventImageMaxHeight),
                                              false
                                          });
                }
            }

            if (VideoThumbnailsUrls != null && VideoThumbnailsUrls.Count > 0)
            {
                if (String.IsNullOrEmpty(imgLeft.Src))
                {
                    lnkLeftImage.HRef = VideoThumbnailsUrls[0];
                    imgLeft.Src = VideoThumbnailsUrls[0];
                    VideoThumbnailsUrls.RemoveAt(0);
                }

                foreach (var url in VideoThumbnailsUrls)
                {
                    dtImages.Rows.Add(new object[]
                                          {
                                              url, url, true
                                          });
                }
            }


            rptEventImages.DataSource = dtImages;
            rptEventImages.DataBind();

            ltrEventText.Text = Text;

            if (Time.HasValue)
            {
                ltrEventTime.Text = TimeString;
                spanEventTime.Visible = true;
            }
            else
            {
                spanEventTime.Visible = false;
            }
        }

        private void loadComments(int id)
        {
            EventComment[] eventComments = EventComment.Fetch(id, EventComment.eSortColumn.Date);

            int count = eventComments.Length > 3 && ShowMoreLink ? 2 : eventComments.Length;

            if (eventComments.Length > 3)
            {
                pnlShowMore.Visible = ShowMoreLink;
                lnkShowMore.Text = String.Format("Show {0} more comments".Translate(), eventComments.Length - 2);
            }

            DataTable dtEventComments = new DataTable();

            dtEventComments.Columns.Add("ID");
            dtEventComments.Columns.Add("Username");
            dtEventComments.Columns.Add("Comment");
            dtEventComments.Columns.Add("Date");

            for (int i = 0; i < count; i++)
            {
                dtEventComments.Rows.Add(new object[]
                                             {
                                                 eventComments[i].ID,
                                                 eventComments[i].Username,
                                                 Server.HtmlEncode(eventComments[i].Comment),
                                                 eventComments[i].Date
                                             });
            }

            rptEventComments.DataSource = dtEventComments;
            rptEventComments.DataBind();
        }

        protected void rptEventComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            EventComment.Delete(Convert.ToInt32(e.CommandArgument));
            ShowMoreLink = true;
        }

        protected void rptEventComments_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
        }

        protected void lnkShowMore_Click(object sender, EventArgs e)
        {
            ShowMoreLink = false;
        }

        protected void rptEventComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
            lnkDelete.Text = "<i class=\"fa fa-trash-o\"></i>&nbsp;" + "Delete".Translate();

            lnkDelete.Visible = CurrentUserSession != null &&
                                (CurrentUserSession.Username == (string)DataBinder.Eval(e.Item.DataItem, "Username") ||
                                 CurrentUserSession.Username == FromUsername);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            spanAddComment.Visible = false;
            spanCancel.Visible = false;
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            Event.Delete(Convert.ToInt32(ID));

            Visible = false;
        }

        protected void rptEventImages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlImage imgUrl = (HtmlImage) e.Item.FindControl("imgUrl");
            if ((bool)DataBinder.Eval(e.Item.DataItem, "IsVideoThumbnail"))
            {
                imgUrl.Width = 50;
            }
        }
    }
}