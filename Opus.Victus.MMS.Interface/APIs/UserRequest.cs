using Acubec.Solutions.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opus.Victus.MMS.Interface.APIs
{
    public class UserRequest : IRequestEntity
    {
        public Guid RequestID { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }

        public bool Disabled { get; set; }

    }

    public class UserResponse : IResponse
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string RequestID { get; set; }
        public ResponseTypes ResponseType { get; set; }
    }
}
