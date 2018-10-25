using Newtonsoft.Json.Linq;
using SiinMusic.Entity;
using SiinMusic.Service;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace SiinMusic.Frames
{
    public sealed partial class Register : Page
    {
        private Account account;
        public Register()
        {
            this.account = new Account();
            this.InitializeComponent();
        }

        private async void Choose_Avatar(object sender, RoutedEventArgs e)
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

                account.avatar = objImgur.SelectToken("data").SelectToken("link").ToString();
                
                PreviewAvatar.Source = bitmapImage;
            }
        }

        private void Selected_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton radioGender = sender as RadioButton;
            this.account.gender = Int32.Parse(radioGender.Tag.ToString());
            Debug.WriteLine(this.account.gender);
        }

        bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        private async void Register_Button(object sender, RoutedEventArgs e)
        {
            var username = Username.Text;
            var password = Password.Password;
            var confirmpassword = ConfirmPassword.Password;
            var fullname = Fullname.Text;
            var email = Email.Text;
            var birthday = BirthDay.Date;
            var avatar = "";
            if (ChooseAvatar.Content.ToString() != "Choose File")
            {
                avatar = ChooseAvatar.Content.ToString();
            };
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
            };
            if (confirmpassword != password)
            {
                ConfirmPassword_Message.Text = "Confirm Password does not match";
            }
            else
            {
                ConfirmPassword_Message.Text = "";
                this.account.password = password;
            };
            if (fullname == "")
            {
                Fullname_Message.Text = "Full Name is empty.";
            }
            else
            {
                Fullname_Message.Text = "";
                this.account.fullname = fullname;
            };
            if (!ValidateEmail(email))
            {
                Email_Message.Text = "Email invalid.";
            }
            else
            {
                Email_Message.Text = "";
                this.account.email = email;
            };
            if (birthday >= DateTime.Today)
            {
                BirthDay_Message.Text = "Birth Day invalid.";
            }
            else
            {
                BirthDay_Message.Text = "";
                this.account.birthday = birthday.ToString();
            }
            if (Username_Message.Text == "" && Password_Message.Text == "" && ConfirmPassword_Message.Text == "" && Fullname_Message.Text == "" && Email_Message.Text == "" && birthday < DateTime.Today)
            {
                if (await ApiHandle.CheckExistsUsername(this.account.username))
                {
                    var response = await ApiHandle.Sign_Up(this.account);
                    Debug.WriteLine(response);
                    Username_Message.Text = "";
                    ContentDialog uploadSuccess = new ContentDialog()
                    {
                        Title = "Register success!",
                        CloseButtonText = "Ok"
                    };

                    await uploadSuccess.ShowAsync();
                }
                else
                {
                    Username_Message.Text = "Username exists.";
                }
            }
        }

        private void Login_Now(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
