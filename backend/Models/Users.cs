using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSO.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }


        public bool IsDeleted { get; set; } = false;

        public List<UserRoles> UserRoles { get; set; } = new();
    }
}
