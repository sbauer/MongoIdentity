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
    public class UserLockoutTests: IntegrationTest
    {
        [Test]
        public void CanSetAccountLocked()
        {
            DateTime lockoutTime = DateTime.UtcNow.AddDays(1);
            var user = CreateBasicUser();

            var result = UserManager.SetLockoutEnabled(user.Id, true);

            var newUser = UserManager.FindById(user.Id);

            result.Succeeded.ShouldBe(true);
            newUser.LockedOut.ShouldBe(true);
        }

        [Test]
        public void CanUnlockAccount()
        {
            DateTime lockoutTime = DateTime.UtcNow.AddDays(1);
            var user = CreateBasicUser();

            var result = UserManager.SetLockoutEnabled(user.Id, true);
            result.Succeeded.ShouldBe(true);

            var unlockResult = UserManager.SetLockoutEnabled(user.Id, false);

            var newUser = UserManager.FindById(user.Id);

            unlockResult.Succeeded.ShouldBe(true);
            newUser.LockedOut.ShouldBe(false);
        }

        [Test]
        public void CanSetLockoutTime()
        {
            DateTime lockoutTime = DateTime.UtcNow.AddDays(1);
            var user = CreateBasicUser();

            UserManager.SetLockoutEnabled(user.Id, true);

            var result = UserManager.SetLockoutEndDate(user.Id, lockoutTime);

            var newUser = UserManager.FindById(user.Id);

            result.Succeeded.ShouldBe(true);
            newUser.LockedOutUntilUtc.ShouldNotBe(null);
        }

        [Test]
        public void CanGetLockoutTime()
        {
            DateTime lockoutTime = DateTime.UtcNow.AddDays(1);
            var user = CreateBasicUser();

            UserManager.SetLockoutEnabled(user.Id, true);

            UserManager.SetLockoutEndDate(user.Id, lockoutTime);
            UserManager.GetLockoutEndDate(user.Id).Date.ShouldBe(lockoutTime.Date);
            
        }

        [Test]
        public void CanGetFailedAttempts()
        {
            var user = CreateBasicUser();
            user.IncrementLoginFailureCount();

            UserManager.Update(user);

            UserManager.GetAccessFailedCount(user.Id).ShouldBe(1);
        }

        [Test]
        public void CanResetFailedAttempts()
        {
            var user = CreateBasicUser();
            user.IncrementLoginFailureCount();

            UserManager.Update(user);

            UserManager.ResetAccessFailedCount(user.Id);

            user = UserManager.FindById(user.Id);
            user.FailedLoginAttempts.ShouldBe(0);
        }

        [Test]
        public void CanIncrementFailedAttempts()
        {
            UserManager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var user = CreateBasicUser();

            var result = UserManager.AccessFailed(user.Id);

            result.Succeeded.ShouldBe(true);
            UserManager.GetAccessFailedCount(user.Id).ShouldBe(1);
           

        }

    }
}
