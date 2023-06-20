using AutoMapper;
using Moq;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Repositories;
using SOAProject.Services.DepartmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemTest.DepartmentTests
{
    public class DepartmentServiceTest
    {
        [Fact]
        public async Task GetAllDepartmentsAsync_ReturnsAllDepartments()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            var departments = new List<Department>
            {
                new Department { DepartmentId = 1, Name = "Department 1", Location = "Location 1", City = "City 1" },
                new Department { DepartmentId = 2, Name = "Department 2", Location = "Location 2", City = "City 2" },
                new Department { DepartmentId = 3, Name = "Department 3", Location = "Location 3", City = "City 3" }
            };

            mockUnitOfWork.Setup(uow => uow.DepartmentRepository.GetAllDepartmentsAsync()).ReturnsAsync(departments);

            var departmentDTOs = new List<DepartmentDTO>
            {
                new DepartmentDTO { DepartmentId = 1, Name = "Department 1", Location = "Location 1", City = "City 1" },
                new DepartmentDTO { DepartmentId = 2, Name = "Department 2", Location = "Location 2", City = "City 2" },
                new DepartmentDTO { DepartmentId = 3, Name = "Department 3", Location = "Location 3", City = "City 3" }
            };

            mockMapper.Setup(mapper => mapper.Map<IEnumerable<DepartmentDTO>>(departments)).Returns(departmentDTOs);

            var departmentService = new DepartmentService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await departmentService.GetAllDepartmentsAsync();

            // Assert
            Assert.Equal(departmentDTOs, result);
        }
        [Fact]
        public async Task GetDepartmentByIdAsync_ExistingId_ReturnsDepartment()
        {
            // Arrange
            int departmentId = 1;
            var department = new Department { DepartmentId = departmentId, Name = "Department 1", Location = "Location 1", City = "City 1" };
            var departmentDTO = new DepartmentDTO { DepartmentId = department.DepartmentId, Name = department.Name, Location = department.Location, City = department.City };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.DepartmentRepository.GetDepartmentByIdAsync(departmentId)).ReturnsAsync(department);
            mockMapper.Setup(mapper => mapper.Map<DepartmentDTO>(department)).Returns(departmentDTO);

            var departmentService = new DepartmentService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await departmentService.GetDepartmentByIdAsync(departmentId);

            // Assert
            Assert.Equal(departmentDTO, result);
        }

        [Fact]
        public async Task CreateDepartmentAsync_ValidDepartment_ReturnsNewDepartment()
        {
            // Arrange
            var departmentDTO = new DepartmentDTO { Name = "New Department", Location = "New Location", City = "New City" };
            var department = new Department { DepartmentId = 1, Name = "New Department", Location = "New Location", City = "New City" };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(mapper => mapper.Map<Department>(departmentDTO)).Returns(department);
            mockUnitOfWork.Setup(uow => uow.DepartmentRepository.AddDepartmentAsync(department)).Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(0)).Verifiable();
            mockMapper.Setup(mapper => mapper.Map<DepartmentDTO>(department)).Returns(departmentDTO);

            var departmentService = new DepartmentService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await departmentService.CreateDepartmentAsync(departmentDTO);

            // Assert
            Assert.Equal(departmentDTO, result);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task UpdateDepartmentAsync_ExistingId_UpdatesDepartment()
        {
            // Arrange
            int departmentId = 1;
            var departmentDTO = new DepartmentDTO { DepartmentId = departmentId, Name = "Updated Department", Location = "Updated Location", City = "Updated City" };
            var existingDepartment = new Department { DepartmentId = departmentId, Name = "Department 1", Location = "Location 1", City = "City 1" };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.DepartmentRepository.GetDepartmentByIdAsync(departmentId)).ReturnsAsync(existingDepartment);
            mockMapper.Setup(mapper => mapper.Map(departmentDTO, existingDepartment));



            var departmentService = new DepartmentService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            await departmentService.UpdateDepartmentAsync(departmentId, departmentDTO);

            // Assert
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteDepartmentAsync_ExistingId_DeletesDepartment()
        {
            // Arrange
            int departmentId = 1;
            var existingDepartment = new Department { DepartmentId = departmentId, Name = "Department 1", Location = "Location 1", City = "City 1" };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.DepartmentRepository.GetDepartmentByIdAsync(departmentId)).ReturnsAsync(existingDepartment);
            mockUnitOfWork.Setup(uow => uow.DepartmentRepository.DeleteDepartment(existingDepartment));
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(0)).Verifiable();

            var departmentService = new DepartmentService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            await departmentService.DeleteDepartmentAsync(departmentId);

            // Assert
            mockUnitOfWork.Verify(uow => uow.DepartmentRepository.DeleteDepartment(existingDepartment), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int departmentId = 1;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.DepartmentRepository.GetDepartmentByIdAsync(departmentId)).ReturnsAsync((Department)null);

            var departmentService = new DepartmentService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await departmentService.GetDepartmentByIdAsync(departmentId);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task UpdateDepartmentAsync_NonExistingId_ThrowsException()
        {
            // Arrange
            int departmentId = 1;
            var departmentDTO = new DepartmentDTO { DepartmentId = departmentId, Name = "Updated Department", Location = "Updated Location", City = "Updated City" };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.DepartmentRepository.GetDepartmentByIdAsync(departmentId)).ReturnsAsync((Department)null);

            var departmentService = new DepartmentService(mockUnitOfWork.Object, mockMapper.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => departmentService.UpdateDepartmentAsync(departmentId, departmentDTO));

            mockUnitOfWork.Verify(uow => uow.DepartmentRepository.GetDepartmentByIdAsync(departmentId), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
    }
}
