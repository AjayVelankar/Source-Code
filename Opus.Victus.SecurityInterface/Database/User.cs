using Acubec.Solutions.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.SecurityInterface.Database
{
    [Table("Users")]
    public class UserDataEntity : IDataEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public bool PasswordChangeRequired { get; set; }

        public bool Disabled { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string CreatedDateBy { get; set; }
        public string LastUpdatedy { get; set; }
    }
}
