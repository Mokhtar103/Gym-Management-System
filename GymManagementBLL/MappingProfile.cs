using AutoMapper;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
            MapMembership();
            MapBooking();
        }

        private void MapSession()
        {
            CreateMap<Session, SessionVM>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());

            CreateMap<CreateSessionVM, Session>();
            CreateMap<UpdateSessionVM, Session>().ReverseMap();

            CreateMap<Category, CategorySelectVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<Trainer, TrainerSelectVM>();


        }

        private void MapMembership()
        {
            CreateMap<Membership, MembershipVM>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<CreateMembershipVM, Membership>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<Plan, PlanForSelectListVM>();
            CreateMap<Member, MemberForSelectListVM>();
        }

        private void MapBooking()
        {
            CreateMap<Booking, MemberSessionVM>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreatedAt.ToString()));

            CreateMap<CreateBookingVM, Booking>();
        }
    }
}
