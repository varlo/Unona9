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
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using Image=System.Drawing.Image;

namespace AspNetDating.Admin
{
    /// <summary>
    ///		Summary description for EditPhotosCtrl.
    /// </summary>
    public partial class EditPhotosCtrl : UserControl
    {
        //protected System.Web.UI.WebControls.Label lblError;
        protected MessageBox MessageBox;

        private class IndexedPhotos
        {
            private SortedList sortedList;
            private HttpSessionState Session;

            public IndexedPhotos(HttpSessionState session)
            {
                Session = session;
            }

            public Photo this[string photoID]
            {
                get
                {
                    if (Session["temp_photos"] == null)
                        return null;
                    else
                        sortedList = (SortedList) Session["temp_photos"];

                    return (Photo) sortedList[photoID];
                }

                set
                {
                    if (Session["temp_photos"] == null)
                        sortedList = new SortedList();
                    else
                        sortedList = (SortedList) Session["temp_photos"];

                    sortedList[photoID] = value;
                    Session["temp_photos"] = sortedList;
                }
            }

            public void Remove(string photoID)
            {
                if (sortedList.Contains(photoID))
                    sortedList.Remove(photoID);
            }

            public void Clear()
            {
                Session["temp_photos"] = null;
            }

            public Photo[] GetPhotosArray()
            {
                sortedList = (SortedList) Session["temp_photos"];
                if (sortedList != null)
                {
                    Array array = Array.CreateInstance(typeof (Photo), sortedList.Values.Count);
                    sortedList.Values.CopyTo(array, 0);
                    return array as Photo[];
                }
                else
                    return null;
            }
        }

        private IndexedPhotos TemporaryPhotos;

        protected string NewTempID
        {
            get
            {
                if (Session["LastCount"] == null)
                    Session["LastCount"] = 0;

                int id = (int) Session["LastCount"];
                ++id;
                Session["LastCount"] = id;
                return "TempID" + id.ToString();
            }
        }

        protected string CurrentTempID
        {
            get
            {
                if (Session["LastCount"] == null)
                    return null;

                int id = (int) Session["LastCount"];
                return "TempID" + id.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TemporaryPhotos = new IndexedPhotos(Session);

            if (!Page.IsPostBack)
            {
                LoadStrings();
                LoadPhotos(true);
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dlPhotos.ItemDataBound += new DataListItemEventHandler(dlPhotos_ItemDataBound);
            this.dlPhotos.ItemCommand += new DataListCommandEventHandler(dlPhotos_ItemCommand);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
        }

        #endregion

        private User user;

        public User User
        {
            set
            {
                user = value;
                if (user != null)
                {
                    if (hidUsername.Value != user.Username)
                        Session["temp_photos"] = null;

                    hidUsername.Value = user.Username;
                }
                else
                    hidUsername.Value = "";
            }
            get
            {
                if (user == null
                    && hidUsername.Value != "")
                    user = User.Load(hidUsername.Value);
                return user;
            }
        }


        private void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
            if (!((AdminPageBase)Page).HasWriteAccess) 
                btnSave.Enabled = false;
            btnCancel.Text = Lang.TransA("Cancel");
        }

        private void LoadPhotos(bool fromDB)
        {
            if (User == null) return;

            Photo[] photos;

            if (fromDB)
            {
                photos = Photo.Fetch(User.Username);

                if (photos != null)
                    foreach (Photo photo in photos)
                    {
                        TemporaryPhotos[photo.ExtendedID] = photo;
                    }
            }

            DataTable dtPhotos = new DataTable("Photos");
            dtPhotos.Columns.Add("PhotoID");
            dtPhotos.Columns.Add("Name");
            dtPhotos.Columns.Add("Description");
            dtPhotos.Columns.Add("Approved", typeof (bool));
            dtPhotos.Columns.Add("Primary", typeof (bool));
            dtPhotos.Columns.Add("ExplicitPhoto", typeof (bool));

            photos = TemporaryPhotos.GetPhotosArray();

            if (photos != null && photos.Length > 0)
                foreach (Photo photo in photos)
                {
                    dtPhotos.Rows.Add(new object[]
                                          {
                                              photo.ExtendedID,
                                              photo.Name,
                                              photo.Description,
                                              photo.Approved,
                                              photo.Primary,
                                              photo.ExplicitPhoto
                                          });
                }

            if (photos == null || photos.Length < Config.Photos.MaxPhotos)
            {
                dtPhotos.Rows.Add(new object[]
                                      {
                                          0, //photo.Id,
                                          "", //photo.Name,
                                          "", //photo.Description,
                                          true, //photo.Approved,
                                          false, //photo.Primary
                                          false //photo.ExplicitPhoto
                                      });
            }

            dlPhotos.DataSource = dtPhotos;
            dlPhotos.DataBind();
        }

        private void SaveTextFields()
        {
            TextBox txtName;
            TextBox txtDescription;
            CheckBox chkExplicitPhoto;

            foreach (Photo photo in TemporaryPhotos.GetPhotosArray())
            {
                foreach (DataListItem item in dlPhotos.Items)
                {
                    HtmlInputHidden hidField = (HtmlInputHidden) item.FindControl("hidPictureID");
                    if (photo.ExtendedID == hidField.Value)
                    {
                        txtName = (TextBox) item.FindControl("txtName");
                        txtDescription = (TextBox) item.FindControl("txtDescription");

                        photo.Name = txtName.Text;
                        photo.Description = txtDescription.Text;

                        if (Config.Photos.EnableExplicitPhotos)
                        {
                            chkExplicitPhoto = (CheckBox) item.FindControl("chkExplicitPhoto");
                            photo.ExplicitPhoto = chkExplicitPhoto.Checked;

                            if (photo.ExplicitPhoto && Config.Photos.MakeExplicitPhotosPrivate)
                            {
                                photo.PrivatePhoto = true;
                            }
                        }

                        break;
                    }
                }
            }
        }

        private void dlPhotos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            string photoID = (string) e.CommandArgument;
            Photo photo = null;

            switch (e.CommandName)
            {
                case "UploadPhoto":
                    Image image = null;
                    try
                    {
                        HtmlInputFile fileField = (HtmlInputFile) e.Item.FindControl("ufPhoto");
                        image = Image.FromStream
                            (fileField.PostedFile.InputStream);
                    }
                    catch
                    {
                        MessageBox.Show(Lang.TransA("Invalid image!"), Misc.MessageType.Error);
                        return;
                    }

                    photo = TemporaryPhotos[photoID];

                    if (photo == null)
                    {
                        photo = new Photo();
                        photo.ExtendedID = photoID;
                        photo.User = User;
                    }

                    photo.Image = image;
                    photo.Approved = true;

                    TemporaryPhotos[photoID] = photo;

                    SaveTextFields();
                    LoadPhotos(false);
                    break;

                case "Delete":
                    if (!((AdminPageBase)Page).HasWriteAccess) return;
                    
                    if (TemporaryPhotos[photoID] != null)
                    {
                        photo = TemporaryPhotos[photoID];

                        if (photo.Id.ToString() == photo.ExtendedID)
                        {
                            try
                            {
                                photo = Photo.Fetch(photo.Id);
                                photo.Delete();
                            }
                            catch (NotFoundException)
                            {
                            }

                            TemporaryPhotos.Remove(photoID);
                        }
                        else
                        {
                            TemporaryPhotos.Remove(photoID);
                        }
                    }
                    SaveTextFields();
                    LoadPhotos(false);
                    break;

                case "SetPrimary":
                    if (!((AdminPageBase)Page).HasWriteAccess) return;
                    
                    foreach (Photo photo_ in TemporaryPhotos.GetPhotosArray())
                        photo_.Primary = false;

                    photo = TemporaryPhotos[photoID];
                    photo.Primary = true;

                    SaveTextFields();
                    LoadPhotos(false);
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            TemporaryPhotos.Clear();
            Response.Redirect("BrowseUsers.aspx");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!((AdminPageBase)Page).HasWriteAccess) 
                return;
            
            if (TemporaryPhotos.GetPhotosArray() != null)
            {
                SaveTextFields();
                foreach (Photo photo in TemporaryPhotos.GetPhotosArray())
                {
                    if (photo.Id > 0)
                    {
                        try
                        {
                            string cacheFileDir = Config.Directories.ImagesCacheDirectory + "/" + photo.Id%10;
                            string cacheFileMask = String.Format("photo_{0}_*.jpg", photo.Id);
                            foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                            {
                                File.Delete(file);
                            }
                            cacheFileMask = String.Format("photoface_{0}_*.jpg", photo.Id);
                            foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                            {
                                File.Delete(file);
                            }
                        }
                        catch (Exception err)
                        {
                            Global.Logger.LogError(err);
                        }
                    }
                    
                    photo.Save(true);
                }
            }

            TemporaryPhotos.Clear();
            LoadPhotos(true);
            Response.Redirect("BrowseUsers.aspx");
        }

        private void dlPhotos_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            LinkButton lnkSetPrimary = e.Item.FindControl("lnkSetPrimary") as LinkButton;
            LinkButton lnkDelete = (LinkButton) e.Item.FindControl("lnkDelete");

            lnkDelete.Attributes.Add("onclick",
                                     String.Format("javascript: return confirm('{0}')",
                                                   Lang.TransA("Do you really want to delete this photo?")));


            if (!((AdminPageBase)Page).HasWriteAccess) 
            {
                if (lnkSetPrimary != null)
                    lnkSetPrimary.Enabled = false;

                lnkDelete.Enabled = false;
            }
        }
    }
}