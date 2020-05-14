using Acubec.Solutions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.SecurityInterface.Exceptions
{
    public class InvalidUserException : AcubecException
    {
        public InvalidUserException(string requestId) : base(requestId, 410)
        {

        }
    }
}
