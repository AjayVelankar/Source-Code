using Acubec.Solutions.Core;
using MongoFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.SecurityInterface.Database
{
    public class SecurityDataContext : MongoDbContext, IDataContext
    {
        public SecurityDataContext(IMongoDbConnection connection) : base(connection)
        {
        }

        public MongoDbSet<UserDataEntity> Users { get; set; }
    }
}
