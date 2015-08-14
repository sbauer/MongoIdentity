using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;

namespace MongoIdentity
{
    public class RoleStore<TRole, TUser>: IRoleStore<TRole>
        where TRole : IdentityRole
        where TUser : IdentityUser
    {
        private readonly IMongoContext<TUser, TRole> _context;

        public RoleStore(IMongoContext<TUser, TRole> context)
        {
            _context = context;
        }

        public void Dispose()
        {
            
        }

        public Task CreateAsync(TRole role)
        {
            return _context.Roles.InsertOneAsync(role);
        }

        public Task UpdateAsync(TRole role)
        {
            var filter = GetRoleFilter(role);

            return _context.Roles.ReplaceOneAsync(filter, role);
        }

        public Task DeleteAsync(TRole role)
        {
            var filter = GetRoleFilter(role);

            return _context.Roles.DeleteOneAsync(filter);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            return _context.Roles.Find(x => x.Id == roleId).FirstOrDefaultAsync();
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            return _context.Roles.Find(x => x.Name == roleName).FirstOrDefaultAsync();
        }

        private FilterDefinition<TRole> GetRoleFilter(TRole role)
        {
            return Builders<TRole>.Filter.Where(x => x.Id == role.Id);
        }
    }
}
