using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerDeFavorisApi.Models
{
    public class Userinfo
    {
        public string Login { get; set; }
        public string Password { get; set; } = "";
        public User.Role Role { get; set; } = User.Role.User;        
    }
}