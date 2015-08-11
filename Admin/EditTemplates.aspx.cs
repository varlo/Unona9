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
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for EditTemplates.
    /// </summary>
    public partial class EditTemplates : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Site Management".TranslateA();
            Subtitle = "Edit Templates".TranslateA();
            Description = "Here you can change your email and other templates...".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                }

                LoadStrings();
                LoadLanguages();
            }

            LoadTemplate();
        }

        private void LoadLanguages()
        {
            foreach (Language language in Language.FetchAll())
            {
                if (!language.Active) continue;
                ddLanguage.Items.Add(
                    new ListItem(language.Name, language.Id.ToString()));
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
                trTemplateName.Visible = false;
            }
        }

        private void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
        }

        private void ApplyTableProperties(Table table)
        {
            /*table.BorderColor = Color.Black;
            table.BorderWidth = 1;
            table.BorderStyle = BorderStyle.Solid;*/
            table.CssClass = "table";
        }

        private void ApplyHeaderProperties(TableHeaderCell header)
        {
            header.ColumnSpan = 2;
        }

        private void ApplyDescriptionProperties(TableCell description)
        {
            //description.BorderWidth = 1;
            description.ColumnSpan = 2;
            //description.CssClass = "font_css";
        }

        private void ApplyLableProperties(TableCell labelCell)
        {
            labelCell.Wrap = false;
            //labelCell.Width = new Unit(1, UnitType.Percentage);
            //labelCell.CssClass = "font_css";
        }

        private void LoadAllTemplates()
        {
            phTemplates.Controls.Clear();

            if (ddLanguage.SelectedIndex == 0)
            {
                btnSave.Visible = false;
                return;
            }

            btnSave.Visible = true;
            int languageId = Convert.ToInt32(ddLanguage.SelectedValue);

            phTemplates.Controls.Add(new LiteralControl(
                                         "<div class=\"theader\">Email Templates</div>"));
            LoadTemplateTables(typeof(EmailTemplates), languageId);
            phTemplates.Controls.Add(new LiteralControl(
                                         "<div class=\"theader\">Miscellaneous Templates</div>"));
            LoadTemplateTables(typeof(MiscTemplates), languageId);
        }

        private void SaveAllTemplates()
        {
            if (ddLanguage.SelectedIndex == 0) return;
            int languageId = Convert.ToInt32(ddLanguage.SelectedValue);

            SaveTemplates(typeof(EmailTemplates), languageId);
            SaveTemplates(typeof(MiscTemplates), languageId);
            MessageBox.Show(Lang.TransA("Settings have been successfully updated!"), Misc.MessageType.Success);
        }

        private void LoadTemplateTables(Type type, int languageId)
        {
            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(type, languageId);

            Reflection.PropertyData lastItem = null;
            Table tblTemplate = null;

            foreach (Reflection.PropertyData item in items)
            {
                //header
                if (lastItem == null || lastItem.ClassDesc != item.ClassDesc)
                {
                    if (lastItem != null)
                    {
                        //add previous email template
                        phTemplates.Controls.Add(tblTemplate);
                        phTemplates.Controls.Add(new LiteralControl("<div class=\"tseparator1\" />"));
                    }

                    tblTemplate = new Table();
                    ApplyTableProperties(tblTemplate);

                    TableHeaderRow headerRow = new TableHeaderRow();
                    TableHeaderCell header = new TableHeaderCell();
                    ApplyHeaderProperties(header);
                    header.Text = item.ClassDesc;
                    headerRow.Cells.Add(header);
                    tblTemplate.Rows.Add(headerRow);
                }

                //description
                if (item.PropertyName == "Description")
                {
                    TableRow descRow = new TableRow();
                    TableCell description = new TableCell();
                    ApplyDescriptionProperties(description);
                    description.Text = (string)item.Value;
                    descRow.Cells.Add(description);
                    descRow.CssClass = "tdescription";
                    tblTemplate.Rows.Add(descRow);
                }
                else
                //all other items of the templates
                {
                    TableRow itemRow = new TableRow();
                    TableCell labelCell = new TableCell();
                    ApplyLableProperties(labelCell);
                    labelCell.Text = item.PropertyDesc;
                    itemRow.Cells.Add(labelCell);

                    TableCell valueCell = new TableCell();

                    Control control = CreateControl(item);
                    valueCell.Controls.Add(control);


                    itemRow.Cells.Add(valueCell);
                    tblTemplate.Rows.Add(itemRow);
                }
                lastItem = item;
            }
            //add the last one
            phTemplates.Controls.Add(tblTemplate);
        }

        private Control CreateControl(Reflection.PropertyData item)
        {
            //Creating control from its type
            Control control;
            //if control type is specified create it
            if (item.ControlType != null)
            {
                try
                {
                    control = (Control)Activator.CreateInstance(item.ControlType, null);
                }
                catch (InvalidCastException)
                {
                    throw new InvalidCastException(
                        String.Format(
                            "The ControlAttribute for property named {0} must specify descedant of System.Web.UI.Control class",
                            item.PropertyName));
                }

                //applying the value of the control
                control.GetType().GetProperty(item.ControlProperty).SetValue(control, item.Value, null);
            }
            else //otherwise create TextBox
            {
                control = new TextBox();
                (control as TextBox).Text = item.Value.ToString();
            }

            Hashtable propertiesToApply = item.PropertiesToApply;
            foreach (object key in propertiesToApply.Keys)
            {
                PropertyInfo propertyInfo = control.GetType().GetProperty((string)key);
                object _value = propertiesToApply[key];

                try
                {
                    if (propertyInfo.GetValue(control, null).GetType() == typeof(Unit))
                    {
                        if (_value is String)
                            _value = new Unit((string)_value);
                        else if (_value is Int32)
                            _value = new Unit((int)_value);
                        else if (_value is Double)
                            _value = new Unit((double)_value);
                    }

                    propertyInfo.SetValue(control, _value, null);
                }
                catch
                {
                    throw new Exception(String.Format("Invalid Property/Value pair ({0} / {1}) for control of type {2}",
                                                      key.ToString(), propertiesToApply[key],
                                                      control.GetType().ToString()));
                }
            }

            control.ID = item.ID;
            control.EnableViewState = false;
            return control;
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.templates;
            base.OnInit(e);
        }

        private Control FindControl(Control control, string id)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl.ID == id)
                    return ctrl;

                Control ctr = FindControl(ctrl, id);
                if (ctr != null)
                    return ctr;
            }

            return null;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            //            SaveAllTemplates();
            SaveTemplate(Convert.ToInt32(ddLanguage.SelectedValue), ddTemplateName.SelectedValue.Split('+')[1]);
        }

        private void SaveTemplates(Type parentType, int languageId)
        {
            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(parentType, languageId);

            foreach (Reflection.PropertyData item in items)
            {
                if (item.PropertyName == "Description")
                    continue;

                Control control = FindControl(phTemplates, item.ID);

                if (control == null)
                    throw new Exception("No Control with such ID!");

                if (item.ControlType != null)
                    item.Value = control.GetType().GetProperty(item.ControlProperty).GetValue(control, null);
                else
                {
                                 
                    TextBox txtValue = (TextBox)control;
                    if (item.Value is String)
                        item.Value = txtValue.Text;
                    else if (item.Value is Int32)
                        item.Value = Convert.ToInt32(txtValue.Text);
                    else if (item.Value is Double)
                        item.Value = Convert.ToDouble(txtValue.Text);
                }
            }

            Reflection.SavePropertiesData(parentType, items, languageId);
        }

        protected void ddLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            phTemplate.Visible = false;
            btnSave.Visible = false;

            if (ddLanguage.SelectedItem.Value == String.Empty)
            {
                trTemplateName.Visible = false;
            }
            else
            {
                int languageID = Convert.ToInt32(ddLanguage.SelectedItem.Value);
                trTemplateName.Visible = true;
                PopulateDDTemplateName(languageID);
            }
        }

        protected void ddTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddTemplateName.SelectedValue == "-1")
            {
                phTemplate.Visible = false;
                btnSave.Visible = false;
                return;
            }
            int languageID;
            if (Int32.TryParse(ddLanguage.SelectedValue, out languageID))
            {
                phTemplate.Visible = true;
                btnSave.Visible = true;
                string id = ddTemplateName.SelectedValue;
                //                loadTemplate(languageID, id);    
                LoadTemplate();
            }
        }

        private void PopulateDDTemplateName(int languageID)
        {
            ddTemplateName.Items.Clear();

            ddTemplateName.Items.Add(new ListItem("", "-1"));

            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(typeof(EmailTemplates), languageID);
            PopulateItems(items);
            items = Reflection.GetPropertiesData(typeof(MiscTemplates), languageID);
            PopulateItems(items);
        }

        private void PopulateItems(Reflection.PropertyData[] items)
        {
            foreach (Reflection.PropertyData item in items)
            {
                if (ddTemplateName.Items.FindByText(item.ClassDesc) != null) continue;
                ddTemplateName.Items.Add(new ListItem(item.ClassDesc, item.ID.Split('+')[0] + "+" + item.ClassDesc));
            }
        }

        private void LoadTemplate()
        {
            phTemplate.Controls.Clear();

            if (ddLanguage.SelectedIndex == 0 || ddTemplateName.SelectedIndex == -1 || ddTemplateName.SelectedIndex == 0)
            {
                return;
            }

            LoadTemplate(Convert.ToInt32(ddLanguage.SelectedValue), ddTemplateName.SelectedValue);
        }
        private void LoadTemplate(int languageID, string id)
        {
            Table tblTemplate = null;
            Type type = Type.GetType(id.Split('+')[0]);
            string classDesc = id.Split('+')[1];
            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(type, languageID);

            foreach (Reflection.PropertyData item in items)
            {
                if (item.ClassDesc != classDesc) continue;

                #region create table

                if (tblTemplate == null)
                {
                    tblTemplate = new Table();
                    ApplyTableProperties(tblTemplate);

                    TableHeaderRow headerRow = new TableHeaderRow();
                    TableHeaderCell header = new TableHeaderCell();
                    ApplyHeaderProperties(header);
                    header.Text = item.ClassDesc;
                    headerRow.Cells.Add(header);
                    tblTemplate.Rows.Add(headerRow);
                }

                //description
                if (item.PropertyName == "Description")
                {
                    TableRow descRow = new TableRow();
                    TableCell description = new TableCell();
                    ApplyDescriptionProperties(description);
                    description.Text = (string)item.Value;
                    descRow.Cells.Add(description);
                    descRow.CssClass = "tdescription";
                    tblTemplate.Rows.Add(descRow);
                }
                else
                //all other items of the templates
                {
                    TableRow itemRow = new TableRow();
                    TableCell labelCell = new TableCell();
                    ApplyLableProperties(labelCell);
                    labelCell.Text = item.PropertyDesc;
                    itemRow.Cells.Add(labelCell);

                    TableCell valueCell = new TableCell();

                    Control control = CreateControl(item);
                    valueCell.Controls.Add(control);

                    itemRow.Cells.Add(valueCell);
                    tblTemplate.Rows.Add(itemRow);
                }

                #endregion
            }

            phTemplate.Controls.Add(tblTemplate);

            phTemplates.Visible = false;
        }

        private void SaveTemplate(int languageID, string classDesc)
        {
            Type type = Type.GetType(ddTemplateName.SelectedValue.Split('+')[0]);
            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(type, languageID);

            foreach (Reflection.PropertyData item in items)
            {
                if (item.ClassDesc != classDesc) continue;
                if (item.PropertyName == "Description")
                    continue;

                Control control = FindControl(phTemplate, item.ID);

                if (control == null)
                    throw new Exception("No Control with such ID!");

                if (item.ControlType != null)
                    item.Value = control.GetType().GetProperty(item.ControlProperty).GetValue(control, null);
                else
                {
                    TextBox txtValue = (TextBox)control;
                    if (item.Value is String)
                        item.Value = txtValue.Text;
                    else if (item.Value is Int32)
                        item.Value = Convert.ToInt32(txtValue.Text);
                    else if (item.Value is Double)
                        item.Value = Convert.ToDouble(txtValue.Text);
                }
            }

            Reflection.SavePropertiesData(type, items, languageID);

            MessageBox.Show(Lang.TransA("Template have been successfully updated!"), Misc.MessageType.Success);
        }
    }
}