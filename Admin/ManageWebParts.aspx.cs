using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class ManageWebParts : AdminPageBase
    {
        private OrderedWebParts allParts;
        private void LoadStrings()
        {
            btnAddToZone.Text = "<i class=\"fa fa-plus-square\"></i>&nbsp;" + "Add to Zone".TranslateA();
            btnDisableWebPart.Text = "<i class=\"fa fa-eye-slash\"></i>&nbsp;" + "Disable component".TranslateA();
            btnEnableWebPart.Text = "<i class=\"fa fa-eye\"></i>&nbsp;" + "Enable component".TranslateA();
            btnLeftZoneRemove.Text = "<i class=\"fa fa-minus-square\"></i>&nbsp;" + "Remove component".TranslateA();
            btnRightZoneRemove.Text = "<i class=\"fa fa-minus-square\"></i>&nbsp;" + "Remove component".TranslateA();
            cbResetUsersLayout.Text = "Reset layout for existing users".TranslateA();
            btnStoreWebPartLayoutToDB.Text = "Save changes".TranslateA();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Site Management".TranslateA();
            Subtitle = "Manage Web Parts".TranslateA();
            Description = "Use this section to change the layout of the components on the members home page...".TranslateA();

            allParts = Config.WebParts.AllParts;
            if (!IsPostBack)
            {
                LoadStrings();
                PopulateWebPartPool();
                PopulateLeftZone();
                PopulateRightZone();
            }

            if (!HasWriteAccess)
            {
                btnStoreWebPartLayoutToDB.Enabled = false;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.manageWebParts;

            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            allParts.StoreDirtySettingsToSession();
        }

        private void PopulateLeftZone()
        {
            var parts =
                allParts.DirtyWebParts.Where(p => p.Zone == WebPartZone.HomePageLeftZone &&
                    p.IsVisibleByDefault && !p.Disabled);

            lbLeftZone.Items.Clear();
            foreach (var part in parts)
            {
                lbLeftZone.Items.Add(new ListItem(part.Name, part.Name));
            }
        }

        private void PopulateRightZone()
        {
            var parts =
                allParts.DirtyWebParts.Where(p => p.Zone == WebPartZone.HomePageRightZone && 
                    p.IsVisibleByDefault &&
                    !p.Disabled);

            lbRightZone.Items.Clear();
            foreach (var part in parts)
            {
                lbRightZone.Items.Add(new ListItem(part.Name, part.Name));
            }
        }

        private void PopulateWebPartPool()
        {
            lbWebPartPool.Items.Clear();
            var allDirtyParts = Config.WebParts.AllParts.DirtyWebParts;
            foreach (var part in allDirtyParts.OrderBy(p => p.Name))
            {
                var item = new ListItem(part.Name + (part.Disabled ? " (Disabled)" : String.Empty), part.Name);
                lbWebPartPool.Items.Add(item);
            }
        }

        protected void btnLeftZoneRemove_Click(object sender, EventArgs e)
        {
            var item = lbLeftZone.SelectedItem;

            if (item != null)
            {
                lbLeftZone.Items.Remove(item);
                var part = allParts.DirtyWebParts.First(p => p.Name == item.Value);
                part.IsVisibleByDefault = false;
            }

        }

        protected void btnRightZoneRemove_Click(object sender, EventArgs e)
        {
            var item = lbRightZone.SelectedItem;

            if (item != null)
            {
                lbRightZone.Items.Remove(item);
                var part = allParts.DirtyWebParts.First(p => p.Name == item.Value);
                part.IsVisibleByDefault = false;
            }
        }

        protected void btnAddToZone_Click(object sender, EventArgs e)
        {
            var item = lbWebPartPool.SelectedItem;

            if (item != null)
            {
                var part = allParts.DirtyWebParts.First(p => p.Name == item.Value);

                if (!part.IsVisibleByDefault)
                {
                    part.IsVisibleByDefault = true;
                    PopulateLeftZone();
                    PopulateRightZone();
                }
            }
        }

        protected void btnEnableWebPart_Click(object sender, EventArgs e)
        {
            var item = lbWebPartPool.SelectedItem;

            if (item != null)
            {
                var part = allParts.DirtyWebParts.First(p => p.Name == item.Value);

                if (part.Disabled)
                {
                    part.Disabled = false;
                    PopulateWebPartPool();
                }
            }
        }

        protected void btnDisableWebPart_Click(object sender, EventArgs e)
        {
            var item = lbWebPartPool.SelectedItem;

            if (item != null)
            {
                var part = allParts.DirtyWebParts.First(p => p.Name == item.Value);

                if (!part.Disabled)
                {
                    part.Disabled = true;
                    part.IsVisibleByDefault = false;
                    PopulateLeftZone();
                    PopulateRightZone();
                    PopulateWebPartPool();
                }
            }
        }

        protected void imgbLeftZoneUp_Click(object sender, EventArgs e)
        {
            MoveUp(lbLeftZone.SelectedItem);
        }

        protected void imgbLeftZoneDown_Click(object sender, EventArgs e)
        {
            MoveDown(lbLeftZone.SelectedItem);
        }

        protected void imgbRightZoneUp_Click(object sender, EventArgs e)
        {
            MoveUp(lbRightZone.SelectedItem);
        }

        protected void imgbRightZoneDown_Click(object sender, EventArgs e)
        {
            MoveDown(lbRightZone.SelectedItem);
        }

        private void MoveUp(ListItem item)
        {
            if (item != null)
            {
                allParts.MoveUp(item.Value);

                PopulateLeftZone();
                PopulateRightZone();
                PopulateWebPartPool();
            }
        }

        private void MoveDown(ListItem item)
        {
            if (item != null)
            {
                allParts.MoveDown(item.Value);

                PopulateLeftZone();
                PopulateRightZone();
                PopulateWebPartPool();
            }
        }

        protected void btnStoreWebPartLayoutToDB_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            allParts.SaveDirtyValues(cbResetUsersLayout.Checked);
            Master.MessageBox.Show("The layout of the components has been saved.", Misc.MessageType.Success);
        }
    }
}