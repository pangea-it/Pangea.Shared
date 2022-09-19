using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pangea.Shared.Exceptions;
using Pangea.Shared.Extensions.General;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;

namespace Pangea.Shared.Middlewares
{
    public class GlobalExceptionHandler
    {
        #region Class Members

        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        private ILogger _logger;

        #endregion

        #region Constructors

        public GlobalExceptionHandler
        (
            RequestDelegate next,
            ILogger<GlobalExceptionHandler> logger,
            ILoggerFactory loggerFactory
        )
        {
            _next = next;
            _loggerFactory = loggerFactory;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var controller = new StackTrace(ex)
                    .GetFrames()
                    .FirstOrDefault(fr => fr.GetMethod()?.DeclaringType?.ReflectedType?.BaseType == typeof(ControllerBase))?
                    .GetMethod()?
                    .DeclaringType?
                    .ReflectedType;

                if (controller != null) _logger = _loggerFactory.CreateLogger(controller);

                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ErrorModel response = exception switch
            {
                ValidationException => new ErrorModel
                {
                    Message = exception.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                },

                ResourceNotFoundException => new ErrorModel
                {
                    Message= exception.Message,
                    StatusCode = (int)HttpStatusCode.NotFound,
                },
                
                _ => new ErrorModel
                {
                    Message = "Something Unexpected occured",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                },
            };

            context.Response.StatusCode = response.StatusCode;

            return context.Response.WriteAsync(response.ToJson());
        }

        #endregion
    }

    internal class ErrorModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
