using SOAProject.Models;

namespace SOAProject.Repositories.DepartmentRepository
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task AddDepartmentAsync (Department department);
        void UpdateDepartment (Department department);
        void DeleteDepartment (Department department);
    }
}
