using Data.Repository.IRepository;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace HallReservationApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HallController : Controller
	{
		//DI
		private readonly IUnitOfWork _unitOfWork;
		private IBLServices _BLServices;
		public HallController(IUnitOfWork unitOfWork, IBLServices BLServices)
		{
			_unitOfWork = unitOfWork;
			_BLServices = BLServices;
		}
		[HttpPost]
		public async Task<ActionResult> CreateHall(string HallName, int capacity,
			string[] AddOptions, int BasePrice)
		{
			//преревіряємо чи всі данні валідні
			if(HallName.IsNullOrEmpty() || (capacity <= 0) || (BasePrice <= 0)) 
			{
				return BadRequest("some of values weren't given");
			}
			//інціалізуємо змінну ля передачі додаткових опцій
			string additionalOptions = "";
			//перервіряємо чи масив не пустий, якщо ні, то прерводимо із масиву в рядок
			if(AddOptions.Length > 0) { additionalOptions = String.Join('/', AddOptions); }
			//створюємо новий зал та запонюємо параметри
			Hall hall = new Hall() 
			{
				Name = HallName,
				Capacity = capacity,
				AdditionalOptions = additionalOptions,
				PricePerHour = BasePrice,
				reserved = false
			};
			await _unitOfWork.Hall.AddAsync(hall);

			return Ok("Hall created successfully");
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateHall(int id, Hall updatedHall)
		{
			//перервіряємо чи данні збігаться
			if(id != updatedHall.Id) 
			{
				return BadRequest();
			}
			//оновлюємо  
			await _unitOfWork.Hall.UpdateAsync(updatedHall);
			return Ok($"Hall with id: {id}, updated successfully");
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteHall(int id)
		{
			//оотримуємо зал із БД
			Hall? hallToBeDeleted = await _unitOfWork.Hall.GetFirstOrDefaultAsync(x => 
				x.Id == id);
			//преервіряємо чи даний зал існує
			if (hallToBeDeleted == null) 
			{
				return NotFound($"hall with id: {id} not found");
			}
			//якщо  існують якісь бронювання на зал який має видалятися їх теж потрібно видалити
			List<Reservation> reservationToBeDeleyed = (await _unitOfWork.Reservation
				.GetAllAsync(u => u.hallId == id)).ToList();
			//перевіряємо чи існують резервації, та якщо так видаляємо їх
			if (reservationToBeDeleyed.Any())
			{
				foreach (Reservation reservation in reservationToBeDeleyed)
				{
					await _unitOfWork.Reservation.DeleteAsync(reservation);
				}
			}
			//якщо зал існує видаляємо його
			 await _unitOfWork.Hall.DeleteAsync(hallToBeDeleted);
			return Ok($"Hall with id: {id}, deleted successfully");
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Hall>>> GetAllByFilterAsync(
			DateTime ReservationDateTime,
			TimeOnly duration, int capacity)
		{

			return Ok( await _BLServices.Hall.FindHallsAsync(ReservationDateTime, duration, capacity));

		}
	}
}
