using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using Shouldly;

namespace MongoIdentity.UnitTests
{
    public class IdentityUserTests
    {
        [Test]
        public void IdShouldBeCreated()
        {
            var user = IdentityUserMother.EmptyUser();

            user.Id.ShouldNotBeEmpty();
        }

        [Test]
        public void CanAddNewRole()
        {
            var user = IdentityUserMother.EmptyUser();

            user.AddRole("test").ShouldBe(true);
            user.Roles.ShouldContain("test");
        }

        [Test]
        public void CannotAddDuplicateRole()
        {
            var user = IdentityUserMother.UserWithTestRole();

            user.AddRole("test").ShouldBe(false);
        }

        [Test]
        public void CanSeeIfUserIsInRole()
        {
            var user = IdentityUserMother.UserWithTestRole();

            user.IsInRole("test").ShouldBe(true);
        }

        [Test]
        public void CannotDeleteRoleIfNotInRole()
        {
            var user = IdentityUserMother.EmptyUser();

            user.RemoveRole("test").ShouldBe(false);
        }

        [Test]
        public void CanDeleteRole()
        {
            var user = IdentityUserMother.UserWithTestRole();

            user.RemoveRole("test").ShouldBe(true);
            user.IsInRole("test").ShouldBe(false);
        }

        [Test]
        public void CanGetRoles()
        {
            var user = IdentityUserMother.UserWithTestRole();

            user.Roles.ShouldNotBeEmpty();
            user.Roles.Count.ShouldBe(1);
        }

        [Test]
        public void CanAddClaim()
        {
            var claim = ClaimMother.CreateClaim("testing", "test");
            var user = IdentityUserMother.EmptyUser();

            user.AddClaim(claim).ShouldBe(true);
            user.Claims.ShouldContain(x=>x.ClaimType == "testing" && x.ClaimValue == "test");
        }

        [Test]
        public void CannotAddDuplicateClaim()
        {
            var claim = ClaimMother.CreateClaim("testing", "test");
            var user = IdentityUserMother.UserWithClaimAndRole(claim);

            user.AddClaim(claim).ShouldBe(false);
        }

        [Test]
        public void CanCheckIfUserHasClaim()
        {
            var claim = ClaimMother.CreateClaim("testing", "test");
            var user = IdentityUserMother.UserWithClaimAndRole(claim);

            user.HasClaim(claim).ShouldBe(true);
        }

        [Test]
        public void CanGetClaims()
        {
            var claim = ClaimMother.CreateClaim("testing", "test");
            var user = IdentityUserMother.UserWithClaimAndRole(claim);

            user.Claims.ShouldNotBeEmpty();
            user.Claims.Count.ShouldBe(1);
        }

        [Test]
        public void CanRemoveClaim()
        {
            var claim = ClaimMother.CreateClaim("testing", "test");
            var user = IdentityUserMother.UserWithClaimAndRole(claim);

            user.RemoveClaim(claim).ShouldBe(true);
            user.Claims.ShouldBeEmpty();
        }

        [Test]
        public void CannotRemoveClaimIfNotAssigned()
        {
            var claim = ClaimMother.CreateClaim("testing", "test");
            var anotherClaim = ClaimMother.CreateClaim("testing1", "test1");

            var user = IdentityUserMother.UserWithClaimAndRole(claim);

            user.RemoveClaim(anotherClaim).ShouldBe(false);
        }

        [Test]
        public void CanAddLogin()
        {
            var user = IdentityUserMother.EmptyUser();
            var login = new UserLoginInfo("test", "testing");

            user.AddLogin(login).ShouldBe(true);
            user.Logins.ShouldContain(login);
        }

        [Test]
        public void CannotAddDuplicateLogins()
        {
            var user = IdentityUserMother.EmptyUser();
            var login = new UserLoginInfo("test", "testing");

            user.AddLogin(login).ShouldBe(true);
            user.AddLogin(login).ShouldBe(false);
        }

        [Test]
        public void CanCheckIfLoginExists()
        {
            var user = IdentityUserMother.EmptyUser();
            var login = new UserLoginInfo("test", "testing");

            user.AddLogin(login).ShouldBe(true);
            user.HasLogin(login).ShouldBe(true);
        }

        [Test]
        public void CanGetLogins()
        {
            var user = IdentityUserMother.UserWithLogin();

            user.Logins.ShouldNotBeEmpty();
        }

        [Test]
        public void CanRemoveLogin()
        {
            var user = IdentityUserMother.UserWithLogin();
            var login = new UserLoginInfo("test", "testing");

            user.RemoveLogin(login).ShouldBe(true);
            user.Logins.ShouldBeEmpty();
        }

        [Test]
        public void CannotRemoveLoginIfItDoesntExist()
        {
            var user = IdentityUserMother.UserWithLogin();
            var login = new UserLoginInfo("test1", "testing1");

            user.RemoveLogin(login).ShouldBe(false);
        }

        [Test]
        public void IncrementFailureCountShouldIncreaseFailureTotal()
        {
            var user = IdentityUserMother.UserWithLogin();

            user.FailedLoginAttempts.ShouldBe(0);

            user.IncrementLoginFailureCount();

            user.FailedLoginAttempts.ShouldBe(1);
        }

        [Test]
        public void ResetLoginFailuresShouldSetCountToZero()
        {
            var user = IdentityUserMother.UserWithLogin();
            user.IncrementLoginFailureCount();
            user.ResetLoginFailureCount();

            user.FailedLoginAttempts.ShouldBe(0);
        }
    }
}
