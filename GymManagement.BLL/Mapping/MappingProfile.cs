
using AutoMapper;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SesisonViewModels;
using GymManagement.DAL.Models;

namespace GymManagement.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // HealthRecord Mapping
            CreateMap<HealthRecordViewModel, HealthRecord>()
                .ReverseMap();

            // CreateMemberViewModel -> Member
            CreateMap<CreateMemberViewModel, DAL.Models.Member>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.Memberships, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }))
                .ForMember(dest => dest.HealthRecord,
                    opt => opt.MapFrom(src => src.HealthRecordViewModel));

            // Member -> CreateMemberViewModel
            CreateMap<DAL.Models.Member, CreateMemberViewModel>()
                .ForMember(dest => dest.BuildingNumber,
                    opt => opt.MapFrom(src => src.Address != null
                        ? src.Address.BuildingNumber : 0))
                .ForMember(dest => dest.City,
                    opt => opt.MapFrom(src => src.Address != null
                        ? src.Address.City : string.Empty))
                .ForMember(dest => dest.Street,
                    opt => opt.MapFrom(src => src.Address != null
                        ? src.Address.Street : string.Empty))
                .ForMember(dest => dest.HealthRecordViewModel,
                    opt => opt.MapFrom(src => src.HealthRecord))
                .ForMember(dest => dest.PhotoFile, opt => opt.Ignore());

            // Member -> MemberViewModel
            CreateMap<DAL.Models.Member, MemberViewModel>()
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => src.Address != null
                        ? $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"
                        : null))
                .ForMember(dest => dest.PlanName, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipStartDate, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipEndDate, opt => opt.Ignore());

            // Trainer -> TrainerSelectViewModel
            CreateMap<Trainer, TrainerSelectViewModel>();

            // Category -> CategorySelectViewModel
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.CategoryName));

            // CreateSessionViewModel -> Session
            CreateMap<CreateSessionViewModel, Session>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Trainer, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.MemberSessions, opt => opt.Ignore());
        }
    }
}
