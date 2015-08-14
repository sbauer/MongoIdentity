using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace MongoIdentity.TestHelpers
{
    public class IdentityUserMother
    {
        public static IdentityUser EmptyUser()
        {
            return new IdentityUser();
        }

        public static IdentityUser BasicUser(string username = "test", string email="test@testing.com")
        {
            return new IdentityUser() {EmailAddress = email, UserName = username};
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
