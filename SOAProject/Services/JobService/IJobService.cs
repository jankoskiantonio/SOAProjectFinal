using SOAProject.DTOs;

namespace SOAProject.Services.JobService
{
    public interface IJobService
    {
        Task<IEnumerable<JobDTO>> GetAllJobsAsync();
        Task<JobDTO> GetJobByIdAsync(int id);
        Task<JobDTO> CreateJobAsync(JobDTO jobDTO);
        Task UpdateJobAsync(int id, JobDTO jobDTO);
        Task DeleteJobAsync(int id);
    }
}
