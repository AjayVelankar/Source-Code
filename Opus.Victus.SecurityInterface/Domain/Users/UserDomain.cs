using Acubec.Solutions.Core;
using Acubec.Solutions.Core.Domain;
using Acubec.Solutions.Utilities;
using Opus.Victus.MMS.Interface;
using Opus.Victus.MMS.Interface.APIs;
using Opus.Victus.SecurityInterface.Database;
using Opus.Victus.SecurityInterface.Domain.Users.Entities;
using Opus.Victus.SecurityInterface.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.SecurityInterface.Domain
{
    public class UserSearchOptions : IDomainSearchOptions
    {
        public Key<Guid> ID { get; set; }
        IKey IDomainSearchOptions.ID
        {
            get => ID;
            set => ID = (Key<Guid>)value;
        }
    }
    internal sealed class UserServiceProvider : BaseServiceProvider
    {
        Guid _key;
        public UserServiceProvider(Guid key)
        {
            _key = key;
        }
        internal UserEntity GetUser()
        {

            var entity = _dbContext.Users.Where(c => c.Id == _key.ToString()).FirstOrDefault();
            return _mapper.Map<UserEntity>(entity);
        }

        internal UserDataEntity Validate(string userName, string password)
        {
            //var t = new UserDataEntity()
            //{
            //    Name = "Ajay Velankar",
            //    EmailId = userName,
            //    Password = password,
            //    PhoneNumber = "8390434007",
            //    Id = Guid.NewGuid().ToString()
            //};
            
            return _dbContext.Users.Where(c => c.EmailId == userName && c.Password == password).FirstOrDefault();
        }

        internal void CreateUser(UserEntity userRequest)
        {
            var entity = _mapper.Map<UserDataEntity>(userRequest);
            _dbContext.Users.Add(entity);
            _dbContext.SaveChanges();
        }
    }
    internal sealed class UserDomain : Domain<Guid, UserEntity, UserServiceProvider>
    {
        IMapper _mapper;
        public UserDomain(Guid key, bool delay = false) 
            : base(key, delay)
        {
        }

        protected override UserEntity GetEntity()
        {
            _mapper = ContextFactory.Current.ResolveDependency<IMapper>();
            return this._serviceProvider.GetUser();
        }

        protected override UserServiceProvider GetServiceProvider()
        {
            return new UserServiceProvider(_key);
        }

        public void Validate(LoginRequest loginRequest)
        {
            var password =  ContextFactory.Current.ResolveDependency<IHSM>().Hashstring(loginRequest.Password, Constants.PasswordHash);
            var user =  this._serviceProvider.Validate(loginRequest.UserId, password);

            if (user == null)
                throw new InvalidUserException(loginRequest.RequestID.ToString());
            Guid id = Guid.Parse(user.Id);

            this._key = new Key<Guid>(id);
            this._serviceProvider = GetServiceProvider();
            this._entity = this.GetEntity();
        }

        internal UserRequest CreateUser(UserRequest userRequest)
        {
            var entity = _mapper.Map<UserEntity>(userRequest);
            var randomString = Guid.NewGuid().ToString().Substring(6);
            var password = ContextFactory.Current.ResolveDependency<IHSM>().Hashstring(randomString, Constants.PasswordHash);
            //_serviceProvider.CreateUser(userRequest);
            return userRequest;
        }
    }
}
