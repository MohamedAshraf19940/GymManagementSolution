using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SesisonViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork uow , IMapper mapper)
        {
            _uow=uow;
            _mapper=mapper;
        }


        public async Task<Result<SessionViewModel?>> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _uow.SessionRepository.GetSessionByIdWithTrainerAndCategory(sessionId, ct);
            if (session is null) return Result<SessionViewModel?>.NotFound("Session Not Found");
            else
            {
                var mappedSession = _mapper.Map<SessionViewModel>(session);
                mappedSession.AvailableSlots = session.Capacity  - (await _uow.SessionRepository.GetCountOfBookedSlotsAsync(session.Id));

                return Result<SessionViewModel?>.Ok(mappedSession);
            }
        }
        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _uow.SessionRepository.GetAllSessionWithTrainerAndCategory(ct: ct);
            if (sessions == null ||!sessions.Any()) return null;

            //Category Name
            //Trainer Name

            var mappedSessions = sessions.Select(s => new SessionViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Capacity = s.Capacity,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                TrainerName = $"{s.Trainer.Name} ",
                CategoryName = s.Category.CategoryName,
            });


            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity  /*25*/ - (await _uow.SessionRepository.GetCountOfBookedSlotsAsync(session.Id));
            }

            return mappedSessions;



        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel vm, CancellationToken ct = default)
        {
            if (vm.EndDate <= vm.StartDate) return Result.Validation("End Date Must Be After Start Date");
            if (vm.StartDate <= DateTime.Now) return Result.Validation(" Start Date Must Be In  Future");
            if(vm.Capacity < 1 || vm.Capacity > 25) return Result.Validation("Capacity Must Be Between 1 and 25"); ;

            var trainer = await _uow.GetRepository<Trainer>().GetByIdAsync(vm.TrainerId, ct);
            if (trainer == null) return Result.NotFound("Trainer Not Found"); 

            var category = await _uow.GetRepository<Category>().GetByIdAsync(vm.CategoryId, ct);
            if (category == null) return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Speciality>(category.CategoryName, true, out var CategorySpeciality);
            if (!isValid || trainer.Speciality != CategorySpeciality)  return Result.Validation("Cannot Assign Session To This Trainer");

            var session = _mapper.Map<Session>(vm);


            _uow.GetRepository<Session>().Add(session);
            var res= await _uow.SaveChangesAsync(ct);

            return res>0  ? Result.Ok() : Result.Fail("Failed To Create Session" );
        }
  


        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropdownAsync(CancellationToken ct = default)
        {
           var result= await _uow.GetRepository<Category>().GetAllAsync(ct:ct);

            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(result);

           
        }

 

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropdownAsync(CancellationToken ct = default)
        {
            var result = await _uow.GetRepository<Trainer>().GetAllAsync(ct: ct);

            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }



        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _uow.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session is null) return Result<UpdateSessionViewModel>.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Fail("Cannot Update Session That Has Already Started");


            var bookingCount = await _uow.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if(bookingCount > 0 )
                return Result<UpdateSessionViewModel>.Fail("Cannot Update Session That Has Already Bookings");

            var mappedSession = _mapper.Map<UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.Ok(mappedSession);



        }
        public async Task<Result> UpdateSessionAsync ( int id, UpdateSessionViewModel sessionVm, CancellationToken ct = default)
        {

            var session = await _uow.SessionRepository.GetByIdAsync(id, ct);
            if (session is null) return Result.NotFound("Session Not Found");


            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot Update Session That Has Already Started");

            if (sessionVm.StartDate <=DateTime.Now)
                return Result.Fail(" Start Date Must Be In The Future");


            if (sessionVm.EndDate <= sessionVm.StartDate)
                return Result.Fail("End Date Must Be After Start Date");


            var bookingCount = await _uow.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if (bookingCount > 0)
                return Result.Fail("Cannot Update Session That Has Already Bookings");


            var trainer = await _uow.GetRepository<Trainer>().GetByIdAsync(sessionVm.TrainerId);
            if (trainer is null) return Result.NotFound("Trainer Not Found");


            var category = await _uow.GetRepository<Category>().GetByIdAsync(session.CategoryId);
            if (category is null) return Result.NotFound("Category Not Found");


            var isValid = Enum.TryParse<Speciality>(category.CategoryName, true, out var CategorySpeciality);
            if (!isValid || trainer.Speciality != CategorySpeciality) return Result.Validation("Cannot Assign Session To This Trainer");


            _mapper.Map(sessionVm, session);
            session.UpdatedAt=DateTime.Now;

            _uow.SessionRepository.Update(session);
            var res = await _uow.SaveChangesAsync(ct);

            return res>0 ? Result.Ok() : Result.Fail("Failed To Update Session");

        }

        public async Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _uow.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session is null) return Result.NotFound("Session Not Found");



            if (session.EndDate > DateTime.Now) return Result.Fail("Cannot Delete Session That Not Ended Yet");

            var bookingCount = await _uow.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if (bookingCount > 0)
                return Result.Fail("Cannot Delete Session That Has Already Bookings");


            _uow.SessionRepository.Delete(session);
            var res = await _uow.SaveChangesAsync(ct);

            return res>0 ? Result.Ok() : Result.Fail("Failed To Delete Session");
        }
    }
}
