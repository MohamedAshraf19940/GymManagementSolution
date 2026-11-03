using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberShipViewModels
{
	public class MemberShipLookupViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public int DurationDays { get; set; }
	}
}