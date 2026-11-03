using GymManagement.BLL.ViewModels.MemberShipViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberShipService
    {
        Task<IEnumerable<MemberShipViewModel>> GetAllAsync(
            CancellationToken ct = default);

        Task<MemberShipViewModel?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<bool> CreateAsync(
            MemberShipViewModel model,
            CancellationToken ct = default);

        Task<bool> UpdateAsync(
            MemberShipViewModel model,
            CancellationToken ct = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken ct = default);

        Task<IEnumerable<MemberShipLookupViewModel>> GetMembersAsync(
            CancellationToken ct = default);

        Task<IEnumerable<MemberShipLookupViewModel>> GetActivePlansAsync(
            CancellationToken ct = default);
    }
}