using BasicCRM.Business.Exceptions;
using System.Text.Json;

namespace BasicCRM.API.Middleware
{
    public class GlobalHandleExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;

                string response = JsonSerializer.Serialize(ex.ValidationErrors);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(response);
            }
            catch(NotFoundException ex)
            {
                context.Response.StatusCode = (int)StatusCodes.Status404NotFound;

                string response = JsonSerializer.Serialize(ex.Message);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(response);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;

                string response = JsonSerializer.Serialize(ex.Message);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(response);
            }
        }

        private async Task SendResponse(HttpContext context, Exception ex)
        {
   
        }
    }
}
