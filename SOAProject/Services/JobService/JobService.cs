using AutoMapper;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Repositories;

namespace SOAProject.Services.JobService
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobDTO>> GetAllJobsAsync()
        {
            var jobs = await _unitOfWork.JobRepository.GetAllJobsAsync();
            return _mapper.Map<IEnumerable<JobDTO>>(jobs);
        }

        public async Task<JobDTO> GetJobByIdAsync(int id)
        {
            var job = await _unitOfWork.JobRepository.GetJobByIdAsync(id);
            return _mapper.Map<JobDTO>(job);
        }

        public async Task<JobDTO> CreateJobAsync(JobDTO jobDTO)
        {
            try
            {
                var job = _mapper.Map<Job>(jobDTO);
                _unitOfWork.JobRepository.AddJobAsync(job);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<JobDTO>(job);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task UpdateJobAsync(int id, JobDTO jobDTO)
        {
            try
            {
                var job = await _unitOfWork.JobRepository.GetJobByIdAsync(id);
                if (job == null)
                {
                    throw new ArgumentException("Job not found.");
                }
                _mapper.Map(jobDTO, job);
                await _unitOfWork.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _unitOfWork.JobRepository.GetJobByIdAsync(id);
            if (job == null)
            {
                throw new ArgumentException("Job not found.");
            }
            _unitOfWork.JobRepository.DeleteJob(job);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
