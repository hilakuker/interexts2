//using Interext.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Interext.OtherCalsses;

namespace Interext.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Birth day is incorrect.")]
        [BirthdateValidation(ErrorMessage = "Birth Day cannot be bigger than Today's date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage=" ")]
        [Display(Name = "Your Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = " ")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        
        [Display(Name = "Username")]
        public string UserName { get; set; }

//[AtLeastOneRequiredAttribute("Email", "Password", "FirstName", "LastName", "Address", ErrorMessage = "Please fill the required fields")]

        //[Required(ErrorMessage = "Password field is required.")]
        [Required(ErrorMessage = " ")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "First Name field is required.")]
        [Required(ErrorMessage = " ")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Last Name field is required.")]
        [Required(ErrorMessage = " ")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Email field is required.")]
        [Required(ErrorMessage = " ")]
        [EmailAddress(ErrorMessage = "Email is incorrect.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Birth day is incorrect.")]
        [BirthdateValidation(ErrorMessage = "Birth Day cannot be bigger than Today's date")]

        public DateTime BirthDate { get; set; }

        public string BirthDateDay { get; set; }
        public string BirthDateMonth { get; set; }
        public string BirthDateYear { get; set; }


        [Display(Name = "Image Url")]
        [Required(ErrorMessage = "Please upload profile image")]
        public string ImageUrl { get; set; }


        [Required(ErrorMessage = "Gender field is required.")]
        public string Gender { get; set; }


        [Display(Name = "Address")]
        //[Required(ErrorMessage = "Address field is required.")]
        [Required(ErrorMessage = " ")]
        public string Address { get; set; }
    }

    public class ProfileViewModel
    {
        //[Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

  
        [Display(Name = "Image Url")]
        public new string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Interests")]
        public virtual ICollection<Interest> Interests { get; set; }

        public string BirthDate { get; set; }

        [Display(Name = "Events")]
        public virtual ICollection<Event> Events { get; set; }
        public string Address { get; set; }
        public string InterestsToDisplay { get; set; }
    }
}
