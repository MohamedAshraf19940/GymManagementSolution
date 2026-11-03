using AutoMapper;
using AutoMapper.Execution;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMemberViewModel, DAL.Models.Member>()
                .ForMember(dest => dest.Photo,
                    opt => opt.Ignore())
                .ForMember(dest => dest.HealthRecord,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Memberships,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Bookings,
                    opt => opt.Ignore()).ReverseMap();

            CreateMap<DAL.Models.Member, CreateMemberViewModel>();
        }
    }
}