using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Security.Policy;
using System.Text.Json;
using VirtualWallet.Business.Exceptions;

namespace VirtualWallet.Web.Helper
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                bool isApiRequest = context.Request.Path.StartsWithSegments("/api");

                if (isApiRequest)
                {
                    //TODO да логвам ексепшъна в някакъв файл
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    //var developerErrorMessage = error.Message; логване на грешката
                    
                }
                else
                {
                    switch (error)
                    {
                        // custom application error
                        case DuplicateEntityException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case EmailAlreadyExistException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case EntityAlreadyAdminException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        default:
                            // unhandled error
                            //TODO да логвам ексепшъна в някакъв файл
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            var homeErrorUrl = context.Request.PathBase + "/Home/Error";
                            context.Response.Redirect(homeErrorUrl);
                            return;
                    }
                }

                var result = JsonSerializer.Serialize(new { message = "An error occurred. Please try again later." });
                await response.WriteAsync(result);
            }
        }
    }
}
