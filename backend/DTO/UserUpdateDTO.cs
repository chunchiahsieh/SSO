using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.DTO
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string checkPassword { get; set; }
    }
}