using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MongoIdentity.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace MongoIdentity.IntegrationTests
{
    public class MongoContextTests
    {
        [Test]
        public void ShouldBeAbleToCreateAValidContext()
        {
            var context = new TestMongoContext();

            context.Database.ShouldNotBe(null);
            context.Users.ShouldNotBe(null);
            context.Roles.ShouldNotBe(null);
        }

        [Test]
        [ExpectedException(typeof (ConnectionStringNotFoundException))]
        public void MissingConnectionStringElementShouldThrowException()
        {
            var context = new MissingConnectionStringContext();
        }

        [Test]
        [ExpectedException(typeof(EmptyConnectionStringException))]
        public void EmptyConnectionStringElementShouldThrowException()
        {
            var context = new EmptyMongoContext();
        }

        [Test]
        public void ShouldBeAbleToSpecifyCustomConnectionStringName()
        {
            var context = new CustomMongoContext();

            context.Database.ShouldNotBe(null);
            context.Users.ShouldNotBe(null);
            context.Roles.ShouldNotBe(null);
        }
    }

    public class MissingConnectionStringContext : MongoContext<IdentityUser, IdentityRole>
    {
        
    }

    public class EmptyMongoContext : MongoContext<IdentityUser, IdentityRole>
    {

    }


    public class CustomMongoContext : MongoContext<IdentityUser, IdentityRole>
    {
        public CustomMongoContext():base("TestMongoContext")
        {
            
        }
    }

}
