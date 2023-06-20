using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SOAProject.Controllers;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Services.EmployeeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemTest.EmployeeTests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<IEnumerable<EmployeeDTO>>(It.IsAny<IEnumerable<EmployeeDTO>>()))
                .Returns((IEnumerable<EmployeeDTO> source) => source.Select(e => new EmployeeDTO
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    HireDate = e.HireDate,
                    Salary = e.Salary,
                    DailyHours = e.DailyHours,
                    LeaveDays = e.LeaveDays,
                    DepartmentId = e.DepartmentId,
                    JobId = e.JobId,
                }).ToList());

            _controller = new EmployeeController(_mockEmployeeService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsOkResultWithEmployeeDTOs()
        {
            // Arrange
            var mockEmployeeService = new Mock<IEmployeeService>();
            var expectedEmployees = new List<EmployeeDTO>
{
                new EmployeeDTO
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

                new EmployeeDTO
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

                new EmployeeDTO
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
            mockEmployeeService.Setup(x => x.GetAllEmployeesAsync())
                           .ReturnsAsync(expectedEmployees);

            var controller = new EmployeeController(mockEmployeeService.Object, _mockMapper.Object);

            // Act
            var result = await controller.GetAllEmployeesAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualEmployees = Assert.IsType<List<EmployeeDTO>>(okResult.Value);

            Assert.Equal(expectedEmployees.Count, actualEmployees.Count);

            for (int i = 0; i < expectedEmployees.Count; i++)
            {
                Assert.Equal(expectedEmployees[i].EmployeeId, actualEmployees[i].EmployeeId);
                Assert.Equal(expectedEmployees[i].FullName, actualEmployees[i].FullName);
                Assert.Equal(expectedEmployees[i].Email, actualEmployees[i].Email);
                Assert.Equal(expectedEmployees[i].Phone, actualEmployees[i].Phone);
                Assert.Equal(expectedEmployees[i].HireDate, actualEmployees[i].HireDate);
                Assert.Equal(expectedEmployees[i].Salary, actualEmployees[i].Salary);
                Assert.Equal(expectedEmployees[i].DailyHours, actualEmployees[i].DailyHours);
                Assert.Equal(expectedEmployees[i].LeaveDays, actualEmployees[i].LeaveDays);
                Assert.Equal(expectedEmployees[i].DepartmentId, actualEmployees[i].DepartmentId);
                Assert.Equal(expectedEmployees[i].JobId, actualEmployees[i].JobId);
            }
        }


        [Fact]
        public async Task GetEmployeeByIdAsync_ReturnsExpectedEmployee()
        {
            // Arrange
            var mockEmployeeService = new Mock<IEmployeeService>();
            var expectedEmployee = new EmployeeDTO
            {
                EmployeeId = 1,
                FullName = "Test Employee",
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
            mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(1)).ReturnsAsync(expectedEmployee);

            var mockMapper = new Mock<IMapper>(); // Create a mock mapper if needed
            mockMapper.Setup(x => x.Map<EmployeeDTO>(It.IsAny<object>())).Returns(expectedEmployee);

            var controller = new EmployeeController(mockEmployeeService.Object, mockMapper.Object); // Use the mock mapper

            // Act
            var result = await controller.GetEmployeeByIdAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualEmployee = Assert.IsType<EmployeeDTO>(okResult.Value);
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

        [Fact]
        public async Task GetEmployeeByIdAsync_ReturnsNotFoundResult_WhenEmployeeIsNotFound()
        {
            // Arrange
            int nonExistentEmployeeId = 100;
            EmployeeDTO nullEmployee = null;
            _mockEmployeeService.Setup(repo => repo.GetEmployeeByIdAsync(nonExistentEmployeeId))
                .ReturnsAsync(nullEmployee);
            var controller = new EmployeeController(_mockEmployeeService.Object, _mockMapper.Object);

            // Act
            var result = await controller.GetEmployeeByIdAsync(nonExistentEmployeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            int employeeId = 1;
            var employeeDTO = new EmployeeDTO {
                EmployeeId = employeeId,
                FullName = "Test Employee",
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

            _mockEmployeeService.Setup(x => x.UpdateEmployeeAsync(employeeId, It.IsAny<EmployeeDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateEmployeeAsync(employeeId, employeeDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockEmployeeService.Verify(x => x.UpdateEmployeeAsync(employeeId, It.IsAny<EmployeeDTO>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_EmployeeNotFound_ReturnsNotFound()
        {
            // Arrange
            int employeeId = -1;
            var employeeDTO = new EmployeeDTO {
                EmployeeId = 2,
                FullName = "Test Employee",
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

            _mockEmployeeService.Setup(x => x.UpdateEmployeeAsync(employeeId, It.IsAny<EmployeeDTO>()))
                .ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await _controller.UpdateEmployeeAsync(employeeId, employeeDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockEmployeeService.Verify(x => x.UpdateEmployeeAsync(employeeId, It.IsAny<EmployeeDTO>()), Times.Never);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ExistingEmployee_ReturnsNoContent()
        {
            // Arrange
            int employeeId = 1;
            var existingEmployee = new EmployeeDTO {
                EmployeeId = 1,
                FullName = "Test Employee",
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

            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(existingEmployee);

            _mockEmployeeService.Setup(x => x.DeleteEmployeeAsync(employeeId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteEmployeeAsync(employeeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockEmployeeService.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
            _mockEmployeeService.Verify(x => x.DeleteEmployeeAsync(employeeId), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_NonExistingEmployee_ReturnsNotFound()
        {
            // Arrange
            int employeeId = 1;
            EmployeeDTO nonExistingEmployee = null;

            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(nonExistingEmployee);

            // Act
            var result = await _controller.DeleteEmployeeAsync(employeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockEmployeeService.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
            _mockEmployeeService.Verify(x => x.DeleteEmployeeAsync(employeeId), Times.Never);
        }
    }
}

