﻿using Microsoft.Extensions.Logging;
using SilentApp.Services.Contracts;

namespace SilentApp.FunctionsApp.Functions
{
    public abstract class FunctionBase
    {

        protected static void LogError(ILogger log, string functionName, Error error)
        {
            log.LogError(@"Error occurred during execution of {function}:
Error type: {ErrorType}
Error code: {ErrorCode}
Message: {ErrorMessage}"
                ,functionName, error.Type, error.Code, error.Message);
        }
    }
}
