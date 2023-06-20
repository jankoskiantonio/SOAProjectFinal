using SOAProject.Data;
using SOAProject.Models;
using SOAProject.Repositories.DepartmentRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.AspNetCore.Mvc;
using SOAProject.DTOs;
using System.Xml.Linq;

namespace EmployeeManagementSystemTest.DepartmentTests
{
    public class DepartmentRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public DepartmentRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_ReturnsExpectedItem()
        {
            //Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var expectedDepartment = new Department
                {
                    DepartmentId = 1,
                    Name = "New Department",
                    Location = "New Location",
                    City = "New City"
                };
                
                context.Departments.Add(expectedDepartment);
                await context.SaveChangesAsync();

                var repository = new DepartmentRepository(context);

                //Act
                var actualDepartment = await repository.GetDepartmentByIdAsync(1);

                //Assert
                Assert.Equal(expectedDepartment.DepartmentId, actualDepartment.DepartmentId);
                Assert.Equal(expectedDepartment.Name, actualDepartment.Name);
                Assert.Equal(expectedDepartment.Location, actualDepartment.Location);
                Assert.Equal(expectedDepartment.City, actualDepartment.City);
            }

        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ReturnsAllDepartments()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new DepartmentRepository(context);
                var expectedDepartments = new List<Department>
                {
                    new Department
                {
                    DepartmentId = 1,
                    Name = "New Department 1",
                    Location = "New Location 1",
                    City = "New City 1"
                },

                new Department
                {
                    DepartmentId = 2,
                    Name = "New Department 2",
                    Location = "New Location 2",
                    City = "New City 2"
                },

                new Department
                {
                    DepartmentId = 3,
                    Name = "New Department 3",
                    Location = "New Location 3",
                    City = "New City 3"
                },
            };
                context.Departments.AddRange(expectedDepartments);
                await context.SaveChangesAsync();

                // Act
                var actualDepartments = await repository.GetAllDepartmentsAsync();

                // Assert
                Assert.Equal(expectedDepartments.Count, actualDepartments.Count());
                foreach (var expectedDepartment in expectedDepartments)
                {
                    Assert.Contains(expectedDepartment, actualDepartments);
                }
            }
        }

        [Fact]
        public async Task AddDepartmentAsync_AddsDepartmentToContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new DepartmentRepository(context);
                var department = new Department
                {
                    DepartmentId = 1,
                    Name = "New Department",
                    Location = "New Location",
                    City = "New City"
                };

                // Act
                await repository.AddDepartmentAsync(department);
                await context.SaveChangesAsync();

                // Assert
                var addedDepartment = await context.Departments.FindAsync(department.DepartmentId);
                Assert.NotNull(addedDepartment);
                Assert.Equal(department.DepartmentId, addedDepartment.DepartmentId);
                Assert.Equal(department.Name, addedDepartment.Name);
                Assert.Equal(department.Location, addedDepartment.Location);
                Assert.Equal(department.City, addedDepartment.City);
            }
        }

        [Fact]
        public async Task UpdateDepartment_UpdatesDepartmentInContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new DepartmentRepository(context);
                var department = new Department
                {
                    DepartmentId = 1,
                    Name = "New Department",
                    Location = "New Location",
                    City = "New City"
                };
                context.Departments.Add(department);
                await context.SaveChangesAsync();

                // Act
                department.Name = "Updated Department";
                repository.UpdateDepartment(department);
                await context.SaveChangesAsync();

                // Assert
                var updatedDepartment = await context.Departments.FindAsync(department.DepartmentId);
                Assert.Equal(department.Name, updatedDepartment.Name);
            }
        }

        [Fact]
        public async Task DeleteDepartment_RemovesDepartmentFromContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new DepartmentRepository(context);
                var department = new Department
                {
                    DepartmentId = 1,
                    Name = "New Department",
                    Location = "New Location",
                    City = "New City"
                };
                context.Departments.Add(department);
                await context.SaveChangesAsync();

                // Act
                repository.DeleteDepartment(department);
                await context.SaveChangesAsync();

                // Assert
                var deletedDepartment = await context.Departments.FindAsync(department.DepartmentId);
                Assert.Null(deletedDepartment);
            }
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new DepartmentRepository(context);

                // Act
                var result = await repository.GetDepartmentByIdAsync(1);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ReturnsEmptyList_WhenNoDepartmentsExist()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new DepartmentRepository(context);

                // Act
                var result = await repository.GetAllDepartmentsAsync();

                // Assert
                Assert.Empty(result);
            }
        }
        [Fact]
        public async Task AddDepartmentAsync_DoesNotAddDepartment_WhenNullDepartmentIsProvided()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new DepartmentRepository(context);
                var initialCount = await context.Departments.CountAsync();

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                {
                    await repository.AddDepartmentAsync(null);
                    await context.SaveChangesAsync();
                });

                // Assert
                var finalCount = await context.Departments.CountAsync();
                Assert.Equal(initialCount, finalCount);
            }
        }


        [Fact]
        public async Task DeleteDepartment_DoesNotThrowException_WhenDeletingNonExistingDepartment()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var repository = new DepartmentRepository(context);
                var department = new Department { DepartmentId = 1 };

                // Act & Assert
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
                {
                    repository.DeleteDepartment(department);
                    await context.SaveChangesAsync();
                });
            }
        }

    }
}