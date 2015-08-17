using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace MongoIdentity.UnitTests
{
    public class IdentityRoleTests
    {
        [Test]
        public void ShouldGenerateNewId()
        {
            var role = new IdentityRole() {Name = "Role Name"};

            role.Id.ShouldNotBeNullOrEmpty();
        }

       
    }
}
