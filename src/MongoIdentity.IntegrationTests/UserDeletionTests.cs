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
    public class UserDeletionTests: IntegrationTest
    {
        [Test]
        public void CanDeleteExistingUser()
        {
            var user = new IdentityUser() {UserName = "test", Email = "test@test.com"};

            var createResult = UserManager.Create(user);
            createResult.Succeeded.ShouldBe(true);

            var result = UserManager.Delete(user);

            result.Succeeded.ShouldBe(true);

            var taskResult = Task.FromResult(Context.Users.Find(x => x.Id == user.Id).FirstOrDefaultAsync());

            taskResult.Result.Result.ShouldBe(null);

        }

    }
}
