using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SOAProject.Controllers;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Services.JobService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystemTest.JobTests
{
    public class JobControllerTests
    {
        private readonly Mock<IJobService> _mockJobService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly JobController _controller;

        public JobControllerTests()
        {
            _mockJobService = new Mock<IJobService>();
            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<IEnumerable<JobDTO>>(It.IsAny<IEnumerable<JobDTO>>()))
                .Returns((IEnumerable<JobDTO> source) => source.Select(j => new JobDTO
                {
                    JobId = j.JobId,
                    JobName = j.JobName
                }).ToList());

            _controller = new JobController(_mockJobService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllJobsAsync_ReturnsOkResultWithJobDTOs()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            var expectedJobs = new List<JobDTO>
{
                new JobDTO
                {
                    JobId = 1,
                    JobName = "Job Name 1"
                },

                new JobDTO
                {
                    JobId = 2,
                    JobName = "Job Name 2"
                },

                new JobDTO
                {
                    JobId = 3,
                    JobName = "Job Name 3"
                }
            };
            mockJobService.Setup(x => x.GetAllJobsAsync())
                           .ReturnsAsync(expectedJobs);

            var controller = new JobController(mockJobService.Object, _mockMapper.Object);

            // Act
            var result = await controller.GetAllJobsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualJobs = Assert.IsType<List<JobDTO>>(okResult.Value);

            Assert.Equal(expectedJobs.Count, actualJobs.Count);

            for (int i = 0; i < expectedJobs.Count; i++)
            {
                Assert.Equal(expectedJobs[i].JobId, actualJobs[i].JobId);
                Assert.Equal(expectedJobs[i].JobName, actualJobs[i].JobName);
            }
        }


        [Fact]
        public async Task GetJobByIdAsync_ReturnsExpectedJob()
        {
            // Arrange
            var mockJobService = new Mock<IJobService>();
            var expectedJob = new JobDTO
            {
                JobId = 1,
                JobName = "Test Job"
            };
            mockJobService.Setup(x => x.GetJobByIdAsync(1)).ReturnsAsync(expectedJob);

            var mockMapper = new Mock<IMapper>(); // Create a mock mapper if needed
            mockMapper.Setup(x => x.Map<JobDTO>(It.IsAny<object>())).Returns(expectedJob);

            var controller = new JobController(mockJobService.Object, mockMapper.Object); // Use the mock mapper

            // Act
            var result = await controller.GetJobByIdAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualJob = Assert.IsType<JobDTO>(okResult.Value);
            Assert.Equal(expectedJob.JobId, actualJob.JobId);
            Assert.Equal(expectedJob.JobName, actualJob.JobName);
        }

        [Fact]
        public async Task GetJobByIdAsync_ReturnsNotFoundResult_WhenJobIsNotFound()
        {
            // Arrange
            int nonExistentJobId = 100;
            JobDTO nullJob = null;
            _mockJobService.Setup(repo => repo.GetJobByIdAsync(nonExistentJobId))
                .ReturnsAsync(nullJob);
            var controller = new JobController(_mockJobService.Object, _mockMapper.Object);

            // Act
            var result = await controller.GetJobByIdAsync(nonExistentJobId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateJobAsync_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            int jobId = 1;
            var jobDTO = new JobDTO { JobId = jobId, JobName = "Updated Job" };

            _mockJobService.Setup(x => x.UpdateJobAsync(jobId, It.IsAny<JobDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateJobAsync(jobId, jobDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockJobService.Verify(x => x.UpdateJobAsync(jobId, It.IsAny<JobDTO>()), Times.Once);
        }

        [Fact]
        public async Task UpdateJobAsync_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int jobId = 1;
            var jobDTO = new JobDTO { JobId = 2, JobName = "Updated Job" };

            // Act
            var result = await _controller.UpdateJobAsync(jobId, jobDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            _mockJobService.Verify(x => x.UpdateJobAsync(It.IsAny<int>(), It.IsAny<JobDTO>()), Times.Never);
        }

        [Fact]
        public async Task UpdateJobAsync_JobNotFound_ReturnsNotFound()
        {
            // Arrange
            int jobId = 1;
            var jobDTO = new JobDTO { JobId = jobId, JobName = "Updated Job" };

            _mockJobService.Setup(x => x.UpdateJobAsync(jobId, It.IsAny<JobDTO>()))
                .ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await _controller.UpdateJobAsync(jobId, jobDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockJobService.Verify(x => x.UpdateJobAsync(jobId, It.IsAny<JobDTO>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJobAsync_ExistingJob_ReturnsNoContent()
        {
            // Arrange
            int jobId = 1;
            var existingJob = new JobDTO { JobId = jobId, JobName = "Existing Job" };

            _mockJobService.Setup(x => x.GetJobByIdAsync(jobId))
                .ReturnsAsync(existingJob);

            _mockJobService.Setup(x => x.DeleteJobAsync(jobId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteJobAsync(jobId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockJobService.Verify(x => x.GetJobByIdAsync(jobId), Times.Once);
            _mockJobService.Verify(x => x.DeleteJobAsync(jobId), Times.Once);
        }

        [Fact]
        public async Task DeleteJobAsync_NonExistingJob_ReturnsNotFound()
        {
            // Arrange
            int jobId = 1;
            JobDTO nonExistingJob = null;

            _mockJobService.Setup(x => x.GetJobByIdAsync(jobId))
                .ReturnsAsync(nonExistingJob);

            // Act
            var result = await _controller.DeleteJobAsync(jobId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockJobService.Verify(x => x.GetJobByIdAsync(jobId), Times.Once);
            _mockJobService.Verify(x => x.DeleteJobAsync(jobId), Times.Never);
        }
    }
}

