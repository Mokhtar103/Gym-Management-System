using AutoMapper.Execution;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Entities.Enums;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOFWork _unitOFWork;

        public TrainerService(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

        public bool CreateTrainer(CreatedTrainerVM model)
        {
            try
            {
                if(IsEmailExists(model.Email) || IsPhoneExists(model.Email)) 
                    return false;

                var trainer = new Trainer
                {
                    
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Address = new Address
                    {
                        BuildingNumber = model.BuildingNumber,
                        City = model.City,
                        Street = model.Street
                    },
                    Specialities = model.SelectedSpecialities,

                };

                _unitOFWork.GetRepository<Trainer>().Add(trainer);

                return _unitOFWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public IEnumerable<GetAllTrainersVM> GetAllTrainers()
        {
            var trainer = _unitOFWork.GetRepository<Trainer>().GetAll();

            if (trainer is null || !trainer.Any())
            {
                return [];
            }

            var trainerVM = trainer.Select( t => new GetAllTrainersVM {
               Id = t.Id,
               Name = t.Name,
               Email = t.Email,
               Phone = t.Phone,
               Specialities = t.Specialities.ToString(),

            });

            return trainerVM;
        }

        public GetTrainerDetailsVM? GetTrainerDetails(int trainerId)
        {
            var trainer = _unitOFWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null)
                return null;

            var trainerVM = new GetTrainerDetailsVM
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialities = trainer.Specialities,
                DateOfBirth = trainer.DateOfBirth,
                Address = FormatAddress(trainer.Address)
            };
            return trainerVM;
        }


        public bool UpdateTrainer(int trainerId, UpdatedTrainerVM model)
        {
            var trainer = _unitOFWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null)
                return false;

            var emailExist = _unitOFWork.GetRepository<Trainer>()
                                .GetAll(m => m.Email == model.Email && m.Id != trainerId);

            var phoneExist = _unitOFWork.GetRepository<Trainer>()
                                .GetAll(m => m.Phone == model.Phone && m.Id != trainerId);

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address = new Address
            {
                BuildingNumber = model.BuildingNumber,
                City = model.City,
                Street = model.Street
            };
            trainer.Specialities = model.SelectedSpecialities;
            trainer.UpdatedAt = DateTime.Now;

            _unitOFWork.GetRepository<Trainer>().Update(trainer);


            return _unitOFWork.SaveChanges() > 0;




        }

        public UpdatedTrainerVM? GetTrainerForUpdate(int trainerId)
        {
            var trainer = _unitOFWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null)
                return null;

            var trainerToUpdateVM = new UpdatedTrainerVM
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
                SelectedSpecialities = trainer.Specialities

            };

            return trainerToUpdateVM;
        }

        public bool RemoveTrainer(int trainerId)
        {
            var trainer = _unitOFWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null)
                return false;

            var FutureSessions = _unitOFWork.GetRepository<Session>()
                                    .GetAll(s => s.TrainerId == trainerId && s.StartDate >= DateTime.Now);

            if (FutureSessions is not null && FutureSessions.Any())
                return false;

            var sessions = _unitOFWork.GetRepository<Session>()
                                    .GetAll(s => s.TrainerId == trainerId);
            try
            {

                if (sessions.Any())
                {
                    foreach (var session in sessions)
                    {
                        _unitOFWork.GetRepository<Session>().Delete(session);
                    }
                }
                _unitOFWork.GetRepository<Trainer>().Delete(trainer);
                return _unitOFWork.SaveChanges() > 0;

            }
            catch (Exception)
            {

                throw;
            }


        }

        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            var member = _unitOFWork.GetRepository<Trainer>().GetAll(m => m.Email == email);
            return member is not null && member.Any();
        }
        private bool IsPhoneExists(string phone)
        {
            var member = _unitOFWork.GetRepository<Trainer>().GetAll(m => m.Phone == phone);
            return member is not null && member.Any();
        }

        private string FormatAddress(Address address)
        {
            if (address is null)
                return string.Empty;
            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }


        #endregion
    }
}
