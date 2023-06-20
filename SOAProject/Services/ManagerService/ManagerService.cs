using AutoMapper;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Repositories;

namespace SOAProject.Services.ManagerService
{
    public class ManagerService : IManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ManagerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ManagerDTO>> GetAllManagersAsync()
        {
            var managers = await _unitOfWork.ManagerRepository.GetAllManagersAsync();
            return _mapper.Map<IEnumerable<ManagerDTO>>(managers);
        }

        public async Task<ManagerDTO> GetManagerByIdAsync(int id)
        {
            var manager = await _unitOfWork.ManagerRepository.GetManagerByIdAsync(id);
            return _mapper.Map<ManagerDTO>(manager);
        }

        public async Task<ManagerDTO> CreateManagerAsync(ManagerDTO managerDTO)
        {
            try
            {
                var manager = _mapper.Map<Manager>(managerDTO);
                _unitOfWork.ManagerRepository.AddManagerAsync(manager);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ManagerDTO>(manager);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task UpdateManagerAsync(int id, ManagerDTO managerDTO)
        {
            try
            {
                var manager = await _unitOfWork.ManagerRepository.GetManagerByIdAsync(id);
                if (manager == null)
                {
                    throw new ArgumentException("Manager not found.");
                }
                _mapper.Map(managerDTO, manager);
                await _unitOfWork.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteManagerAsync(int id)
        {
            var manager = await _unitOfWork.ManagerRepository.GetManagerByIdAsync(id);
            if (manager == null)
            {
                throw new ArgumentException("Manager not found.");
            }
            _unitOfWork.ManagerRepository.DeleteManager(manager);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
