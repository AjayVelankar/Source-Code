using Acubec.Solutions.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opus.Victus.MMS.Interface.Exceptions
{
    internal static class RequestExtentions
    {
        public static void CheckRequestId(this IRequestEntity request)
        {
            if(request.RequestID == default(Guid))
            {
                request.RequestID = Guid.NewGuid();
                ContextFactory.Current.Logger.WriteDebugEntry(request.RequestID.ToString(), () => $"RequestId Created:{request.RequestID}");
            }
        }
    }
}
