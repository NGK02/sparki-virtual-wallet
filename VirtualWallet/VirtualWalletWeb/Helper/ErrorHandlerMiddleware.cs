using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
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
                        //TODO да логвам ексепшъна в някаквъ файл
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Redirect("Error"); 
                        return;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
