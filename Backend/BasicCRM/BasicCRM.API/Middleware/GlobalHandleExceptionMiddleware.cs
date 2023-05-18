using BasicCRM.API.Models;
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
                
                var response = new Response()
                {
                    Success = false,
                    Errors = ex.ValidationErrors
                };
                
                string json = JsonSerializer.Serialize(response);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
            catch(NotFoundException ex)
            {
                context.Response.StatusCode = (int)StatusCodes.Status404NotFound;

                var response = new Response()
                {
                    Success = false,
                    Message = ex.Message
                };

                string json = JsonSerializer.Serialize(response);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;

                var response = new Response()
                {
                    Success = false,
                    Message = ex.Message
                };

                string json = JsonSerializer.Serialize(response);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
        }

        private async Task SendResponse(HttpContext context, Exception ex)
        {
   
        }
    }
}
