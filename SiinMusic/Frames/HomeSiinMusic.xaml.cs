using Newtonsoft.Json.Linq;
using SiinMusic.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace SiinMusic.Frames
{
    public sealed partial class HomeSiinMusic : Page
    {
        ObservableCollection<MenuItem> songsitem = new ObservableCollection<MenuItem>();
        public HomeSiinMusic()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            songsitem.Clear();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SiinMusic/1.0");
            var response = httpClient.GetAsync("https://siin-music.appspot.com/_api/v1/list/song");
            var contents = await response.Result.Content.ReadAsStringAsync();
            if (contents == "")
            {
                No_Song.Text = "List song is empty!";
            }
            else
            {
                JObject data = JObject.Parse(contents);
                try
                {
                    foreach (KeyValuePair<string, JToken> keyValuePair in data)
                    {
                        songsitem.Add(new MenuItem
                        {
                            SongAuthor = (string)keyValuePair.Value.SelectToken("author"),
                            SongThumbnail = (string)keyValuePair.Value.SelectToken("thumbnail"),
                            SongTitle = (string)keyValuePair.Value.SelectToken("title"),
                            SongUrl = (string)keyValuePair.Value.SelectToken("url"),
                            SongId = long.Parse((string)keyValuePair.Value.SelectToken("id"))
                        });
                    }
                }
                catch (NullReferenceException err)
                {
                    Debug.WriteLine(err);
                }

                SongResults.ItemsSource = songsitem;
            }
        }

        private void SearchResults_ItemClick(object sender, ItemClickEventArgs e)
        {
            MenuItem menuItem = e.ClickedItem as MenuItem;
            ((Parent as Frame).FindName("NameMusic") as TextBlock).Text = menuItem.SongTitle;
            ((Parent as Frame).FindName("AuthorMusic") as TextBlock).Text = menuItem.SongAuthor;
            ((Parent as Frame).FindName("ThumbnailMusic") as ImageBrush).ImageSource = new BitmapImage(new Uri(menuItem.SongThumbnail));
            ((Parent as Frame).FindName("mediaPlayer") as MediaPlayerElement).Source = MediaSource.CreateFromUri(new Uri(menuItem.SongUrl));
            ((Parent as Frame).FindName("mediaPlayer") as MediaPlayerElement).AutoPlay = true;

            if (RecentPlay.Songsitem.Any(p => p.SongId == menuItem.SongId) == false) RecentPlay.Songsitem.Add(menuItem);

        }
    }
}
