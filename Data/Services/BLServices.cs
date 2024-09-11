using Data.Repository.IRepository;
using Data.Services.HallServices;
using Data.Services.HallServices.Interfaces;
using Data.Services.ReservationServices;
using Data.Services.ReservationServices.Interfces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
	public class BLServices: IBLServices
	{
		private IUnitOfWork _unitOfWork;
		public IHallBL Hall { get; private set; }
		public IReservationBL Reservation { get; private set; }

        public BLServices(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
			Hall = new HallBL(_unitOfWork);
			Reservation = new ReservationBL();
		}
    }
}
