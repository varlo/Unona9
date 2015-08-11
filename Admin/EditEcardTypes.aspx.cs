using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditEcardTypes : AdminPageBase
    {
        protected bool ShowExistedEcardType
        {
            get
            {
                if (ViewState["ShowExistedEcardType"] == null) return false;
                return (bool)ViewState["ShowExistedEcardType"];
            }
            set { ViewState["ShowExistedEcardType"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editEcards;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Site Management".TranslateA();
            Subtitle = "Edit e-card types".TranslateA();
            Description = "Use this section to edit e-card types".TranslateA();

            if (!IsPostBack)
            {
                loadStrings();
                populateDDName();
            }
        }

        private void loadStrings()
        {
            if (!HasWriteAccess)
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }

            btnSave.Text = Lang.TransA("Save");
            btnDelete.Text = Lang.TransA("Delete");
        }

        private void populateDDName()
        {
            ddName.Items.Clear();

            ddName.Items.Add(new ListItem("", "-1"));

            foreach (EcardType et in EcardType.Fetch())
            {
                ddName.Items.Add(new ListItem(et.Name, et.ID.ToString()));
            }

            ddName.Items.Add(new ListItem(Lang.TransA("- Add new -"), "-2"));
        }

        protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddName.SelectedValue == "-1")
            {
                ShowExistedEcardType = false;
                pnlEditEcardType.Visible = false;
                return;
            }

            if (ddName.SelectedValue == "-2")
            {
                ShowExistedEcardType = false;
                txtName.Text = String.Empty;
                cbActive.Checked = false;
                txtCreditsRequired.Text = String.Empty;
                tdImage.Visible = false;
                tdFlash.Visible = false;
                pnlEditEcardType.Visible = true;
                return;
            }

            ShowExistedEcardType = true;
            pnlEditEcardType.Visible = true;

            EcardType ecardType = EcardType.Fetch(Convert.ToInt32(ddName.SelectedValue));

            if (ecardType != null)
            {
                txtName.Text = ecardType.Name;
                cbActive.Checked = ecardType.Active;
                tdImage.Visible = ecardType.Type == EcardType.eType.Image;
                tdFlash.Visible = !tdImage.Visible;
                txtCreditsRequired.Text = ecardType.CreditsRequired == null ? String.Empty :
                    ecardType.CreditsRequired.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (txtName.Text.Length == 0)
            {
                Master.MessageBox.Show(Lang.TransA("Please enter name"), Misc.MessageType.Error);
                return;
            }

            byte[] content = new byte[fuContent.PostedFile.InputStream.Length];

            while (true)
            {
                int bytesRead = fuContent.PostedFile.InputStream.Read(content, 0, content.Length);
                if (bytesRead == 0) break;
            }

            string mimeType = String.Empty;
            string extension = System.IO.Path.GetExtension(fuContent.PostedFile.FileName).ToLower();

            if (fuContent.PostedFile.FileName.Length > 0)
            {
                switch (extension)
                {
                    case ".gif":
                        mimeType = "image/gif";
                        break;
                    case ".jpg":
                        mimeType = "image/jpeg";
                        break;
                    case ".jpeg":
                        mimeType = "image/jpeg";
                        break;
                    case ".jpe":
                        mimeType = "image/jpeg";
                        break;
                    case ".png":
                        mimeType = "image/png";
                        break;
                    case ".swf":
                        mimeType = "application/x-shockwave-flash";
                        break;
                    default:
                        Master.MessageBox.Show(Lang.TransA("Invalid file format!"), Misc.MessageType.Error);
                        return;
                }
            }

            EcardType ecardType = null;

            ecardType = EcardType.Fetch(Convert.ToInt32(ddName.SelectedValue));

            if (ecardType == null)
            {
                ecardType = new EcardType();
            }

            ecardType.Name = txtName.Text.Trim();
            ecardType.Active = cbActive.Checked;
            int credits;
            if (Int32.TryParse(txtCreditsRequired.Text.Trim(), out credits))
                ecardType.CreditsRequired = credits;
            else
                ecardType.CreditsRequired = null;
            if (content.Length > 0) ecardType.Content = content;
            else if (!ShowExistedEcardType)
            {
                Master.MessageBox.Show(Lang.TransA("Invalid file format!"), Misc.MessageType.Error);
                return;
            }
            if (mimeType.Length > 0)
                ecardType.Type = mimeType.Split('/')[0] == "image" ? EcardType.eType.Image : EcardType.eType.Flash;
            ecardType.Save();

            tdImage.Visible = ecardType.Type == EcardType.eType.Image;
            tdFlash.Visible = !tdImage.Visible;

            populateDDName();
            ddName.SelectedValue = ecardType.ID.ToString();
            Master.MessageBox.Show(Lang.TransA("The e-card has been modified successfully!"), Misc.MessageType.Success);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            int id = Convert.ToInt32(ddName.SelectedValue);
            if (id == -1 || id == -2)
            {
                txtName.Text = String.Empty;
                cbActive.Checked = false;
                ddName.SelectedIndex = 0;
                pnlEditEcardType.Visible = false;
                return;
            }

            EcardType.Delete(id);
            txtName.Text = String.Empty;
            cbActive.Checked = false;
            txtCreditsRequired.Text = String.Empty;
            tdImage.Visible = false;
            tdFlash.Visible = false;
            populateDDName();
        }
    }
}
