using SOAProject.Repositories.DepartmentRepository;
using SOAProject.Repositories.EmployeeRepository;
using SOAProject.Repositories.JobRepository;
using SOAProject.Repositories.ManagerRepository;

namespace SOAProject.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IJobRepository JobRepository { get; }
        IManagerRepository ManagerRepository { get; }

        Task<int> SaveChangesAsync();

        Task RollbackTransactionAsync();
    }
}
