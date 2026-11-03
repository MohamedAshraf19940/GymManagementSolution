
using AutoMapper;
using GymManagement.BLL.Services.Attachment;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(
            IUnitOfWork uow,
            IMapper mapper,
            IAttachmentService attachmentService)
        {
            _uow = uow;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public async Task<bool> CreateMemberAsync(
            CreateMemberViewModel memberViewModel,
            CancellationToken ct = default)
        {
            var memberRepository = _uow.GetRepository<Member>();

            // Check Email
            var emailExists = await memberRepository.AnyAsync(
                m => m.Email == memberViewModel.Email, ct);

            // Check Phone
            var phoneExists = await memberRepository.AnyAsync(
                m => m.Phone == memberViewModel.Phone, ct);

            if (emailExists || phoneExists)
                return false;

            // Upload Image
            var storedPhotoName = await _attachmentService.UploadAsync(
                memberViewModel.PhotoFile.OpenReadStream(),
                memberViewModel.PhotoFile.FileName,
                "MembersPhotos");

            // Map ViewModel to Member
            var member = _mapper.Map<Member>(memberViewModel);

            // Assign uploaded photo path
            member.Photo = storedPhotoName;

            member.Address = new Address
            {
                BuildingNumber = memberViewModel.BuildingNumber,
                City = memberViewModel.City,
                Street = memberViewModel.Street
            };

            // Add Member
            memberRepository.Add(member);

            // Save Changes
            var result = await _uow.SaveChangesAsync(ct);

            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(
            CancellationToken ct = default)
        {
            var members = await _uow.GetRepository<Member>()
                .GetAllAsync(ct: ct);

            if (!members.Any())
                return [];

            return _mapper.Map<IEnumerable<MemberViewModel>>(members);
        }

        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var member = await _uow.GetRepository<Member>()
                .GetByIdAsync(id, ct);

            if (member is null)
                return null;

            var viewModel = _mapper.Map<MemberViewModel>(member);

            var activeMembership = await _uow.GetRepository<MemberShip>()
                .FirstOrDefaultAsync(
                    m => m.MemberId == id && m.EndDate > DateTime.Now);

            if (activeMembership is not null)
            {
                viewModel.PlanName = activeMembership.Plan.Name;
                viewModel.MembershipStartDate =
                    activeMembership.CreatedAt.ToString("yyyy-MM-dd");

                viewModel.MembershipEndDate =
                    activeMembership.EndDate.ToString("yyyy-MM-dd");
            }

            return viewModel;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordAsync(
            int memberId,
            CancellationToken ct = default)
        {
            var record = await _uow.GetRepository<HealthRecord>()
                .FirstOrDefaultAsync(x => x.MemberId == memberId, ct);

            if (record is null)
                return null;

            return _mapper.Map<HealthRecordViewModel>(record);
        }

        public async Task<bool> RemoveMemberAsync(
            int memberId,
            CancellationToken ct = default)
        {
            var member = await _uow.GetRepository<Member>()
                .GetByIdAsync(memberId, ct);

            if (member is null)
                return false;

            // Check Future Bookings
            var hasFutureBookings = await _uow.GetRepository<Booking>()
                .AnyAsync(
                    b => b.MemberId == memberId &&
                         b.CreatedAt > DateTime.Now,
                    ct);

            if (hasFutureBookings)
                return false;

            _uow.GetRepository<Member>().Delete(member);

            var result = await _uow.SaveChangesAsync(ct);

            return result > 0;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(
            int id,
            CancellationToken ct = default)
        {
            var member = await _uow.GetRepository<Member>()
                .GetByIdAsync(id, ct);

            if (member is null)
                return null;

            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public async Task<bool> UpdateMemberDetailsAsync(
            int id,
            MemberToUpdateViewModel memberVM,
            CancellationToken ct = default)
        {
            var memberRepository = _uow.GetRepository<Member>();

            var member = await memberRepository.GetByIdAsync(id, ct);

            if (member is null)
                return false;

            var emailExists = await memberRepository.AnyAsync(
                m => m.Email == memberVM.Email && m.Id != id, ct);

            var phoneExists = await memberRepository.AnyAsync(
                m => m.Phone == memberVM.Phone && m.Id != id, ct);

            if (emailExists || phoneExists)
                return false;

            _mapper.Map(memberVM, member);

            member.UpdatedAt = DateTime.Now;

            memberRepository.Update(member);

            var result = await _uow.SaveChangesAsync(ct);

            return result > 0;
        }
    }
}


