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
    public class UserRoleTests: IntegrationTest
    {
        [Test]
        public void CanAddRoleToUser()
        {
            var user = CreateBasicUser();
            var result = UserManager.AddToRole(user.Id, "test");

            var existing = UserManager.FindById(user.Id);

            result.Succeeded.ShouldBe(true);
            existing.Roles.ShouldContain("test");
        }

        [Test]
        public void CanAddRolesToUser()
        {
            var user = CreateBasicUser();
            var result = UserManager.AddToRoles(user.Id, "test","test2");

            var existing = UserManager.FindById(user.Id);

            result.Succeeded.ShouldBe(true);
            existing.Roles.ShouldContain("test");
            existing.Roles.ShouldContain("test2");

        }

        [Test]
        public void CanVerifyInRole()
        {
            var user = CreateBasicUser();

            UserManager.AddToRoles(user.Id, "test");

            UserManager.IsInRole(user.Id,"test").ShouldBe(true);
        }

        [Test]
        public void CanGetRoles()
        {
            var user = CreateBasicUser();

            UserManager.AddToRoles(user.Id, "test");

            var roles = UserManager.GetRoles(user.Id);

            roles.Count.ShouldBe(1);
            roles.ShouldContain("test");
        }

        [Test]
        public void CanRemoveRoles()
        {
            var user = CreateBasicUser();

            UserManager.AddToRoles(user.Id, "test");

            UserManager.RemoveFromRole(user.Id, "test");

            UserManager.IsInRole(user.Id,"test").ShouldBe(false);
        }

        [Test]
        public void CanRemoveManyRoles()
        {
            var user = CreateBasicUser();

            UserManager.AddToRoles(user.Id, "test","test2");

            UserManager.RemoveFromRoles(user.Id, "test","test2");

            UserManager.IsInRole(user.Id, "test").ShouldBe(false);
            UserManager.IsInRole(user.Id, "test2").ShouldBe(false);

        }
    }
}
