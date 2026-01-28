using GymManagementBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionVM> GetAllSessions();

        SessionVM? GetSessionById(int sessionId);

        bool CreateSession(CreateSessionVM model);

        bool UpdateSession(int sessionId, UpdateSessionVM model);

        UpdateSessionVM? GetSessionToUpdate(int sessionId);

        bool RemoveSession(int sessionId);

        IEnumerable<CategorySelectVM> GetCategoriesDropDown();

        IEnumerable<TrainerSelectVM> GetTrainersDropDown();
    }
}
