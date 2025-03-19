using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Models
{
    public class Roles
    {
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; }
        public string Description { get; set; }

        [Required]
        public string SystemName { get; set; }

        public List<UserRoles> UserRoles { get; set; } = new();
    }
}