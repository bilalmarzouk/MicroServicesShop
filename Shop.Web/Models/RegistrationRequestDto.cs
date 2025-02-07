﻿using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Models
{
    public class RegistrationRequestDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Role { get; set; }
    }
}
