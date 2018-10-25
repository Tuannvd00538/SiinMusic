using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiinMusic.Entity
{
    class MenuItem
    {
        public MenuItem()
        {
        }

        public MenuItem(long songId,string songThumbnail, string songTitle, string songAuthor, string songUrl)
        {
            SongId = songId;
            SongThumbnail = songThumbnail;
            SongTitle = songTitle;
            SongAuthor = songAuthor;
            SongUrl = songUrl;
        }

        public string SongThumbnail { get; set; }
        public string SongTitle { get; set; }
        public string SongAuthor { get; set; }
        public string SongUrl { get; set; }
        public long SongId { get; set; }
    }
}
