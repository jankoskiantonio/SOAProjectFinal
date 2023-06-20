using Microsoft.EntityFrameworkCore;
using SOAProject.Data;
using SOAProject.Models;

namespace SOAProject.Repositories.ManagerRepository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _context;
        public ManagerRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Manager>> GetAllManagersAsync()
        {
            return await _context.Managers.ToListAsync();
        }

        public async Task<Manager> GetManagerByIdAsync(int id)
        {
            return await _context.Managers.FindAsync(id);
        }

        public async Task AddManagerAsync(Manager manager)
        {
            await _context.Managers.AddAsync(manager);
        }

        public void UpdateManager(Manager manager)
        {
            _context.Entry(manager).State = EntityState.Modified;
        }

        public void DeleteManager(Manager manager)
        {
            _context.Managers.Remove(manager);
        }
    }
}
