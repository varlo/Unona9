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
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetDating.Admin
{
    /// <summary>
    ///		Summary description for MessageBox.
    /// </summary>
    public partial class MessageBox : UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            divMessage.Visible = false;
        }

        private string cssClass;

        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        public void Show(string message, Classes.Misc.MessageType messageType)
        {
            divMessage.InnerHtml = message;
            divMessage.Visible = true;
            switch (messageType)
            {
                case Classes.Misc.MessageType.Error:
                    divMessage.Attributes.Add("class", "alert alert-danger");
                    break;
                case Classes.Misc.MessageType.Success:
                    divMessage.Attributes.Add("class", "alert alert-info");
                    break;
            }
        }

    }
}