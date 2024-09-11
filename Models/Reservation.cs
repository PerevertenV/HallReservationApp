using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Reservation
	{
		[Key]
		public int Id { get; set; }
		[Required]
		//id залу який бронюється
		public int hallId { get; set; }
		[ForeignKey(nameof(hallId))]
		public Hall hall {  get; set; }
		//дата час резервації
		[Required]
		public DateTime dateTimeOfReserv { get; set; }
		//на скільки зал заброньований
		[Required]
		public TimeOnly reservTime { get; set; }
		//Вибрані додаткові опції
		public string? SelectedAddOpt { get; set; }
		//кінцева сума для резервації
		[Required]
		public int FinalSum { get; set; }
    }
}
