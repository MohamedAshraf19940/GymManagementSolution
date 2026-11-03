using GymManagement.BLL.Services;
using System.ComponentModel.DataAnnotations;

namespace GymManagement.BLL.ViewModels.MemberShipViewModels
{
    public class MemberShipViewModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a member")]
        public int MemberId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a plan")]
        public int PlanId { get; set; }

        public string? MemberName { get; set; }

        public string? PlanName { get; set; }

        public decimal PlanPrice { get; set; }

        public int DurationDays { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Status { get; set; }

        // Members dropdown list
        public IEnumerable<MemberShipLookupViewModel> Members { get; set; } = [];

        // Plans dropdown list
        public IEnumerable<MemberShipLookupViewModel> Plans { get; set; } = [];
    }
}