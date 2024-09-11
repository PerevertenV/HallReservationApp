using Data.Data;
using Data.Repository.IRepository;
using Microsoft.Identity.Client;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private AppDbContext _context;
		public IHallRepository Hall { get; private set; }
		public IReservationRepository Reservation { get; private set; }

        public UnitOfWork(AppDbContext _context)
        {
			_context = _context;
			Hall = new HallRepository(_context);
			Reservation = new ReservationRepository(_context);
        }
    }
}
