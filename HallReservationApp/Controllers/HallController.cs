using Data.Repository.IRepository;
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

		public HallController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
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
			Hall? hallToBeDeleted = await _unitOfWork.Hall.GetFirstOrDefaultAsync(x => x.Id == id);
			//преервіряємо чи даний зал існує
			if (hallToBeDeleted == null) 
			{
				return NotFound($"hall with id: {id} not found");
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
			//перевіряємо на валідність отримані змінні
			if(ReservationDateTime == null || duration == null || capacity == null) 
			{
				return BadRequest("some of values weren't given");
			}
			//отримуємо всіз зали які не є заброньованими
			List<Hall> hallList = (await _unitOfWork.Hall.GetAllAsync(u => 
				u.reserved == true && u.Capacity == capacity)).ToList();

			//отримуємо резервації для того щоб отримати зали які є зарезервовані
			//але підходять під умову резерваці(в той самий день але на інші години резервація)
			List<Reservation> reservationList = (await _unitOfWork.Reservation.GetAllAsync()).ToList();
			if(reservationList != null)
			{
				//вибираємо лише ті резервації котрі збігаються із потрібною датою резервації
				reservationList = reservationList.Where(u =>
					u.dateTimeOfReserv.Date == ReservationDateTime.Date).ToList();
				//перевіряємо чи список не пустий після фільтрації
				if (reservationList != null)
				{
	
					//фільтрація за часом, представлені перетворення часу для коректної
					//відфільтрації резервацій котрі не потрапляють у заданий чаосвий проміжок
					reservationList = reservationList.Where(u =>
						(u.dateTimeOfReserv.Add(u.reservTime.ToTimeSpan()) < ReservationDateTime)
						|| (u.dateTimeOfReserv < ReservationDateTime.Add(duration.ToTimeSpan())))
						.ToList();

					Hall singleHall = new Hall();
					//перевіряємо чи є елементи після останьої фільтрації
					if (reservationList.Any())
					{
						//якщо ще залишились елементи то отримуємо ці зали та додаємо до списку
						foreach (Reservation reserv in reservationList)
						{
							singleHall = await _unitOfWork.Hall.GetFirstOrDefaultAsync(u =>
								u.Id == reserv.hallId && u.Capacity == capacity);

							if (singleHall != null) { hallList.Add(singleHall); }
						}
					}
				}
			}
			//після всіх пошуків преедаємо список елементів
			return Ok(hallList);
		}
	}
}
