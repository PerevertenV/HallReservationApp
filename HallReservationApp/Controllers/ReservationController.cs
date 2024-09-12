using Data.Repository.IRepository;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace HallReservationApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBLServices _BLServices;

		public ReservationController(IUnitOfWork unitOfWork, IBLServices BLServices)
		{
			_unitOfWork = unitOfWork;
			_BLServices = BLServices;
		}

		[HttpPost]
		public async Task<ActionResult> CreateReservation(int hallId, DateTime reservDateTime, 
			TimeOnly duration, string? AddOptions)
		{
			//отримуємо резервації для перевірки чи вже не існує така резервація
			List<Reservation> reservListFromDB = (await _unitOfWork.Reservation
				.GetAllAsync(u => u.hallId == hallId)).ToList();

			//перервіряємо на збіг
			if (reservListFromDB.Any(u => u.dateTimeOfReserv <= reservDateTime
				&& reservDateTime <= u.dateTimeOfReserv.Add(u.reservTime.ToTimeSpan())))
			{ 
				return BadRequest("A reservation for the same hall and date already exists"); 
			}
			//отримуємо зал за ID
			Hall? reservatedHall = await _unitOfWork.Hall.GetFirstOrDefaultAsync(u => 
				u.Id == hallId);
			//Перевіряєм очи існує зал за переданим ID
			if (reservatedHall == null) 
			{
				return NotFound($"Hall with ID: {hallId} does not exist");
			}
			//створюємо нову резервацію
			Reservation newReserv = new Reservation()
			{
				hallId = hallId,
				dateTimeOfReserv = reservDateTime,
				reservTime = duration,
				SelectedAddOpt = AddOptions,
				FinalSum = _BLServices.Reservation.CountFullPrice(reservatedHall.PricePerHour,
					AddOptions, reservDateTime, duration)
			};
			//встановлюємо що зал вже зарезервований
			reservatedHall.reserved = true;
			//оновлюємо дані та зберігаємо
			await _unitOfWork.Reservation.AddAsync(newReserv);
			await _unitOfWork.Hall.UpdateAsync(reservatedHall);
			//повертаємо повідомлення про успіх та повертаємо фінальну суму
			return Ok($"Reservation created successfully, with a final sum: {newReserv.FinalSum}");
		}

		[HttpGet]
		public async Task<ActionResult> GenerateReport() 
		{
			List<Reservation> listForMakingReport = (await _unitOfWork.Reservation
				.GetAllAsync()).ToList();
			string resultString = "";

			if (listForMakingReport.Any())
			{
				//отримуємо найпопулярніший зал щоб передати його назву
				Hall? theMostPopularHall = await _unitOfWork.Hall
					.GetFirstOrDefaultAsync(u =>
					u.Id == _BLServices.Reservation.TheMostPopularHall(listForMakingReport));

				//створюємо звіт викликаючи методи
				resultString += $"The most popular hall is: {theMostPopularHall.Name}\n" +
					"The most popular reservation duration is: " +
					$"{_BLServices.Reservation.TheMostPopularDuration(listForMakingReport)}\n" +
					"Average final sum is: " +
					$"{Math.Round(_BLServices.Reservation
					.AvrgPriceForReservations(listForMakingReport), 2)}\n" +
					"The most popular additional option is: " +
					$"{_BLServices.Reservation.TheMostPopularAddOption(listForMakingReport)}";
			}
			return Ok(resultString);
		}
	}
}
