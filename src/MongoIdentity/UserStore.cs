using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;

namespace MongoIdentity
{
    public class UserStore<TUser, TRole> : IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserRoleStore<TUser>,
        IUserLoginStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserEmailStore<TUser>,
        IUserClaimStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserTwoFactorStore<TUser, string>,
        IUserLockoutStore<TUser, string>
        where TUser : IdentityUser
        where TRole : IdentityRole
    {
        private readonly IMongoContext<TUser, TRole> _context;

        public UserStore(IMongoContext<TUser,TRole> context)
        {
            _context = context;
        }

        public void Dispose()
        {

        }

        public Task CreateAsync(TUser user)
        {
            EnsureUser(user);

            return _context.Users.InsertOneAsync(user);
        }

        public Task UpdateAsync(TUser user)
        {
            EnsureUser(user);

            var filter = GetUserFilter(user);
            return _context.Users.ReplaceOneAsync(filter, user);
        }

        public Task DeleteAsync(TUser user)
        {
            EnsureUser(user);

            var filter = GetUserFilter(user);
            return _context.Users.DeleteOneAsync(filter);
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            return _context.Users.Find(x => x.Id == userId).FirstOrDefaultAsync();
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return _context.Users.Find(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            EnsureUser(user);

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            EnsureUser(user);

            return Task.FromResult(user.AddRole(roleName));
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            EnsureUser(user);

            return Task.FromResult(user.RemoveRole(roleName));
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.Roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            EnsureUser(user);

            return Task.FromResult(user.IsInRole(roleName));
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            EnsureUser(user);

            return Task.FromResult(user.AddLogin(login));
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            EnsureUser(user);

            return Task.FromResult(user.RemoveLogin(login));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.Logins);
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            return
                _context.Users.Find(
                    x => x.Logins.Any(y => y.ProviderKey == login.ProviderKey && y.LoginProvider == login.LoginProvider))
                    .FirstOrDefaultAsync();
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            EnsureUser(user);

            user.SecurityStamp = stamp;

            return Task.FromResult(true);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            EnsureUser(user);

            user.EmailAddress = email;

            return Task.FromResult(true);
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.EmailAddress);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            EnsureUser(user);

            user.EmailConfirmed = confirmed;

            return Task.FromResult(true);
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            return _context.Users.Find(x => x.EmailAddress.ToLower() == email.ToLower()).FirstOrDefaultAsync();
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult((IList<Claim>) user.Claims.Select(x => x.ToClaim()).ToList());
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            EnsureUser(user);

            return Task.FromResult(user.AddClaim(claim));
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            EnsureUser(user);

            return Task.FromResult(user.RemoveClaim(claim));
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            EnsureUser(user);

            user.PhoneNumber = phoneNumber;

            return Task.FromResult(true);
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            EnsureUser(user);

            user.PhoneNumberConfirmed = confirmed;

            return Task.FromResult(true);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            EnsureUser(user);

            user.TwoFactorEnabled = enabled;

            return Task.FromResult(true);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.LockedOutUntilUtc ?? new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            EnsureUser(user);

            user.LockedOutUntilUtc = new DateTime(lockoutEnd.Ticks, DateTimeKind.Utc);

            return Task.FromResult(true);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.IncrementLoginFailureCount());
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.ResetLoginFailureCount());
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.FailedLoginAttempts);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            EnsureUser(user);

            return Task.FromResult(user.LockedOut);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            EnsureUser(user);

            user.LockedOut = enabled;

            return Task.FromResult(true);
        }

        private void EnsureUser(TUser user)
        {
            if(user == null)
                throw new ArgumentNullException("user");
        }

        private static FilterDefinition<TUser> GetUserFilter(TUser user)
        {
            return Builders<TUser>.Filter.Where(x => x.Id == user.Id);
        }

    }
}
