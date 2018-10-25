using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SiinMusic.Frames
{
    public sealed partial class Setting : Page
    {
        public Setting()
        {
            this.InitializeComponent();
        }

        private async void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StorageFile config_login = await ApplicationData.Current.LocalFolder.GetFileAsync("config_login.json");
            await config_login.DeleteAsync();
        }
    }
}
