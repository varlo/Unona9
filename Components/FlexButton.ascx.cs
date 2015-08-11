using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace AspNetDating.Components
{
    [Themeable(true)]
    public partial class FlexButton : UserControl, IButtonControl
    {
        #region Properties

        public event EventHandler Click;

        public enum Type
        {
            Button,
            ImageButton,
            LinkButton
        }

        private string imageUrl;

        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                imageUrl = value;
                if (Controls.Count > 0)
                {
                    if (Controls[0] is ImageButton)
                        ((ImageButton) Controls[0]).ImageUrl = value;
                }
            }
        }

        private string postBackUrl;

        public string PostBackUrl
        {
            get { return postBackUrl; }
            set
            {
                postBackUrl = value;
                if (Controls.Count > 0)
                {
                    if (Controls[0] is Button)
                        ((Button) Controls[0]).PostBackUrl = value;
                    if (Controls[0] is ImageButton)
                        ((ImageButton) Controls[0]).PostBackUrl = value;
                    if (Controls[0] is LinkButton)
                        ((LinkButton) Controls[0]).PostBackUrl = value;
                }
            }
        }

        private string cssClass;

        public string CssClass
        {
            get { return cssClass; }
            set
            {
                cssClass = value;
                if (Controls.Count > 0)
                {
                    ((WebControl) Controls[0]).CssClass = value;
                }
            }
        }

        private short tabIndex;

        public short TabIndex
        {
            get { return tabIndex; }
            set
            {
                tabIndex = value;
                if (Controls.Count > 0)
                {
                    ((WebControl) Controls[0]).TabIndex = value;
                }
            }
        }

        private string toolTip;

        public string ToolTip
        {
            get { return toolTip; }
            set
            {
                toolTip = value;
                if (Controls.Count > 0)
                {
                    ((WebControl) Controls[0]).ToolTip = value;
                }
            }
        }

        private string text;

        public string Text
        {
            get { return text; }

            set
            {
                text = value;
                if (Controls.Count > 0)
                {
                    if (Controls[0] is Button)
                        ((Button) Controls[0]).Text = value;
                    if (Controls[0] is LinkButton)
                        ((LinkButton) Controls[0]).Text = value;
                }
            }
        }

        public Type RenderAs {get;set;}

        #endregion

        public override string ClientID
        {
            get 
            {
                var button = Controls.OfType<Control>().FirstOrDefault(c => c.ID == "button" || c.ID == "imgbutton" || c.ID == "lnkbutton");

                if (button != null)
                    return button.ClientID;
                else
                    return base.ClientID; 
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            switch (RenderAs)
            {
                case Type.Button:
                    var btn = new Button
                                  {
                                      ID = "button",
                                      Text = Text,
                                      PostBackUrl = PostBackUrl,
                                      CssClass = CssClass,
                                      TabIndex = TabIndex
                                  };
                    btn.Click += btn_Click;
                    Controls.Add(btn);
                    break;
                case Type.ImageButton:
                    var imgButton = new ImageButton
                                        {
                                            ID = "imgbutton",
                                            PostBackUrl = PostBackUrl,
                                            ImageUrl = ImageUrl,
                                            CssClass = CssClass,
                                            TabIndex = TabIndex
                                        };
                    imgButton.Click += imgButton_Click;
                    Controls.Add(imgButton);
                    break;
                case Type.LinkButton:
                    var lnkButton = new LinkButton
                                        {
                                            ID = "lnkbutton",
                                            Text = Text,
                                            PostBackUrl = PostBackUrl,
                                            CssClass = CssClass,
                                            TabIndex = TabIndex
                                        };
                    lnkButton.Click += lnkButton_Click;
                    Controls.Add(lnkButton);
                    break;
            }
        }

        protected void lnkButton_Click(object sender, EventArgs e)
        {
            OnClick(sender, e);
        }

        protected void imgButton_Click(object sender, ImageClickEventArgs e)
        {
            OnClick(sender, e);
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            OnClick(sender, e);
        }

        protected virtual void OnClick(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(sender, e);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region IButtonControl Members

        public bool CausesValidation
        {
            get { return false; }
            set { }
        }

#pragma warning disable 0067

        public event CommandEventHandler Command;

        public string CommandArgument { get; set; }

        public string CommandName { get; set; }

        public string ValidationGroup { get; set; }

        #endregion
    }
}