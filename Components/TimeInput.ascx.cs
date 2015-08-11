using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Drawing;

namespace AspNetDating.Components
{
    public partial class TimeInput : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.EndsWith("tt"))
                    ddAMPM.Visible = false;

                lblExample.Text = 
                    String.Format("(e.g. {0})", new DateTime(1, 1, 1, 18, 59, 0).ToShortTimeString());
            }
        }

        public bool ValidTimeEntered
        {
            get
            {
                return txtTime.Text.Trim().Length == 0 || Time.HasValue;
            }
        }

        public DateTime? Time
        {
            get
            {
                string timeString = txtTime.Text.Trim();

                if (ddAMPM.Visible)
                {
                    timeString = String.Format("{0} {1}", timeString, ddAMPM.SelectedValue);
                }

                DateTime time;
                if (DateTime.TryParse(timeString, out time))
                    return time;

                return null;
            }
            set
            {
                if (!value.HasValue)
                {
                    txtTime.Text = String.Empty;
                    ddAMPM.SelectedValue = "AM";
                    return;
                }

                if (ddAMPM.Visible)
                {
                    if (value.Value.Hour > 12)
                        ddAMPM.SelectedValue = "PM";
                    else
                        ddAMPM.SelectedValue = "AM";

                    var format = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.TrimEnd('t', ' ');

                    txtTime.Text = value.Value.ToString(format);
                }
                else
                {
                    txtTime.Text = value.Value.ToShortTimeString();
                }
            }
        }
    }
}