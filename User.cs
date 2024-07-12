using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginApp
{
    public class User
    {
        public string Username { get; set; } //Get User in json and set in program.cs
        public string Password { get; set; } //Get Password in json and set in program.cs
        public bool Key { get; set; } //Get Key in json and set in program.cs
    }
}
