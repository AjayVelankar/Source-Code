using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acubec.Solutions.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opus.Victus.MMS.Interface;
using Opus.Victus.MMS.Interface.APIs;
using Opus.Victus.MMS.Interface.Exceptions;
using Opus.Victus.SecurityInterface.Domain;

namespace Opus.Victus.SecurityInterface.Controllers
{
    [Route("[controller]")]
    public class ValidatorController : ControllerBase
    {

        [HttpPost("Validate")]
        public LoginResponse Validate([FromBody]LoginRequest loginRequest)
        {
            try
            {

                var domain = new UserDomain(loginRequest.RequestID,true);
                domain.Validate(loginRequest);

                var parameter = new TokenParameter()
                {
                    SecretKey = ContextFactory.Current.ConfigManager["SecretKey"],
                    Audience = ContextFactory.Current.ConfigManager["Audience"],
                    Issuer = ContextFactory.Current.ConfigManager["Issuer"],
                    UserId = loginRequest.UserId,
                };
                parameter.Claims.Add(new System.Security.Claims.Claim("Name", domain.Entity.Name));
                parameter.Claims.Add(new System.Security.Claims.Claim("PhoneNumber", domain.Entity.PhoneNumber));
                parameter.Claims.Add(new System.Security.Claims.Claim("EmailId", domain.Entity.EmailId));

                var tokanizer = ContextFactory.Current.ResolveDependency<ITokanizer>();
                var token = tokanizer.CreateToken(parameter);
                return new LoginResponse()
                {
                    ErrorCode = ErrorConstnts.SucessCode , 
                    ErrorMessage = ErrorConstnts.ManageMessage(ErrorConstnts.SucessCode),
                    ResponseType = ResponseTypes.Success,
                    Token = token
                };
            }
            catch (AcubecException e)
            {
                ContextFactory.Current.Logger.ExceptionCaught(loginRequest.RequestID.ToString(), nameof(Validate), $"exception with Code{e.ExceptionCode}", e);
                return new LoginResponse()
                {
                    ErrorCode = e.ExceptionCode.ToString(),
                    ErrorMessage = ErrorConstnts.ManageMessage(e.ExceptionCode.ToString()),
                    ResponseType = ResponseTypes.BusinessError
                };
            }
            catch (Exception ex)
            {
                ContextFactory.Current.Logger.ExceptionCaught(loginRequest.RequestID.ToString(), nameof(Validate), $"exception with Code 396", ex);
                return new LoginResponse()
                {
                    ErrorCode = "396",
                    ErrorMessage = ErrorConstnts.ManageMessage(ErrorConstnts.GenericErrorCode),
                    ResponseType = ResponseTypes.GenericError

                };
            }
        }

        public UserResponse CreateUser([FromBody]UserRequest userRequest)
        {
            try
            {
                var domain = new UserDomain(userRequest.RequestID);
                var request = domain.CreateUser(userRequest);
                return new UserResponse()
                {
                    ErrorCode = ErrorConstnts.SucessCode,
                    ResponseType = ResponseTypes.SuccessMessage,
                }; 
            }
            catch (AcubecException e)
            {
                ContextFactory.Current.Logger.ExceptionCaught(userRequest.RequestID.ToString(), nameof(Validate), $"exception with Code{e.ExceptionCode}", e);
                return new UserResponse()
                {
                    ErrorCode = e.ExceptionCode.ToString(),
                    ResponseType = ResponseTypes.BusinessError,
                    ErrorMessage = ErrorConstnts.ManageMessage(e.ExceptionCode.ToString()),
                };
            }
            catch (Exception ex)
            {
                ContextFactory.Current.Logger.ExceptionCaught(userRequest.RequestID.ToString(), nameof(Validate), $"exception with Code 396", ex);
                return new UserResponse()
                {
                    ErrorCode = "396",
                    ResponseType = ResponseTypes.GenericError,
                    ErrorMessage = ErrorConstnts.ManageMessage(ErrorConstnts.GenericErrorCode)
                };
            }
        }
    }
}
