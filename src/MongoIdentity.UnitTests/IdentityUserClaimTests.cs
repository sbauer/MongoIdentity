using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace MongoIdentity.UnitTests
{
    public class IdentityUserClaimTests
    {
        private static readonly Claim BasicClaim = new Claim("My Claim","test");

        [Test]
        public void CanCreateUserClaim()
        {
            var claim = new IdentityUserClaim();
            claim.ShouldNotBe(null);
        }

        [Test]
        public void CanCreateUserClaimBasedOnSecurityClaim()
        {
            var claim = new IdentityUserClaim(BasicClaim);

            claim.ClaimType.ShouldBe(BasicClaim.Type);
            claim.ClaimValue.ShouldBe(BasicClaim.Value);
        }

        [Test]
        public void CanConvertToSecurityClaim()
        {
            var claim = new IdentityUserClaim(BasicClaim);

            var securityClaim = claim.ToClaim();

            securityClaim.Type.ShouldBe(claim.ClaimType);
            securityClaim.Value.ShouldBe(claim.ClaimValue);
        }
    }
}
