
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CookieFy.Pages {

    [Authorize(Roles = "Admin")]
    public class AdminArea : PageModel {

    }
}