using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiinMusic.Entity
{
    class Song
    {
        private string _author;
        private string _thumbnail;
        private string _title;
        private string _url;

        public Song()
        {
        }

        public Song(string author, string thumbnail, string title, string url)
        {
            this._author = author;
            this._thumbnail = thumbnail;
            this._title = title;
            this._url = url;
        }

        public string author { get => _author; set => _author = value; }
        public string thumbnail { get => _thumbnail; set => _thumbnail = value; }
        public string title { get => _title; set => _title = value; }
        public string url { get => _url; set => _url = value; }
    }
}
