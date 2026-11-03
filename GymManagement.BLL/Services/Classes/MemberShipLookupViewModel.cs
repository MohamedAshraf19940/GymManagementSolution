namespace GymManagement.BLL.Services
{
    public class MemberShipLookupViewModel
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public decimal Price { get; internal set; }
        public int DurationDays { get; internal set; }
    }
}