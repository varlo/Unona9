using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MultiFileUploader
{
    public partial class Page
    {
        public Page()
        {
            InitializeComponent();
        }

        public ObservableCollection<FileUploadEntry> FileUploadEntries =
            new ObservableCollection<FileUploadEntry>();

        private int filesUploaded;
        public const string imageTypes = "*.jpg;*.png;*.gif";

        private void AddFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "All Files (*.*)|*.*", Multiselect = true };
            bool? dialogResult = ofd.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                foreach (var fileInfo in ofd.Files)
                {
                    var entry = new FileUploadEntry { FileInfo = fileInfo };
                    FileUploadEntries.Add(entry);
                }

                if (UploadFilesListBox.ItemsSource == null)
                    UploadFilesListBox.ItemsSource = FileUploadEntries;
            }

            if (UploadFilesListBox.Items.Count > 0)
                UploadFilesButton.IsEnabled = true;
        }

        private void UploadFilesButton_Click(object sender, RoutedEventArgs e)
        {
            AddFilesButton.IsEnabled = false;
            UploadFilesButton.IsEnabled = false;
            foreach (var entry in FileUploadEntries)
            {
                var homeUrl = HtmlPage.Document.DocumentUri.AbsoluteUri.Remove(
                    HtmlPage.Document.DocumentUri.AbsoluteUri.LastIndexOf('/'));
                var request = (HttpWebRequest) WebRequest.Create(
                                                   new Uri(homeUrl + "/SilverlightUpload.ashx"));
                request.Method = "POST";
                request.Headers["guid"] = ((App)Application.Current).Guid;
                request.Headers["type"] = ((App) Application.Current).Type;
                request.BeginGetRequestStream(ContinueGettingResponse, 
                    new object[] { request, entry });
            }
        }

        private void ContinueGettingResponse(IAsyncResult ar)
        {
            var request = (HttpWebRequest)((object[])ar.AsyncState)[0];
            var entry = (FileUploadEntry) ((object[])ar.AsyncState)[1];
            var stream = request.EndGetRequestStream(ar);

            var fileStream = entry.FileInfo.OpenRead();
            var buffer = new byte[1024];
            while (fileStream.Read(buffer, 0, buffer.Length) > 0)
            {
                stream.Write(buffer, 0, buffer.Length);
                Dispatcher.BeginInvoke(() => { entry.BytesUploaded += buffer.Length; });
                
            }
            Dispatcher.BeginInvoke(() => { entry.BytesUploaded = entry.BytesTotal; });
            
            stream.Close();

            request.BeginGetResponse(RetriveResponseAsync, request);
        }

        private void RetriveResponseAsync(IAsyncResult ar)
        {
            var request = (HttpWebRequest) ar.AsyncState;
            HttpWebResponse response = (HttpWebResponse) request.EndGetResponse(ar);    
            response.Close();
            
            filesUploaded++;
            if (FileUploadEntries.Count == filesUploaded)
            {
                Dispatcher.BeginInvoke(() => HtmlPage.Window.CreateInstance("silverlightUploadIsCompleted"));
            }
        }
    }

    public class FileUploadEntry : INotifyPropertyChanged
    {
        public FileInfo FileInfo { get; set; }
        public string Filename
        {
            get
            {
                return FileInfo.Name;
            }
        }

        private long bytesUploaded;
        public long BytesUploaded 
        { 
            get
            {
                return bytesUploaded;
            }
            set
            {
                if (bytesUploaded != value)
                {
                    bytesUploaded = value;
                    NotifyPropertyChanged("BytesUploaded");
                }
            }
        }
        public long BytesTotal
        {
            get
            {
                return FileInfo.Length;
            }
        }
        public string BytesTotalFormatted
        {
            get
            {
                return String.Format("{0:0.00} KB", BytesTotal / 1024.00);
            }
        }

        private BitmapImage fileBitmap;
        public BitmapImage FileBitmap
        {
            get
            {
                if (fileBitmap == null && Page.imageTypes.Split(';').FirstOrDefault(ext => ext == "*" + FileInfo.Extension.ToLower()) != null)
                {
                    fileBitmap = new BitmapImage();
                    FileStream stream = FileInfo.OpenRead();
                    fileBitmap.SetSource(stream);
                    stream.Close();
                }
                return fileBitmap;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}