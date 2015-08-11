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
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;

namespace AspNetDating
{
    /// <summary>
    /// The "Thank You" page shown after successfull payment
    /// </summary>
    public partial class ThankYou : PageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThankYou"/> class.
        /// </summary>
        public ThankYou()
        {
            RequiresAuthorization = false;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            WideBoxStart1.Title = Lang.Trans("Status");
            MiscTemplates.SubscriptionCompleted subscriptionCompletedTemplate =
                new MiscTemplates.SubscriptionCompleted(LanguageId);
            lblMessage.Text = subscriptionCompletedTemplate.Text;


            if (CurrentUserSession != null)
            {
                Classes.User user = new User(CurrentUserSession.Username);
                user.Load();
                CurrentUserSession.Paid = user.Paid;
                CurrentUserSession.BillingPlanOptions = null;
                CurrentUserSession.Credits = user.Credits;
            }

            //if (!Config.Credits.Required)
            //{
            //    if (CurrentUserSession != null && !Classes.User.IsNonPaidMember(CurrentUserSession.Username))
            //    {
            //        CurrentUserSession.Paid = true;
            //        CurrentUserSession.BillingPlanOptions = null;
            //    }
            //}
            //else
            //{
            //    if (CurrentUserSession != null)
            //        CurrentUserSession.Credits = Classes.User.Load(CurrentUserSession.Username).Credits;
            //}
        }
    }
}