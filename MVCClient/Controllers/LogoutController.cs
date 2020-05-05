using Microsoft.AspNetCore.Mvc;

namespace MVCClient.Controllers
{
	public class LogoutController : Controller
	{
		public IActionResult Index()
		{
			return SignOut("Cookies", "oidc");
		}
	}
}
