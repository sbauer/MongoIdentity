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
    public class RoleTests: IntegrationTest
    {
        [Test]
        public void CanCreateRole()
        {
            var role = new IdentityRole() {Name = "Test"};

            var result = RoleManager.Create(role);
            result.Succeeded.ShouldBe(true);

            var searchResult = Task.FromResult(Context.Roles.Find(x => x.Id == role.Id).FirstOrDefaultAsync());

            searchResult.Result.Result.Id.ShouldBe(role.Id);
        }

        [Test]
        public void CanUpdateRole()
        {
            var role = CreateBasicRole();

            role.Name = "New Name";

            var result = RoleManager.Update(role);
            result.Succeeded.ShouldBe(true);

            var searchResult = Task.FromResult(Context.Roles.Find(x => x.Id == role.Id).FirstOrDefaultAsync());

            searchResult.Result.Result.Name.ShouldBe(role.Name);
        }

        [Test]
        public void CanDeleteRole()
        {
            var role = CreateBasicRole();

            var result = RoleManager.Delete(role);

            result.Succeeded.ShouldBe(true);

            var searchResult = Task.FromResult(Context.Roles.Find(x => x.Id == role.Id).FirstOrDefaultAsync());

            searchResult.Result.Result.ShouldBe(null);
        }

        [Test]
        public void CanFindById()
        {
            var role = CreateBasicRole();

            var findResult = RoleManager.FindById(role.Id);

            findResult.Id.ShouldBe(role.Id);
            findResult.Name.ShouldBe(role.Name);
        }

        [Test]
        public void CanFindByName()
        {
            var role = CreateBasicRole();

            var findResult = RoleManager.FindByName(role.Name);

            findResult.Id.ShouldBe(role.Id);
            findResult.Name.ShouldBe(role.Name);
        }
    }
}
