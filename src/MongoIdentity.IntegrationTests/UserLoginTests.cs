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
    public class UserLoginTests: IntegrationTest
    {
        protected static UserLoginInfo BasicLogin = new UserLoginInfo("Facebook","111");

        [Test]
        public void CanAddLogin()
        {
            var user = CreateBasicUser();
            var result = UserManager.AddLogin(user.Id, BasicLogin);

            result.Succeeded.ShouldBe(true);

            var existingUser = UserManager.FindById(user.Id);

            existingUser.HasLogin(BasicLogin).ShouldBe(true);
        }

        [Test]
        public void CannotAddDuplicateLogin()
        {
            var user = CreateBasicUser();
            var result = UserManager.AddLogin(user.Id, BasicLogin);
            var duplicateResult = UserManager.AddLogin(user.Id, BasicLogin);

            duplicateResult.Succeeded.ShouldBe(false);
        }

        [Test]
        public void CanGetLogins()
        {
            var user = CreateBasicUser();
            UserManager.AddLogin(user.Id, BasicLogin);

            var logins = UserManager.GetLogins(user.Id);

            logins.Count.ShouldBe(1);
            logins[0].LoginProvider.ShouldBe(BasicLogin.LoginProvider);
            logins[0].ProviderKey.ShouldBe(BasicLogin.ProviderKey);

        }

        [Test]
        public void CanDeleteLogins()
        {
            var user = CreateBasicUser();
            UserManager.AddLogin(user.Id, BasicLogin);

            var result = UserManager.RemoveLogin(user.Id, BasicLogin);

            result.Succeeded.ShouldBe(true);

            UserManager.GetLogins(user.Id).Count.ShouldBe(0);
        }
    }
}
