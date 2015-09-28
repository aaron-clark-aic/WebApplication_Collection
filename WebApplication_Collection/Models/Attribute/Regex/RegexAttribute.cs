using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApplication_collection.Models.Filter
{
    public class RegexAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 验证参数是否正确[配合RegularExpressionDictionaryAttribute]使用
        /// [aaron.clark.aic][2015-01-30 13:34][refactor]
        /// </summary>
        /// <param name="actionContext"></param>
        private static void IsParameterValidated(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                string str = "Format of Parameter is error ";
                foreach (var a in actionContext.ModelState.Keys)
                {
                    str = str + " [" + a.ToString() + "]";
                }
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, str);
            }
        }


        public override async void OnActionExecuting(HttpActionContext actionContext) {
            IsParameterValidated(actionContext);
        }


    }
}