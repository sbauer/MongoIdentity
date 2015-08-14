using Microsoft.AspNet.Identity;
using MongoDB.Bson;

namespace MongoIdentity
{
    public class IdentityRole : IRole<string>
    {
        public IdentityRole()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}