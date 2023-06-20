using AutoMapper;
using Moq;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Repositories;
using SOAProject.Services.ManagerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagementSystemTest.ManagerTests
{
    public class ManagerServiceTest
    {
        [Fact]
        public async Task GetAllManagersAsync_ReturnsAllManagers()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            var managers = new List<Manager>
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
                    DepartmentId = 2,
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
                    DepartmentId = 3
                }
            };

            mockUnitOfWork.Setup(uow => uow.ManagerRepository.GetAllManagersAsync()).ReturnsAsync(managers);

            var managerDTOs = new List<ManagerDTO>
            {
                new ManagerDTO
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
                    DepartmentId = 1
                },
                new ManagerDTO
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
                    DepartmentId = 2,
                },

                new ManagerDTO
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
                    DepartmentId = 3
                }
            };

            mockMapper.Setup(mapper => mapper.Map<IEnumerable<ManagerDTO>>(managers)).Returns(managerDTOs);

            var managerService = new ManagerService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await managerService.GetAllManagersAsync();

            // Assert
            Assert.Equal(managerDTOs, result);
        }

        [Fact]
        public async Task GetManagerByIdAsync_ExistingId_ReturnsManager()
        {
            // Arrange
            int managerId = 1;
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
                DepartmentId = 1
            };
            var managerDTO = new ManagerDTO
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
                DepartmentId = 1
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.ManagerRepository.GetManagerByIdAsync(managerId)).ReturnsAsync(manager);
            mockMapper.Setup(mapper => mapper.Map<ManagerDTO>(manager)).Returns(managerDTO);

            var managerService = new ManagerService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await managerService.GetManagerByIdAsync(managerId);

            // Assert
            Assert.Equal(managerDTO, result);
        }

        [Fact]
        public async Task CreateManagerAsync_ValidManager_ReturnsNewManager()
        {
            // Arrange
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
                DepartmentId = 1
            };
            var managerDTO = new ManagerDTO
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
                DepartmentId = 1
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(mapper => mapper.Map<Manager>(managerDTO)).Returns(manager);
            mockUnitOfWork.Setup(uow => uow.ManagerRepository.AddManagerAsync(manager)).Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(0)).Verifiable();
            mockMapper.Setup(mapper => mapper.Map<ManagerDTO>(manager)).Returns(managerDTO);

            var managerService = new ManagerService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await managerService.CreateManagerAsync(managerDTO);

            // Assert
            Assert.Equal(managerDTO, result);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateManagerAsync_ExistingId_UpdatesManager()
        {
            // Arrange
            int managerId = 1;
            var existingManager = new Manager
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
                DepartmentId = 1
            };
            var managerDTO = new ManagerDTO
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
                DepartmentId = 1
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.ManagerRepository.GetManagerByIdAsync(managerId)).ReturnsAsync(existingManager);
            mockMapper.Setup(mapper => mapper.Map(managerDTO, existingManager));

            var managerService = new ManagerService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            await managerService.UpdateManagerAsync(managerId, managerDTO);

            // Assert
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteManagerAsync_ExistingId_DeletesManager()
        {
            // Arrange
            int managerId = 1;
            var existingManager = new Manager
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
                DepartmentId = 1
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.ManagerRepository.GetManagerByIdAsync(managerId)).ReturnsAsync(existingManager);
            mockUnitOfWork.Setup(uow => uow.ManagerRepository.DeleteManager(existingManager));
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(0)).Verifiable();

            var managerService = new ManagerService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            await managerService.DeleteManagerAsync(managerId);

            // Assert
            mockUnitOfWork.Verify(uow => uow.ManagerRepository.DeleteManager(existingManager), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetManagerByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int managerId = 1;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.ManagerRepository.GetManagerByIdAsync(managerId)).ReturnsAsync((Manager)null);

            var managerService = new ManagerService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await managerService.GetManagerByIdAsync(managerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateManagerAsync_NonExistingId_ThrowsException()
        {
            // Arrange
            int managerId = 1;
            var managerDTO = new ManagerDTO
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
                DepartmentId = 1
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.ManagerRepository.GetManagerByIdAsync(managerId)).ReturnsAsync((Manager)null);

            var managerService = new ManagerService(mockUnitOfWork.Object, mockMapper.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => managerService.UpdateManagerAsync(managerId, managerDTO));

            mockUnitOfWork.Verify(uow => uow.ManagerRepository.GetManagerByIdAsync(managerId), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
    }
}
