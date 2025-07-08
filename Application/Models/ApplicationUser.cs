using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace Application.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; }
        public string CustomerCode { get; set; }
        public string Designation { get; set; }
        public string Country { get; set; }
        public string AuthCode { get; set; }

        // public DateTime DOB { get; set; }
        public string Pin { get; set; }
        public string Number { get; set; }
        public string PhotoUrl { get; set; }
    }

}
