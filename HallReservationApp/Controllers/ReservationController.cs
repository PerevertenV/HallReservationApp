using Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HallReservationApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ReservationController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpPost]
		public async Task<ActionResult> CreateReservation(int hallId, DateTime reservDateTime, 
			TimeOnly duration, string AddOptions)
		{
			return Ok("Reservation created successfully");
		}
	}
}
