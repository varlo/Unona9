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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for EditContentPages.
    /// </summary>
    public partial class EditContentPages : AdminPageBase
    {
        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "98%", "500px");

            Title = "Site Management".TranslateA();
            Subtitle = "Edit Content Pages".TranslateA();
            Description = "Use this section to edit content pages".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                }

                LoadStrings();
                PopulateDDLanguage();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            trPageContent.Visible = !cbURL.Checked;
            trURL.Visible = cbURL.Checked;
            trMetaKeyword.Visible = !cbURL.Checked;
            trMetaDescription.Visible = !cbURL.Checked;
            trUrlRewriteCheckbox.Visible = !cbURL.Checked;
            trUrlRewriteTextbox.Visible = !cbURL.Checked && cbRewriteUrl.Checked;
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editContentPages;
            base.OnInit(e);
        }

        public void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
            btnDelete.Text = Lang.TransA("Delete");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            string title = txtPageTitle.Text;
            string linkText = txtLinkText.Text.Trim();
            string content = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;
            int? headerPosition = ddHeaderPosition.SelectedValue == "-1" ? null : (int?)Convert.ToInt32(ddHeaderPosition.SelectedValue);
            int? footerPosition = ddFooterPosition.SelectedValue == "-1" ? null : (int?)Convert.ToInt32(ddFooterPosition.SelectedValue);
            int languageID = Convert.ToInt32(ddLanguage.SelectedItem.Value);
            Classes.ContentPage.eVisibility visibleFor =
                (Classes.ContentPage.eVisibility)Convert.ToInt32(ddVisibleFor.SelectedValue);
            string url = cbURL.Checked ? txtURL.Text.Trim() : null;
            string rewriteUrl = cbRewriteUrl.Checked ? txtUrlRewrite.Text.Trim() : null;
            string metaDescription = txtMetaDescription.Text.Trim();
            string metaKeyword = txtMetaKeyword.Text.Trim();

            if (title == null || title.Trim().Length == 0)
            {
                Master.MessageBox.Show(Lang.TransA("Please enter Page Title"), Misc.MessageType.Error);
                return;
            }

            if (cbURL.Checked && url == String.Empty)
            {
                Master.MessageBox.Show(Lang.TransA("Please enter URL"), Misc.MessageType.Error);
                return;
            }

            if (cbRewriteUrl.Checked && rewriteUrl == String.Empty)
            {
                Master.MessageBox.Show(Lang.TransA("Please enter URL"), Misc.MessageType.Error);
                return;
            }

            Classes.ContentPage cp =
                Classes.ContentPage.FetchContentPage(Convert.ToInt32(ddPageName.SelectedItem.Value));

            if (cp == null)
            {
                cp = Classes.ContentPage.Create(title, linkText, content, headerPosition, footerPosition,
                    visibleFor, url, metaDescription, metaKeyword, languageID, rewriteUrl);
            }
            else
            {
                cp.Title = title;
                cp.LinkText = linkText;
                cp.Content = content;
                cp.HeaderPosition = headerPosition;
                cp.FooterPosition = footerPosition;
                cp.VisibleFor = visibleFor;
                cp.URL = url;
                cp.MetaDescription = metaDescription;
                cp.MetaKeyword = metaKeyword;
                cp.LanguageID = languageID;
                cp.UrlRewrite = rewriteUrl;
            }

            cp.Save();
            PopulateDDPageName(languageID);
            ddPageName.SelectedValue = cp.ID.ToString();

            Master.MessageBox.Show(Lang.TransA("The page has been modified successfully!"), Misc.MessageType.Success);
        }

        private void PopulateDDLanguage()
        {
            ddLanguage.Items.Add(new ListItem("", "-1"));

            foreach (Language language in Language.FetchAll())
            {
                if (!language.Active) continue;
                ddLanguage.Items.Add(new ListItem(language.Name, language.Id.ToString()));
            }

            if (ddLanguage.Items.Count <= 2)
            {
                if (ddLanguage.Items.Count == 2)
                    ddLanguage.SelectedIndex = 1;
                trLanguage.Visible = false;
                ddLanguage_SelectedIndexChanged(this, null);
            }
            else
            {
                trLanguage.Visible = true;
                trPageName.Visible = false;
            }
        }

        private void PopulateDDPageName(int languageID)
        {
            ddPageName.Items.Clear();

            ddPageName.Items.Add(new ListItem("", "-1"));

            foreach (Classes.ContentPage cp in Classes.ContentPage.FetchContentPages(languageID, Classes.ContentPage.eSortColumn.None))
            {
                ddPageName.Items.Add(new ListItem(cp.Title, cp.ID.ToString()));
            }

            ddPageName.Items.Add(new ListItem(Lang.TransA("- Add new -"), "-2"));
        }

        private void PopulateDDVisibleFor()
        {
            ddVisibleFor.Items.Clear();
            ddVisibleFor.Items.Add(new ListItem("Logged-On Users".TranslateA(), ((int)Classes.ContentPage.eVisibility.LoggedOnUsers).ToString()));
            ddVisibleFor.Items.Add(new ListItem("Not Logged-On Users".TranslateA(), ((int)Classes.ContentPage.eVisibility.NotLoggedOnUsers).ToString()));
            ddVisibleFor.Items.Add(new ListItem("Paid".TranslateA(), ((int)Classes.ContentPage.eVisibility.Paid).ToString()));
            ddVisibleFor.Items.Add(new ListItem("Unpaid".TranslateA(), ((int)Classes.ContentPage.eVisibility.Unpaid).ToString()));
            ddVisibleFor.Items.Add(new ListItem("All".TranslateA(), ((int)Classes.ContentPage.eVisibility.All).ToString()));
        }

        private void PopulateContentPageInfo()
        {
            Classes.ContentPage[] contentPages =
                Classes.ContentPage.FetchContentPages(Convert.ToInt32(ddLanguage.SelectedValue),
                                                      Classes.ContentPage.eSortColumn.None);

            ddHeaderPosition.Items.Clear();
            ddFooterPosition.Items.Clear();
            ddHeaderPosition.Items.Add(new ListItem("Don't show".TranslateA(), "-1"));
            ddFooterPosition.Items.Add(new ListItem("Don't show".TranslateA(), "-1"));

            PopulateDDVisibleFor();

            for (int i = 0; i < contentPages.Length + 1; i++)
            {
                ddHeaderPosition.Items.Add(new ListItem((i + 1).ToString()));
                ddFooterPosition.Items.Add(new ListItem((i + 1).ToString()));
            }
        }

        protected void ddPageName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddPageName.SelectedItem.Value == "-1")
            {
                divEditPage.Visible = false;
                return;
            }
            else if (ddPageName.SelectedItem.Value == "-2")
            {
                PopulateContentPageInfo();

                divEditPage.Visible = true;
                txtPageTitle.Text = String.Empty;
                txtLinkText.Text = String.Empty;
                if (ckeditor != null)
                    ckeditor.Text = String.Empty;
                if (htmlEditor != null)
                    htmlEditor.Content = String.Empty;
                ddHeaderPosition.SelectedIndex = 0;
                ddFooterPosition.SelectedIndex = 0;
                ddPageName.SelectedIndex = ddPageName.Items.Count - 1;
                ddVisibleFor.SelectedValue = ((int)Classes.ContentPage.eVisibility.All).ToString();
                txtURL.Text = String.Empty;
                txtMetaDescription.Text = String.Empty;
                txtMetaKeyword.Text = String.Empty;
                cbURL.Checked = false;
                return;
            }
            else
            {
                divEditPage.Visible = true;
                int id = Convert.ToInt32(ddPageName.SelectedItem.Value);

                Classes.ContentPage cp = Classes.ContentPage.FetchContentPage(id);

                if (cp != null)
                {
                    txtPageTitle.Text = cp.Title;
                    txtLinkText.Text = cp.LinkText;
                    if (cp.URL != null)
                    {
                        txtURL.Text = cp.URL;
                        cbURL.Checked = true;
                    }
                    else
                    {
                        txtURL.Text = String.Empty;
                        cbURL.Checked = false;
                    }
                    if (cp.UrlRewrite != null)
                    {
                        txtUrlRewrite.Text = cp.UrlRewrite;
                        cbRewriteUrl.Checked = true;
                    }
                    else
                    {
                        txtUrlRewrite.Text = String.Empty;
                        cbRewriteUrl.Checked = false;
                    }
                    if (ckeditor != null)
                        ckeditor.Text = cp.Content;
                    if (htmlEditor != null)
                        htmlEditor.Content = cp.Content;

                    PopulateContentPageInfo();

                    if (ddHeaderPosition.Items.FindByValue(cp.HeaderPosition.ToString()) == null)
                    {
                        if (cp.HeaderPosition != null)
                        {
                            ddHeaderPosition.Items.Add(cp.HeaderPosition.ToString());
                        }
                    }

                    if (ddFooterPosition.Items.FindByValue(cp.FooterPosition.ToString()) == null)
                    {
                        if (cp.FooterPosition != null)
                        {
                            ddFooterPosition.Items.Add(cp.FooterPosition.ToString());
                        }
                    }

                    ddHeaderPosition.SelectedValue = cp.HeaderPosition == null ? "-1" : cp.HeaderPosition.ToString();
                    ddFooterPosition.SelectedValue = cp.FooterPosition == null ? "-1" : cp.FooterPosition.ToString();
                    ddVisibleFor.SelectedValue = ((int)cp.VisibleFor).ToString();
                    txtMetaDescription.Text = cp.MetaDescription;
                    txtMetaKeyword.Text = cp.MetaKeyword;
                }
            }
        }

        protected void ddLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            divEditPage.Visible = false;

            if (ddLanguage.SelectedItem.Value == "-1")
            {
                trPageName.Visible = false;
            }
            else
            {
                int languageID = Convert.ToInt32(ddLanguage.SelectedItem.Value);
                trPageName.Visible = true;
                PopulateDDPageName(languageID);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            int languageID = Convert.ToInt32(ddLanguage.SelectedItem.Value);
            int id = Convert.ToInt32(ddPageName.SelectedItem.Value);
            if (id == -1 || id == -2)
            {
                txtPageTitle.Text = String.Empty;
                txtLinkText.Text = String.Empty;
                if (ckeditor != null)
                    ckeditor.Text = String.Empty;
                if (htmlEditor != null)
                    htmlEditor.Content = String.Empty;
                divEditPage.Visible = false;
                ddPageName.SelectedIndex = 0;
                return;
            }
            Classes.ContentPage.Delete(id);
            txtPageTitle.Text = String.Empty;
            if (ckeditor != null)
                ckeditor.Text = String.Empty;
            if (htmlEditor != null)
                htmlEditor.Content = String.Empty;
            txtMetaDescription.Text = String.Empty;
            txtMetaKeyword.Text = String.Empty;
            txtURL.Text = String.Empty;
            cbURL.Checked = false;
            PopulateDDPageName(languageID);
        }
    }
}