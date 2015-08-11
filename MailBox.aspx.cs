/* ASPnetDating 
 * Copyright (C) 2003-2014 eStream 
 * http://www.aspnetdating.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at http://www.aspnetdating.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;
using System.Collections.Specialized;

namespace AspNetDating
{
    public partial class MailBox : PageBase
    {
        protected SmallBoxStart SmallBoxStart1;
        protected LargeBoxStart LargeBoxStart1;
        protected LargeBoxStart LargeBoxStart2;
        private bool updateHistory = true;
        private PermissionCheckResult permission;

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        public MessageSearchResults Results
        {
            set
            {
                if (ViewState["MessageSearchResults_guid"] == null)
                {
                    ViewState["MessageSearchResults_guid"] = Guid.NewGuid().ToString();
                }

                Session["MessageSearchResults" + ViewState["MessageSearchResults_guid"]] = value;

                CurrentPage = 1;
            }
            get
            {
                if (ViewState["MessageSearchResults_guid"] != null)
                {
                    return (MessageSearchResults)
                           Session["MessageSearchResults" + ViewState["MessageSearchResults_guid"]];
                }
                Results = new MessageSearchResults();
                return Results;
            }
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage
        {
            set
            {
                Trace.Write("MailBox.aspx.cs", "CurrentPage = " + value);
                ViewState["Messages_CurrentPage"] = value;
                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                if (updateHistory && scriptManager != null && scriptManager.IsInAsyncPostBack)
                {
                    var parameters = new NameValueCollection();
                    parameters.Add("page", value.ToString());
                    parameters.Add("currentFolder", ((int)currentFolder).ToString());
                    scriptManager.AddHistoryPoint(parameters, Page.Title);
                }
                PreparePaginator();
            }
            get
            {
                if (ViewState["Messages_CurrentPage"] == null
                    || (int)ViewState["Messages_CurrentPage"] == 0)
                {
                    return 1;
                }
                return (int)ViewState["Messages_CurrentPage"];
            }
        }

        private string fromUsername;

        private string toUsername;

        /// <summary>
        /// Prepares the paginator.
        /// </summary>
        private void PreparePaginator()
        {
            int messagesPerPage = 10;
            if (Results == null || CurrentPage <= 1)
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else
            {
                lnkFirst.Enabled = true;
                lnkPrev.Enabled = true;
            }
            if (Results == null || CurrentPage >= Results.GetTotalPages(messagesPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Messages.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * messagesPerPage + 1;
                int toUser = CurrentPage * messagesPerPage;
                if (Results.Messages.Length < toUser)
                {
                    toUser = Results.Messages.Length;
                }

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.Messages.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";

            permission = CurrentUserSession.CanReadEmail();


            if (!Page.IsPostBack)
            {
                LoadStrings();
                if (Page.Request.Params["sel"] == "recec")
                    lnkReceivedEcards_Click(null, null);
                else if (Page.Request.Params["sel"] == "sentec")
                    lnkSentEcards_Click(null, null);
                else if (!string.IsNullOrEmpty(Page.Request.Params["uid"]))
                {
                    currentFolder = Message.eFolder.Inbox;
                    txtSearchMail.Text = Page.Request.Params["uid"];
                    pnlReceivedEcards.Visible = false;
                    btnFilter_Click(null, null);
                }
                else
                {
                    lnkInbox_Click(null, null);
                    btnDelete.Attributes.Add("onclick",
                                             String.Format("javascript: return confirm('{0}')",
                                                           Lang.Trans("Do you really want to delete selected messages?")));
                }
            }

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
                scriptManager.Navigate += scriptManager_Navigate;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lnkInbox.Click += new EventHandler(this.lnkInbox_Click);
            this.lnkOutbox.Click += new EventHandler(this.lnkOutbox_Click);
            this.lnkTrash.Click += new EventHandler(this.lnkTrash_Click);
            this.lnkReceivedEcards.Click += new EventHandler(lnkReceivedEcards_Click);
            this.lnkSentEcards.Click += new EventHandler(lnkSentEcards_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnDeleteEcard.Click += new EventHandler(btnDeleteEcard_Click);
            this.dgEcards.PageIndexChanged += new DataGridPageChangedEventHandler(dgEcards_PageIndexChanged);
        }

        #endregion

        private void LoadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Folders");
            lnkInbox.Text = Lang.Trans("Inbox");
            lnkOutbox.Text = Lang.Trans("Outbox");
            lnkTrash.Text = Lang.Trans("Trash");
            btnDelete.Text = "<i class=\"fa fa-trash-o\"></i>&nbsp;" + Lang.Trans("Delete selected messages");
            lnkReceivedEcards.Text = Lang.Trans("Received e-cards");
            lnkSentEcards.Text = Lang.Trans("Sent e-cards");
            btnDeleteEcard.Text = "<i class=\"fa fa-trash-o\"></i>&nbsp;" + Lang.Trans("Delete selected e-cards");
            btnFilter.Text = "<i class=\"fa fa-filter\"></i>&nbsp;" + Lang.Trans("Filter");
            btnClearFilter.Text = "<i class=\"fa fa-times\"></i>&nbsp;" + Lang.Trans("Clear");
            lnkFirst.Text = "<i class=\"fa fa-angle-double-left\"></i>";
            lnkPrev.Text = "<i class=\"fa fa-angle-left\"></i>";
            lnkNext.Text = "<i class=\"fa fa-angle-right\"></i>";
            lnkLast.Text = "<i class=\"fa fa-angle-double-right\"></i>";

            bool ecardsEnabled = CurrentUserSession.CanSendEcards() != PermissionCheckResult.No;
            divReceivedEcards.Visible = ecardsEnabled;
            divSentEcards.Visible = ecardsEnabled;
            dgEcards.Columns[2].Visible = !Config.Users.DisableAgeInformation;//hide Age column
        }

        private Message.eFolder currentFolder
        {
            get
            {
                if (ViewState["mailbox_currentFolder"] != null)
                    return (Message.eFolder)ViewState["mailbox_currentFolder"];
                return Message.eFolder.Inbox;
            }
            set
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                if (updateHistory && scriptManager != null && scriptManager.IsInAsyncPostBack)
                {
                    scriptManager.AddHistoryPoint("currentFolder", ((int)value).ToString());
                }

                ViewState["mailbox_currentFolder"] = value;
            }
        }

        protected Ecard.eFolder currentEcardFolder
        {
            get
            {
                if (ViewState["ecard_currentFolder"] != null)
                    return (Ecard.eFolder)ViewState["ecard_currentFolder"];
                else
                    return Ecard.eFolder.Received;
            }
            set { ViewState["ecard_currentFolder"] = value; }
        }

        private void bindMessages()
        {
            if (currentFolder == Message.eFolder.Outbox)
            {
                if (Results.Messages == null || Results.Messages.Length == 0)
                {
                    btnDelete.Visible = false;
                    lblError.Text = Lang.Trans("There are no messages in the outbox folder!");
                    gridMessages.Visible = false;

                    return;
                }
                else
                {
                    btnDelete.Visible = true;
                    gridMessages.Columns[1].HeaderText = "";
                    gridMessages.Columns[2].HeaderText = Lang.Trans("Photo");
                    gridMessages.Columns[3].HeaderText = Lang.Trans("Recipient");
                    gridMessages.Columns[4].HeaderText = Lang.Trans("Message");
                    gridMessages.Columns[5].HeaderText = Lang.Trans("Date");

                    gridMessages.Visible = true;
                }
            }
            else if (currentFolder == Message.eFolder.Inbox)
            {
                if (Results.Messages == null || Results.Messages.Length == 0)
                {
                    btnDelete.Visible = false;
                    lblError.Text = Lang.Trans("There are no messages in your mailbox!");
                    gridMessages.Visible = false;
                    return;
                }
                else
                {
                    btnDelete.Visible = true;
                    gridMessages.Columns[1].HeaderText = "";
                    gridMessages.Columns[2].HeaderText = Lang.Trans("Photo");
                    gridMessages.Columns[3].HeaderText = Lang.Trans("Sender");
                    gridMessages.Columns[4].HeaderText = Lang.Trans("Message");
                    gridMessages.Columns[5].HeaderText = Lang.Trans("Date");

                    gridMessages.Visible = true;
                }
            }
            else if (currentFolder == Message.eFolder.Trash)
            {
                if (Results.Messages == null || Results.Messages.Length == 0)
                {
                    btnDelete.Visible = false;
                    lblError.Text = Lang.Trans("There are no messages in the trash!");
                    gridMessages.Visible = false;
                    return;
                }
                else
                {
                    btnDelete.Visible = true;
                    gridMessages.Columns[1].HeaderText = "";
                    gridMessages.Columns[2].HeaderText = Lang.Trans("Photo");
                    gridMessages.Columns[3].HeaderText = Lang.Trans("Sender");
                    gridMessages.Columns[4].HeaderText = Lang.Trans("Message");
                    gridMessages.Columns[5].HeaderText = Lang.Trans("Date");

                    gridMessages.Visible = true;
                }
            }

            DataTable dtMessages = new DataTable("Messages");
            dtMessages.Columns.Add("Id");
            dtMessages.Columns.Add("Date", typeof(DateTime));
            dtMessages.Columns.Add("Username");
            dtMessages.Columns.Add("Message");
            dtMessages.Columns.Add("IsRead");
            dtMessages.Columns.Add("IsDeleted");
            dtMessages.Columns.Add("IsRepliedTo");
            dtMessages.Columns.Add("PhotoID", typeof(int));

            if (CurrentPage > Results.GetTotalPages(10))
                CurrentPage = Results.GetTotalPages(10);

            Message[] messages = Results.GetPage(CurrentPage, 10);

            foreach (Message msg in messages)
            {
                string body = String.Empty;

                if (permission == PermissionCheckResult.Yes || msg.fromUsername == Config.Users.SystemUsername ||
                    permission == PermissionCheckResult.YesWithCredits && msg.IsRead)
                    body = Server.HtmlEncode(msg.Body);
                else if (permission == PermissionCheckResult.No)
                    body = "You are not allowed to view messages".Translate();
                else if (permission == PermissionCheckResult.YesButMoreCreditsNeeded && CurrentUserSession.BillingPlanOptions.CanReadEmail.UpgradableToNextPlan)
                    body = "Upgrade your plan or buy more credits to read the message".Translate();
                else if (permission == PermissionCheckResult.YesButMoreCreditsNeeded)
                    body = "Buy more credits to read the message".Translate();
                else if (permission == PermissionCheckResult.YesButPlanUpgradeNeeded)
                    body = "Upgrade to read".Translate();
                else if (permission == PermissionCheckResult.YesWithCredits)
                    body = "Click here to read the message".Translate();

                DateTime timestamp = msg.Timestamp.Add(Config.Misc.TimeOffset);
                string username = currentFolder == Message.eFolder.Outbox
                                      ?
                                  msg.ToUser.Username
                                      : msg.FromUser.Username;

                //thumbnail photoID
                int photoId = 0;

                if (Config.Photos.ShowThumbnailsInMailbox)
                {
                    User user;
                    try
                    {
                        user = Classes.User.Load(username);
                    }
                    catch (NotFoundException) { continue; }

                    Photo photo = null;
                    try
                    {
                        photo = user.GetPrimaryPhoto();
                    }
                    catch (NotFoundException)
                    {
                    }

                    #region Check user.Gender and set photoId

                    if (photo == null || !photo.Approved)
                    {
                        photoId = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    else
                    {
                        photoId = photo.Id;
                    }

                    #endregion
                }

                var permissionCheckResult = CurrentUserSession.CanSeeMessageStatus();

                bool canSeeMessageStatus = permissionCheckResult == PermissionCheckResult.Yes
                                          ||
                                          (CurrentUserSession.Level != null &&
                                           CurrentUserSession.Level.Restrictions.CanSeeMessageStatus);
                bool isRead = false;
                bool isDeleted = false;
                bool isRepliedTo = false;
                if (!canSeeMessageStatus)
                {
                    isRead = currentFolder == Message.eFolder.Inbox ? msg.IsRead : true;
                }
                else
                {
                    isRead = msg.IsRead;
                    if (currentFolder == Message.eFolder.Outbox && msg.ToFolder == Message.eFolder.Trash)
                    {
                        isDeleted = true;
                    }

                    isRepliedTo = Message.HasReplies(msg.Id, msg.toUsername, msg.fromUsername);
                }
                dtMessages.Rows.Add(new object[]
                                        {
                                            msg.Id,
                                            timestamp,
                                            username,
                                            body.Length > 50 ? body.Substring(0, 50) + "..." : body,
                                            isRead,
                                            isDeleted,
                                            isRepliedTo,
                                            photoId
                                        });
            }

            dtMessages.DefaultView.Sort = "Date DESC";

            gridMessages.DataSource = dtMessages;
            gridMessages.DataBind();

            if (!Config.Photos.ShowThumbnailsInMailbox)
            {
                foreach (DataGridColumn column in gridMessages.Columns)
                    if (column.HeaderText == Lang.Trans("Photo"))
                        column.Visible = false;
            }
        }

        private void lnkInbox_Click(object sender, EventArgs e)
        {
            pnlMailBox.Visible = true;
            pnlReceivedEcards.Visible = false;
            if (sender != null)
                gridMessages.CurrentPageIndex = 0;

            currentFolder = Message.eFolder.Inbox;
            LargeBoxStart1.Title = Lang.Trans("Inbox");

            //Results = new MessageSearchResults();

            Results.Messages = Message.Search(0, fromUsername, Message.eFolder.None, CurrentUserSession.Username,
                                     Message.eFolder.Inbox, 0, true, true, false, null);
            CurrentPage = 1;

            bindMessages();
        }

        private void lnkOutbox_Click(object sender, EventArgs e)
        {
            pnlMailBox.Visible = true;
            pnlReceivedEcards.Visible = false;
            if (sender != null)
                gridMessages.CurrentPageIndex = 0;

            currentFolder = Message.eFolder.Outbox;
            LargeBoxStart1.Title = Lang.Trans("Outbox");

            Results.Messages = Message.Search(0, CurrentUserSession.Username, Message.eFolder.Outbox, toUsername,
                                     Message.eFolder.None, 0, true, true, false, null);
            CurrentPage = 1;

            bindMessages();
        }

        private void lnkTrash_Click(object sender, EventArgs e)
        {
            pnlMailBox.Visible = true;
            pnlReceivedEcards.Visible = false;
            if (sender != null)
                gridMessages.CurrentPageIndex = 0;

            currentFolder = Message.eFolder.Trash;
            LargeBoxStart1.Title = Lang.Trans("Trash");

            Results.Messages = Message.Search(0, null, Message.eFolder.None, CurrentUserSession.Username,
                                         Message.eFolder.Trash, 0, true, true, false, null);
            CurrentPage = 1;

            bindMessages();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            HtmlInputCheckBox cbCheck;
            foreach (DataGridItem item in gridMessages.Items)
            {
                cbCheck = (HtmlInputCheckBox)item.FindControl("cbSelect");
                Message message = Message.Fetch(Convert.ToInt32(cbCheck.Value));

                if (cbCheck.Checked)
                {
                    switch (currentFolder)
                    {
                        case Message.eFolder.Inbox:
                            message.ToFolder = Message.eFolder.Trash;
                            break;
                        case Message.eFolder.Outbox:
                            message.FromFolder = Message.eFolder.Deleted;
                            break;
                        case Message.eFolder.Trash:
                            message.ToFolder = Message.eFolder.Deleted;
                            break;
                    }
                }
            }

            switch (currentFolder)
            {
                case Message.eFolder.Inbox:
                    lnkInbox_Click(null, null);
                    break;
                case Message.eFolder.Outbox:
                    lnkOutbox_Click(null, null);
                    break;
                case Message.eFolder.Trash:
                    lnkTrash_Click(null, null);
                    break;
            }
        }

        private void bindEcards(Ecard[] ecards)
        {
            DataTable dtEcards = new DataTable("Ecards");
            dtEcards.Columns.Add("Id");
            dtEcards.Columns.Add("Username");
            dtEcards.Columns.Add("EcardTypeID");
            dtEcards.Columns.Add("EcardTypeName");
            dtEcards.Columns.Add("IsOpened");
            dtEcards.Columns.Add("DatePosted", typeof(DateTime));

            foreach (Ecard ecard in ecards)
            {
                EcardType ecardType = EcardType.Fetch(ecard.EcardTypeID);

                if (ecardType != null)
                {
                    DateTime datePosted = ecard.Date.Add(Config.Misc.TimeOffset);
                    User user = Classes.User.Load(
                        currentEcardFolder == Ecard.eFolder.Received ? ecard.FromUsername : ecard.ToUsername);

                    dtEcards.Rows.Add(new object[]
                                         {
                                             ecard.ID,
                                             currentEcardFolder == Ecard.eFolder.Received
                                                 ? ecard.FromUsername
                                                 : ecard.ToUsername,   
                                             ecardType.ID,
                                             ecardType.Name,
                                             ecard.IsOpened,
                                             datePosted
                                         });    
                }
            }

//            dtEcards.DefaultView.Sort = "DatePosted DESC";

            if (ecards.Length <= (dgEcards.CurrentPageIndex * dgEcards.PageSize))
                dgEcards.CurrentPageIndex--;

            dgEcards.DataSource = dtEcards;
            dgEcards.DataBind();
        }

        private void lnkReceivedEcards_Click(object sender, EventArgs e)
        {
            currentEcardFolder = Ecard.eFolder.Received;
            pnlReceivedEcards.Visible = true;
            pnlMailBox.Visible = false;
            if (sender != null)
                dgEcards.CurrentPageIndex = 0;

            LargeBoxStart2.Title = Lang.Trans("Received e-cards");

            Ecard[] ecards = Ecard.FetchReceived(CurrentUserSession.Username);

            if (ecards.Length == 0)
            {
                btnDeleteEcard.Visible = false;
                lblMessage2.Text = Lang.Trans("There are no e-cards.");
                dgEcards.Visible = false;
            }
            else
            {
                btnDeleteEcard.Visible = true;
                lblMessage2.Text = "";
                dgEcards.Columns[1].HeaderText = Lang.Trans("User");
                dgEcards.Columns[2].HeaderText = Lang.Trans("E-card");
                dgEcards.Columns[3].HeaderText = Lang.Trans("Date");

                bindEcards(ecards);

                dgEcards.Visible = true;
            }
        }

        private void btnDeleteEcard_Click(object sender, EventArgs e)
        {
            HtmlInputCheckBox cbCheck;
            foreach (DataGridItem item in dgEcards.Items)
            {
                cbCheck = (HtmlInputCheckBox)item.FindControl("cbSelect2");

                Ecard ecard = Ecard.Fetch(Convert.ToInt32(cbCheck.Value));

                if (ecard == null)
                {
                    switch (currentEcardFolder)
                    {
                        case Ecard.eFolder.Received:
                            lnkReceivedEcards_Click(null, null);
                            return;
                        case Ecard.eFolder.Sent:
                            lnkSentEcards_Click(null, null);
                            return;
                    }
                }

                if (cbCheck.Checked)
                {
                    switch (currentEcardFolder)
                    {
                        case Ecard.eFolder.Received:
                            ecard.DeleteFromReceivedEcards();
                            break;
                        case Ecard.eFolder.Sent:
                            ecard.DeleteFromSentEcards();
                            break;
                    }
                }
            }

            switch (currentEcardFolder)
            {
                case Ecard.eFolder.Received:
                    lnkReceivedEcards_Click(null, null);
                    break;
                case Ecard.eFolder.Sent:
                    lnkSentEcards_Click(null, null);
                    break;
            }
        }

        private void lnkSentEcards_Click(object sender, EventArgs e)
        {
            currentEcardFolder = Ecard.eFolder.Sent;
            pnlReceivedEcards.Visible = true;
            pnlMailBox.Visible = false;
            if (sender != null)
                dgEcards.CurrentPageIndex = 0;

            LargeBoxStart2.Title = Lang.Trans("Sent e-cards");

            Ecard[] ecards = Ecard.FetchSent(CurrentUserSession.Username);

            if (ecards.Length == 0)
            {
                btnDeleteEcard.Visible = false;
                lblMessage2.Text = Lang.Trans("There are no e-cards.");
                dgEcards.Visible = false;
            }
            else
            {
                btnDeleteEcard.Visible = true;
                lblMessage2.Text = "";
                dgEcards.Columns[1].HeaderText = Lang.Trans("User");
                dgEcards.Columns[2].HeaderText = Lang.Trans("E-card");
                dgEcards.Columns[3].HeaderText = Lang.Trans("Date");


                bindEcards(ecards);

                dgEcards.Visible = true;
            }
        }

        private void dgEcards_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgEcards.CurrentPageIndex = e.NewPageIndex;
            switch (currentEcardFolder)
            {
                case Ecard.eFolder.Received:
                    lnkReceivedEcards_Click(null, null);
                    break;
                case Ecard.eFolder.Sent:
                    lnkSentEcards_Click(null, null);
                    break;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gridMessages.CurrentPageIndex = 0;

            if (txtSearchMail.Text.Trim() == "")
            {
                if (currentFolder == Message.eFolder.Inbox)
                {
                    lnkInbox_Click(null, null);
                    return;
                }
                if (currentFolder == Message.eFolder.Outbox)
                {
                    lnkOutbox_Click(null, null);
                    return;
                }
            }
            else if (currentFolder == Message.eFolder.Inbox)
            {
                fromUsername = txtSearchMail.Text.Trim();
                lnkInbox_Click(null, null);
            }
            else if (currentFolder == Message.eFolder.Outbox)
            {
                toUsername = txtSearchMail.Text.Trim();
                lnkOutbox_Click(null, null);
            }
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            gridMessages.CurrentPageIndex = 0;
            txtSearchMail.Text = "";
            lnkInbox_Click(null, null);
        }

        /// <summary>
        /// Handles the Click event of the lnkFirst control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage = 1;
            }
            bindMessages();
        }

        /// <summary>
        /// Handles the Click event of the lnkPrev control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
            bindMessages();
        }

        /// <summary>
        /// Handles the Click event of the lnkNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (Results == null)
                Response.Redirect("~/Home.aspx");

            if (CurrentPage < Results.GetTotalPages(10))
            {
                CurrentPage++;
            }
            bindMessages();
        }

        /// <summary>
        /// Handles the Click event of the lnkLast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (Results == null)
                Response.Redirect("~/Home.aspx");

            if (CurrentPage < Results.GetTotalPages(10))
            {
                CurrentPage = Results.GetTotalPages(10);
            }
            bindMessages();
        }

        private void scriptManager_Navigate(object sender, HistoryEventArgs e)
        {
            if (!pnlMailBox.Visible) return;

            int historyPage = 0;
            try
            {
                historyPage = Convert.ToInt32(e.State["page"] ?? "1");
            }
            catch (FormatException)
            {
            }
            if (historyPage < 0 || historyPage > Results.GetTotalPages(10)) historyPage = 0;

            updateHistory = false;
            CurrentPage = historyPage;
            if (e.State["currentFolder"] != null)
                currentFolder = (Message.eFolder)Convert.ToInt32(e.State["currentFolder"]);
            bindMessages();
        }


        protected void gridMessages_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.DataItem != null &&
                permission == PermissionCheckResult.YesWithCredits &&
                ((string)DataBinder.Eval(e.Item.DataItem, "Username")) != Config.Users.SystemUsername &&
                !Boolean.Parse((string)DataBinder.Eval(e.Item.DataItem, "IsRead")) &&
                (currentFolder == Message.eFolder.Inbox || currentFolder == Message.eFolder.Trash && ((string)DataBinder.Eval(e.Item.DataItem, "Username")) != CurrentUserSession.Username))
            {
                var anchor = e.Item.FindControl("lnkReadMessage") as HtmlAnchor;
                if (anchor != null)
                {
                    string onclick = String.Format("return confirm('" + "Reading this message will subtract {0} credits from your balance.".Translate() + "');",
                        CurrentUserSession.BillingPlanOptions.CanReadEmail.Credits);

                    anchor.Attributes.Add("onclick", onclick);
                }
            }
        }
    }
}