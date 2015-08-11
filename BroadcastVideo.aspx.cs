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
using System.IO;
using System.Reflection;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class BroadcastVideo : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterClientScriptInclude("FlashInterop", "scripts/FlashInterop.js");
            //string scriptText;

            //using (TextReader reader = new StreamReader(Server.MapPath("~").TrimEnd('/') + "/scripts/FlashInteropIE.js"))
            //{
            //    scriptText = reader.ReadToEnd();
            //}

            //ClientScript.RegisterClientScriptBlock(GetType(), "FlashInteropIE", scriptText, false);

            #region check if permissions are granted
            var permissionCheckResult = CurrentUserSession.CanBroadcastVideo();

            if (permissionCheckResult == PermissionCheckResult.Yes)
            {
            }
            else if (permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded ||
                    permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded)
            {
                Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanBroadcastVideo;
                Response.Redirect("~/Profile.aspx?sel=payment");
                return;
            }
            else if (permissionCheckResult == PermissionCheckResult.No)
            {
                StatusPageMessage = Lang.Trans("You are not allowed to broadcast video!");
                Response.Redirect("ShowStatus.aspx");
                return;
            }
            #endregion

            LoadStrings();

            btnStartBroadcast.Attributes.Add("onclick", 
                "window.open('BroadcastVideoWindow.aspx', 'broadcastvideo', " + 
                "'width=640,height=500,resizable=0,menubar=0,status=0,toolbar=0'); return false;");
        }

        private void LoadStrings()
        {
            WideBoxStart1.Title = Lang.Trans("Live Video Broadcast");
            btnStartBroadcast.Text = Lang.Trans("Start Broadcast");
        }
    }
}