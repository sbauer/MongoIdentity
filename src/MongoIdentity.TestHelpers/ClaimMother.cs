using System.Security.Claims;

namespace MongoIdentity.TestHelpers
{
    public class ClaimMother
    {
        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value);
        }
    }
}
