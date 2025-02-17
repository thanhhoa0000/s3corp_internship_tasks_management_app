namespace TaskManagementApp.Frontends.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter your username")]
        [MinLength(6, ErrorMessage = "Username must be at least 6 characters")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "Enter your passwoord")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public required string Password { get; set; }
        [Phone(ErrorMessage = "Invalid phone number format"), MaxLength(12)]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Enter your first name")]
        [MinLength(2, ErrorMessage = "Must be at least 2 characters"), MaxLength(30)]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Enter your last name")]
        [MinLength(2, ErrorMessage = "Must be at least 2 characters"), MaxLength(50)]
        public string? LastName { get; set; }
        public Role Role { get; set; } = Role.Normal;
    }
}
