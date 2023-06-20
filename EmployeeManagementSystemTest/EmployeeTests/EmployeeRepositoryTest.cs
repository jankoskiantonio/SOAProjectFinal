using SOAProject.Data;
using SOAProject.Models;
using SOAProject.Repositories.EmployeeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.AspNetCore.Mvc;
using SOAProject.DTOs;

namespace EmployeeManagementSystemTest.EmployeeTests
{
    public class EmployeeRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public EmployeeRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ReturnsExpectedItem()
        {
            //Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var expectedEmployee = new Employee
                {
                    EmployeeId = 1,
                    FullName = "Employee Name 1",
                    Email = "testemployee1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    JobId = 1,
                };
                context.Employees.Add(expectedEmployee);
                await context.SaveChangesAsync();

                var repository = new EmployeeRepository(context);

                //Act
                var actualEmployee = await repository.GetEmployeeByIdAsync(1);

                //Assert
                Assert.Equal(expectedEmployee.EmployeeId, actualEmployee.EmployeeId);
                Assert.Equal(expectedEmployee.FullName, actualEmployee.FullName);
                Assert.Equal(expectedEmployee.Email, actualEmployee.Email);
                Assert.Equal(expectedEmployee.Phone, actualEmployee.Phone);
                Assert.Equal(expectedEmployee.HireDate, actualEmployee.HireDate);
                Assert.Equal(expectedEmployee.Salary, actualEmployee.Salary);
                Assert.Equal(expectedEmployee.DailyHours, actualEmployee.DailyHours);
                Assert.Equal(expectedEmployee.LeaveDays, actualEmployee.LeaveDays);
                Assert.Equal(expectedEmployee.DepartmentId, actualEmployee.DepartmentId);
                Assert.Equal(expectedEmployee.JobId, actualEmployee.JobId);
            }

        }

        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsAllEmployees()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new EmployeeRepository(context);
                var expectedEmployees = new List<Employee>
                {
                    new Employee
                {
                    EmployeeId = 1,
                    FullName = "Employee Name 1",
                    Email = "testemployee1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    JobId = 1,
                },

                new Employee
                {
                    EmployeeId = 2,
                    FullName = "Employee Name 2",
                    Email = "testemployee2@gmail.com",
                    Phone = "075500001",
                    Address = "Test Address 2",
                    HireDate = DateTime.Now,
                    Salary = 300001,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    JobId = 1,
                },

                new Employee
                {
                    EmployeeId = 3,
                    FullName = "Employee Name 3",
                    Email = "testemployee3@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 3",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    JobId = 1,
                },
                };
                context.Employees.AddRange(expectedEmployees);
                await context.SaveChangesAsync();

                // Act
                var actualEmployees = await repository.GetAllEmployeesAsync();

                // Assert
                Assert.Equal(expectedEmployees.Count, actualEmployees.Count());
                foreach (var expectedEmployee in expectedEmployees)
                {
                    Assert.Contains(expectedEmployee, actualEmployees);
                }
            }
        }

        [Fact]
        public async Task AddEmployeeAsync_AddsEmployeeToContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new EmployeeRepository(context);
                var employee = new Employee
                {
                    EmployeeId = 1,
                    FullName = "Employee Name 1",
                    Email = "testemployee1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    JobId = 1,
                };

                // Act
                await repository.AddEmployeeAsync(employee);
                await context.SaveChangesAsync();

                // Assert
                var addedEmployee = await context.Employees.FindAsync(employee.EmployeeId);
                Assert.NotNull(addedEmployee);
                Assert.Equal(employee.FullName, addedEmployee.FullName);
            }
        }

        [Fact]
        public async Task UpdateEmployee_UpdatesEmployeeInContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new EmployeeRepository(context);
                var employee = new Employee
                {
                    EmployeeId = 1,
                    FullName = "Employee Name 1",
                    Email = "testemployee1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    JobId = 1,
                };
                context.Employees.Add(employee);
                await context.SaveChangesAsync();

                // Act
                employee.FullName = "Updated Employee";
                repository.UpdateEmployee(employee);
                await context.SaveChangesAsync();

                // Assert
                var updatedEmployee = await context.Employees.FindAsync(employee.EmployeeId);
                Assert.Equal(employee.FullName, updatedEmployee.FullName);
            }
        }

        [Fact]
        public async Task DeleteEmployee_RemovesEmployeeFromContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new EmployeeRepository(context);
                var employee = new Employee
                {
                    EmployeeId = 1,
                    FullName = "Employee Name 1",
                    Email = "testemployee1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    JobId = 1,
                };
                context.Employees.Add(employee);
                await context.SaveChangesAsync();

                // Act
                repository.DeleteEmployee(employee);
                await context.SaveChangesAsync();

                // Assert
                var deletedEmployee = await context.Employees.FindAsync(employee.EmployeeId);
                Assert.Null(deletedEmployee);
            }
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new EmployeeRepository(context);

                // Act
                var result = await repository.GetEmployeeByIdAsync(1);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsEmptyList_WhenNoEmployeesExist()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new EmployeeRepository(context);

                // Act
                var result = await repository.GetAllEmployeesAsync();

                // Assert
                Assert.Empty(result);
            }
        }
        [Fact]
        public async Task AddEmployeeAsync_DoesNotAddEmployee_WhenNullEmployeeIsProvided()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new EmployeeRepository(context);
                var initialCount = await context.Employees.CountAsync();

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                {
                    await repository.AddEmployeeAsync(null);
                    await context.SaveChangesAsync();
                });

                // Assert
                var finalCount = await context.Employees.CountAsync();
                Assert.Equal(initialCount, finalCount);
            }
        }


        [Fact]
        public async Task DeleteEmployee_DoesNotThrowException_WhenDeletingNonExistingEmployee()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new EmployeeRepository(context);
                var employee = new Employee { EmployeeId = 1 };

                // Act & Assert
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
                {
                    repository.DeleteEmployee(employee);
                    await context.SaveChangesAsync();
                });
            }
        }

    }
}