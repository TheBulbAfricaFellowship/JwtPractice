using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtPractice.Domain.Entities
{
    public class ApplicationUserRole : IdentityUserRole<long>
    {

    }



    //public static class ApplicationUserRole: IdentityUserRole<long>
    //{
    //    public const string Admin = "Admin";
    //    public const string Candidate = "Candidate";
    //}
}
