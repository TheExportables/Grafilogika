using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Grafilogika.Models {
    public class UsersContext : DbContext {
        public UsersContext()
            : base("DefaultConnection") {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Jelenlegi jelszó")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Új jelszó")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Új jelszó megerősítés")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel {
        [Required]
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [Display(Name = "Emlékezz rám!")]
        public bool RememberMe { get; set; }
    }

    public class ProfileModel
    {
        public IEnumerable<Grafilogika.Models.Games> games{ get; set; }
        public LocalPasswordModel pwChange { get; set; }
        public Users currentUser { get; set; }
    }

    public class RegisterModel {
        [Required]
        [Display(Name = "Felhasználónév")]
        [StringLength(100, ErrorMessage = "Maximum 100 karakter hosszú lehet!")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} legalább {2} karakter hosszú legyen.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Jelszó megerősítés")]
        [Compare("Password", ErrorMessage = "A két jelszó nem egyezik!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "E-mail cím")]
        [StringLength(100, ErrorMessage = "Maximum 100 karakter hosszú lehet!")]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ExternalLogin {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
