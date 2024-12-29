using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Lesson30_WebApi.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]//atribut harda istifade olunacaq class ve method ustunde
    public class MyAuthorizedAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var user = context.HttpContext.Items["User"];
            var user = context.HttpContext.User;
            if (!user.Claims.Any())
            {
                context.Result = new JsonResult(new { Message = "Unauthorized" })
                {
                    StatusCode=StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
