
namespace bawabetak_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; } = "Full Name";
        public string? Photo { get; set; }
        public string? Address { get; set; }
        public string? NationalNumber { get; set; }
        public string? IdentityDocument { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public bool IsEmailVerified { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public bool IsCompleteRegistration { get; set; } = false;
    }
}
