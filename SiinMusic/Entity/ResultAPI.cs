using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiinMusic.Entity
{
    class ResultAPI
    {
        private string status;
        private string data;

        public ResultAPI()
        {
        }

        public ResultAPI(string status, string data)
        {
            this.status = status;
            this.data = data;
        }

        public string Status { get => status; set => status = value; }
        public string Data { get => data; set => data = value; }
    }
}
