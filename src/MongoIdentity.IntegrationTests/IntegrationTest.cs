using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using MongoIdentity.TestHelpers;
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
            var provider = new DpapiDataProtectionProvider("identity-test");
            UserManager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(provider.Create("EmailConfirm"));

            RoleManager = new RoleManager<IdentityRole>(RoleStore);

        }

        protected IdentityUser CreateBasicUser()
        {
            var user = IdentityUserMother.BasicUser();
            UserManager.Create(user);
            return user;
        }
    }

    public class TestMongoContext : MongoContext<IdentityUser, IdentityRole>
    {
        
    }
}
