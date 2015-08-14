using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoIdentity.TestHelpers;
using NUnit.Framework;
using Shouldly;

namespace MongoIdentity.IntegrationTests
{
    public class UserPhoneTests: IntegrationTest
    {
        [Test]
        public void HasPhoneConfirmedShouldBeFalseIfNoPhone()
        {
            var user = new IdentityUser() {UserName = "test", EmailAddress = "test@test.com"};

            UserManager.Create(user);

            var result = UserManager.IsPhoneNumberConfirmed(user.Id);
            result.ShouldBe(false);
        }

        [Test]
        public void CanSetPhoneNumber()
        {
            var user = new IdentityUser() { UserName = "test", EmailAddress = "test@test.com" };

            UserManager.Create(user);

            var result = UserManager.SetPhoneNumber(user.Id, "444-444-4444");

            result.Succeeded.ShouldBe(true);

            var found = UserManager.FindById(user.Id);

            found.PhoneNumber.ShouldBe("444-444-4444");
        }

        [Test]
        public void CanVerifyPhoneNumber()
        {
            var user = new IdentityUser() { UserName = "test", EmailAddress = "test@test.com" };

            UserManager.Create(user);

            var token = UserManager.GenerateChangePhoneNumberToken(user.Id, "444-444-4444");

            var result = UserManager.ChangePhoneNumber(user.Id, "444-444-4444", token);

            result.Succeeded.ShouldBe(true);

            var found = UserManager.FindById(user.Id);

            found.PhoneNumber.ShouldBe("444-444-4444");
            found.PhoneNumberConfirmed.ShouldBe(true);
            UserManager.IsPhoneNumberConfirmed(user.Id).ShouldBe(true);
        }

        [Test]
        public void CanGetPhoneNumber()
        {
            var user = IdentityUserMother.BasicUser();
            user.PhoneNumber = "444-444-4444";

            UserManager.Create(user);

            UserManager.GetPhoneNumber(user.Id).ShouldBe(user.PhoneNumber);
        }
    }
}
