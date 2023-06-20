using AutoMapper;
using Moq;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Repositories;
using SOAProject.Services.EmployeeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagementSystemTest.EmployeeTests
{
    public class EmployeeServiceTest
    {
        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsAllEmployees()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            var employees = new List<Employee>
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
                }
            };

            mockUnitOfWork.Setup(uow => uow.EmployeeRepository.GetAllEmployeesAsync()).ReturnsAsync(employees);

            var employeeDTOs = new List<EmployeeDTO>
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
                }
            };

            mockMapper.Setup(mapper => mapper.Map<IEnumerable<EmployeeDTO>>(employees)).Returns(employeeDTOs);

            var employeeService = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await employeeService.GetAllEmployeesAsync();

            // Assert
            Assert.Equal(employeeDTOs, result);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ExistingId_ReturnsEmployee()
        {
            // Arrange
            int employeeId = 1;
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
            var employeeDTO = new EmployeeDTO
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

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.EmployeeRepository.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);
            mockMapper.Setup(mapper => mapper.Map<EmployeeDTO>(employee)).Returns(employeeDTO);

            var employeeService = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.Equal(employeeDTO, result);
        }

        [Fact]
        public async Task CreateEmployeeAsync_ValidEmployee_ReturnsNewEmployee()
        {
            // Arrange
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
            var employeeDTO = new EmployeeDTO
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

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(mapper => mapper.Map<Employee>(employeeDTO)).Returns(employee);
            mockUnitOfWork.Setup(uow => uow.EmployeeRepository.AddEmployeeAsync(employee)).Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(0)).Verifiable();
            mockMapper.Setup(mapper => mapper.Map<EmployeeDTO>(employee)).Returns(employeeDTO);

            var employeeService = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await employeeService.CreateEmployeeAsync(employeeDTO);

            // Assert
            Assert.Equal(employeeDTO, result);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ExistingId_UpdatesEmployee()
        {
            // Arrange
            int employeeId = 1;
            var existingEmployee = new Employee
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
            var employeeDTO = new EmployeeDTO
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

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.EmployeeRepository.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(existingEmployee);
            mockMapper.Setup(mapper => mapper.Map(employeeDTO, existingEmployee));

            var employeeService = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            await employeeService.UpdateEmployeeAsync(employeeId, employeeDTO);

            // Assert
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ExistingId_DeletesEmployee()
        {
            // Arrange
            int employeeId = 1;
            var existingEmployee = new Employee
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

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.EmployeeRepository.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(existingEmployee);
            mockUnitOfWork.Setup(uow => uow.EmployeeRepository.DeleteEmployee(existingEmployee));
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(0)).Verifiable();

            var employeeService = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            await employeeService.DeleteEmployeeAsync(employeeId);

            // Assert
            mockUnitOfWork.Verify(uow => uow.EmployeeRepository.DeleteEmployee(existingEmployee), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int employeeId = 1;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.EmployeeRepository.GetEmployeeByIdAsync(employeeId)).ReturnsAsync((Employee)null);

            var employeeService = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_NonExistingId_ThrowsException()
        {
            // Arrange
            int employeeId = 1;
            var employeeDTO = new EmployeeDTO
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

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(uow => uow.EmployeeRepository.GetEmployeeByIdAsync(employeeId)).ReturnsAsync((Employee)null);

            var employeeService = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => employeeService.UpdateEmployeeAsync(employeeId, employeeDTO));

            mockUnitOfWork.Verify(uow => uow.EmployeeRepository.GetEmployeeByIdAsync(employeeId), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
    }
}
