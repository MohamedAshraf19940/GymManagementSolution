
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberShipViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Services
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberShipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MemberShipLookupViewModel>> GetMembersAsync(
            CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>()
                .GetAllAsync(ct: ct);

            return members.Select(m => new MemberShipLookupViewModel
            {
                Id = m.Id,
                Name = m.Name
            });
        }

        public async Task<IEnumerable<MemberShipLookupViewModel>> GetActivePlansAsync(
            CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>()
                .GetAllAsync(ct: ct);

            return plans
                .Where(p => p.IsActive)
                .Select(p => new MemberShipLookupViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    DurationDays = p.DurationDays
                });
        }

        public async Task<IEnumerable<MemberShipViewModel>> GetAllAsync(
            CancellationToken ct = default)
        {
            var memberships = await _unitOfWork
                .GetRepository<MemberShip>()
                .GetAllAsync(ct: ct);

            var result = new List<MemberShipViewModel>();

            foreach (var membership in memberships)
            {
                var member = await _unitOfWork.GetRepository<Member>()
                    .GetByIdAsync(membership.MemberId, ct);

                var plan = await _unitOfWork.GetRepository<Plan>()
                    .GetByIdAsync(membership.PlanId, ct);

                result.Add(new MemberShipViewModel
                {
                    Id = membership.Id,
                    MemberId = membership.MemberId,
                    PlanId = membership.PlanId,
                    MemberName = member?.Name,
                    PlanName = plan?.Name,
                    PlanPrice = plan?.Price ?? 0,
                    DurationDays = plan?.DurationDays ?? 0,
                    StartDate = membership.CreatedAt,
                    EndDate = membership.EndDate,
                    Status = membership.Status
                });
            }

            return result;
        }

        public async Task<MemberShipViewModel?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var membership = await _unitOfWork
                .GetRepository<MemberShip>()
                .GetByIdAsync(id, ct);

            if (membership is null)
                return null;

            var member = await _unitOfWork.GetRepository<Member>()
                .GetByIdAsync(membership.MemberId, ct);

            var plan = await _unitOfWork.GetRepository<Plan>()
                .GetByIdAsync(membership.PlanId, ct);

            return new MemberShipViewModel
            {
                Id = membership.Id,
                MemberId = membership.MemberId,
                PlanId = membership.PlanId,
                MemberName = member?.Name,
                PlanName = plan?.Name,
                PlanPrice = plan?.Price ?? 0,
                DurationDays = plan?.DurationDays ?? 0,
                StartDate = membership.CreatedAt,
                EndDate = membership.EndDate,
                Status = membership.Status
            };
        }

        public async Task<bool> CreateAsync(
            MemberShipViewModel model,
            CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>()
                .GetByIdAsync(model.MemberId, ct);

            var plan = await _unitOfWork.GetRepository<Plan>()
                .GetByIdAsync(model.PlanId, ct);

            if (member is null || plan is null ||
                !plan.IsActive || plan.DurationDays <= 0)
                return false;

            var hasActiveMembership =
                await _unitOfWork.GetRepository<MemberShip>().AnyAsync(
                    m => m.MemberId == model.MemberId &&
                         m.EndDate > DateTime.Now,
                    ct);

            if (hasActiveMembership)
                return false;

            var startDate = DateTime.Now;

            var membership = new MemberShip
            {
                MemberId = model.MemberId,
                PlanId = model.PlanId,
                CreatedAt = startDate,
                EndDate = startDate.AddDays(plan.DurationDays)
            };

            _unitOfWork.GetRepository<MemberShip>().Add(membership);

            return await _unitOfWork.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> UpdateAsync(
            MemberShipViewModel model,
            CancellationToken ct = default)
        {
            var repository = _unitOfWork.GetRepository<MemberShip>();

            var membership = await repository.GetByIdAsync(model.Id, ct);

            if (membership is null)
                return false;

            var member = await _unitOfWork.GetRepository<Member>()
                .GetByIdAsync(model.MemberId, ct);

            var plan = await _unitOfWork.GetRepository<Plan>()
                .GetByIdAsync(model.PlanId, ct);

            if (member is null || plan is null ||
                !plan.IsActive || plan.DurationDays <= 0)
                return false;

            var hasAnotherActiveMembership = await repository.AnyAsync(
                m => m.MemberId == model.MemberId &&
                     m.Id != model.Id &&
                     m.EndDate > DateTime.Now,
                ct);

            if (hasAnotherActiveMembership)
                return false;

            membership.MemberId = model.MemberId;
            membership.PlanId = model.PlanId;
            membership.EndDate =
                membership.CreatedAt.AddDays(plan.DurationDays);
            membership.UpdatedAt = DateTime.Now;

            repository.Update(membership);

            return await _unitOfWork.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            var repository = _unitOfWork.GetRepository<MemberShip>();

            var membership = await repository.GetByIdAsync(id, ct);

            if (membership is null)
                return false;

            repository.Delete(membership);

            return await _unitOfWork.SaveChangesAsync(ct) > 0;
        }
    }
}
