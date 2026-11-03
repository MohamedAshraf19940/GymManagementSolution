namespace GymManagement.DAL.Models
{
    public class Plan:BaseEntity
    {
     

        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public ICollection<MemberShip> Memberships { get; set; } = default!;


    }
}
