using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SOAProject.Controllers;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Services.ManagerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemTest.ManagerTests
{
    public class ManagerControllerTests
    {
        private readonly Mock<IManagerService> _managerServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ManagerController _controller;

        public ManagerControllerTests()
        {
            _managerServiceMock = new Mock<IManagerService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ManagerController(_managerServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllManagersAsync_ReturnsOkResult()
        {
            // Arrange
            var expectedManagers = new List<ManagerDTO>();
            _managerServiceMock.Setup(service => service.GetAllManagersAsync()).ReturnsAsync(expectedManagers);

            // Act
            var result = await _controller.GetAllManagersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualManagers = Assert.IsAssignableFrom<IEnumerable<ManagerDTO>>(okResult.Value);
            Assert.Equal(expectedManagers.Count, actualManagers.Count());
        }

        [Fact]
        public async Task UpdateManagerAsync_ValidData_ReturnsNoContentResult()
        {
            // Arrange
            int managerId = 1;
            var managerDTO = new ManagerDTO
            {
                ManagerId = managerId,
                FullName = "John Doe",
                Email = "johndoe@example.com",
                Phone = "1234567890",
                Address = "123 Main Street",
                HireDate = DateTime.Now,
                Salary = 50000,
                DailyHours = 8,
                LeaveDays = 10,
                DepartmentId = 1
            };

            _managerServiceMock.Setup(service => service.UpdateManagerAsync(managerId, managerDTO)).Verifiable();

            // Act
            var result = await _controller.UpdateManagerAsync(managerId, managerDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _managerServiceMock.Verify(service => service.UpdateManagerAsync(managerId, managerDTO), Times.Once);
        }

        [Fact]
        public async Task GetManagerByIdAsync_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int managerId = 1;
            _managerServiceMock.Setup(service => service.GetManagerByIdAsync(managerId)).ReturnsAsync((ManagerDTO)null);

            // Act
            var result = await _controller.GetManagerByIdAsync(managerId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateManagerAsync_ValidManager_ReturnsOkResult()
        {
            // Arrange
            var managerDTO = new ManagerDTO();
            _managerServiceMock.Setup(service => service.CreateManagerAsync(managerDTO)).ReturnsAsync(managerDTO);
            _mapperMock.Setup(mapper => mapper.Map<ManagerDTO>(It.IsAny<ManagerDTO>())).Returns(managerDTO);

            // Act
            var result = await _controller.CreateManagerAsync(managerDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualManagerDTO = Assert.IsAssignableFrom<ManagerDTO>(okResult.Value);
            Assert.Same(managerDTO, actualManagerDTO);
        }

        [Fact]
        public async Task UpdateManagerAsync_ExistingId_ReturnsNoContent()
        {
            // Arrange
            int managerId = 1;
            var managerDTO = new ManagerDTO { ManagerId = managerId };
            _managerServiceMock.Setup(service => service.UpdateManagerAsync(managerId, managerDTO)).Verifiable();
            _mapperMock.Setup(mapper => mapper.Map<ManagerDTO>(It.IsAny<ManagerDTO>())).Returns(managerDTO);

            // Act
            var result = await _controller.UpdateManagerAsync(managerId, managerDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _managerServiceMock.Verify(service => service.UpdateManagerAsync(managerId, managerDTO), Times.Once);


        }
    }
}
