using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NUnit.Framework;

namespace MongoIdentity.IntegrationTests
{
    public abstract class IntegrationTest
    {
        protected IMongoContext<IdentityUser, IdentityRole> Context;
        protected UserManager<IdentityUser> UserManager;
        protected RoleManager<IdentityRole> RoleManager;

        protected IUserStore<IdentityUser> UserStore;
        protected IRoleStore<IdentityRole> RoleStore;


        [SetUp]
        public void Setup()
        {
            Context = new TestMongoContext();
            Task.WaitAll(new Task[]
            {Context.Database.DropCollectionAsync("users"), Context.Database.DropCollectionAsync("roles")});

            UserStore = new UserStore<IdentityUser,IdentityRole>(Context);
            RoleStore = new RoleStore<IdentityRole,IdentityUser>(Context);
            UserManager = new UserManager<IdentityUser>(UserStore);
            RoleManager = new RoleManager<IdentityRole>(RoleStore);

        }
    }

    public class TestMongoContext : MongoContext<IdentityUser, IdentityRole>
    {
        
    }
}
