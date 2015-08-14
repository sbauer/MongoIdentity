using System.Security.Claims;

namespace MongoIdentity
{
    public class IdentityUserClaim
    {
        public IdentityUserClaim()
        {
            
        }

        public IdentityUserClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
    }
}