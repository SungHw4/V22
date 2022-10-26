using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace V22WebServer.MiddleWare
{
    public class ResultFilter
    {
        public void OnResultExcuting(ResultExecutingContext context)
        {
            ObjectResult objRet = (ObjectResult)context.Result;
            objRet.Value = "Change Seccess";
        }
    }

}
