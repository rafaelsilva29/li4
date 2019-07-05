using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace CookieFy.Includes
{
    public class AlertDecoratorResult : IActionResult {

        public IActionResult Result { get; }
        public string Type { get; }
        public string Title { get; }
        public string Body { get; }
        public string DurationAlert { get; }

        public AlertDecoratorResult(IActionResult result, string type, string title, string body, string durationAlert) {

            Result = result;
            Type = type;
            Title = title;
            Body = body;
            DurationAlert = durationAlert;
        }

        public async Task ExecuteResultAsync(ActionContext context) {

            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();

            var tempData = factory.GetTempData(context.HttpContext);
            tempData["_alert.type"] = Type;
            tempData["_alert.title"] = Title;
            tempData["_alert.body"] = Body;
            tempData["_alert.DurationAlert"] = DurationAlert;

            await Result.ExecuteResultAsync(context);
        }
    }
}
