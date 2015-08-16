using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using Shouldly;

namespace MongoIdentity.IntegrationTests
{
    public class UserClaimTests: IntegrationTest
    {
        private readonly Claim _basicClaim = new Claim("test","testing");
        [Test]
        public void CanAddClaim()
        {
            var user = CreateBasicUser();

            UserManager.AddClaim(user.Id, _basicClaim);

            user = UserManager.FindById(user.Id);

            user.HasClaim(_basicClaim).ShouldBe(true);


        }

        [Test]
        public void CanGetClaims()
        {
            var user = CreateBasicUser();

            UserManager.AddClaim(user.Id, _basicClaim);

            var claims = UserManager.GetClaims(user.Id);

            claims.Count.ShouldBe(1);
            claims[0].Type.ShouldBe("test");
            claims[0].Value.ShouldBe("testing");
        }

        [Test]
        public void CanDeleteClaim()
        {
            var user = CreateBasicUser();

            UserManager.AddClaim(user.Id, _basicClaim);

            var claims = UserManager.GetClaims(user.Id);

            claims.Count.ShouldBe(1);

            UserManager.RemoveClaim(user.Id, _basicClaim);

            UserManager.GetClaims(user.Id).Count.ShouldBe(0);
        }
    }
}
