using SOAProject.DTOs;

namespace SOAProject.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
        Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employeeDTO);
        Task UpdateEmployeeAsync(int id, EmployeeDTO employeeDTO);
        Task DeleteEmployeeAsync(int id);
    }
}
