using SOAProject.Models;

namespace SOAProject.Repositories.ManagerRepository
{
    public interface IManagerRepository
    {
        Task<IEnumerable<Manager>> GetAllManagersAsync();
        Task<Manager> GetManagerByIdAsync(int id);
        Task AddManagerAsync(Manager manager);
        void UpdateManager(Manager manager);
        void DeleteManager(Manager manager);
    }
}
