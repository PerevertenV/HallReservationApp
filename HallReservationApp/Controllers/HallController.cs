using Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace HallReservationApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HallController : Controller
	{
		//DI
		private readonly IUnitOfWork _unitOfWork;

		public HallController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpPost]
		public async Task<ActionResult> CreateHall(string HallName, int capacity,
			string[] AddOptions, int BasePrice)
		{

			//await _unitOfWork.Hall.AddAsync(hall);
			return Ok("Hall created successfully");
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateHall(int id, Hall updatedHall)
		{
			return Ok($"Hall with id: {id}, updated successfully");
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteHall(int id)
		{
			return Ok($"Hall with id: {id}, deleted successfully");
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Hall>>> GetAllByFilterAsync(DateTime ReservationDateime,
			TimeOnly duration, int capacity)
		{
			return Ok();
		}
	}
}
