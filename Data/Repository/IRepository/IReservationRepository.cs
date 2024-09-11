using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Repository.IRepository;

namespace Data.Repository.IRepository
{
	public interface IReservationRepository: IRepository<Reservation>
	{
		public int CountFullPrice (int BPPH, string? additionalOptions, DateTime reservTime, TimeOnly duration);
	}
}
