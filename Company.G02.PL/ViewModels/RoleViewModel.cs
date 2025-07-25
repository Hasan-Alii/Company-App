using Microsoft.AspNetCore.Identity;

namespace Company.G02.PL.ViewModels
{
    public class RoleViewModel : IdentityRole
    {
        public string? Id { get; set; }
        public string RoleName { get; set; }
    }
}
