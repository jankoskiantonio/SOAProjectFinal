using AutoMapper;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Repositories;

namespace SOAProject.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllDepartmentsAsync();
            return _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
        }

        public async Task<DepartmentDTO> GetDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetDepartmentByIdAsync(id);
            return _mapper.Map<DepartmentDTO>(department);
        }

        public async Task<DepartmentDTO> CreateDepartmentAsync(DepartmentDTO departmentDTO)
        {
            try
            {
                var department = _mapper.Map<Department>(departmentDTO);
                _unitOfWork.DepartmentRepository.AddDepartmentAsync(department);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<DepartmentDTO>(department);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task UpdateDepartmentAsync(int id, DepartmentDTO departmentDTO)
        {
            try
            {
                var department = await _unitOfWork.DepartmentRepository.GetDepartmentByIdAsync(id);
                if (department == null)
                {
                    throw new ArgumentException("Department not found.");
                }
                _mapper.Map(departmentDTO, department);
                await _unitOfWork.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                throw new ArgumentException("Department not found.");
            }
            _unitOfWork.DepartmentRepository.DeleteDepartment(department);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
