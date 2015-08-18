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
    public class UserEmailTests: IntegrationTest
    {
        [Test]
        public void CanGetUserEmail()
        {
            var user = IdentityUserMother.BasicUser();
            UserManager.Create(user);

            var email = UserManager.GetEmail(user.Id);

            email.ShouldBe(user.Email);
        }

        [Test]
        public void CanSetUnconfirmedEmail()
        {
            var user = IdentityUserMother.BasicUser();
            UserManager.Create(user);

            var result = UserManager.SetEmail(user.Id,"new@testing.com");

            var newUser = UserManager.FindById(user.Id);

            result.Succeeded.ShouldBe(true);
            newUser.Email.ShouldBe("new@testing.com");
            newUser.EmailConfirmed.ShouldBe(false);
        }

        [Test]
        public void CanConfirmEmailAddress()
        {
            var user = IdentityUserMother.BasicUser();
            UserManager.Create(user);

            var token = UserManager.GenerateEmailConfirmationToken(user.Id);

            token.ShouldNotBeEmpty();

            var result = UserManager.ConfirmEmail(user.Id, token);

            result.Succeeded.ShouldBe(true);

            var newUser = UserManager.FindById(user.Id);

            newUser.EmailConfirmed.ShouldBe(true);
        }

        [Test]
        public void CanGetEmailConfirmed()
        {
            var user = IdentityUserMother.BasicUser();
            UserManager.Create(user);

            var token = UserManager.GenerateEmailConfirmationToken(user.Id);
            var result = UserManager.ConfirmEmail(user.Id, token);

            result.Succeeded.ShouldBe(true);

            UserManager.IsEmailConfirmed(user.Id).ShouldBe(true);
        }
    }
}
