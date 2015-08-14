using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using Shouldly;

namespace MongoIdentity.IntegrationTests
{
    public class UserLookupTests: IntegrationTest
    {
        [Test]
        public void ShouldBeAbleToGetUserByName()
        {
            var user = new IdentityUser() { UserName = "test" };

            UserManager.Create(user);

            var found = UserManager.FindByName("test");

            found.Id.ShouldBe(user.Id);
        }

        [Test]
        public void ShouldBeAbleToGetUserByEmail()
        {
            var user = new IdentityUser() { UserName = "test", EmailAddress = "test@testing.com"};

            UserManager.Create(user);

            var found = UserManager.FindByEmail("test@testing.com");

            found.Id.ShouldBe(user.Id);
        }

        [Test]
        public void ShouldBeAbleToGetUserById()
        {
            var user = new IdentityUser() { UserName = "test", EmailAddress = "test@testing.com" };

            UserManager.Create(user);

            var found = UserManager.FindById(user.Id);

            found.Id.ShouldBe(user.Id);
        }
    }
}
