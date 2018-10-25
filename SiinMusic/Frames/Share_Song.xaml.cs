using Newtonsoft.Json.Linq;
using SiinMusic.Entity;
using SiinMusic.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SiinMusic.Frames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Share_Song : Page
    {
        public Share_Song()
        {
            this.InitializeComponent();
        }

        private async void ChooseThumbnail_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,

                ViewMode = PickerViewMode.Thumbnail
            };

            fileOpenPicker.FileTypeFilter.Clear();
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".bmp");

            StorageFile file = await fileOpenPicker.PickSingleFileAsync();
            if (file != null)
            {
                IRandomAccessStream fileStream =
                await file.OpenAsync(FileAccessMode.Read);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(fileStream);

                IBuffer buffer = await FileIO.ReadBufferAsync(file);

                var request = (HttpWebRequest)WebRequest.Create("https://api.imgur.com/3/image");
                request.Headers.Add("Authorization", "Client-ID 490b09a77765fd3");
                request.Method = "POST";

                ASCIIEncoding enc = new ASCIIEncoding();
                //string postData = Convert.ToBase64String(file);
                //byte[] bytes = enc.GetBytes(postData);

                byte[] bytes = buffer.ToArray();

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;

                Stream writer = request.GetRequestStream();
                writer.Write(bytes, 0, bytes.Length);

                var response = (HttpWebResponse)request.GetResponse();
                var url = new StreamReader(response.GetResponseStream());
                JObject objImgur = JObject.Parse(url.ReadToEnd());

                Url_Thumbnail.Text = objImgur.SelectToken("data").SelectToken("link").ToString();

                PreviewThumbnail.Source = bitmapImage;
            }
        }

        private bool Check_Url_Song (string input)
        {
            if (input.EndsWith(".mp3"))
            {
                return true;
            }
            return false;
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var title = Title.Text;
            var thumbnail = Url_Thumbnail.Text;
            var song = Url_Song.Text;
            if (title == "")
            {
                Title_Message.Text = "Title is empty!";
            }
            else
            {
                Title_Message.Text = "";
            }
            if (thumbnail == "")
            {
                Url_Thumbnail_Mesage.Text = "Thumbnail is empty!";
            }
            else
            {
                Url_Thumbnail_Mesage.Text = "";
            }
            if (song == "")
            {
                Url_Song_Message.Text = "Song url is empty!";
            }
            if (Check_Url_Song(song))
            {
                Url_Song_Message.Text = "";
            }
            else
            {
                Url_Song_Message.Text = "Song url invalid!";
            }
            if (Title_Message.Text == "" && Url_Thumbnail_Mesage.Text == "" && Url_Song_Message.Text == "" && Check_Url_Song(song))
            {
                StorageFile config_login = await ApplicationData.Current.LocalFolder.GetFileAsync("config_login.json");
                JObject data = JObject.Parse(await FileIO.ReadTextAsync(config_login));

                Song songEntity = new Song
                {
                    author = data.SelectToken("fullname").ToString(),
                    thumbnail = thumbnail,
                    title = title,
                    url = song
                };
                var rs = await ApiHandle.Upload_Song(songEntity);
                if (rs.Status == "OK")
                {
                    ContentDialog uploadSuccess = new ContentDialog()
                    {
                        Title = "Upload success!",
                        CloseButtonText = "Ok"
                    };

                    await uploadSuccess.ShowAsync();
                }
            }
        }
    }
}
