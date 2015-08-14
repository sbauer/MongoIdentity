using System;

namespace MongoIdentity.Exceptions
{
    public class EmptyConnectionStringException : Exception
    {
        public EmptyConnectionStringException(string name) : base(string.Format("Connection String '{0}' cannot be empty.", name))
        {

        }
    }
}