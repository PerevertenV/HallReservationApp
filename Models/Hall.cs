using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Hall
	{
         //змінна ключ ID
        [Key]
        public int Id { get; set; }
        //Назва залу (наприклад "Зал А")
        [Required]
        public string Name { get; set; }
        // місткість (осіб)
        [Required]
        public int Capacity { get; set; }
        //додаткові опції(Наприклад  "Проєктор/Wifi")
        public string? AdditionalOptions { get; set; }
        //базова ціна за годину
        [Required]
        public int PricePerHour { get; set; }
        // змінна яка трекає чи зал зарезервований чи ні
        [Required]
        public bool reserved { get; set; }
    }
}
