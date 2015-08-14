using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoIdentity
{
    public class IdentityUser: IUser<string>
    {
        public IdentityUser()
        {
            Id = ObjectId.GenerateNewId().ToString();
            Roles = new List<string>();
            Logins = new List<UserLoginInfo>();
            Claims = new List<IdentityUserClaim>();
        }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
        public string UserName { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        [BsonIgnoreIfNull]
        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        [BsonIgnoreIfNull]
        public virtual IList<string> Roles { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual int FailedLoginAttempts { get; set; }

        public virtual DateTime? LockedOutUntilUtc { get; set; }

        public virtual bool LockedOut { get; set; }

        public virtual bool IsInRole(string name)
        {
            return Roles.Contains(name);
        }

        public virtual bool AddRole(string name)
        {
            if (IsInRole(name))
                return false;

            Roles.Add(name);

            return true;
        }

        public virtual bool RemoveRole(string name)
        {
            if (!IsInRole(name))
                return false;

            Roles.Remove(name);

            return true;
        }

        [BsonIgnoreIfNull]
        public virtual IList<UserLoginInfo> Logins { get; set; }

        [BsonIgnoreIfNull]
        public virtual IList<IdentityUserClaim> Claims { get; set; } 

        public virtual bool AddLogin(UserLoginInfo info)
        {
            if (HasLogin(info))
                return false;

            Logins.Add(info);

            return true;
        }

        public virtual bool HasLogin(UserLoginInfo info)
        {
            return Logins.Any(x => x.LoginProvider == info.LoginProvider && x.ProviderKey == info.ProviderKey);
        }

        public virtual bool RemoveLogin(UserLoginInfo info)
        {
            if (!HasLogin(info))
                return false;

            var existing =
                Logins.FirstOrDefault(x => x.ProviderKey == info.ProviderKey && x.LoginProvider == info.LoginProvider);

            Logins.Remove(existing);

            return true;
        }

        public virtual bool HasClaim(Claim claim)
        {
            return Claims.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
        }

        public virtual bool AddClaim(Claim claim)
        {
            if (HasClaim(claim))
                return false;

            Claims.Add(new IdentityUserClaim(claim));
            return true;
        }

        public virtual bool RemoveClaim(Claim claim)
        {
            if (!HasClaim(claim))
                return false;

            var existing = Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            Claims.Remove(existing);

            return true;
        }

        public virtual int IncrementLoginFailureCount()
        {
            FailedLoginAttempts++;

            return FailedLoginAttempts;
        }

        public virtual int ResetLoginFailureCount()
        {
            FailedLoginAttempts = 0;

            return FailedLoginAttempts;
        }

    }
}
