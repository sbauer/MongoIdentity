using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoIdentity
{
    public interface IMongoContext<TUser, TRole>
    {
        IMongoCollection<TUser> Users { get; }
        IMongoCollection<TRole> Roles { get; }

        IMongoDatabase Database { get; }
    }

    public abstract class MongoContext<TUser,TRole> : IMongoContext<TUser, TRole>
    {
        private string _connectionStringName;
        protected IMongoClient Client;
        public IMongoDatabase Database { get; private set; }

        protected MongoContext()
        {
            SetConnectionStringName();
            OpenSession();
        }
        protected MongoContext(string connectionStringName)
        {
            SetConnectionStringName(connectionStringName);
            OpenSession();
        }

        private void OpenSession()
        {
            var connectionInfo = ConfigurationManager.ConnectionStrings[_connectionStringName];

            if(connectionInfo == null)
                throw new ApplicationException(String.Format("{0} connection string name not found in configuration file.", _connectionStringName));

            if(string.IsNullOrEmpty(connectionInfo.ConnectionString))
                throw new ApplicationException(String.Format("{0} connection string cannot have any empty value.", _connectionStringName));

            var url = new MongoUrl(connectionInfo.ConnectionString);
            Client = new MongoClient(url);
            Database = Client.GetDatabase(url.DatabaseName);

            Users = Database.GetCollection<TUser>("users");
            Roles = Database.GetCollection<TRole>("roles");
        }


        public IMongoCollection<TUser> Users { get; private set; }

        public IMongoCollection<TRole> Roles { get; private set; }

        private void SetConnectionStringName(string connectionName = null)
        {
            _connectionStringName = string.IsNullOrEmpty(connectionName) ? GetType().Name : connectionName;
        }

    }
}
