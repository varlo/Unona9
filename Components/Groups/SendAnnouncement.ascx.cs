using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Groups
{
    public partial class SendAnnouncement : System.Web.UI.UserControl
    {
        public const string EMAIL_TEMPLATE = @"<div id='letter' style='width: 850px'>
    <div id='header' style='width: 100%; height: 74px; background: url(http://unona.net/images/email_logo.jpg) no-repeat bottom left'>
        <ul style='height: 74px; text-align: right; padding-top: 60px; padding-right: 5px; margin-bottom: 0; font-size: 8pt'>
            <li style='display: inline'><a href='http://unona.net/Default.aspx'>Home</a></li>
            <li style='display: inline'><a href='http://www.unona.net/Login.aspx'>Log in</a></li>
            <li style='display: inline'><a href='http://www.unona.net/TopPhotos.aspx'>Photos</a></li>
            <li style='display: inline'><a href='http://www.unona.net/Search.aspx'>Search</a></li>
        </ul>
    </div>
    <div id='subheader' style='clear: both; width: 100%; height: 35px; background: url(http://unona.net/App_Themes/Unona/horbar.jpg) no-repeat bottom left;'>
        &nbsp;</div>
    <div id='body' style='padding: 10px'>
        <p style='font-size: 8pt'>{0}</p>
        <p style='text-align: center'>
            <a href='http://www.facebook.com/UnonaDating' target='_blank' title='Share on facebook'
                style='margin-left: 20px'>
                <img src='http://www.Unona.net/Images/facebook.png' alt='Share on facebook' border='0' /></a>
            <a href='http://www.twitter.com/UnonaDating' target='_blank' title='Share on twitter'
                style='margin-left: 20px'>
                <img src='http://www.Unona.net/Images/twitter.png' alt='Share on twitter' border='0' /></a>
            <a href='https://www.youtube.com/c/UnonaNet' target='_blank'
                title='Share on youtube' style='margin-left: 20px'>
                <img src='http://www.Unona.net/Images/youtube.png' alt='Share on youtube' border='0' /></a></p>
        <p style='text-align: center; font-size: 8pt'>
            To make sure you get our newsletters with special offers and news, please make sure
            to add our address to Contacts List.
            <br />
            If you prefer not to receive our mail, please click <a href='http://unona.net/Unsubscribe.aspx?username=%%USER%%'>
                Unsubscribe me</a><br /><br />
            &copy; 2015 Unona Internationsl Enterprises Inc. <a href='http://www.Unona.net'>www.Unona.net</a></p>
    </div>
    <div id='subheader' style='clear: both; width: 100%; height: 35px; background: url(http://unona.net/App_Themes/Unona/horbar.jpg) no-repeat bottom left;'>
        <b><b>&nbsp;</b></b></div>
</div>
";

        public int GroupID
        {
            get
            {
                return (int)ViewState["CurrentGroupId"];
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        protected Group CurrentGroup
        {
            get
            {
                if (Page is ShowGroup)
                {
                    return ((ShowGroup)Page).Group;
                }
                else
                {
                    return Group.Fetch(GroupID);
                }
            }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlSendAnnoucement.Visible = true;
            btnSendAnnouncement.Visible = true;

            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            txtAnnouncement.Text = "";
            txtAgeFrom.Text = Config.Users.MinAge.ToString();
            txtAgeTo.Text = Config.Users.MaxAge.ToString();
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Send Announcement");
            btnSendAnnouncement.Text = Lang.Trans("Send");

            ddGender.Items.Add(new ListItem(Lang.Trans("All"), "-1"));

            if (!Config.Users.DisableGenderInformation)
            {
                ddGender.Items.Add(new ListItem(Lang.Trans("All males"), ((int)User.eGender.Male).ToString()));
                ddGender.Items.Add(new ListItem(Lang.Trans("All females"), ((int)User.eGender.Female).ToString()));

                if (Config.Users.CouplesSupport)
                {
                    ddGender.Items.Add(
                        new ListItem(Lang.Trans("All couples"), ((int)User.eGender.Couple).ToString()));
                }
            }

            pnlGender.Visible = !Config.Users.DisableGenderInformation;
            pnlAge.Visible = !Config.Users.DisableAgeInformation;
        }

        protected void btnSendAnnouncement_Click(object sender, EventArgs e)
        {
            #region Validate input data

            if (txtAnnouncement.Text.Trim() == String.Empty)
            {
                lblError.CssClass = "alert text-danger";
                lblError.Text = Lang.Trans("Please enter announcement!");
                return;
            }

            if (Config.Users.DisableAgeInformation)
            {
                txtAgeFrom.Text = Config.Users.MinAge.ToString();
                txtAgeTo.Text = Config.Users.MaxAge.ToString();
            }

            int ageFrom, ageTo;
            if (!Int32.TryParse(txtAgeFrom.Text, out ageFrom) || !Int32.TryParse(txtAgeTo.Text, out ageTo))
            {
                lblError.CssClass = "error";
                lblError.Text = Lang.Trans("Please specify valid age!");
                return;
            }

            if (ageFrom < Config.Users.MinAge || ageTo > Config.Users.MaxAge)
            {
                lblError.CssClass = "alert text-danger";
                lblError.Text = Lang.Trans("Please specify valid age!");
                return;
            }

            if (ageFrom > ageTo)
            {
                lblError.CssClass = "alert text-danger";
                lblError.Text = Lang.Trans("Please specify valid age!");
                return;
            }

            #endregion

            GroupMember[] groupMembers = GroupMember.Fetch(GroupID, true);
            var lRecipients = new List<string>();

            foreach (GroupMember groupMember in groupMembers)
            {
                User user = null;

                try
                {
                    user = User.Load(groupMember.Username);
                }
                catch (NotFoundException)
                {
                    continue;
                }

                if (!user.Deleted && user.Age >= Convert.ToInt32(txtAgeFrom.Text) && user.Age <= Convert.ToInt32(txtAgeTo.Text))
                {
                    if (ddGender.SelectedValue == "-1")
                    {
                        lRecipients.Add(groupMember.Username);
                    }
                    else if (ddGender.SelectedValue == ((int)User.eGender.Male).ToString())
                    {
                        if (user.Gender == User.eGender.Male)
                        {
                            lRecipients.Add(groupMember.Username);
                        }
                    }
                    else if (ddGender.SelectedValue == ((int)User.eGender.Female).ToString())
                    {
                        if (user.Gender == User.eGender.Female)
                        {
                            lRecipients.Add(groupMember.Username);
                        }
                    }
                    else if (ddGender.SelectedValue == ((int)User.eGender.Couple).ToString())
                    {
                        if (user.Gender == User.eGender.Couple)
                        {
                            lRecipients.Add(groupMember.Username);
                        }
                    }
                }
            }

            foreach (string recipient in lRecipients)
            {
                Message msg = new Message(CurrentUserSession.Username, recipient);
                msg.Body = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtAnnouncement.Text.Trim()) : txtAnnouncement.Text.Trim();
                msg.Send();
            }

            pnlSendAnnoucement.Visible = false;
            btnSendAnnouncement.Visible = false;
            lblError.CssClass = "alert text-info";
            lblError.Text = Lang.Trans("Your announcement has been sent!");
        }
    }
}