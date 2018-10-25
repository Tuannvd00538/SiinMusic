using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiinMusic.Entity
{
    class Account
    {
        private string _username;
        private string _password;
        private string _fullname;
        private string _email;
        private string _birthday;
        private string _avatar;
        private int _gender;
    
        public Account()
        {
        }

        public Account(string username, string password)
        {
            this._username = username;
            this._password = password;
        }

        public Account(string username, string password, string fullname, string email, string birthday, string avatar, int gender)
        {
            this._username = username;
            this._password = password;
            this._fullname = fullname;
            this._email = email;
            this._birthday = birthday;
            this._avatar = avatar;
            this._gender = gender;
        }
        
        public string username { get => _username; set => _username = value; }
        public string password { get => _password; set => _password = value; }
        public string fullname { get => _fullname; set => _fullname = value; }
        public string email { get => _email; set => _email = value; }
        public string birthday { get => _birthday; set => _birthday = value; }
        public string avatar { get => _avatar; set => _avatar = value; }
        public int gender { get => _gender; set => _gender = value; }
    }
}
