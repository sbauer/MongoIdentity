using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MongoIdentity.UnitTests
{
    public class ClaimMother
    {
        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value);
        }
    }
}
