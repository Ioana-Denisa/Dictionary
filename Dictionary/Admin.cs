using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1_Dictionar
{
    internal class Admin
    {
        public string username { get; set; }
        public string password { get; set; }
        public Admin(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
