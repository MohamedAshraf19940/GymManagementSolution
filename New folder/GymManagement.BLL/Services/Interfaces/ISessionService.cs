using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.SesisonViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task< Result<SessionViewModel?>> GetSessionByIdAsync(int sessionId , CancellationToken ct = default);
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);

        Task<Result> CreateSessionAsync(CreateSessionViewModel createSessionVm, CancellationToken ct = default);

        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct=default);
        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel createSessionVm, CancellationToken ct = default);

        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropdownAsync(CancellationToken ct = default);

        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropdownAsync(CancellationToken ct = default);

        Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct = default);
    }
}
