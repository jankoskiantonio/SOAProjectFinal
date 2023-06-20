using Microsoft.EntityFrameworkCore.Storage;
using SOAProject.Data;
using SOAProject.Repositories.DepartmentRepository;
using SOAProject.Repositories.EmployeeRepository;
using SOAProject.Repositories.JobRepository;
using SOAProject.Repositories.ManagerRepository;

namespace SOAProject.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IEmployeeRepository _employeeRepository;
        private IDepartmentRepository _departmentRepository;
        private IJobRepository _jobRepository;
        private IManagerRepository _managerRepository;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _employeeRepository = new EmployeeRepository.EmployeeRepository(_context);
            _departmentRepository = new DepartmentRepository.DepartmentRepository(_context);
            _jobRepository = new JobRepository.JobRepository(_context);
            _managerRepository = new ManagerRepository.ManagerRepository(_context);
        }

        public IEmployeeRepository EmployeeRepository => _employeeRepository;
        public IDepartmentRepository DepartmentRepository => _departmentRepository;
        public IJobRepository JobRepository => _jobRepository;
        public IManagerRepository ManagerRepository => _managerRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
