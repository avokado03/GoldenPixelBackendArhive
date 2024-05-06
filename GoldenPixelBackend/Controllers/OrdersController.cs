using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenPixelBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{

		[HttpGet]
		public IActionResult Get()
		{
			return Ok();
		}
	}
}
