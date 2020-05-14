using Acubec.Solutions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.MMS.Interface
{
    public class LoginRequest : IRequestEntity
    {
        public Guid RequestID { get; set; }

        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse :IResponse
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string Token { get; set; }

        public string RequestID { get; set; }
        public ResponseTypes ResponseType { get; set; }
    }
}
