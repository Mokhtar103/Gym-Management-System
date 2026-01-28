using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOFWork unitOFWork, IMapper mapper)
        {
            _unitOFWork = unitOFWork;
            _mapper = mapper;
        }

        public IEnumerable<MemberSessionVM> GetAllMembersForSession(int id)
        {
            var bookings = _unitOFWork.BookingRepository.GetSessionById(id);
            var memberSessions = _mapper.Map<IEnumerable<MemberSessionVM>>(bookings);
            return memberSessions;
        }

        public IEnumerable<SessionVM> GetAllSessionsWithTrainerAndCategory()
        {
            var sessions = _unitOFWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            var sessionVMs = _mapper.Map<IEnumerable<SessionVM>>(sessions);
            
            foreach (var session in sessionVMs)
            {
                var bookedCount = _unitOFWork.SessionRepository.GetCountOfBookedSlots(session.Id);
                session.AvailableSlots = session.Capacity - bookedCount;
            }

            return sessionVMs;  

        }
       

        public bool CreateBooking(CreateBookingVM model)
        {
            var session = _unitOFWork.SessionRepository.GetById(model.SessionId);

            if (session is null || session.StartDate <= DateTime.UtcNow)
                return false;

            var membershipRepo = _unitOFWork.MembershipRepository;
            var activeMembershipForMember = membershipRepo.GetFirstOrDefault(m => m.Status == "Active" && m.MemberId == model.MemberId);

            if (activeMembershipForMember is null)
                return false;

            var sessionRepo = _unitOFWork.SessionRepository;
            var bookedSlots = sessionRepo.GetCountOfBookedSlots(model.SessionId);

            var availableSlots = session.Capacity - bookedSlots;
            if (availableSlots <= 0)
                return false;

            var booking = _mapper.Map<Booking>(model);
            booking.IsAttended = false;

            _unitOFWork.BookingRepository.Add(booking);
            return _unitOFWork.SaveChanges() > 0;
        }

        public bool CancelBooking(MemberAttendanceVM model)
        {
            try
            {
                var session = _unitOFWork.SessionRepository.GetById(model.SessionId);
                if (session is null || session.StartDate <= DateTime.Now) return false;

                var Booking = _unitOFWork.BookingRepository.GetAll(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId)
                                                           .FirstOrDefault();
                if (Booking is null) return false;
                _unitOFWork.BookingRepository.Delete(Booking);
                return _unitOFWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }



        public IEnumerable<MemberForSelectListVM> GetMemberForDropdown(int id)
        {
            var bookingRepo = _unitOFWork.BookingRepository;
            var bookedMemberIds = bookingRepo.GetAll(s => s.Id == id)
                                             .Select(ms => ms.MemberId)
                                             .ToList();
            var membersAvailableToBook = _unitOFWork.GetRepository<Member>().GetAll(m => !bookedMemberIds.Contains(m.Id));

            var memberSelectList = _mapper.Map<IEnumerable<MemberForSelectListVM>>(membersAvailableToBook);

            return memberSelectList;
        }

        public bool MemberAttended(MemberAttendanceVM model)
        {
            try
            {
                var memberSession = _unitOFWork.GetRepository<Booking>()
                                           .GetAll(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId)
                                           .FirstOrDefault();
                if (memberSession is null) return false;

                memberSession.IsAttended = true;
                memberSession.UpdatedAt = DateTime.Now;
                _unitOFWork.GetRepository<Booking>().Update(memberSession);
                return _unitOFWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }
    }
}
