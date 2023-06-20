using SOAProject.DTOs;

namespace SOAProject.Services.DepartmentService
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();
        Task<DepartmentDTO> GetDepartmentByIdAsync(int id);
        Task<DepartmentDTO> CreateDepartmentAsync(DepartmentDTO departmentDTO);
        Task UpdateDepartmentAsync(int id, DepartmentDTO departmentDTO);
        Task DeleteDepartmentAsync(int id);
    }
}
