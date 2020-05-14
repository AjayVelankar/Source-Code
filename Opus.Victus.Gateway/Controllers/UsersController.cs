using Acubec.Solutions.Core;
using Microsoft.AspNetCore.Mvc;
using Opus.Victus.MMS.Interface;
using Opus.Victus.MMS.Interface.Exceptions;
using Opus.Victus.MMS.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.Gateway.Controllers
{
    [Route("Users")]
    public class UsersController : RootController
    {
        [HttpPost("login")]
        public async Task<LoginResponse> Post([FromBody]LoginRequest request)
        {
            try
            {
                ContextFactory.Current.Logger.StartMethodWithRequest<LoginRequest>(request.RequestID.ToString(), nameof(Post), request);
                return await request.ValidateUser();
            }
            catch (AcubecException e)
            {
                ContextFactory.Current.Logger.ExceptionCaught(request.RequestID.ToString(), nameof(Post), $"exception with Code{e.ExceptionCode}", e);
                return new LoginResponse()
                {
                    ErrorCode = e.ExceptionCode.ToString(),
                    ErrorMessage = ErrorConstnts.ManageMessage(e.ExceptionCode.ToString())
                };
            }
            catch (Exception ex)
            {
                ContextFactory.Current.Logger.ExceptionCaught(request.RequestID.ToString(), nameof(Post), "Generic exception", ex);
                return new LoginResponse()
                {
                    ErrorCode = ErrorConstnts.GenericErrorCode.ToString(),
                    ErrorMessage = ErrorConstnts.ManageMessage(ErrorConstnts.GenericErrorCode)
                };
            }
        }
    }
}
