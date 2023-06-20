using Microsoft.EntityFrameworkCore;
using SOAProject.Data;
using SOAProject.Models;

namespace SOAProject.Repositories.JobRepository
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;
        public JobRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task<Job> GetJobByIdAsync(int id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public async Task AddJobAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
        }

        public void UpdateJob(Job job)
        {
            _context.Entry(job).State = EntityState.Modified;
        }

        public void DeleteJob(Job job)
        {
            _context.Jobs.Remove(job);
        }
    }
}
