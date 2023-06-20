using SOAProject.Models;

namespace SOAProject.Repositories.JobRepository
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task<Job> GetJobByIdAsync(int id);
        Task AddJobAsync(Job job);
        void UpdateJob(Job job);
        void DeleteJob(Job job);
    }
}
