﻿using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MinLength(DataModelsConstants.UserNameMinLength)]
        [MaxLength(DataModelsConstants.UserNameMaxLength)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(DataModelsConstants.UserNameMinLength)]
        [MaxLength(DataModelsConstants.UserNameMaxLength)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        public List<SelectListItem> Currencies { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Birthdate { get; set; }
    }
}
