using Acubec.Solutions.Core;
using Acubec.Solutions.Utilities;
using Opus.Victus.MMS.Interface.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opus.Victus.MMS.Interface.Services
{
    public static class LoginService
    {
        public static async Task<LoginResponse> ValidateUser(this LoginRequest loginRequest)
        {
            try
            {
                loginRequest.CheckRequestId();
                var response = await APIHelper.HttpClientPostAsync<LoginResponse, LoginRequest>(loginRequest.RequestID.ToString()
                        , "SecurityFactory", "Validator/Validate", loginRequest);
                return response;
            }
            catch (AcubecException e)
            {
                ContextFactory.Current.Logger.ExceptionCaught(loginRequest.RequestID.ToString(), nameof(ValidateUser), $"exception with Code{e.ExceptionCode}", e);
                return new LoginResponse()
                {
                    ErrorCode = e.ExceptionCode.ToString(),
                    ErrorMessage = ErrorConstnts.ManageMessage(e.ExceptionCode.ToString()),
                };
            }
            catch (Exception ex)
            {
                ContextFactory.Current.Logger.ExceptionCaught(loginRequest.RequestID.ToString(), nameof(ValidateUser), $"exception with Code 396", ex);
                return new LoginResponse()
                {
                    ErrorCode = "396",
                    ErrorMessage = ErrorConstnts.ManageMessage(ErrorConstnts.GenericErrorCode)
                };
            }
        }

        public static async Task<LoginResponse> CreateUser(this LoginRequest loginRequest)
        {
            try
            {
                loginRequest.CheckRequestId();
                var response = await APIHelper.HttpClientPostAsync<LoginResponse, LoginRequest>(loginRequest.RequestID.ToString()
                        , "SecurityFactory", "Validator/Validate", loginRequest);
                return response;
            }
            catch (AcubecException e)
            {
                ContextFactory.Current.Logger.ExceptionCaught(loginRequest.RequestID.ToString(), nameof(ValidateUser), $"exception with Code{e.ExceptionCode}", e);
                return new LoginResponse()
                {
                    ErrorCode = e.ExceptionCode.ToString(),
                    ErrorMessage = ErrorConstnts.ManageMessage(e.ExceptionCode.ToString()),
                };
            }
            catch (Exception ex)
            {
                ContextFactory.Current.Logger.ExceptionCaught(loginRequest.RequestID.ToString(), nameof(ValidateUser), $"exception with Code 396", ex);
                return new LoginResponse()
                {
                    ErrorCode = "396",
                    ErrorMessage = ErrorConstnts.ManageMessage(ErrorConstnts.GenericErrorCode)
                };
            }
        }
    }
}
