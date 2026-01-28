using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOFWork unitOFWork, IMapper mapper)
        {
            _unitOFWork = unitOFWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionVM model)
        {
            if (!IsTrainerExists(model.TrainerId))
                return false;
            if (!IsCategoryExists(model.CategoryId))
                return false;
            if (!IsSessionTimeValid(model.StartDate, model.EndDate))
                return false;

            var session = _mapper.Map<CreateSessionVM, Session>(model);
            _unitOFWork.GetRepository<Session>().Add(session);

            return _unitOFWork.SaveChanges() > 0;

        }

        public IEnumerable<SessionVM> GetAllSessions()
        {
            var sessions = _unitOFWork.SessionRepository
                            .GetAllSessionsWithTrainerAndCategory()
                            .OrderByDescending(s => s.StartDate);

            if (sessions == null || !sessions.Any())
                return [];

            var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionVM>>(sessions);

            foreach(var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - _unitOFWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            }

            return mappedSessions;



        }

        public SessionVM? GetSessionById(int sessionId)
        {
            var session = _unitOFWork.SessionRepository
                            .GetSessionByIdWithTrainerAndCategory(sessionId);
            if (session == null)
                return null;

            var mappedSession = _mapper.Map<Session, SessionVM>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity - _unitOFWork.SessionRepository.GetCountOfBookedSlots(mappedSession.Id);

            return mappedSession;
        }

        public UpdateSessionVM? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOFWork.GetRepository<Session>().GetById(sessionId);

            if (session is null)
                return null;

            return _mapper.Map<Session, UpdateSessionVM>(session);      

        }


        public bool UpdateSession(int sessionId, UpdateSessionVM model)
        {
            var session = _unitOFWork.GetRepository<Session>().GetById(sessionId);


            if(!isSessionAvailableForUpdate(session))
                return false;
            if (!IsTrainerExists(model.TrainerId))
                return false;
            if (!IsSessionTimeValid(model.StartDate, model.EndDate))
                return false;

            session.Description = model.Description;
            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;
            session.TrainerId = model.TrainerId;
            session.UpdatedAt = DateTime.UtcNow;

            _unitOFWork.GetRepository<Session>().Update(session);

            return _unitOFWork.SaveChanges() > 0;



        }

        public bool RemoveSession(int sessionId)
        {
            var session = _unitOFWork.GetRepository<Session>().GetById(sessionId);

            if(!isSessionAvailableForRemove(session))
                return false;

            _unitOFWork.GetRepository<Session>().Delete(session);

            return _unitOFWork.SaveChanges() > 0;

        }

        public IEnumerable<CategorySelectVM> GetCategoriesDropDown()
        {
            var categories = _unitOFWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectVM>>(categories);
        }

        public IEnumerable<TrainerSelectVM> GetTrainersDropDown()
        {
            var trainers = _unitOFWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectVM>>(trainers);
        }


        #region Helper Methods
        private bool IsTrainerExists(int trainerId)
        {
            var trainer = _unitOFWork.GetRepository<Trainer>().GetById(trainerId);
            return trainer != null;
        }

        private bool IsCategoryExists(int categoryId)
        {
            var category = _unitOFWork.GetRepository<Category>().GetById(categoryId);
            return category != null;
        }

        private bool IsSessionTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && startDate > DateTime.UtcNow;
        }

        private bool isSessionAvailableForUpdate(Session session)
        {
            if (session is null)
                return false;

            if(session.EndDate < DateTime.UtcNow)
                return false;

            if (session.StartDate <= DateTime.UtcNow)
                return false;

            var HasActiveBookings = _unitOFWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (HasActiveBookings)
                return false;

            return true;

        }

        private bool isSessionAvailableForRemove(Session session)
        {
            if (session is null)
                return false;

            if (session.StartDate > DateTime.UtcNow)
                return false;

            if (session.StartDate <= DateTime.UtcNow && session.EndDate > DateTime.UtcNow)
                return false;

            var HasActiveBookings = _unitOFWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (HasActiveBookings)
                return false;

            return true;

        }

        #endregion
    }
}
