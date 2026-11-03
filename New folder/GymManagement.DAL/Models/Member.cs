using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Member:GymUser
    {
        public string? Photo { get; set; }
        //JoinDate = CreatedAt Of BaseEntity


        #region Relations
        public HealthRecord HealthRecord { get; set; } = default!;

        #endregion

        public ICollection<MemberShip> Memberships { get; set; } = default!;
        public ICollection<Booking> Bookings { get; set; } = default!;

    }
}
