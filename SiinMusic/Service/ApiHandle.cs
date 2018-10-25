using Newtonsoft.Json;
using SiinMusic.Entity;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SiinMusic.Service
{
    class ApiHandle
    {
        private static readonly string Home_Url = "https://siin-music.appspot.com";
        private static readonly string Register_Url = Home_Url + "/_api/v1/register";
        private static readonly string Login_Url = Home_Url + "/_api/v1/login";
        private static readonly string Check_Exitst = Home_Url + "/_api/v1/check/username?q=";
        private static readonly string Api_Upload_Song = Home_Url + "/_api/v1/upload/song";
        public async static Task<string> Sign_Up(Account account)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SiinMusic/1.0");
            var content = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(Register_Url, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            return contents;
        }

        public async static Task<bool> CheckExistsUsername(string username)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SiinMusic/1.0");
            var response = httpClient.GetAsync(Check_Exitst + username);
            var contents = bool.Parse(await response.Result.Content.ReadAsStringAsync());
            return contents;
        }

        public async static Task<ResultAPI> Sign_In(Account account)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SiinMusic/1.0");
            var content = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(Login_Url, content);
            var result = new ResultAPI
            {
                Status = response.Result.StatusCode.ToString(),
                Data = await response.Result.Content.ReadAsStringAsync()
            };
            return result;
        }

        public async static Task<ResultAPI> Upload_Song(Song song)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SiinMusic/1.0");
            var content = new StringContent(JsonConvert.SerializeObject(song), Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(Api_Upload_Song, content);
            var result = new ResultAPI
            {
                Status = response.Result.StatusCode.ToString(),
                Data = await response.Result.Content.ReadAsStringAsync()
            };
            return result;
        }
    }
}
