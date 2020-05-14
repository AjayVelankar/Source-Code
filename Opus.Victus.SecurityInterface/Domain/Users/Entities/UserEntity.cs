using Acubec.Solutions.Core;
using Acubec.Solutions.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.SecurityInterface.Domain.Users.Entities
{
    internal sealed class UserEntity : IDomainEntity
    {
        Key<Guid> _guid;
        public UserEntity(Guid guid)
        {
            _guid = guid;
        }
        public IKey Key => _guid;
        public Guid Id => _guid;

        public string Name { get; set; }

        public string Password { get; set; }

        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }

        public bool Disabled { get; set; }
    }
}
