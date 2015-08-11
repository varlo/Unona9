using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    public partial class EditSkinColor : EditSkinBaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.RegisterJQuery();
            Page.RegisterJPicker();
        }

        public override string GetStyleLine()
        {
            if (txtColor.Text.Trim().Length > 0)
                return "color: " + "#" + txtColor.Text + ";";
            return "color: inherit;";
        }

        public override void SetStyleLine(string line)
        {
            var match = Regex.Match(line, @"color:(.*?);", RegexOptions.IgnoreCase);
            if (match.Success)
                txtColor.Text = match.Groups[1].Value.Replace("!important", "").Trim().Replace("#", "");
        }
    }
}