using AutoMapper;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Repositories;

namespace SOAProject.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAllEmployeesAsync();
            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(id);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employeeDTO)
        {
            try
            {
                var employee = _mapper.Map<Employee>(employeeDTO);
                _unitOfWork.EmployeeRepository.AddEmployeeAsync(employee);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<EmployeeDTO>(employee);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task UpdateEmployeeAsync(int id, EmployeeDTO employeeDTO)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    throw new ArgumentException("Employee not found.");
                }
                _mapper.Map(employeeDTO, employee);
                await _unitOfWork.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(id);
            if(employee == null)
            {
                throw new ArgumentException("Employee not found.");
            }
            _unitOfWork.EmployeeRepository.DeleteEmployee(employee);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
