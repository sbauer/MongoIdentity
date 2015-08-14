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
    public class UserCreationTests: IntegrationTest
    {
        [Test]
        public void CanCreateNewUser()
        {
            var userToCreate = new IdentityUser() {UserName = "test"};
            UserManager.Create(userToCreate);

            var result =
                Task.FromResult(
                    Context.Users.Find(x => x.UserName == userToCreate.UserName).FirstOrDefaultAsync());

            
            result.Result.Result.Id.ShouldBe(userToCreate.Id);
        }

        [Test]
        public void CannotCreateDuplicateNewUser()
        {
            var userToCreate = new IdentityUser() { UserName = "test" };
            var userToCreate2 = new IdentityUser() { UserName = "test" };

            UserManager.Create(userToCreate);
            var result = UserManager.Create(userToCreate2);

            result.Succeeded.ShouldBe(false);
            result.Errors.ShouldNotBeEmpty();
        }

        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotPassNullUsers()
        {
            UserManager.Create(null);
        }
    }
}
