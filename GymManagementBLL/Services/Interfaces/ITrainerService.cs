using GymManagementBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<GetAllTrainersVM> GetAllTrainers();

        bool CreateTrainer(CreatedTrainerVM model);

        bool UpdateTrainer(int trainerId, UpdatedTrainerVM model);

        bool RemoveTrainer(int trainerId);

        UpdatedTrainerVM? GetTrainerForUpdate(int trainerId);

        GetTrainerDetailsVM? GetTrainerDetails(int trainerId);
    }
}
