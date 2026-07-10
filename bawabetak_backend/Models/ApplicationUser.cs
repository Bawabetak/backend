
namespace bawabetak_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Photo { get; set; }
        public string Address { get; set; }
        public string NationalNumber { get; set; }
        public string IdentityDocument { get; set; }
        public Status Status { get; set; } = Status.Pending;
    }
}
