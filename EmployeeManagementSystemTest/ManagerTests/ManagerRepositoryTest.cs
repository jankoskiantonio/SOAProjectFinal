using SOAProject.Data;
using SOAProject.Models;
using SOAProject.Repositories.ManagerRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.AspNetCore.Mvc;
using SOAProject.DTOs;

namespace EmployeeManagementSystemTest.ManagerTests
{
    public class ManagerRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public ManagerRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetManagerByIdAsync_ReturnsExpectedItem()
        {
            //Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var expectedManager = new Manager
                {
                    ManagerId = 1,
                    FullName = "Manager Name 1",
                    Email = "testmanager1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    
                };
                context.Managers.Add(expectedManager);
                await context.SaveChangesAsync();

                var repository = new ManagerRepository(context);

                //Act
                var actualManager = await repository.GetManagerByIdAsync(1);

                //Assert
                Assert.Equal(expectedManager.ManagerId, actualManager.ManagerId);
                Assert.Equal(expectedManager.FullName, actualManager.FullName);
                Assert.Equal(expectedManager.Email, actualManager.Email);
                Assert.Equal(expectedManager.Phone, actualManager.Phone);
                Assert.Equal(expectedManager.HireDate, actualManager.HireDate);
                Assert.Equal(expectedManager.Salary, actualManager.Salary);
                Assert.Equal(expectedManager.DailyHours, actualManager.DailyHours);
                Assert.Equal(expectedManager.LeaveDays, actualManager.LeaveDays);
                Assert.Equal(expectedManager.DepartmentId, actualManager.DepartmentId);
            }

        }

        [Fact]
        public async Task GetAllManagersAsync_ReturnsAllManagers()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new ManagerRepository(context);
                var expectedManagers = new List<Manager>
                {
                    new Manager
                {
                    ManagerId = 1,
                    FullName = "Manager Name 1",
                    Email = "testmanager1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    
                },

                new Manager
                {
                    ManagerId = 2,
                    FullName = "Manager Name 2",
                    Email = "testmanager2@gmail.com",
                    Phone = "075500001",
                    Address = "Test Address 2",
                    HireDate = DateTime.Now,
                    Salary = 300001,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    
                },

                new Manager
                {
                    ManagerId = 3,
                    FullName = "Manager Name 3",
                    Email = "testmanager3@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 3",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    
                },
                };
                context.Managers.AddRange(expectedManagers);
                await context.SaveChangesAsync();

                // Act
                var actualManagers = await repository.GetAllManagersAsync();

                // Assert
                Assert.Equal(expectedManagers.Count, actualManagers.Count());
                foreach (var expectedManager in expectedManagers)
                {
                    Assert.Contains(expectedManager, actualManagers);
                }
            }
        }

        [Fact]
        public async Task AddManagerAsync_AddsManagerToContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new ManagerRepository(context);
                var manager = new Manager
                {
                    ManagerId = 1,
                    FullName = "Manager Name 1",
                    Email = "testmanager1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    
                };

                // Act
                await repository.AddManagerAsync(manager);
                await context.SaveChangesAsync();

                // Assert
                var addedManager = await context.Managers.FindAsync(manager.ManagerId);
                Assert.NotNull(addedManager);
                Assert.Equal(manager.FullName, addedManager.FullName);
            }
        }

        [Fact]
        public async Task UpdateManager_UpdatesManagerInContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new ManagerRepository(context);
                var manager = new Manager
                {
                    ManagerId = 1,
                    FullName = "Manager Name 1",
                    Email = "testmanager1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    
                };
                context.Managers.Add(manager);
                await context.SaveChangesAsync();

                // Act
                manager.FullName = "Updated Manager";
                repository.UpdateManager(manager);
                await context.SaveChangesAsync();

                // Assert
                var updatedManager = await context.Managers.FindAsync(manager.ManagerId);
                Assert.Equal(manager.FullName, updatedManager.FullName);
            }
        }

        [Fact]
        public async Task DeleteManager_RemovesManagerFromContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new ManagerRepository(context);
                var manager = new Manager
                {
                    ManagerId = 1,
                    FullName = "Manager Name 1",
                    Email = "testmanager1@gmail.com",
                    Phone = "075500000",
                    Address = "Test Address 1",
                    HireDate = DateTime.Now,
                    Salary = 300000,
                    DailyHours = 8,
                    LeaveDays = 10,
                    DepartmentId = 1,
                    
                };
                context.Managers.Add(manager);
                await context.SaveChangesAsync();

                // Act
                repository.DeleteManager(manager);
                await context.SaveChangesAsync();

                // Assert
                var deletedManager = await context.Managers.FindAsync(manager.ManagerId);
                Assert.Null(deletedManager);
            }
        }

        [Fact]
        public async Task GetManagerByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new ManagerRepository(context);

                // Act
                var result = await repository.GetManagerByIdAsync(1);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAllManagersAsync_ReturnsEmptyList_WhenNoManagersExist()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new ManagerRepository(context);

                // Act
                var result = await repository.GetAllManagersAsync();

                // Assert
                Assert.Empty(result);
            }
        }
        [Fact]
        public async Task AddManagerAsync_DoesNotAddManager_WhenNullManagerIsProvided()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new ManagerRepository(context);
                var initialCount = await context.Managers.CountAsync();

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                {
                    await repository.AddManagerAsync(null);
                    await context.SaveChangesAsync();
                });

                // Assert
                var finalCount = await context.Managers.CountAsync();
                Assert.Equal(initialCount, finalCount);
            }
        }


        [Fact]
        public async Task DeleteManager_DoesNotThrowException_WhenDeletingNonExistingManager()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new ManagerRepository(context);
                var manager = new Manager { ManagerId = 1 };

                // Act & Assert
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
                {
                    repository.DeleteManager(manager);
                    await context.SaveChangesAsync();
                });
            }
        }

    }
}