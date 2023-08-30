using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace WebApiCodeGenLib.Helpers
{
    public class AuthenticationFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //base.OnActionExecuting(actionContext);
            if (actionContext.Request.Headers.Contains("GentechToken"))
            {
                IEnumerable<string> values = new List<string>();
                actionContext.Request.Headers.TryGetValues("GentechToken", out values);
                var token = values.FirstOrDefault();
                var finaltoken = "";
                if(values.Count() > 0)
                {
                    if (token.StartsWith("Bearer"))
                    {
                        finaltoken = token.Substring(7).ToString();
                        var jwtandler = new JWTTokenHandler();
                       var isvalid = jwtandler.ValidateToken(finaltoken);
                        if(isvalid)
                        {
                            /// if needed do something (^_^)
                        }
                        else
                        {
                            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                        }
                    }
                }
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            
        }

    }
}
