﻿using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.DTOs.Auth
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
