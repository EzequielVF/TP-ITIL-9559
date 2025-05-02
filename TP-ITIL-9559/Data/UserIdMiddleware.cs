using TP_ITIL_9559.Data.Domain;

namespace TP_ITIL_9559.Data
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string userId = context.Request.Headers["user-id"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                context.Response.Headers.Add("user-id", userId);
            }
            await _next(context);
        }


        private string GetUserIdFromSource(HttpContext context)
        {
            return context.Request.Headers["userId"].ToString(); 
        }
    }
}
