using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MongoIdentity.UnitTests
{
    public class IdentityUserMother
    {
        public static IdentityUser EmptyUser()
        {
            return new IdentityUser();
        }

        public static IdentityUser UserWithTestRole()
        {
            var user =  new IdentityUser();
            user.AddRole("test");

            return user;
        }

        public static IdentityUser UserWithLogin()
        {
            var user = new IdentityUser();
            user.AddLogin(new UserLoginInfo("test", "testing"));

            return user;
        }

        public static IdentityUser UserWithClaimAndRole(Claim claim, string role = "test")
        {
            var user = new IdentityUser();
            user.AddRole(role);
            user.AddClaim(claim);

            return user;
        }
    }
}
