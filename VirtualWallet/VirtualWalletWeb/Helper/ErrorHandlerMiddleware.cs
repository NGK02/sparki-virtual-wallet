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
                    switch (error)
                    {//Не съм добавил AutoMapperMappingException и DbUpdateException като те се кечват като Exception и даваме Internal Serv Error
                        case EntityNotFoundException e:
                            response.StatusCode = (int) HttpStatusCode.NotFound; 
                            break;
                        case UnauthenticatedOperationException e:
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            break;
                        case UnauthorizedOperationException e:
                            response.StatusCode = (int)HttpStatusCode.Forbidden;
                            break;
                        case InsufficientFundsException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case UsernameAlreadyExistException e:
                            response.StatusCode = (int)HttpStatusCode.Conflict;
                            break;
                        case EmailAlreadyExistException e:
                            response.StatusCode = (int)HttpStatusCode.Conflict;
                            break;
                        case PhoneNumberAlreadyExistException e:
                            response.StatusCode = (int)HttpStatusCode.Conflict;
                            break;
                        case EntityAlreadyBlockedException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case EntityAlreadyUnblockedException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case InvalidOperationException e:
                            response.StatusCode= (int)HttpStatusCode.BadRequest;
                            break;
                        case ArgumentNullException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case ArgumentException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        default:
                        //TODO да логвам ексепшъна в някакъв файл
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            //var developerErrorMessage = error.Message; логване на грешката
                            // "An error occurred. Please try again later."
                            break;
                    }

                    var result = JsonSerializer.Serialize(new { message = error.Message });
                    await response.WriteAsync(result);
                }
                else
                {
                    switch (error)
                    {
                        // custom application error
                        
                        default:
                            // unhandled error
                            //TODO да логвам ексепшъна в някакъв файл
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            var homeErrorUrl = context.Request.PathBase + "/Home/Error";
                            context.Response.Redirect(homeErrorUrl);
                            return;
                    }
                }

            }
        }
    }
}
