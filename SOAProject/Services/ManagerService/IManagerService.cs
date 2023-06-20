using SOAProject.DTOs;

namespace SOAProject.Services.ManagerService
{
    public interface IManagerService
    {
        Task<IEnumerable<ManagerDTO>> GetAllManagersAsync();
        Task<ManagerDTO> GetManagerByIdAsync(int id);
        Task<ManagerDTO> CreateManagerAsync(ManagerDTO managerDTO);
        Task UpdateManagerAsync(int id, ManagerDTO managerDTO);
        Task DeleteManagerAsync(int id);
    }
}
