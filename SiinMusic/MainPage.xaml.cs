using Newtonsoft.Json.Linq;
using SiinMusic.Entity;
using SiinMusic.Service;
using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SiinMusic
{
    public sealed partial class MainPage : Page
    {
        private Account account;
        public MainPage()
        {
            this.account = new Account();
            this.InitializeComponent();
        }

        private async void Login_Button(object sender, RoutedEventArgs e)
        {
            var username = Username.Text;
            var password = Password.Password;
            if (username == "")
            {
                Username_Message.Text = "Username is empty.";
            }
            else
            {
                Username_Message.Text = "";
                this.account.username = username;
            };
            if (password == "")
            {
                Password_Message.Text = "Password is empty.";
            }
            else
            {
                Password_Message.Text = "";
                this.account.password = password;
            };
            if (Username_Message.Text == "" && Password_Message.Text == "")
            {
                Response_Login.Text = "";
                ResultAPI response = await ApiHandle.Sign_In(this.account);
                JObject data = JObject.Parse(response.Data);
                var errorCode = response.Status;
                if (errorCode == "Unauthorized")
                {
                    Response_Login.Text = data.SelectToken("error").ToString();
                }
                else
                {
                    this.Frame.Navigate(typeof(Frames.MainSiinMusic));

                    StorageFolder folder = ApplicationData.Current.LocalFolder;
                    StorageFile file = await folder.CreateFileAsync("config_login.json", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file, data.ToString());
                }
            }
        }

        private void Tab_Register(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Frames.Register));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFile config_login = await ApplicationData.Current.LocalFolder.GetFileAsync("config_login.json");
                String info = await FileIO.ReadTextAsync(config_login);

                if (info != "")
                {
                    
                    this.Frame.Navigate(typeof(Frames.MainSiinMusic));
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Login...");
            }
        }

        private void Password_KeyUp(object sender, KeyRoutedEventArgs e)
        {

        }
    }
}
