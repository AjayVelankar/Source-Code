
using System;
using System.Collections.Generic;
using System.Text;

namespace Opus.Victus.MMS.Interface.Exceptions
{
    public static class ErrorConstnts
    {
        public const string SucessCode = "00";
        public const string GenericErrorCode = "96";

        public static string ManageMessage(string errorCode)
        {
            return errorCode;
        }
    }
}
