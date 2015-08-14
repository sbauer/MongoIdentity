using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoIdentity.Exceptions
{
    public class ConnectionStringNotFoundException:Exception
    {
        public ConnectionStringNotFoundException(string name):base(string.Format("Connection String '{0}' not found in configuration file.",name))
        {
            
        }
    }
}
