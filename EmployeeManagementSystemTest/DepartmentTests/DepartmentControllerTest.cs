using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SOAProject.Controllers;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Services.DepartmentService;
using SOAProject.Services.EmployeeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemTest.DepartmentTests
{
    public class DepartmentControllerTests
    {
        private readonly Mock<IDepartmentService> _mockDepartmentService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DepartmentController _controller;

        public DepartmentControllerTests()
        {
            _mockDepartmentService = new Mock<IDepartmentService>();
            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(m => m.Map<IEnumerable<DepartmentDTO>>(It.IsAny<IEnumerable<DepartmentDTO>>()))
                .Returns((IEnumerable<DepartmentDTO> source) => source.Select(d => new DepartmentDTO
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                    Location = d.Location,
                    City = d.City
   
                }).ToList());

            _controller = new DepartmentController(_mockDepartmentService.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ReturnsOkResultWithDepartmentDTOs()
        {
            // Arrange
            var expectedDepartments = new List<DepartmentDTO>
    {
        new DepartmentDTO
        {
            DepartmentId = 1,
            Name = "Department 1",
            Location = "Location 1",
            City = "City 1"
        },
        new DepartmentDTO
        {
            DepartmentId = 2,
            Name = "Department 2",
            Location = "Location 2",
            City = "City 2"
        },
        new DepartmentDTO
        {
            DepartmentId = 3,
            Name = "Department 3",
            Location = "Location 3",
            City = "City 3"
        }
    };

            _mockDepartmentService.Setup(service => service.GetAllDepartmentsAsync())
            .ReturnsAsync(expectedDepartments);

            var controller = new DepartmentController(_mockDepartmentService.Object, _mapperMock.Object);

            // Act
            var result = await controller.GetAllDepartmentsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualDepartments = Assert.IsAssignableFrom<List<DepartmentDTO>>(okResult.Value);

            Assert.Equal(expectedDepartments.Count, actualDepartments.Count);

            for (int i = 0; i < expectedDepartments.Count; i++)
            {
                Assert.Equal(expectedDepartments[i].DepartmentId, actualDepartments[i].DepartmentId);
                Assert.Equal(expectedDepartments[i].Name, actualDepartments[i].Name);
                Assert.Equal(expectedDepartments[i].Location, actualDepartments[i].Location);
                Assert.Equal(expectedDepartments[i].City, actualDepartments[i].City);
            }
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_ReturnsExpectedDepartment()
        {
            // Arrange
            var mockDepartmentService = new Mock<IDepartmentService>();
            var expectedDepartment = new DepartmentDTO
            {
                DepartmentId = 1,
                Name = "Test Department",
                Location = "Test Location",
                City = "Test City"
            };
            mockDepartmentService.Setup(service => service.GetDepartmentByIdAsync(1)).ReturnsAsync(expectedDepartment);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<DepartmentDTO>(It.IsAny<object>())).Returns(expectedDepartment);

            var controller = new DepartmentController(mockDepartmentService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetDepartmentByIdAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualDepartment = Assert.IsType<DepartmentDTO>(okResult.Value);
            Assert.Equal(expectedDepartment.DepartmentId, actualDepartment.DepartmentId);
            Assert.Equal(expectedDepartment.Name, actualDepartment.Name);
            Assert.Equal(expectedDepartment.Location, actualDepartment.Location);
            Assert.Equal(expectedDepartment.City, actualDepartment.City);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int departmentId = 1;
            DepartmentDTO departmentDTO = null;

            _mockDepartmentService.Setup(service => service.GetDepartmentByIdAsync(departmentId)).ReturnsAsync(departmentDTO);

            // Act
            var result = await _controller.GetDepartmentByIdAsync(departmentId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateDepartmentAsync_ValidEmployee_ReturnsOkResult()
        {
            // Arrange
            var departmentDTO = new DepartmentDTO
            {
                DepartmentId = 1,
                Name= "Dept 1",
                Location = "Loc 1",
                City = "Doe",
            };

            _mockDepartmentService.Setup(service => service.CreateDepartmentAsync(departmentDTO)).Verifiable();
            _mapperMock.Setup(mapper => mapper.Map<DepartmentDTO>(departmentDTO)).Returns(departmentDTO);

            var controller = new DepartmentController(_mockDepartmentService.Object, _mapperMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, "hrmanager1@gmail.com"),
        // Add other claims if authorization logic depends on them
    }, "mock"));

            // Act
            var result = await controller.CreateDepartmentAsync(departmentDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var newDepartmentDTO = Assert.IsAssignableFrom<DepartmentDTO>(okResult.Value);

            _mockDepartmentService.Verify(service => service.CreateDepartmentAsync(departmentDTO), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<DepartmentDTO>(departmentDTO), Times.Once);
            Assert.Equal(departmentDTO, newDepartmentDTO);
        }

        [Fact]
        public async Task UpdateDepartmentAsync_ExistingIdAndValidDepartment_ReturnsNoContentResult()
        {
            // Arrange
            int departmentId = 1;
            var departmentDTO = new DepartmentDTO
            {
                DepartmentId = departmentId,
                Name = "Updated Department",
                Location = "Updated Location",
                City = "Updated City"
            };

            _mockDepartmentService.Setup(service => service.UpdateDepartmentAsync(departmentId, departmentDTO)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateDepartmentAsync(departmentId, departmentDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateDepartmentAsync_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int departmentId = 1;
            var departmentDTO = new DepartmentDTO
            {
                DepartmentId = departmentId,
                Name = "Updated Department",
                Location = "Updated Location",
                City = "Updated City"
            };

            _mockDepartmentService.Setup(service => service.UpdateDepartmentAsync(departmentId, departmentDTO))
                .Throws<InvalidOperationException>();

            // Act
            var result = await _controller.UpdateDepartmentAsync(departmentId, departmentDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteDepartmentAsync_ExistingId_ReturnsNoContentResult()
        {
            // Arrange
            int departmentId = 1;
            var department = new Department { DepartmentId = departmentId, Name = "Department 1", Location = "Location 1", City = "City 1" };

            _mockDepartmentService.Setup(service => service.GetDepartmentByIdAsync(departmentId)).ReturnsAsync(new DepartmentDTO());
            _mockDepartmentService.Setup(service => service.DeleteDepartmentAsync(departmentId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteDepartmentAsync(departmentId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteDepartmentAsync_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int departmentId = 1;
            DepartmentDTO departmentDTO = null;

            _mockDepartmentService.Setup(service => service.GetDepartmentByIdAsync(departmentId)).ReturnsAsync(departmentDTO);

            // Act
            var result = await _controller.DeleteDepartmentAsync(departmentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
