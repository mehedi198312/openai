using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenAIApp.Helpers;
using System.ComponentModel;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{

    private readonly IList<Key> _keys;

    public AuthorizeAttribute(params Key[] keys)
    {
        _keys = keys ?? new Key[] { };
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        List<string> keysDescription = new List<string>();
        foreach(var key in _keys)
        {
            FieldInfo fi = key.GetType().GetField(key.ToString());
            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes != null && attributes.Any())
            {
                keysDescription.Add(attributes.First().Description);
            }
        }

        string clientKey = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();


        if (keysDescription.Any() && !keysDescription.Any(r => r.ToString().Equals(clientKey, StringComparison.OrdinalIgnoreCase)))
        {
            context.Result = new JsonResult(new { message = $"Unauthorized request" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }

    }


}
