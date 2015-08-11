﻿/* ASPnetDating 
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
using System.Web.UI.WebControls.WebParts;
using AspNetDating.Classes;

namespace AspNetDating.Components.WebParts
{
    [Editable]
    public partial class NewsBoxWebPart : WebPartUserControl
    {
        #region Properties

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Number of news")]
        public int Count
        {
            get
            {
                if (News1 != null)
                    return News1.Count;

                return Config.Misc.NumberOfNews;
            }
            set
            {
                if (News1 != null)
                    News1.Count = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}