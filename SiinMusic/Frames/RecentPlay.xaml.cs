using Newtonsoft.Json.Linq;
using SiinMusic.Entity;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SiinMusic.Frames
{
    public sealed partial class RecentPlay : Page
    {
        private static ObservableCollection<MenuItem> songsitem = new ObservableCollection<MenuItem>();
        internal static ObservableCollection<MenuItem> Songsitem { get => songsitem; set => songsitem = value; }
        public RecentPlay()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SongResults.ItemsSource = songsitem;
        }

        private void SongResults_ItemClick(object sender, ItemClickEventArgs e)
        {
            MenuItem menuItem = e.ClickedItem as MenuItem;
            ((Parent as Frame).FindName("NameMusic") as TextBlock).Text = menuItem.SongTitle;
            ((Parent as Frame).FindName("AuthorMusic") as TextBlock).Text = menuItem.SongAuthor;
            ((Parent as Frame).FindName("ThumbnailMusic") as ImageBrush).ImageSource = new BitmapImage(new Uri(menuItem.SongThumbnail));
            ((Parent as Frame).FindName("mediaPlayer") as MediaPlayerElement).Source = MediaSource.CreateFromUri(new Uri(menuItem.SongUrl));
            ((Parent as Frame).FindName("mediaPlayer") as MediaPlayerElement).AutoPlay = true;
        }
    }
}
