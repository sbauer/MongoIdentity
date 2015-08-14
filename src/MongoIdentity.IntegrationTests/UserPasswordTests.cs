using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using NUnit.Framework;
using Shouldly;

namespace MongoIdentity.IntegrationTests
{
    public class UserPasswordTests: IntegrationTest
    {
        [Test]
        public void HasPasswordShouldBeFalseWithoutPassword()
        {
            var user = new IdentityUser() {UserName = "test"};

            UserManager.Create(user);

            var result = UserManager.HasPassword(user.Id);

            result.ShouldBe(false);
        }
        [Test]
        public void CanSetPassword()
        {
            var user = new IdentityUser() {UserName = "test"};

            UserManager.Create(user);
            UserManager.AddPassword(user.Id, "testing");

            var result = Task.FromResult(Context.Users.Find(x => x.UserName == user.UserName).FirstOrDefaultAsync());

            result.Result.Result.Id.ShouldBe(user.Id);
            result.Result.Result.PasswordHash.ShouldNotBeEmpty();
        }

        [Test]
        public void HasPasswordShouldBeTrueWithPassword()
        {
            var user = new IdentityUser() { UserName = "test" };

            UserManager.Create(user);
            UserManager.AddPassword(user.Id, "testing");

            var result = UserManager.HasPassword(user.Id);

            result.ShouldBe(true);
        }

        [Test]
        public void CanEnableTwoFactorAuthentication()
        {
            var user = CreateBasicUser();

            var result = UserManager.SetTwoFactorEnabled(user.Id, true);

            result.Succeeded.ShouldBe(true);

            var existing = UserManager.FindById(user.Id);

            existing.TwoFactorEnabled.ShouldBe(true);
        }

        [Test]
        public void CanDisableTwoFactorAuthentication()
        {
            var user = CreateBasicUser();

            UserManager.SetTwoFactorEnabled(user.Id, true);
            UserManager.SetTwoFactorEnabled(user.Id, false);

            var existing = UserManager.FindById(user.Id);

            existing.TwoFactorEnabled.ShouldBe(false);
        }

        [Test]
        public void CanGetTwoFactorStatus()
        {
            var user = CreateBasicUser();

            UserManager.SetTwoFactorEnabled(user.Id, true);

            UserManager.GetTwoFactorEnabled(user.Id).ShouldBe(true);
        }
    }
}
