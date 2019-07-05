using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookieFy.Includes
{
    public static class AlertExtensions {
        public static IActionResult WithSuccess(this IActionResult result, string title, string body, string durationAlert) {
            return Alert(result, "success", title, body, durationAlert);
        }

        public static IActionResult WithInfo(this IActionResult result, string title, string body, string durationAlert) {
            return Alert(result, "info", title, body, durationAlert);
        }

        public static IActionResult WithWarning(this IActionResult result, string title, string body, string durationAlert) {
            return Alert(result, "warning", title, body, durationAlert);
        }

        public static IActionResult WithDanger(this IActionResult result, string title, string body, string durationAlert) {
            return Alert(result, "danger", title, body, durationAlert);
        }

        private static IActionResult Alert(IActionResult result, string type, string title, string body, string durationAlert) {
            return new AlertDecoratorResult(result, type, title, body, durationAlert);
        }
    }
}
