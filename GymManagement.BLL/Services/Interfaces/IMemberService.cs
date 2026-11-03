using GymManagement.BLL.ViewModels.MemberViewModels;


namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {

        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel memberViewModel, CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetHealthRecordAsync(int memberId, CancellationToken ct = default);
        Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default);
        Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel memberVM, CancellationToken ct = default);



    }
}
