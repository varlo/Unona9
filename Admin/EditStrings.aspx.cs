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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditStrings : AdminPageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Site Management".TranslateA();
            Subtitle = "Text Management".TranslateA();
            Description = "Use this section to edit the text used throughout the site".TranslateA();

            Page.RegisterJQuery();
            Page.RegisterJQueryEditInPlace();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    //btnSave.Enabled = false;
                }

                loadStrings();
                loadLanguages();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editTexts;
            base.OnInit(e);
        }

        private void loadStrings()
        {
            ddTranslationType.Items.Add(new ListItem(Lang.TransA("User Area"), false.ToString()));
            ddTranslationType.Items.Add(new ListItem(Lang.TransA("Admin Area"), true.ToString()));
        }

        private void loadLanguages()
        {
            foreach (Language language in Language.FetchAll())
            {
                ddLanguage.Items.Add(
                    new ListItem(language.Name, language.Id.ToString()));
            }

            if (ddLanguage.Items.Count <= 2)
            {
                if (ddLanguage.Items.Count == 2)
                    ddLanguage.SelectedIndex = 1;
                pnlLanguage.Visible = false;
                ddLanguage_SelectedIndexChanged(this, null);
            }
            else
            {
                pnlLanguage.Visible = true;
            }
        }

        private void loadTranslations(int languageId, bool adminPanel)
        {
            DataTable dtTranslations = new DataTable();
            dtTranslations.Columns.Add("Key");
            dtTranslations.Columns.Add("Value");
            dtTranslations.DefaultView.Sort = "Value";

            foreach (string key in Translation.FetchTranslationKeys(adminPanel))
            {
                var value = Translation.FetchTranslation(languageId, key, adminPanel);

                dtTranslations.Rows.Add(new object[] { key, value });
            }

            dgTranslations.Visible = true;

            dgTranslations.DataSource = dtTranslations.DefaultView;
            dgTranslations.DataBind();
        }

        protected void ddLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddLanguage.SelectedIndex == 0)
            {
                dgTranslations.Visible = false;
                return;
            }

            int languageId = Convert.ToInt32(ddLanguage.SelectedValue);
            bool adminPanel = Convert.ToBoolean(ddTranslationType.SelectedValue);
            loadTranslations(languageId, adminPanel);
        }

        protected void ddTranslationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddLanguage.SelectedValue.Length == 0)
                return;

            int languageId = Convert.ToInt32(ddLanguage.SelectedValue);
            bool adminPanel = Convert.ToBoolean(ddTranslationType.SelectedValue);
            loadTranslations(languageId, adminPanel);
        }
    }
}